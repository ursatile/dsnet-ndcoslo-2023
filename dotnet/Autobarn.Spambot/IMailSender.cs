using Autobarn.Messages;
using Microsoft.Extensions.Logging;

namespace Autobarn.Spambot;

public interface IMailSender {
	Task SendEmailAboutNewCarForSale(Customer recipient, NewVehiclePriceMessage vehicle);
}

public class SmtpMailSender : IMailSender {
	private readonly ILogger<SmtpMailSender> logger;

	public SmtpMailSender(ILogger<SmtpMailSender> logger) {
		this.logger = logger;
	}

	public Task SendEmailAboutNewCarForSale(Customer recipient, NewVehiclePriceMessage vehicle) {
		logger.LogDebug($"Mailing {recipient.Name} <{recipient.Email}> about {vehicle}");
		return Task.CompletedTask;
	}
}
