$(document).ready(function () {
	var map = new google.maps.Map(document.getElementById('map'), {
		center: { lat: mapLat, lng: mapLong },
		zoom: 12
	});

	var selfMarker = new google.maps.Marker({
		map: map,
		position: { lat: mapLat, lng: mapLong },
		draggable: false,
		animation: google.maps.Animation.DROP,
		icon: "/Content/Icon/pink.png"
	});
});