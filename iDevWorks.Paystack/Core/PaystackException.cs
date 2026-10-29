namespace iDevWorks.Paystack;

public class PaystackException(PaystackErrorCode code, string? message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public PaystackErrorCode Code { get; } = code;
}
