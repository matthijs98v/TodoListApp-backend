using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Api.Api.Hubs;
using TodoApp.Api.Application.Common.Utilities;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Application.Services;
using TodoApp.Api.Infrastructure.Data;
using TodoApp.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSignalR();

// Register services and repository
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoListService, TodoListService>();
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<IListMemberService, ListMemberService>();
builder.Services.AddScoped<IListMemberRepository, ListMemberRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
        };

        // Check the cookie first
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["Authorization"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

// For the migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowFrontend");
}

app.UseAuthorization();

app.MapControllers();
app.MapHub<TodoHub>("/todoHub");

// Middleware for error handeling
app.Use(async (context, next) =>
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
});

app.Run();
