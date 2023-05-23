using Autobarn.PricingServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

static Action<ListenOptions> UseCertIfAvailable(string pfxFilePath, string pfxPassword) {
	if (File.Exists(pfxFilePath)) return listen => listen.UseHttps(pfxFilePath, pfxPassword);
	return listen => listen.UseHttps();
}

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
	var pfxPassword = Environment.GetEnvironmentVariable("UrsatilePfxPassword");
	var https = UseCertIfAvailable(@"D:\Dropbox\workshop.ursatile.com\workshop.ursatile.com.pfx", pfxPassword);
	options.ListenAnyIP(5002, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
	options.Listen(IPAddress.Any, 5003, https);
});

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<PricerService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
