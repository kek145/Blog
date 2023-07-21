﻿namespace BlogAPI.Domain.Enum;

public enum StatusCode
{
    Ok = 200,
    Created = 201,
    NoContent = 204,
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}