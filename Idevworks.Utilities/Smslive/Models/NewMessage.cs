namespace iDevWorks.BulkSMS
{
    internal class NewMessage
    {
        public string SenderID { get; init; } = string.Empty;
        public string MessageText { get; init; } = string.Empty;
        public string MobileNumber { get; init; } = string.Empty;
        public string Route { get; init; } = string.Empty;
    }
}
