namespace iDevWorks.BulkSMS
{
    public class Message(int batchID, string messageID, string senderID, string messageText, string mobileNumber, DateTime submitDate, decimal charged, List<Message.DeliveryReport> reports)
    {
        public int BatchID { get; } = batchID;
        public string MessageID { get; } = messageID;
        public string SenderID { get; } = senderID;
        public string MessageText { get; } = messageText;
        public string MobileNumber { get; } = mobileNumber;
        public DateTime SubmitDate { get; } = submitDate;
        public decimal Charged { get; } = charged;
        public List<DeliveryReport> Reports { get; } = reports;

        public class DeliveryReport(string status, string smscID, DateTime? reportDate)
        {
            public string Status { get; } = status;
            public string SmscID { get; } = smscID;
            public DateTime? ReportDate { get; } = reportDate;
        }
    }
}
