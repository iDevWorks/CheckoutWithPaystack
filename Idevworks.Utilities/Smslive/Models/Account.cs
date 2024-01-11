namespace iDevWorks.BulkSMS
{
    public class Account(string accountID, string accountName, decimal walletBalance, decimal smsBalance, int status)
    {
        public string AccountID { get; } = accountID;
        public string AccountName { get; } = accountName;
        public decimal WalletBalance { get; } = walletBalance;
        public decimal SmsBalance { get; } = smsBalance;
        public int Status { get; } = status;
    }
}
