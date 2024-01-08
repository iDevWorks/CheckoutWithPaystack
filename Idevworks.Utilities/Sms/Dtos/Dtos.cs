using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Idevworks.Utilities.SMS.Dtos
{

    internal class SmsCampaignRequest
    {
        public string SenderID { get; set; }
        public string MessageText { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public List<string> MobileNumbers { get; set; }
        public string Route { get; set; }
    }

    internal class SmsCampaignResponse
    {
        public long BatchID { get; set; }
        public string SenderID { get; set; }
        public string MessageText { get; set; }
        public int MessageParts { get; set; }
        public long TotalProcessed { get; set; }
        public long TotalFailed { get; set; }
        public decimal Charged { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Status { get; set; }

    }

    internal class SmsMessageRequest
    {
        public string SenderID { get; set; }
        public string MessageText { get; set; }
        //public DateTime? DeliveryTime { get; set; }
        public string MobileNumber { get; set; }
        public string Route { get; set; }
    }

    public class SmsMessageResponse
    {
        public int BatchID { get; set; }
        public string MessageID { get; set; }
        public string SenderID { get; set; }
        public string MessageText { get; set; }
        public string MobileNumber { get; set; }
        public DateTime SubmitDate { get; set; }
        public decimal Charged { get; set; }
        public List<DeliveryReport> Reports { get; set; }



        public class DeliveryReport
        {
            public string Status { get; set; }
            public string SmscID { get; set; }
            public DateTime? ReportDate { get; set; }
        }
    }




    internal class AccountResponse
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal SmsBalance { get; set; }
        public int Status { get; set; }
    }


    internal class SenderIdResponse
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string SenderId { get; set; }
        public string Status { get; set; }
    }

    public class Pager
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

    internal class ApiErrorException : Exception
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public List<ValidationError> Errors { get; set; }



        public class ValidationError
        {
            public string Field { get; set; }

            public string Message { get; set; }

        }

    }

    
}
