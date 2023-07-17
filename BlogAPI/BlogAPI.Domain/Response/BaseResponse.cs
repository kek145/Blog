﻿using BlogAPI.Domain.Enum;

namespace BlogAPI.Domain.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
    public string Description { get; set; }
    public StatusCode StatusCode { get; set; }
}

public interface IBaseResponse<T>
{
    string Description { get; }
    StatusCode StatusCode { get; }
}