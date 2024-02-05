namespace iDevWorks.Ticket
{
    public class Message(int messageId, Guid threadId, DateTime sentTime, string[] receiverIds, string subject, string body)
    {
        public int MessageId { get; } = messageId;
        public Guid ThreadId { get; } = threadId;
        public DateTime SentTime { get; } = sentTime;
        public string[] ReceiverIds { get; } = receiverIds;
        public string Subject { get; } = subject;
        public string Body { get; } = body;
    }
}
