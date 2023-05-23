using Autobarn.Messages;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;
using MailKit.Security;

namespace Autobarn.Spambot;

public interface IMailSender {
	Task SendEmailAboutNewCarForSale(Customer recipient, NewVehiclePriceMessage message);
}

public class SmtpMailSender : IMailSender {
	private readonly ILogger<SmtpMailSender> logger;

	public SmtpMailSender(ILogger<SmtpMailSender> logger) {
		this.logger = logger;
	}

	public async Task SendEmailAboutNewCarForSale(Customer recipient, NewVehiclePriceMessage message) {
		logger.LogDebug($"Mailing {recipient.Name} <{recipient.Email}> about {message}");
		var email = new MimeMessage();
		email.From.Add(MailboxAddress.Parse("hello@autobarn.com"));
		email.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
		email.Subject = $"New car - {message.Make} {message.Model}, {message.Color}, {message.Year}";
		email.Body = BuildMailBody(recipient, message);
		using var smtp = new SmtpClient();
		await smtp.ConnectAsync("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);
		await smtp.AuthenticateAsync("1a7576ce41ac3f", "4f473bb8e09e09");
		await smtp.SendAsync(email);
		await smtp.DisconnectAsync(true);
	}

	private const string TEMPLATE = @"
		<p>Dear __NAME__</p>
		<p>There's a new car!</p>
		<p>__REGISTRATION__</p>
		<p>__MAKE__ __MODEL__ (__COLOR__, __YEAR__)</p>
		<p><strong>__PRICE__ __CURRENCY_CODE__</strong></p>
		<p><a href=""https://workshop.ursatile.com:5001/vehicles/details/__REGISTRATION__"">click here to see it</a></p>
	";
	public TextPart BuildMailBody(Customer recipient, NewVehiclePriceMessage message) {
		var tokens = new Dictionary<string, string> {
			{"__NAME__", recipient.Name},
			{"__REGISTRATION__", message.Registration},
			{"__MAKE__", message.Make},
			{"__MODEL__", message.Model},
			{"__COLOR__", message.Color ?? "(not supplied)"},
			{"__YEAR__", message.Year.ToString()},
			{"__PRICE__", message.Price.ToString()},
			{"__CURRENCY_CODE__", message.CurrencyCode}
		};
		var html = tokens.Aggregate(TEMPLATE,
			(current, token) => current.Replace(token.Key, token.Value));
		return new TextPart(TextFormat.Html) { Text = html };
	}
}
