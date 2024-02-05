namespace iDevWorks.Ticket
{
    public class MessageThread(Guid threadId, string subject, string lastSenderId, string lastMessageBody, DateTime lastMessageDate, int totalMessages, bool isRead)
    {
        public Guid ThreadId { get; } = threadId;
        public string Subject { get; } = subject;
        public string LastSenderId { get; } = lastSenderId;
        public string LastMessageBody { get; } = lastMessageBody;
        public DateTime LastMessageDate { get; } = lastMessageDate;
        public int TotalMessages { get; } = totalMessages;
        public bool IsRead { get; } = isRead;
    }
}
