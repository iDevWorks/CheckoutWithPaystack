namespace iDevWorks.BulkSMS
{
    public class Campaign(long batchID, string senderID, string messageText, int messageParts, long totalProcessed, long totalFailed, decimal charged, DateTime submitDate, string status)
    {
        public long BatchID { get; } = batchID;
        public string SenderID { get; } = senderID;
        public string MessageText { get; } = messageText;
        public int MessageParts { get; } = messageParts;
        public long TotalProcessed { get; } = totalProcessed;
        public long TotalFailed { get; } = totalFailed;
        public decimal Charged { get; } = charged;
        public DateTime SubmitDate { get; } = submitDate;
        public string Status { get; } = status;
    }    
}
