﻿namespace iDevWorks.BulkSMS
{
    internal class NewCampaign
    {
        public string SenderID { get; init; } = string.Empty;
        public string MessageText { get; init; } = string.Empty;
        public DateTime? DeliveryTime { get; init; } = default;
        public List<string> MobileNumbers { get; init; } = [];
        public string Route { get; init; } = string.Empty;
    }    
}
