using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HashGo.Wpf.App.Helpers
{

    public class NetsQRDto
    {
        public NetsQRDto()
        {
            CommunicationData = new List<NetsQRCommunicationData> { new NetsQRCommunicationData() };
            NpxData = new NetsQRNpxData();
        }
        [JsonProperty("mti")]
        public string Mti { get; set; } = "0200";
        [JsonProperty("process_code")]
        public string ProcessCode { get; set; } = "990000";
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("stan")]
        public string Stan { get; set; } = "001002";
        [JsonProperty("transaction_time")]
        public string TransactionTime { get; set; }
        [JsonProperty("transaction_date")]
        public string TransactionDate { get; set; }
        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; } = "000";
        [JsonProperty("condition_code")]
        public string ConditionCode { get; set; } = "85";
        [JsonProperty("institution_code")]
        public string InstitutionCode { get; set; } = "20000000001";
        [JsonProperty("retrieval_ref")]
        public string RetrievalRef { get; set; } = "206026360706";
        [JsonProperty("host_tid")]
        public string HostTid { get; set; }
        [JsonProperty("host_mid")]
        public string HostMid { get; set; }
        [JsonProperty("getQRCode")]
        public string GetQRCode { get; set; } = "Y";
        [JsonProperty("communication_data")]
        public List<NetsQRCommunicationData> CommunicationData { get; set; }
        [JsonProperty("npx_data")]
        public NetsQRNpxData NpxData { get; set; }
        [JsonProperty("invoice_ref")]
        public string InvoiceRef { get; set; }
    }

    public class NetsQRAddon
    {
        [JsonProperty("external_API_keyID")]
        public string ExternalAPIkeyID { get; set; } = "231e4c11-135a-4457-bc84-3cc6d3565506";
    }

    public class NetsQRCommunicationData
    {
        public NetsQRCommunicationData()
        {
            Addon = new NetsQRAddon();
        }
        [JsonProperty("type")]
        public string Type { get; set; } = "https_proxy";
        [JsonProperty("category")]
        public string Category { get; set; } = "URL";
        [JsonProperty("destination")]
        public string Destination { get; set; } = "https://dgateway.xyz/netsqr/webhook";
        [JsonProperty("addon")]
        public NetsQRAddon Addon { get; set; }
    }

    public class NetsQRNpxData
    {
        public string E103 { get; set; } = "37066801";
        //Soofi 22-Aug-2024. remove hardcode.
        public string E201 { get; set; } = "000000000100";
        public string E202 { get; set; } = "SGD";
    }


    public class NetQRResponse
    {
        [JsonProperty("error")]
        public bool Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public NetsQRResponseData data { get; set; }
    }

    public class NetsQRResponseData
    {
        [JsonProperty("retrieval_ref")]
        public string RetrievalRef { get; set; }
        [JsonProperty("mti")]
        public string Mti { get; set; }
        [JsonProperty("txn_identifier")]
        public string TxnIdentifier { get; set; }
        [JsonProperty("process_code")]
        public string ProcessCode { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("stan")]
        public string Stan { get; set; }
        [JsonProperty("transaction_time")]
        public string TransactionTime { get; set; }
        [JsonProperty("transaction_date")]
        public string TransactionDate { get; set; }
        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }
        [JsonProperty("condition_code")]
        public string ConditionCode { get; set; }
        [JsonProperty("institution_code")]
        public string InstitutionCode { get; set; }
        [JsonProperty("response_code")]
        public string ResponseCode { get; set; }
        [JsonProperty("host_tid")]
        public string HostTid { get; set; }
        [JsonProperty("host_mid")]
        public string HostMid { get; set; }
        [JsonProperty("invoice_ref")]
        public string InvoiceRef { get; set; }
        [JsonProperty("qr_code")]
        public string QrCode { get; set; }
    }

    public class TransactionReversalInput
    {
        [JsonProperty("npx_data")]
        public object NpxData = new
        {
            E103 = "37055001"
        };

        [JsonProperty("mti")]
        public string MTI { get; set; } = "0400";

        [JsonProperty("retrieval_ref")]
        public string RetrievalRef { get; set; }

        [JsonProperty("txn_identifier")]
        public string TxnIdentifier { get; set; }

        [JsonProperty("process_code")]
        public string ProcessCode { get; set; } = "990000";

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("stan")]
        public string Stan { get; set; } = "002856";

        [JsonProperty("transaction_time")]
        public string TransactionTime { get; set; }

        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }

        [JsonProperty("condition_code")]
        public string ConditionCode { get; set; } = "83";

        [JsonProperty("institution_code")]
        public string InstitutionCode { get; set; } = "20000000001";

        [JsonProperty("transmission_time")]
        public string TransmissionTime { get; set; } = "0205190014";

        [JsonProperty("transaction_date")]
        public string TransactionDate { get; set; }

        [JsonProperty("host_tid")]
        public string HostTid { get; set; }

        [JsonProperty("host_mid")]
        public string HostMid { get; set; }

        [JsonProperty("invoice_ref")]
        public string InvoiceRef { get; set; }
    }

    public class TransactionReversalOutput
    {
        [JsonProperty("error")]
        public bool Error { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
        public ReversalOutputData data { get; set; }
    }

    public class ReversalOutputData
    {
        [JsonProperty("retrieval_ref")]
        public string RetrievalRef { get; set; }
        [JsonProperty("mti")]
        public string Mti { get; set; }
        [JsonProperty("txn_identifier")]
        public string TxnIdentifier { get; set; }
        [JsonProperty("process_code")]
        public string ProcessCode { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("stan")]
        public string Stan { get; set; }
        [JsonProperty("transaction_time")]
        public string TransactionTime { get; set; }
        [JsonProperty("transaction_date")]
        public string TransactionDate { get; set; }
        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }
        [JsonProperty("condition_code")]
        public string ConditionCode { get; set; }
        [JsonProperty("institution_code")]
        public string InstitutionCode { get; set; }
        [JsonProperty("response_code")]
        public string ResponseCode { get; set; }
        [JsonProperty("host_tid")]
        public string HostTid { get; set; }
        [JsonProperty("host_mid")]
        public string HostMid { get; set; }
        [JsonProperty("transmission_time")]
        public string TransmissionTime { get; set; }
    }


    public class PaymentResponseDto
    {
        public string PaymentId { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        public string RedirectUrl { get; set; }
        public string AuthorizeUri { get; set; }

        public string Settings { get; set; }

        public string DownloadUri { get; set; }

        public string TransactionId { get; set; }

        public long PaidAmount { get; set; }
        public string PaymentLink { get; set; }
        public string NetsQrCode { get; set; }
        public NetQRResponse NetQRPaymentResponse { get; set; }
    }

    

}
