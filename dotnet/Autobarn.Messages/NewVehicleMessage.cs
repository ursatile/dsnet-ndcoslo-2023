namespace Autobarn.Messages;

public class NewVehicleMessage {
	public DateTimeOffset ListedAt { get; set; } = DateTimeOffset.UtcNow;
	public string Registration { get; set; } = String.Empty;
	public string Make { get; set; } = String.Empty;
	public string Model { get; set; } = String.Empty;
	public int Year { get; set; }
	public string? Color { get; set; }
	public override string ToString()
		=> $"{Registration} {Make} {Model} ({Color}, {Year} at {ListedAt}";

	public NewVehiclePriceMessage WithPrice(int price, string currencyCode)
		=> new() {
			Registration = this.Registration,
			Make = this.Make,
			Color = this.Color,
			Year = this.Year,
			ListedAt = this.ListedAt,
			Model = this.Model,
			CurrencyCode = currencyCode,
			Price = price
		};
}
