using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient;

public class PricingClientService : IHostedService {
	private readonly string subscriptionId = $"autobarn.pricingclient";
	private readonly ILogger<PricingClientService> logger;
	private readonly IPubSub pubSub;

	public PricingClientService(ILogger<PricingClientService> logger,
		IPubSub pubSub) {
		this.logger = logger;
		this.pubSub = pubSub;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		await pubSub.SubscribeAsync<NewVehicleMessage>(subscriptionId, HandleNewVehicleMessage);
		logger.LogInformation("Starting PricingClientService...");
	}

	private void HandleNewVehicleMessage(NewVehicleMessage message) {
		logger.LogInformation("Handling NewVehicleMessage");
		logger.LogInformation(message.ToString());
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping PricingClientService...");
		return Task.CompletedTask;
	}
}
