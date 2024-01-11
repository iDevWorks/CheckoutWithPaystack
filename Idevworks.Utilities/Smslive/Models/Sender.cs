namespace iDevWorks.BulkSMS
{
    public class Sender(int requestId, DateTime requestDate, DateTime? approvalDate, string senderId, string status)
    {
        public int RequestId { get; } = requestId;
        public DateTime RequestDate { get; } = requestDate;
        public DateTime? ApprovalDate { get; } = approvalDate;
        public string SenderId { get; } = senderId;
        public string Status { get; } = status;
    }
}
