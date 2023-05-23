using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Spambot;

public class SpambotService : IHostedService {
	private readonly string subscriptionId = $"autobarn.notifier";
	private readonly ILogger<SpambotService> logger;
	private readonly IPubSub pubSub;
	private readonly ICustomerDatabase customerDb;
	private readonly IMailSender mailSender;

	public SpambotService(ILogger<SpambotService> logger,
		IPubSub pubSub,
		ICustomerDatabase customerDb,
		IMailSender mailSender) {
		this.logger = logger;
		this.pubSub = pubSub;
		this.customerDb = customerDb;
		this.mailSender = mailSender;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		await pubSub.SubscribeAsync<NewVehiclePriceMessage>(subscriptionId, HandleNewVehicleMessage);
		logger.LogInformation("Starting NotifierService...");
	}

	private async Task HandleNewVehicleMessage(NewVehiclePriceMessage message) {
		logger.LogInformation("Handling NewVehiclePriceMessage");
		var recipients = customerDb.GetCustomersInterestedInVehicle(message);
		foreach (var recipient in recipients) {
			logger.LogDebug($"Emailing {recipient.Email} about {message}");
			await mailSender.SendEmailAboutNewCarForSale(recipient, message);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping NotifierService...");
		return Task.CompletedTask;
	}
}
