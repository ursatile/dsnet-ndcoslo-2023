using EasyNetQ;
using Messages;

var AMQP = "amqps://qzgmkaky:trYaqbz4ntYWDaNZHqCGoHiv-_OJeP38@smart-maroon-turkey.rmq5.cloudamqp.com/qzgmkaky";
var bus = RabbitHutch.CreateBus(AMQP);

int i = 1;
Console.WriteLine("Press any key to publish a message...");
while(true) {
    Console.ReadKey(true);
    var greeting = new Greeting($"Greeting #{i++}");    
    bus.PubSub.Publish(greeting);
    Console.WriteLine($"Published {greeting}");
};

