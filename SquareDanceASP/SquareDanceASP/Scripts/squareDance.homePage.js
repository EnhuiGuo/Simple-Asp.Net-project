$(document).ready(function () {

    $('#topAddressBarDiv').hide();

    $("#searchBtn").click(function () {
        if ($("#addressBar").val() == "") {
            $.alert({
                icon: "glyphicon glyphicon-warning-sign",
                title: false,
                content: "请输入地址或者邮编号码",
                animation: 'scaleY',
                closeAnimation: 'scaleX',
                theme: 'black'
            });
            return false;
        }
        else {
            return true;
        }
    });

    $('#addressBar').focus(function () {
        var autocomplete = new google.maps.places.Autocomplete(
            (document.getElementById('addressBar')),
            { types: ['geocode'] });
    });
});