namespace iDevWorks.Ticket
{
    public class Message
    {
        public int MessageId { get; }
        public Guid ThreadId { get; }
        public DateTime SentTime { get; }
        public string[] ReceiverIds { get; init; } = [];
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
    }
}
