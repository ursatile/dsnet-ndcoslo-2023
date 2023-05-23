using Grpc.Core;
using Greetings;

namespace GreetingServer.Services;

public class GreeterService : Greeter.GreeterBase {
	private readonly ILogger<GreeterService> _logger;
	public GreeterService(ILogger<GreeterService> logger) {
		_logger = logger;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        
        var name = $"{request.LastName.ToUpperInvariant()}, {request.FirstName}";
        
        var greeting = request.LanguageCode switch {
            "nn-NO" => $"Hei {name}",
            "en-GB" => $"Good morning, {name}",
            "en-US" => $"Howdy, {name}",
            "en-AU" => $"G'day, {name}!",
            "is-IS" => $"Halló {name}",
            "it-IT" => $"Buongiorno  {name}",
            "ja-JP" => $"こんにちわ {name}",
            "ko-KO" => $"안녕하세요 {name}",
            _ => $"Greeting, {name}"
        };

		_logger.LogDebug("Received a HelloRequest");
        _logger.LogDebug($"It's from {name}");
		return Task.FromResult(new HelloReply {
			Message = greeting,
            From = "Dylan's gRPC Server"
		});
	}
}
