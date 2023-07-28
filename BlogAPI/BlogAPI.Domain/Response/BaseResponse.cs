using System.Collections.Generic;
using BlogAPI.Domain.Enum;

namespace BlogAPI.Domain.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; } = null!;
    public StatusCode StatusCode { get; set; }
    public T? Data { get; set; }

    public BaseResponse<T> ServerResponse(string description, StatusCode statusCode)
    {
        return new BaseResponse<T>
        {
            Description = description,
            StatusCode = statusCode
        };
    }

    public BaseResponse<IEnumerable<T>> ServerResponseEnumerable(string description, StatusCode statusCode)
    {
        return new BaseResponse<IEnumerable<T>>
        {
            Description = description,
            StatusCode = statusCode
        };
    }
}

public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCode StatusCode { get; }
    T? Data { get; }
}