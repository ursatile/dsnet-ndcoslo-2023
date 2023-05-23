namespace Autobarn.Messages;

public class NewVehiclePriceMessage : NewVehicleMessage {
	public string PricedBy { get; set; }
	public int Price { get; set; }
	public string CurrencyCode { get; set; } = String.Empty;

	public override string ToString() {
		return $"{base.ToString()} (priced by {PricedBy} - {Price} {CurrencyCode})";
	}
}
