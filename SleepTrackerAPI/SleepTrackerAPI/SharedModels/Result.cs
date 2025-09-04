public enum ResultReason
{
    Unknown,
    NotFound,
    InvalidField,
    DuplicateEntry,
    EntityChildrenFound,
    Success,
    Failure,
}

public class Result
{
    protected ResultReason reason;
    protected string userErrorMessage;

    public Result(ResultReason reason, string userErrorMessage = "")
    {
        this.reason = reason;
        this.userErrorMessage = userErrorMessage;
    }

    public bool Successful => reason == ResultReason.Success;

    public ResultReason Reason => reason;

    public string UserFacingErrorMessage => userErrorMessage;


    public static Result Success()
    {
        return new Result(ResultReason.Success);
    }

    public static Result Fail(ResultReason reason = ResultReason.Unknown, string message = "")
    {
        return new Result(reason, message);
    }
}

public class Result<T> : Result
{
    internal T model;

    public Result(T model, ResultReason reason = ResultReason.Unknown, string userErrorMessage = "") : base(reason, userErrorMessage)
    {
        this.model = model;
    }

    public TResult GetResult<TResult>(Func<T, TResult> selector)
    {
        return selector.Invoke(this.model);
    }

    public static Result<T> Success(T model)
    {
        return new Result<T>(model, ResultReason.Success);
    }

    public static Result<T> Fail(ResultReason reason = ResultReason.Unknown, string message = "")
    {
        return new Result<T>(default, reason, message);
    }
}
