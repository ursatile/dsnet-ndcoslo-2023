using System.Security;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Autobarn.Messages;

namespace Autobarn.AuditLog;

public class AuditLogService : IHostedService {
	private readonly string subscriptionId = $"autobarn.auditlog@{Environment.MachineName}";
	private readonly ILogger<AuditLogService> logger;
	private readonly IPubSub pubSub;

	public AuditLogService(ILogger<AuditLogService> logger,
		IPubSub pubSub) {
		this.logger = logger;
		this.pubSub = pubSub;
	}
	public async Task StartAsync(CancellationToken cancellationToken) {
		//logger.LogCritical("THIS IS CRITICAL!");
		//logger.LogError("This is an error");
		//logger.LogWarning("This is a warning");
		//logger.LogInformation("This is information");
		//logger.LogDebug("THis is DEBUG level logging");
		//logger.LogTrace("This is a TRACE message");
		await pubSub.SubscribeAsync<NewVehicleMessage>(subscriptionId, HandleNewVehicleMessage);
		logger.LogInformation("Starting AuditLogService...");
	}

	private void HandleNewVehicleMessage(NewVehicleMessage message) {
		logger.LogInformation("Handling NewVehicleMessage");
		logger.LogInformation(message.ToString());
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		logger.LogInformation("Stopping AuditLogService...");
		return Task.CompletedTask;
	}
}
