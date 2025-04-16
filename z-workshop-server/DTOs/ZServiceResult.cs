namespace z_workshop_server.DTOs;

public class ZServiceResult<T>
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public T? Data { get; private set; }

    private ZServiceResult(bool success, string message, T? data)
    {
        IsSuccess = success;
        Message = message;
        Data = data;
    }

    public static ZServiceResult<T> Success(string message, T? data = default)
    {
        return new ZServiceResult<T>(true, message, data);
    }

    public static ZServiceResult<T> Failure(string message)
    {
        return new ZServiceResult<T>(false, message, default);
    }
}
