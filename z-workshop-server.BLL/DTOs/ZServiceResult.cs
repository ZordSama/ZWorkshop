namespace z_workshop_server.BLL.DTOs;

public class ZServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }
    public int Code { get; private set; }

    private ZServiceResult(bool success, string message, T? data, int code)
    {
        IsSuccess = success;
        Message = message;
        Data = data;
        Code = code;
    }

    public static ZServiceResult<T> Success(
        string message = "Success",
        T? data = default,
        int code = 200
    )
    {
        return new ZServiceResult<T>(true, message, data, code);
    }

    public static ZServiceResult<T> Failure(string message = "Failure", int code = 500)
    {
        return new ZServiceResult<T>(false, message, default, code);
    }
}
