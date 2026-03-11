using System;
using System.Security.Authentication;
using TodoApp.Api.Application.Common.Utilities;

namespace TodoApp.Api.Api.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        } catch(ArgumentException error)
        {
            await HttpJson.ResponseToJson(context, 409, new
            {
                error.Message
            });
        } catch (InvalidCredentialException error)
        {
            await HttpJson.ResponseToJson(context, 401, new
            {
                error.Message
            });
        } catch (KeyNotFoundException error)
        {
            await HttpJson.ResponseToJson(context, 404, new
            {
                error.Message
            });
        } catch(Exception)
        {
            await HttpJson.ResponseToJson(context, 500, new
            {
                Message="Something went wrong"
            });
        }
    }
}
