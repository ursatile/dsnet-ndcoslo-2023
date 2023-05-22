using EasyNetQ;
using Messages;

var AMQP = "amqps://qzgmkaky:trYaqbz4ntYWDaNZHqCGoHiv-_OJeP38@smart-maroon-turkey.rmq5.cloudamqp.com/qzgmkaky";
var bus = RabbitHutch.CreateBus(AMQP);

var subscriptionId = "dylan-delete-queue";

var sub = bus.PubSub.Subscribe<Greeting>(
    subscriptionId, 
    HandleGreeting    
);

Console.WriteLine("Listening for messages. Press Enter to exit.");
Console.ReadLine();

void HandleGreeting(Greeting greeting) {
	if (greeting.Message.EndsWith("5")) {
		throw new Exception("EVIL NUMBER DETECTED!");
	}
	Console.WriteLine(greeting);
}
