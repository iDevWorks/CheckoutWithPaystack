namespace iDevWorks.BulkSMS
{
    public class Account
    {
        public Account(string accountID, string accountName, decimal walletBalance, decimal smsBalance, int status)
        {
            AccountID = accountID;
            AccountName = accountName;
            WalletBalance = walletBalance;
            SmsBalance = smsBalance;
            Status = status;
        }

        public string AccountID { get; }
        public string AccountName { get; }
        public decimal WalletBalance { get; }
        public decimal SmsBalance { get; }
        public int Status { get; }
    }
}
