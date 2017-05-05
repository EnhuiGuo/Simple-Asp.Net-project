﻿$(document).ready(function () {
    var inputs = document.querySelectorAll('.inputfile');
    Array.prototype.forEach.call(inputs, function (input) {
        //event.preventDefault();
        var label = input.nextElementSibling,
            labelVal = label.innerHTML;

        input.addEventListener('change', function (e) {
            var fileName = '';
            if (this.files && this.files.length > 1)
                fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
            else
                fileName = e.target.value.split('\\').pop();

            if (fileName)
                label.querySelector('span').innerHTML = fileName;
            else
                label.innerHTML = labelVal;

            //var myform = document.getElementById("attachment");
            //var formData = new FormData(myform);

            //$.ajax({
            //    type: "POST",
            //    url: "/Video/VideoUpload",
            //    dataType: "json",
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    async: true,
            //    success: function (date) {

            //    }
            //});
            document.getElementById("videoUpload").submit();
        });
    });
});