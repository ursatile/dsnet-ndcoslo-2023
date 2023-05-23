using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Autobarn.Notifier;

public class NotifierService : IHostedService {
	private readonly string subscriptionId = $"autobarn.notifier";
	private readonly ILogger<NotifierService> logger;
	private readonly IPubSub pubSub;
	private readonly HubConnection hubConnection;

	public NotifierService(ILogger<NotifierService> logger,
		IPubSub pubSub,
		HubConnection hubConnection
		) {
		this.logger = logger;
		this.pubSub = pubSub;
		this.hubConnection = hubConnection;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Starting NotifierService...");
		await pubSub.SubscribeAsync<NewVehiclePriceMessage>(subscriptionId, HandleNewVehicleMessage);
		await hubConnection.StartAsync(cancellationToken);
		logger.LogDebug($"Connected to SignalR!");
	}

	private async Task HandleNewVehicleMessage(NewVehiclePriceMessage message) {
		logger.LogInformation("Handling NewVehiclePriceMessage");
		const string user = "autobarn.notifier";
		var json = JsonConvert.SerializeObject(message);
		await hubConnection.SendAsync("NotifyAllThePeopleWhoAreOnOurWebsiteNow", user, json);
		logger.LogInformation(message.ToString());
	}

	public async Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping NotifierService...");
		await hubConnection.StopAsync(cancellationToken);
	}
}
