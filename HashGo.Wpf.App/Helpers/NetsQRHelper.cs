using ControlzEx.Standard;
using HashGo.Core.Contracts.Services;
using HashGo.Core.Models;
using HashGo.Domain.Helper;
using HashGo.Infrastructure;
using Metsys.Bson;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Windows.Networking.Connectivity;

namespace HashGo.Wpf.App.Helpers
{
    public class NetsQRHelper
    {
        private readonly ILoggingService _logger;

        public NetsQRHelper(ILoggingService logger)
        {
            _logger = logger;
        }

        public PaymentResponseDto ProcessPayment(string hostId, string hostMId, string stanId, decimal amount, string invoiceRef, string gatewayToken)
        {
            var paymentResponse = new PaymentResponseDto();
            try
            {
                Random random = new Random();
                var netsQrObj = new NetsQRDto
                {
                    HostTid = hostId,
                    HostMid = hostMId,
                    //IsProduction = true,
                    Stan = stanId, //Utility.AppendValue(random.Next(0, 999999).ToString(), 6, true),
                    Amount = ((int)(amount * 100)).ToString().PadLeft(12, '0'),
                    TransactionDate = DateTime.Now.ToString("MMdd"),
                    TransactionTime = DateTime.Now.ToString("HHmmss"),
                    InvoiceRef = invoiceRef //Helper.Utility.AppendValue(input.PaymentRequest.Id.ToString(), 10, true)
                };

                netsQrObj.NpxData.E201 = ((int)(amount * 100)).ToString().PadLeft(12, '0');

                var client = new RestClient($"{GatewayUrl}netsqr/api/order/request");
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Authorization", $"Bearer {gatewayToken}");
                request.AddHeader("Content-Type", "application/json");

                var myBody = JsonConvert.SerializeObject(netsQrObj);
                _logger.Info("Send to NETS QR - " + myBody);
                request.AddParameter("application/json", myBody,
                    ParameterType.RequestBody);

                var response = (RestResponse)client.ExecutePost(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<NetQRResponse>(response.Content);

                    if (result.data.QrCode != null)
                    {
                        _logger.Info("Received From NETS QR - " + JsonConvert.SerializeObject(result));

                        paymentResponse.NetsQrCode = result.data.QrCode;
                        paymentResponse.NetQRPaymentResponse = result;
                    }
                }
            }
            catch (Exception ex)
            {
                paymentResponse.Message = ex.Message;
            }

            return paymentResponse;
        }

        public PaymentResponseDto PaymentStatus(string hostId, string hostMId, string stanId, decimal amount, string institutionCode, string txnIdentifier, string invoiceRef, string gatewayToken)
        {
            var paymentResponse = new PaymentResponseDto();

            try
            {
                var netsQrObj = new
                {
                    mti = "0100",
                    IsProduction = true,
                    process_code = "330000",
                    stan = stanId, //"001002",
                    transaction_time = DateTime.Now.ToString("HHmmss"),
                    transaction_date = DateTime.Now.ToString("MMdd"),
                    entry_mode = "000",
                    //Soofi 22-Aug-2024. Condition code change to 85
                    condition_code = "85",
                    institution_code = institutionCode,//input.NetsQrPaymentResponse.data.InstitutionCode,
                    host_tid = hostId,
                    host_mid = hostMId,
                    txn_identifier = txnIdentifier, //input.NetsQrPaymentResponse.data.TxnIdentifier,
                    npx_data = new
                    {
                        E201 = ((int)(amount * 100)).ToString().PadLeft(12, '0'),
                        E202 = "SGD",
                        E103 = hostId
                    },
                    invoice_ref = invoiceRef, // input.NetsQrPaymentResponse.data.InvoiceRef,
                };


                var client = new RestClient($"{GatewayUrl}netsqr/api/transaction/query");
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {gatewayToken}");
                request.AddHeader("Content-Type", "application/json");
                var myBody = JsonConvert.SerializeObject(netsQrObj);
                request.AddParameter("application/json", myBody,
                    ParameterType.RequestBody);

                var response = (RestResponse)client.ExecutePost(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<NetQRResponse>(response.Content);
                    if (result != null)
                    {
                        paymentResponse.IsSuccess = result.data.ResponseCode.Equals("00");
                        if (paymentResponse.IsSuccess)
                        {
                            _logger.Info("Receive from NETS QR Success - " + JsonConvert.SerializeObject(result.data));
                        }
                    }

                }
            }
            catch (Exception e)
            {
                paymentResponse.Message = e.Message;
            }

            //paymentResponse.IsSuccess = true;
            return paymentResponse;
        }

        public PaymentResponseDto ReverseTransaction(string hostId, string hostMId, string stanId, decimal amount, string invoiceRef, string txnIdentifier, string gatewayToken)
        {
            var paymentResponse = new PaymentResponseDto();
            try
            {
                //var pReq = JsonConvert.DeserializeObject<FrontWheelPaymentRequest>(input.Request);

                var netsQrObj = new TransactionReversalInput
                {
                    HostMid = hostMId,
                    HostTid = hostId,
                    //IsProduction = true,
                    TxnIdentifier = txnIdentifier,
                    Amount = ((int)(amount * 100)).ToString().PadLeft(12, '0'),
                    TransactionDate = DateTime.Now.ToString("MMdd"),
                    TransactionTime = DateTime.Now.ToString("HHmmss"),
                    InvoiceRef = invoiceRef
                };
                netsQrObj.EntryMode = "000";
                netsQrObj.ConditionCode = "85";
                netsQrObj.NpxData = new
                {
                    //E201 = ((int)(amount * 100)).ToString().PadLeft(12, '0'),
                    //E202 = "SGD",
                    E103 = hostId
                };
                netsQrObj.TransmissionTime = netsQrObj.TransactionDate + netsQrObj.TransactionTime;
                netsQrObj.Stan = stanId;

                var client = new RestClient($"{GatewayUrl}netsqr/api/transaction/reversal");
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Authorization", $"Bearer {gatewayToken}");
                request.AddHeader("Content-Type", "application/json");
                var myBody = JsonConvert.SerializeObject(netsQrObj);
                request.AddParameter("application/json", myBody,
                    ParameterType.RequestBody);

                var response = (RestResponse)client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = JsonConvert.DeserializeObject<TransactionReversalOutput>(response.Content);

                    if (!result.Error)
                    {
                        paymentResponse.IsSuccess = true;
                        paymentResponse.Message = result.message;
                    }
                }
            }
            catch (Exception ex)
            {
                paymentResponse.Message = ex.Message;
            }

            return paymentResponse;
        }

        public string GatewayUrl
        {
            get
            {
                return "https://dgateway.xyz/";
            }
        }



    }
}
