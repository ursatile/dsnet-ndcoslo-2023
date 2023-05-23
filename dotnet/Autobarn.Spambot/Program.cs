using Autobarn.Spambot;
using EasyNetQ;
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
	var amqp = context.Configuration.GetConnectionString("RabbitMQ");
	var smtpConfig = new SmtpConfig();
	context.Configuration.Bind("Smtp", smtpConfig);
	var bus = RabbitHutch.CreateBus(amqp);
	var db = new FakeCustomerDatabase();
	services.AddSingleton<ICustomerDatabase>(db);
	services.AddSingleton<IMailSender>(svc => {
		var logger = svc.GetService<ILogger<SmtpMailSender>>()!;
		return new SmtpMailSender(smtpConfig, logger);
	});
	services.AddSingleton(bus.PubSub);
	services.AddHostedService<SpambotService>();
});

var host = builder.Build();
await host.RunAsync();
