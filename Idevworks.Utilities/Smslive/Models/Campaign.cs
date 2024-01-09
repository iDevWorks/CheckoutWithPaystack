namespace iDevWorks.BulkSMS.Contracts
{
    public class Campaign
    {
        public Campaign(long batchID, string senderID, string messageText, int messageParts, long totalProcessed, long totalFailed, decimal charged, DateTime submitDate, string status)
        {
            BatchID = batchID;
            SenderID = senderID;
            MessageText = messageText;
            MessageParts = messageParts;
            TotalProcessed = totalProcessed;
            TotalFailed = totalFailed;
            Charged = charged;
            SubmitDate = submitDate;
            Status = status;
        }

        public long BatchID { get; }
        public string SenderID { get; }
        public string MessageText { get; }
        public int MessageParts { get; }
        public long TotalProcessed { get; }
        public long TotalFailed { get; }
        public decimal Charged { get; }
        public DateTime SubmitDate { get; }
        public string Status { get; }
    }    
}
