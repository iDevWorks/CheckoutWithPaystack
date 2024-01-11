namespace iDevWorks.Paystack
{
    internal class PaystackException(Status code, string? message) : Exception
    {
        public Status Code { get; } = code;
        public new string? Message { get; } = message;
    }

    internal enum Status
    {
        REQUEST_OK_JSON_OK = 0,
        REQUEST_OK_JSON_FAIL = 550,
        REQUEST_FAIL_JSON_OK = 551,
        REQUEST_FAIL_JSON_FAIL = 552,
    }
}
