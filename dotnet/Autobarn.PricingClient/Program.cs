using Autobarn.PricingClient;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder()
	.ConfigureLogging((hostingContext, logging) => {
		logging.ClearProviders();
		logging.AddConsole();
	})
.ConfigureServices((context, services) => {
	var amqp = context.Configuration.GetConnectionString("RabbitMQ");
	var bus = RabbitHutch.CreateBus(amqp);
	services.AddSingleton(bus.PubSub);
	services.AddHostedService<PricingClientService>();
});

var host = builder.Build();
await host.RunAsync();
