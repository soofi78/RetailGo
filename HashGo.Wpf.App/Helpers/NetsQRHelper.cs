﻿using ControlzEx.Standard;
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

namespace HashGo.Wpf.App.Helpers
{
    public class NetsQRHelper
    {
        private readonly ILoggingService _logger;

        public NetsQRHelper(ILoggingService logger)
        {
            _logger = logger;
        }

        public PaymentResponseDto ProcessPayment(string hostId, string hostMId, decimal amount, string invoiceRef, string gatewayToken)
        {
            var paymentResponse = new PaymentResponseDto();
            try
            {
                Random random = new Random();
                var netsQrObj = new NetsQRDto
                {
                    HostTid = hostId,
                    HostMid = hostMId,
                    //Stan = Utility.AppendValue(random.Next(0, 999999).ToString(), 10, true),
                    Amount = ((int)(amount * 100)).ToString().PadLeft(12, '0'),
                    TransactionDate = DateTime.Now.ToString("MMdd"),
                    TransactionTime = DateTime.Now.ToString("HHmmss"),
                    InvoiceRef = invoiceRef //Helper.Utility.AppendValue(input.PaymentRequest.Id.ToString(), 10, true)
                };
                var client = new RestClient($"{GatewayUrl}netsqr/api/order/request");
                var request = new RestRequest();
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

        public PaymentResponseDto PaymentStatus(string hostId, string hostMId, string institutionCode, string txnIdentifier, string invoiceRef, string gatewayToken)
        {
            var paymentResponse = new PaymentResponseDto();

            try
            {
                var netsQrObj = new
                {
                    mti = "0100",
                    process_code = "330000",
                    stan = "001002",
                    transaction_time = DateTime.Now.ToString("HHmmss"),
                    transaction_date = DateTime.Now.ToString("MMdd"),
                    entry_mode = "000",
                    condition_code = "00",
                    institution_code = institutionCode,//input.NetsQrPaymentResponse.data.InstitutionCode,
                    host_tid = hostId,
                    host_mid = hostMId,
                    txn_identifier = txnIdentifier, //input.NetsQrPaymentResponse.data.TxnIdentifier,
                    npx_data = new
                    {
                        E103 = "37066801"
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

        public string GatewayUrl
        {
            get
            {
                return "https://dgateway.xyz/";
            }
        }



    }
}
