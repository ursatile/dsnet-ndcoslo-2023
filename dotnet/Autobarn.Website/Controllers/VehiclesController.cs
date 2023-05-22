using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Autobarn.Website.Controllers; 

public class VehiclesController : Controller {
	private readonly IAutobarnDatabase db;
	private readonly IPubSub pubSub;

	public VehiclesController(IAutobarnDatabase db, IPubSub pubSub) {
		this.db = db;
		this.pubSub = pubSub;
	}
	public IActionResult Index() {
		var vehicles = db.ListVehicles();
		return View(vehicles);
	}

	public IActionResult Details(string id) {
		var vehicle = db.FindVehicle(id);
		return View(vehicle);
	}

	[HttpGet]
	public IActionResult Advertise(string id) {
		var vehicleModel = db.FindModel(id);
		var dto = new VehicleDto() {
			ModelCode = vehicleModel.Code,
			ModelName = $"{vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
		};
		return View(dto);
	}

	[HttpPost]
	public IActionResult Advertise(VehicleDto dto) {
		var existingVehicle = db.FindVehicle(dto.Registration);
		if (existingVehicle != default)
			ModelState.AddModelError(nameof(dto.Registration),
				"That registration is already listed in our database.");
		var vehicleModel = db.FindModel(dto.ModelCode);
		if (vehicleModel == default) {
			ModelState.AddModelError(nameof(dto.ModelCode),
				$"Sorry, {dto.ModelCode} is not a valid model code.");
		}
		if (!ModelState.IsValid) return View(dto);
		var vehicle = new Vehicle() {
			Registration = dto.Registration,
			Color = dto.Color,
			VehicleModel = vehicleModel,
			Year = dto.Year
		};
		PublishNewVehicleNotification(vehicle);
		db.CreateVehicle(vehicle);
		return RedirectToAction("Details", new { id = vehicle.Registration });
	}

	void PublishNewVehicleNotification(Vehicle vehicle) {
		var message = new NewVehicleMessage {
			Registration = vehicle.Registration,
			Color = vehicle.Color,
			ListedAt = DateTimeOffset.UtcNow,
			Make = vehicle?.VehicleModel?.Manufacturer?.Name ?? "missing!",
			Model = vehicle?.VehicleModel?.Name ?? "missing!",
			Year = vehicle.Year
		};
		pubSub.Publish(message);
	}
}
