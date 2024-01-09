namespace iDevWorks.BulkSMS.Contracts
{
    internal class MessageRequest
    {
        public string SenderID { get; init; } = string.Empty;
        public string MessageText { get; init; } = string.Empty;
        public string MobileNumber { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;
    }
}
