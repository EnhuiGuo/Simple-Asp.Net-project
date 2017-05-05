$(document).ready(function () {

    var markers = [];

    if (sitters != null && sitters.length > 0) {
        var map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: mapLat, lng: mapLong },
            zoom: 12
        });

        var selfContentString = '<div id="content">' +
            '<h5 id="firstHeading" class="firstHeading">你自己</h5>' +
            '</div>';

        var selfInfowindow = new google.maps.InfoWindow({
            content: selfContentString,
            maxWidth: 200
        });

        var selfMarker = new google.maps.Marker({
            map: map,
            position: { lat: mapLat, lng: mapLong },
            draggable: false,
            animation: google.maps.Animation.DROP,
            icon: "/Content/Icon/pink.png"
        });

        selfMarker.addListener('mouseover', function () {
            selfInfowindow.open(map, selfMarker);
            setTimeout(function () { selfInfowindow.close(); }, 5000);
        });

        $.each(sitters, function (i, sitter) {

            var contentString = '<div id="content">' +
                '<h5 id="firstHeading" class="firstHeading">' + sitter.Name + '</h5>' +
                '<div id="bodyContent">' +
                '<p>地址: ' + sitter.Address + '</P>' +
                '<p>微信: ' + sitter.WeChat + '</P>' +
                '<p>经验: ' + sitter.Years + ' 年 </P>' +
                '</div>';

            var infowindow = new google.maps.InfoWindow({
                content: contentString,
                maxWidth: 200
            });

            var marker = new google.maps.Marker({
                map: map,
                position: { lat: sitter.Latitude, lng: sitter.Longitude },
                draggable: false,
                animation: google.maps.Animation.DROP,
                icon: "/Content/Icon/green.png",
                url: "#" + sitter.UserId + "",
            });

            marker.set("id", sitter.UserId);

            markers.push(marker);

            google.maps.event.addListener(marker, 'click', function () {
                $('html, body').animate({
                    scrollTop: $("#" + sitter.UserId + "").offset().top - 50
                }, 500);
                infowindow.open(map, marker);
                setTimeout(function () { infowindow.close(); }, 5000);
            });

            google.maps.event.addListener(marker, 'mouseover', function () {
                marker.setIcon("/Content/Icon/blue.png")
            });

            google.maps.event.addListener(marker, 'mouseout', function () {
                marker.setIcon("/Content/Icon/green.png")
            });
        });
    }

    $('.list-group-item').mouseenter(function () {
        var markerId = $(this).data("sitter-id");
        setMarkerIcon(markerId);
    })
    .mouseleave(function () {
        var markerId = $(this).data("sitter-id");
        setMarkerIconBack(markerId);
    });

    function setMarkerIcon(markerId) {
        if (markers != null) {
            $.each(markers, function (i, marker) {
                if (marker.id == markerId) {
                    marker.setIcon("/Content/Icon/blue.png")
                }
            });
        }
    }

    function setMarkerIconBack(markerId) {
        if (markers != null) {
            $.each(markers, function (i, marker) {
                if (marker.id == markerId) {
                    marker.setIcon("/Content/Icon/green.png")
                }
            });
        }
    }

    $(window).scroll(function () {
        $('#mapDiv').width($('#sitterListDiv').width());
    });

    $(window).resize(function () {
        $('#mapDiv').width($('#sitterListDiv').width());
    });
});