using System;
using System.Text.Json;

namespace TodoApp.Api.Application.Common.Utilities;

public class HttpJson
{
    public static async Task ResponseToJson(HttpContext context, int statusCode, object Object)
    {
        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(Object));
    }
}
