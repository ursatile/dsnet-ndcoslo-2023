using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.Notifier;

public class NotifierService : IHostedService {
	private readonly string subscriptionId = $"autobarn.notifier";
	private readonly ILogger<NotifierService> logger;
	private readonly IPubSub pubSub;

	public NotifierService(ILogger<NotifierService> logger,
		IPubSub pubSub) {
		this.logger = logger;
		this.pubSub = pubSub;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		await pubSub.SubscribeAsync<NewVehiclePriceMessage>(subscriptionId, HandleNewVehicleMessage);
		logger.LogInformation("Starting NotifierService...");
	}

	private void HandleNewVehicleMessage(NewVehiclePriceMessage message) {
		logger.LogInformation("Handling NewVehiclePriceMessage");
		logger.LogInformation(message.ToString());
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping NotifierService...");
		return Task.CompletedTask;
	}
}
