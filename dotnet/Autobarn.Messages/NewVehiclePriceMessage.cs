namespace Autobarn.Messages;

public class NewVehiclePriceMessage : NewVehicleMessage {
	public int Price { get; set; }
	public string CurrencyCode { get; set; } = String.Empty;

	public override string ToString() {
		return $"{base.ToString()} ({Price} {CurrencyCode})";
	}
}
