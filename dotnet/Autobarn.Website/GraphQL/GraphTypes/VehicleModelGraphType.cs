using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes;

public sealed class VehicleModelGraphType : ObjectGraphType<Model> {
	public VehicleModelGraphType() {
		Name = "model";
		Field(m => m.Name);
		Field(m => m.Code);
		Field(m => m.Manufacturer,
			type: typeof(ManufacturerGraphType),
			nullable: false).Description("The company who makes this model of car");
	}
}
