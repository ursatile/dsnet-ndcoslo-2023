
function displayNotification(user, message) {
	const target = document.getElementById('signalr-notifications');
	const data = JSON.parse(message);
	const div = document.createElement("DIV");
	div.innerHTML = `<p>${data.Make} ${data.Model} (${data.Color}, ${data.Year})</p>
<p>Price: ${data.Price} ${data.CurrencyCode}</p>
<hr />
<a href="/vehicles/details/${data.Registration}">Click for details...</a>`;
	div.style.backgroundColor = data.Color;
	target.append(div);
	window.setTimeout(function () {
		div.classList.add("hide");
		window.setTimeout(function () {
			div.parentNode.removeChild(div);
		}, 1000);
	}, 5000);
}

function connectSignalR() {
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DoTheCrazyClownThing", displayNotification);
	conn.start().then(function () {
		console.log("SignalR is running!");
	}).catch(function (err) {
		;
		console.log("SignalR Failed");
		console.error(err);
	});
}

function ready(fn) {
	if (document.readyState !== 'loading') {
		fn();
	} else {
		document.addEventListener('DOMContentLoaded', fn);
	}
}

ready(connectSignalR);
