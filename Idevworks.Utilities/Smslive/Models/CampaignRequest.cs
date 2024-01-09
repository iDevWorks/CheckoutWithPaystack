namespace iDevWorks.BulkSMS
{
    internal class CampaignRequest
    {
        public string SenderID { get; init; } = string.Empty;
        public string MessageText { get; init; } = string.Empty;
        public DateTime? DeliveryTime { get; init; } = default;
        public List<string> MobileNumbers { get; init; } = new List<string>();
        public string Route { get; init; } = string.Empty;
    }    
}
