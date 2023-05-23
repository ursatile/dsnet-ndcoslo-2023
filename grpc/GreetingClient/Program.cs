using Grpc.Net.Client;
using Greetings;

const string GRPC_SERVER_URL = "https://workshop.ursatile.com:5006";
using var channel = GrpcChannel.ForAddress(GRPC_SERVER_URL);
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while(true) {
    var languageCode = Console.ReadKey(true).KeyChar switch {
           '1' => "nn-NO",
           '2' => "en-GB",
           '3' => "en-US",
           '4' => "en-AU",
           '5' => "is-IS",
           '6' => "it-IT",
           '7' => "ja-JP",
           '8' => "ko-KO",
           '9' => "sk-SK",
           _ => String.Empty
    };
    var request = new HelloRequest {
        LanguageCode = languageCode,
        Name = "NDC Oslo"
    };
    var reply = await client.SayHelloAsync(request);
    Console.WriteLine(reply);    
}

