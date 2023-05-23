using Grpc.Net.Client;
using Greetings;

const string GRPC_SERVER_URL = "https://workshop.ursatile.com:5006";
using var channel = GrpcChannel.ForAddress(GRPC_SERVER_URL);
var client = new Greeter.GreeterClient(channel);

Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while(true) {
    Console.ReadKey(true);
    var request = new HelloRequest {
        Name = "NDC Oslo"
    };
    var reply = await client.SayHelloAsync(request);
    Console.WriteLine(reply);    
}

