using System;
using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.Api;
using Autobarn.Website.Models;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Api;

public class VehiclesControllerTests {

	[Fact]
	public void GET_Vehicle_By_Id_Works() {
		var registration = "TESTCAR1";
		var db = new FakeDatabase();
		var c = new VehiclesController(db, null);
		var result = c.Get(registration);
		result.Registration.ShouldBe(registration);
	}

	[Fact]
	public void PUT_Vehicle_Updates_Vehicle() {
		var registration = "TESTCAR1";
		var db = new FakeDatabase();
		var c = new VehiclesController(db, null);
		var dto = new VehicleDto() {
			Color = "Green"
		};
		c.Put(registration, dto);
		db.Vehicles.Count.ShouldBe(1);
		db.Vehicles.ShouldContain(v => v.Color == "Green");
	}
}

public class FakeDatabase : IAutobarnDatabase {

	private List<Vehicle> vehicles = new();

	public List<Vehicle> Vehicles => vehicles;

	public int CountVehicles() => throw new NotImplementedException();

	public IEnumerable<Vehicle> ListVehicles() => throw new NotImplementedException();

	public IEnumerable<Manufacturer> ListManufacturers() => throw new NotImplementedException();

	public IEnumerable<Model> ListModels() => throw new NotImplementedException();

	public Vehicle FindVehicle(string registration)
		=> new() {Registration = registration};

	public Model FindModel(string code) => new();

	public Manufacturer FindManufacturer(string code) => throw new NotImplementedException();

	public void CreateVehicle(Vehicle vehicle) => throw new NotImplementedException();

	public void UpdateVehicle(Vehicle vehicle) =>
		vehicles.Add(vehicle);

	public void DeleteVehicle(Vehicle vehicle) => throw new NotImplementedException();
}
