$(document).ready(function () {

    var validatedFiles = [];

	$('#addPetBtn').on('click', function () {
	    window.location.href = "/Account/AddDog";
	});

	$('#saveItemBtn').on('click', function () {

	    var data = new FormData();

	    for (var x = 0; x < validatedFiles.length; x++) {
	        data.append("file" + x, validatedFiles[x]);
	    }

	    var model = {
	        "UserId": id,
	        "Name": $('#picName').val(),
	        "Description": $('#picDescription').val()
	    }

	    $.ajax({
	        type: "POST",
	        url: "/Account/UploadItem",
	        contentType: false,
	        processData: false,
	        data: data,
	        success: function (result) {
	            console.log(result);
	        },
	    })
	});

	$('#profileImageForm').submit(function () {

	    if ($('#fileUpload').val() == "")
	    {
	        $('#imageError').html("请上传一张照片");
	        return false;
	    }
	    return true;
	});

	$('#becomeSitterBtn').on('click', function () {
	    window.location.href = "/Sitter/Services";
	});

	$(document).on('change', ':file', function () {
	    validatedFiles = [];
	    var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
	    input.trigger('fileselect', [numFiles, label]);

	    var files = input.get(0).files;

	    if (files.length > 0) {
	        for (i = 0; i < files.length; i++) {
	            if (files[i].type.match('image.*')) {
	                var inList = false;
	                validatedFiles.forEach(function (file) {
	                    if (file.name == files[i].name)
	                    {
	                        inList = true;
	                    }
	                })
	                if (!inList)
	                {
	                    if (files[i].size < 1000000) {
	                        validatedFiles.push(files[i]);
	                        saveFileToPage(files[i]);
	                        $('#imageError').html("");
	                    }
	                    else {
	                        $('#imageError').html(files[i].name + "太大, 不能上传");
	                        $('#updateImage').html("");
	                        $('#fileUpload').val("");
	                    }
	                }
	            }
	            else {
	                $.alert({
	                    icon: "glyphicon glyphicon-warning-sign",
	                    title: false,
	                    content: "文件必须是图片格式",
	                    animation: 'scaleY',
	                    closeAnimation: 'scaleX',
	                    theme: 'black'
	                });
	                $('#imgGallery').html("");
	                $('#fileUpload').val('');
	                $('#picName').val('');
	                break;
	            }
	        }
	        showLabel();
	    }
	});

	$('#uploadImageBtn').on('click', function () {
	    $('#fileUpload').val('');
	});

	$("#editUserModal").on("shown.bs.modal", function () {
	    var input = document.getElementById("Address");
	    var autocomplete = new google.maps.places.Autocomplete(input);
	});

	deleteImage = function (deleteBtn) {
	    $(deleteBtn).parent().remove();
	    var name = $(deleteBtn).data("image");
	    validatedFiles.forEach(function (file, i) {
	        if (file.name == name) {
	            validatedFiles.splice(i, 1);
	        }
	    });
	    showLabel();
	    console.log(validatedFiles.length);
	}

	function saveFileToPage(file) {
	    openFile = function (file) {
	        var reader = new FileReader();
	        reader.onload = function (event) {
	            var dataURL = event.target.result.toString();
	            var data = dataURL.substr(dataURL.indexOf(",") + 1);
	            var name = file.name;
	            var model = {
	                "Name": name,
                    "Data": data,
	            };

	            $.ajax({
	                type: "POST",
	                url: "/Account/_ItemImage",
	                contentType: "application/json",
	                data: JSON.stringify(model),
	                success: function (result) {
	                    $('#updateImage').html(result);
	                }
	            });
	        };
	        reader.readAsDataURL(file);
	    };
	    var data = openFile(file);
	}

	function showLabel()
	{
	    var input = $('#picGroup').find(':text');

	    if (validatedFiles.length > 0) {
	        var log = validatedFiles.length > 1 ? validatedFiles.length + ' files selected' : validatedFiles[0].name;

	        if (input.length) {
	            input.val(log);
	        } else {
	            if (log) alert(log);
	        }
	    }
	    else {
	        input.val("");
	    }
	}
});