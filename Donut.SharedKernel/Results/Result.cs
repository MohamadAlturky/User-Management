namespace Donut.SharedKernel.Results;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {

        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; }



    public static Result Success() => new Result(true, Error.None);
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value, true, Error.None);
    }



    public static Result Failure(Error error) => new Result(false, error);
    public static Result<T> Failure<T>(Error error) => new Result<T>(error);


    public static Result<T> Create<T>(T? value)
    {
        if (value != null)
        {
            return Result.Success(value);
        }
        else
        {
            return Result.Failure<T>(Error.Null);
        }
    }
}
public class Result<T> : Result
{
    private T _value;
    protected internal Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    protected internal Result(Error error)
        : base(false, error)
    {
        _value = default;
    }

    public T Value => IsSuccess ? _value :
        throw new InvalidOperationException("The value of a failure result can't be accessed.");
}