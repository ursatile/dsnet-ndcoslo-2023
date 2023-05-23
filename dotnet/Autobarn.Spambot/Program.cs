using Autobarn.Spambot;
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
		var db = new FakeCustomerDatabase();
		services.AddSingleton<ICustomerDatabase>(db);
		services.AddSingleton<IMailSender, SmtpMailSender>();
		services.AddSingleton(bus.PubSub);
		services.AddHostedService<SpambotService>();
	});

var host = builder.Build();
await host.RunAsync();
