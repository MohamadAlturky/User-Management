namespace Donut.SharedKernel.Results;

public class Error
{

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error Null = new("No Data Found From Domain !!", "The Result Value Is Null");
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    public string Code { get; set; }
    public string Message { get; set; }
}