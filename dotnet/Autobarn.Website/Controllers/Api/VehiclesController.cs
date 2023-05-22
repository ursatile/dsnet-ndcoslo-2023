using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;

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
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Vehicle))]

	// Swagger generates one response per code
	// The last one wins, and if you want an empty response body,
	// you have to use the typeof(void) as the first parameter.
	// [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(void))]
	[ProducesResponseType(typeof(void), StatusCodes.Status409Conflict)]
	public IActionResult Post([FromBody] VehicleDto dto) {
		if (dto.ModelCode == null)
			return BadRequest("Sorry, null model codes are not supported.");
		var vehicleModel = db.FindModel(dto.ModelCode);
		if (vehicleModel == default) {
			return BadRequest($"Sorry, we couldn't find a vehicle model matching {dto.ModelCode}");
		}

		var existing = db.FindVehicle(dto.Registration);
		if (existing != default)
			return Conflict(
				$"Sorry, the registration {dto.Registration} already exists in our database and you can't sell the same car twice!");
		var vehicle = new Vehicle {
			Registration = dto.Registration,
			Color = dto.Color,
			Year = dto.Year,
			VehicleModel = vehicleModel
		};
		db.CreateVehicle(vehicle);
		return Created($"/api/vehicles/{dto.Registration}", vehicle);
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
