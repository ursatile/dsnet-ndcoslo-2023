using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Autobarn.Website.Controllers.Api; 

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase {
	private readonly IAutobarnDatabase db;

	public VehiclesController(IAutobarnDatabase db) {
		this.db = db;
	}

	// GET: api/vehicles
	[HttpGet]
	public IEnumerable<Vehicle> Get()
		=> db.ListVehicles();

	[HttpGet("{id}")]
	public Vehicle Get(string id)
		=> db.FindVehicle(id);

	// POST api/vehicles
	[HttpPost]
	public IActionResult Post([FromBody] VehicleDto dto) {
		if (dto.ModelCode == null)
			return BadRequest("Sorry, null model codes are not supported.");
		var vehicleModel = db.FindModel(dto.ModelCode);
		if (vehicleModel == default) {
			return BadRequest($"Sorry, we couldn't find a vehicle model matching {dto.ModelCode}");
		}
		var vehicle = new Vehicle {
			Registration = dto.Registration,
			Color = dto.Color,
			Year = dto.Year,
			VehicleModel = vehicleModel
		};
		db.CreateVehicle(vehicle);
		return Ok(vehicle);
	}

	[HttpPut("{id}")]
	public IActionResult Put(string id, [FromBody] VehicleDto dto) {
		var vehicleModel = db.FindModel(dto.ModelCode);
		var vehicle = new Vehicle {
			Registration = dto.Registration,
			Color = dto.Color,
			Year = dto.Year,
			ModelCode = vehicleModel.Code
		};
		db.UpdateVehicle(vehicle);
		return Ok(dto);
	}

	// DELETE api/vehicles/ABC123
	[HttpDelete("{id}")]
	public IActionResult Delete(string id) {
		var vehicle = db.FindVehicle(id);
		if (vehicle == default) return NotFound();
		db.DeleteVehicle(vehicle);
		return NoContent();
	}
}
