namespace iDevWorks.BulkSMS
{
    public class Message
    {
        public Message(int batchID, string messageID, string senderID, string messageText, string mobileNumber, DateTime submitDate, decimal charged, List<DeliveryReport> reports)
        {
            BatchID = batchID;
            MessageID = messageID;
            SenderID = senderID;
            MessageText = messageText;
            MobileNumber = mobileNumber;
            SubmitDate = submitDate;
            Charged = charged;
            Reports = reports;
        }

        public int BatchID { get; }
        public string MessageID { get; }
        public string SenderID { get; }
        public string MessageText { get; }
        public string MobileNumber { get; }
        public DateTime SubmitDate { get; }
        public decimal Charged { get; }
        public List<DeliveryReport> Reports { get; }

        public class DeliveryReport
        {
            public DeliveryReport(string status, string smscID, DateTime? reportDate)
            {
                Status = status;
                SmscID = smscID;
                ReportDate = reportDate;
            }

            public string Status { get; }
            public string SmscID { get; }
            public DateTime? ReportDate { get; }
        }
    }
}
