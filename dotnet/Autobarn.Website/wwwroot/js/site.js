
function displayNotification(user, message) {
	console.log(user);
	console.log(message);
}

function connectSignalR() {
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DoTheCrazyClownThing", displayNotification);
	conn.start().then(function() {
		console.log("SignalR is running!");
	}).catch(function(err) {
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
