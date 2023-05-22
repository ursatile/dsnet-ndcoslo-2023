using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Data;
using EasyNetQ;
using EasyNetQ.Internals;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Autobarn.Website.Tests;

public class FakePubSub : IPubSub {
	private readonly ArrayList messages = new();
	public ArrayList Messages => messages;

	public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = new CancellationToken()) {
		messages.Add(message);
		return Task.CompletedTask;
	}

	public AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure,
		CancellationToken cancellationToken = new CancellationToken()) {
		throw new NotImplementedException();
	}
}
public class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class {
	public FakePubSub PubSub { get; set; } = new FakePubSub();

	protected override void ConfigureWebHost(IWebHostBuilder builder) {
		builder.ConfigureServices(services => {
			services.RemoveAll(typeof(IAutobarnDatabase));
			services.AddSingleton<IAutobarnDatabase, AutobarnCsvFileDatabase>();
			services.RemoveAll(typeof(IPubSub));
			services.AddSingleton<IPubSub>(PubSub);
		});
	}
}
