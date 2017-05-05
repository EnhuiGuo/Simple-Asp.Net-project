$(document).ready(function () {

    $("#topSearchBtn").click(function () {
        if ($("#topAddressBar").val() == "") {
            $.alert({
                icon: "glyphicon glyphicon-warning-sign",
                title: false,
                content: "请输入地址或者邮编号码",
                animation: 'scaleY',
                closeAnimation: 'scaleX',
                theme: 'black'
            });
        }
        else {
            window.location.href = "/Sitter/Search";
        }
    });

});

function initAutocomplete() {
    var autocomplete = new google.maps.places.Autocomplete(
    (document.getElementById('topAddressBar')),
    { types: ['geocode'] });
}