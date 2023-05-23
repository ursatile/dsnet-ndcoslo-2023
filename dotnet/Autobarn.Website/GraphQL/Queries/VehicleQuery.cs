using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;

namespace Autobarn.Website.GraphQL.Queries {
	public sealed class VehicleQuery : ObjectGraphType {
		private readonly IAutobarnDatabase db;

		public VehicleQuery(IAutobarnDatabase db) {
			this.db = db;

			Field<ListGraphType<VehicleGraphType>>("Vehicles")
				.Description("Get all the vehicles in the system")
				.Resolve(GetAllVehicles);
		}

		private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> _)
			=> db.ListVehicles();
	}
}
