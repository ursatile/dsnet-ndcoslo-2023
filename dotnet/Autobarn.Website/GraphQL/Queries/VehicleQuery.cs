using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.GraphQL.Queries {
	public sealed class VehicleQuery : ObjectGraphType {
		private readonly IAutobarnDatabase db;

		public VehicleQuery(IAutobarnDatabase db) {
			this.db = db;

			Field<ListGraphType<VehicleGraphType>>("Vehicles")
				.Description("Get all the vehicles in the system")
				.Resolve(GetAllVehicles);

			Field<VehicleGraphType>("Vehicle")
				.Description("Retrieve a single vehicle")
				.Arguments(MakeNonNullStringArgument("registration", "The registration plate of the vehicle we want"))
				.Resolve(GetVehicle);

			Field<ListGraphType<VehicleGraphType>>("VehiclesByColor")
				.Description("Retrieve all vehicles of a specified color")
				.Arguments(MakeNonNullStringArgument("color", "The color of vehicles that we're interested in"))
				.Resolve(GetVehiclesByColor);

		}

		private Vehicle GetVehicle(IResolveFieldContext<object> context) {
			var registration = context.GetArgument<string>("registration");
			return db.FindVehicle(registration);
		}

		private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
			var color = context.GetArgument<string>("color");
			return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
		}

		private QueryArgument MakeNonNullStringArgument(string name, string description)
			=> new QueryArgument<NonNullGraphType<StringGraphType>> {
				Name = name, Description = description
			};

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> _)
			=> db.ListVehicles();
	}
}
