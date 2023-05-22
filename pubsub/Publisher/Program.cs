using EasyNetQ;
using Messages;

var AMQP = "amqps://qzgmkaky:trYaqbz4ntYWDaNZHqCGoHiv-_OJeP38@smart-maroon-turkey.rmq5.cloudamqp.com/qzgmkaky";
var bus = RabbitHutch.CreateBus(AMQP);

Console.WriteLine("Press any key to publish a message...");
var i = 0;
while(true) {   
    Thread.Sleep(TimeSpan.FromSeconds(1));
    var greeting = new Greeting($"Greeting #{i++}");
    bus.PubSub.Publish(greeting);
    Console.WriteLine($"Published {greeting}");
}

