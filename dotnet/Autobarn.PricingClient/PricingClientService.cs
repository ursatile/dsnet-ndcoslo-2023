using Autobarn.Messages;
using Autobarn.PricingEngine;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingClient;

public class PricingClientService : IHostedService {
	private readonly string subscriptionId = $"autobarn.pricingclient";
	private readonly ILogger<PricingClientService> logger;
	private readonly IPubSub pubSub;
	private readonly Pricer.PricerClient grpcClient;

	public PricingClientService(ILogger<PricingClientService> logger,
		IPubSub pubSub,
		Pricer.PricerClient grpcClient
		) {
		this.logger = logger;
		this.pubSub = pubSub;
		this.grpcClient = grpcClient;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		await pubSub.SubscribeAsync<NewVehicleMessage>(subscriptionId, HandleNewVehicleMessage);
		logger.LogInformation("Starting PricingClientService...");
	}

	private async Task HandleNewVehicleMessage(NewVehicleMessage message) {
		logger.LogInformation("Handling NewVehicleMessage");
		logger.LogInformation(message.ToString());
		var priceRequest = new PriceRequest {
			Color = message.Color,
			Make = message.Make,
			Year = message.Year,
			Model = message.Model
		};
		var reply = await grpcClient.GetPriceAsync(priceRequest);
		logger.LogInformation($"{reply.Price} {reply.CurrencyCode}");
		var priceMessage = message.WithPrice(reply.Price, reply.CurrencyCode);
		await pubSub.PublishAsync(priceMessage);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping PricingClientService...");
		return Task.CompletedTask;
	}
}
