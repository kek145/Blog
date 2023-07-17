using BlogAPI.Domain.Enum;

namespace BlogAPI.Domain.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; }
    public StatusCode StatusCode { get; set; }

    public  BaseResponse<T> SuccessRequest(string description)
    {
        return new BaseResponse<T>
        {
            Description = description,
            StatusCode = StatusCode.Ok
        };
    }

    public BaseResponse<T> BadRequestResponse(string description)
    {
        return new BaseResponse<T>
        {
            Description = description,
            StatusCode = StatusCode.BadRequest
        };
    }

    public BaseResponse<T> InternalServerErrorResponse(string description)
    {
        return new BaseResponse<T>
        {
            Description = description,
            StatusCode = StatusCode.InternalServerError
        };
    }
}

public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCode StatusCode { get; }
    BaseResponse<T> SuccessRequest(string description);
}