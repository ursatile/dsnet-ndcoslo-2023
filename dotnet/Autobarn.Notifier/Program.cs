using Autobarn.Notifier;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder()
	.ConfigureLogging((_, logging) => {
		logging.ClearProviders();
		logging.AddConsole();
	})
	.ConfigureServices((context, services) => {
		var hubUrl = context.Configuration["SignalRHubUrl"];
		var hub = new HubConnectionBuilder().WithUrl(hubUrl).Build();
		var amqp = context.Configuration.GetConnectionString("RabbitMQ");
		var bus = RabbitHutch.CreateBus(amqp);
		services.AddSingleton(bus.PubSub);
		services.AddSingleton(hub);
		services.AddHostedService<NotifierService>();
	});

var host = builder.Build();
await host.RunAsync();
