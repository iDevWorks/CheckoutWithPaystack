namespace iDevWorks.Paystack;

public enum PaystackErrorCode
{
    API_ERROR = 551,
    UNKNOWN_ERROR = 552,
    MALFORMED_JSON = 553,
    TIMEOUT = 554,
    NETWORK_ERROR = 555,

    INVALID_SIGNATURE = 556,
    FAILED_TO_PROCESS_EVENT = 558,
}
