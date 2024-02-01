namespace iDevWorks.Ticket
{
    public class MessageThread
    {
        public Guid ThreadId { get;  }
        public string Subject { get; }
        public string LastSenderId { get;  }
        public string LastMessageBody { get;  }
        public DateTime LastMessageDate { get;  }
        public int TotalMessages { get;  }
        public bool IsRead { get; }
    }
}
