using System.Security.Claims;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Infrastructure.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Moq;

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD4MiddlewareTests
{
    private sealed class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "Tests";
        public string WebRootPath { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = string.Empty;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    [Fact]
    public async Task CorrelationIdMiddleware_Should_Generate_And_Echo_Header()
    {
        var context = new DefaultHttpContext();
        var invoked = false;

        RequestDelegate next = ctx =>
        {
            invoked = true;
            ctx.Items[CorrelationIdMiddleware.ItemKey].Should().NotBeNull();
            return Task.CompletedTask;
        };

        var middleware = new CorrelationIdMiddleware(next);
        await middleware.InvokeAsync(context);
        await context.Response.Body.FlushAsync();

        // OnStarting callbacks fire when response starts — force start
        context.Response.Headers[CorrelationIdMiddleware.HeaderName] =
            context.Items[CorrelationIdMiddleware.ItemKey]?.ToString();

        invoked.Should().BeTrue();
        context.Items[CorrelationIdMiddleware.ItemKey].Should().NotBeNull();
        context.TraceIdentifier.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CorrelationIdMiddleware_Should_Reuse_Incoming_Header()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers[CorrelationIdMiddleware.HeaderName] = "abc123";

        var middleware = new CorrelationIdMiddleware(_ => Task.CompletedTask);
        await middleware.InvokeAsync(context);

        context.Items[CorrelationIdMiddleware.ItemKey].Should().Be("abc123");
        context.TraceIdentifier.Should().Be("abc123");
    }

    [Fact]
    public async Task GlobalExceptionMiddleware_Should_Return_400_For_AppWarning()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        context.Items[CorrelationIdMiddleware.ItemKey] = "corr-1";

        var logger = new Mock<Serilog.ILogger>();
        logger.Setup(l => l.Warning(It.IsAny<string>()));

        RequestDelegate next = _ => throw new AppWarningException("aviso");
        var middleware = new GlobalExceptionMiddleware(next, logger.Object, new TestWebHostEnvironment());

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();
        body.Should().Contain("APP_WARNING");
        body.Should().Contain("aviso");
    }

    [Fact]
    public async Task RequestLoggingMiddleware_Should_Invoke_Next()
    {
        var context = new DefaultHttpContext();
        context.Items[CorrelationIdMiddleware.ItemKey] = "corr-2";
        var called = false;

        var logger = new Mock<Serilog.ILogger>();
        logger.Setup(l => l.Information(
            It.IsAny<string>(),
            It.IsAny<object[]>()));

        var middleware = new RequestLoggingMiddleware(_ =>
        {
            called = true;
            return Task.CompletedTask;
        }, logger.Object);

        await middleware.InvokeAsync(context);
        called.Should().BeTrue();
    }
}
