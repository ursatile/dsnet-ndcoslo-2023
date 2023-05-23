using Autobarn.Messages;
using static System.Net.Mime.MediaTypeNames;

namespace Autobarn.Spambot;

public interface IMailSender {
	Task SendEmailAboutNewCarForSale(Customer recipient, NewVehiclePriceMessage message);
}

public class SmtpConfig {
	public string Host { get; set; }
	public int Port { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
}
