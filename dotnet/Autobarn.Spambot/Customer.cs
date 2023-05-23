using System.Xml.Linq;

public class Customer {
	public Customer(string name) {
		Name = name;
	}
	public string Name { get; set; } = String.Empty;
	public string Email => Name.ToLowerInvariant().Replace(" ", ".") + "@example.com";
}
