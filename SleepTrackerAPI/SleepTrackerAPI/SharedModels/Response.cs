public class Response
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public Response(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    public Response(Result result) : this(result.Successful, result.UserFacingErrorMessage)
    {
    }
}