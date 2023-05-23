using Grpc.Core;
using Greetings;

namespace GreetingServer.Services;

public class GreeterService : Greeter.GreeterBase {
	private readonly ILogger<GreeterService> _logger;
	public GreeterService(ILogger<GreeterService> logger) {
		_logger = logger;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        var greeting = request.LanguageCode switch {
            "nn-NO" => $"Hei {request.Name}",
            "en-GB" => $"Good morning, {request.Name}",
            "en-US" => $"Howdy, {request.Name}",
            "en-AU" => $"G'day, {request.Name}!",
            "is-IS" => $"Halló {request.Name}",
            "it-IT" => $"Buongiorno  {request.Name}",
            "ja-JP" => $"こんにちわ {request.Name}",
            "ko-KO" => $"안녕하세요 {request.Name}",
            _ => $"Greeting, {request.Name}"
        };

		_logger.LogDebug("Received a HelloRequest");
		return Task.FromResult(new HelloReply {
			Message = greeting,
            From = "Dylan's gRPC Server"
		});
	}
}
