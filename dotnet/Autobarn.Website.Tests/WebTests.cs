using System.Net.Http.Json;
using Autobarn.Messages;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests; 

public class WebTests : IClassFixture<TestWebApplicationFactory<Startup>> {
	private readonly TestWebApplicationFactory<Startup> factory;

	public WebTests(TestWebApplicationFactory<Startup> factory) {
		this.factory = factory;
	}

	[Fact]
	public async void WebsiteWorks() {
		var client = factory.CreateClient();
		var response = await client.GetAsync("/");
		response.EnsureSuccessStatusCode();
	}
}

public class ApiTests : IClassFixture<TestWebApplicationFactory<Startup>> {
	private readonly TestWebApplicationFactory<Startup> factory;

	public ApiTests(TestWebApplicationFactory<Startup> factory) {
		this.factory = factory;
	}
	[Fact]
	public async void POST_New_Vehicle_Publishes_Message() {
		var client = factory.CreateClient();
		var post = new {
			registration = "NDCOSLO2",
			modelCode = "dmc-delorean",
			color = "Blue",
			year = 2023
		};

		factory.PubSub.Messages.Count.ShouldBe(0);
		var response = await client.PostAsJsonAsync("/api/vehicles", post);
		response.EnsureSuccessStatusCode();
		factory.PubSub.Messages.Count.ShouldBe(1);
		var message = (NewVehicleMessage) factory.PubSub.Messages[0];
		message.Make.ShouldBe("DMC");
	}
}
