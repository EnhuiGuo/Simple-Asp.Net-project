$(document).ready(function () {

	var validatedFiles = [];

	$(document).on('change', ':file', function () {
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
						if (file.name == files[i].name) {
							inList = true;
						}
					});

					if (!inList) {
					    if (files[i].size < 1000000) {
					        validatedFiles.push(files[i]);
					        saveFileToPage(files[i]);
					    }
					    else {
					        $('#errorMessage').append(files[i].name + "太大, 不能上传");
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

	$('#saveDogBtn').on('click', function () {

	    var filesSize = 0;

	    validatedFiles.forEach(function (file) {
	        filesSize += file.size;
	    });

	    if (filesSize > 2000000)
	    {
	        $('#errorMessage').append("照片太多，请删掉几张");
	    }
	    else if ($('#addDogForm').valid())
	    {
	        var data = new FormData();
	        var dog = $('#addDogForm').serializeObject();

	        data.append("Years", dog.Years);
	        data.append("Breed", dog.Breed);
	        data.append("HouseTrained", dog.HouseTrained);
	        data.append("HouseTrainedDetail", dog.HouseTrainedDetail);
	        data.append("Microchipped", dog.Microchipped);
	        data.append("Name", dog.Name);
	        data.append("Sex", dog.Sex);
	        data.append("Spayed", dog.Spayed);
	        data.append("SpecialRequirement", dog.SpecialRequirement);
	        data.append("UserId", dog.UserId);
	        data.append("Weight", dog.Weight);
	        data.append("WellCatDetail", dog.WellCatDetail);
	        data.append("WellCats", dog.WellCats);
	        data.append("WellChild", dog.WellChild);
	        data.append("WellChildDetail", dog.WellChildDetail);
	        data.append("WellDogDetail", dog.WellDogDetail);
	        data.append("WellDogs", dog.WellDogs);

	        for (var x = 0; x < validatedFiles.length; x++) {
	            data.append("file" + x, validatedFiles[x]);
	        }

	        $.ajax({
	            type: "POST",
	            url: "/Account/AddDog",
	            contentType: false,
	            processData: false,
	            data: data,
	            success: function (result) {
	                $('#errorMessage').html('');
	                window.location.href = "/Account/UserProfile";
	            },
	            error: function (err) {
	                if (err.responseJSON != null && err.responseJSON.length > 0) {
	                    $('#errorMessage').html('');
	                    $.each(err.responseJSON, function (i, message) {
	                        if (i == 0)
	                            $('#errorMessage').append(message);
	                        else
	                            $('#errorMessage').append(', ' + message);
	                    })
	                }
	            }
	        });
	    }
	});

	jQuery.fn.serializeObject = function () {
	    var arrayData, objectData;
	    arrayData = this.serializeArray();
	    objectData = {};

	    $.each(arrayData, function () {
	        var value;

	        if (this.value != null) {
	            value = this.value;
	        } else {
	            value = '';
	        }

	        if (objectData[this.name] != null) {
	            if (!objectData[this.name].push) {
	                objectData[this.name] = [objectData[this.name]];
	            }

	            objectData[this.name].push(value);
	        } else {
	            objectData[this.name] = value;
	        }
	    });
	    return objectData;
	};

	function saveFileToPage(file)
	{
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
						$('#imgGallery').append(result);
					}
				});
			};
			reader.readAsDataURL(file);
		};
		var data = openFile(file);
	}

	deleteImage = function (deleteBtn)
	{
		$(deleteBtn).parent().remove();
		var name = $(deleteBtn).data("image");
		validatedFiles.forEach(function (file, i) {
			if (file.name == name)
			{
				validatedFiles.splice(i, 1);
			}
		});
		showLabel();
	}

	function showLabel() {
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

})