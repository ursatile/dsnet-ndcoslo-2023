using Autobarn.PricingClient;
using Autobarn.PricingEngine;
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
	var grpcUrl = context.Configuration["AutobarnPricingServerUrl"]!;
	services.AddGrpcClient<Pricer.PricerClient>(o
		=> o.Address = new Uri(grpcUrl));
	var amqp = context.Configuration.GetConnectionString("RabbitMQ");
	var bus = RabbitHutch.CreateBus(amqp);
	services.AddSingleton(bus.PubSub);
	services.AddHostedService<PricingClientService>();
});

var host = builder.Build();
await host.RunAsync();
