using Autobarn.Data;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Autobarn.Website.GraphQL.GraphTypes;
using Autobarn.Website.GraphQL.Schemas;
using Autobarn.Website.Hubs;
using GraphiQl;
using GraphQL;

namespace Autobarn.Website;

public class Startup {
	protected virtual string DatabaseMode => Configuration["DatabaseMode"];

	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services) {
		services.AddRouting(options => options.LowercaseUrls = true);
		services.AddControllersWithViews().AddNewtonsoftJson();

		var amqp = Configuration.GetConnectionString("RabbitMQ");
		var bus = RabbitHutch.CreateBus(amqp);

		services.AddSingleton(bus.PubSub);
		services.AddSwaggerGen();
		services.AddSwaggerGenNewtonsoftSupport();

		services.AddGraphQL(builder =>
			builder.AddNewtonsoftJson()
				.AddSchema<AutobarnSchema>()
				.AddGraphTypes(typeof(VehicleGraphType).Assembly));

		services.AddSignalR();
		services.AddRazorPages().AddRazorRuntimeCompilation();
		Console.WriteLine(DatabaseMode);
		switch (DatabaseMode) {
			case "sql":
				var sqlConnectionString = Configuration.GetConnectionString("AutobarnSqlConnectionString");
				services.UseAutobarnSqlDatabase(sqlConnectionString);
				break;
			default:
				services.AddSingleton<IAutobarnDatabase, AutobarnCsvFileDatabase>();
				break;
		}
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		} else {
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}
		app.UseHttpsRedirection();
		app.UseDefaultFiles();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthorization();

		// Add endpoints for exposing the Swagger JSON document describing our API:
		app.UseSwagger();
		// ...and the SwaggerUI interactive API tooling.
		app.UseSwaggerUI();

		app.UseGraphQL<AutobarnSchema>();
		app.UseGraphiQl("/graphiql");

		app.UseEndpoints(endpoints => {
			endpoints.MapHub<AutobarnHub>("/hub");
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
		});
	}
}
