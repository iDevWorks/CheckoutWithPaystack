using System.Text.Json.Serialization;

namespace iDevWorks.BulkSMS
{
    public class Sender
    {
        public Sender(int requestId, DateTime requestDate, DateTime? approvalDate, string senderId, string status)
        {
            RequestId = requestId;
            RequestDate = requestDate;
            ApprovalDate = approvalDate;
            SenderId = senderId;
            Status = status;
        }

        public int RequestId { get; }
        public DateTime RequestDate { get; }
        public DateTime? ApprovalDate { get; }

        [JsonPropertyName("SenderId")]
        public string SenderId { get; }
        public string Status { get; }
    }
}
