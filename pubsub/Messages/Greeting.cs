namespace Messages;
public class Greeting {
	public string Message { get; set; }
	public string MachineName = Environment.MachineName;
	public DateTimeOffset CreatedAt = DateTimeOffset.Now;
	public Greeting(string message) {
		this.Message = message;
	}
	public override string ToString() {
		return $"{Message} from {MachineName} at {CreatedAt:O}";
	}
}
