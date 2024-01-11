namespace iDevWorks.BulkSMS
{
    internal class SmsliveException : Exception
    {
        public SmsliveException(HttpResponseMessage responseMessage)
        {
            Code = (int)responseMessage.StatusCode;
            Message = responseMessage.ReasonPhrase;
            Errors = null;
        }

        public SmsliveException(int code, string? message, List<ValidationError>? errors = null)
        {
            Code = code;
            Message = message;
            Errors = errors;
        }

        public int Code { get; }
        public new string? Message { get; } 
        public List<ValidationError>? Errors { get; }

        public class ValidationError(string field, string message)
        {
            public string Field { get; } = field;
            public string Message { get; } = message;
        }
    }    
}
