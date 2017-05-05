$(document).ready(function () { });

function initAutocomplete() {
    var autocomplete = new google.maps.places.Autocomplete(
	(document.getElementById('Address')),
	{ types: ['geocode'] });
}