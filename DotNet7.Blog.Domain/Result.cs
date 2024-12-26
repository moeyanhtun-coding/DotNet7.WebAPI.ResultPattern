namespace DotNet7.Blog.Domain;

public class Result<T>
{
    public bool IsSuccess { get; set; }

    public bool IsError => !IsSuccess;

    public bool IsValidationError => Type == EnumRespType.ValidationError;
    
    public bool IsSystemError => Type == EnumRespType.SystemError;
    private EnumRespType Type { get; init; }
    
    public T Data { get; set; }
   
    public string Message { get; set; }

    public static Result<T> Success(T data, string message = "Success")
    {
        return new Result<T>
        {
            IsSuccess = true,
            Type = EnumRespType.Success,
            Data = data,
            Message = message
        };
    }

    public static Result<T> ValidationError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumRespType.ValidationError,
            Data = data!,
            Message = message
        };
    }

    public static Result<T> SystemError(string message, T? data = default)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Type = EnumRespType.SystemError,
            Data = data!,
            Message = message
        };
    }
        
}