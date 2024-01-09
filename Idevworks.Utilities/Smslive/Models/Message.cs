namespace iDevWorks.BulkSMS.Contracts
{
    public class Message
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
}
