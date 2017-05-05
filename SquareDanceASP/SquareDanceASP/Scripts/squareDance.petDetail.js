$(document).ready(function () {

    deleteImage = function (image) {

        $.confirm({
            title: "要删除么？",
            content: "删除",
            buttons: {
                confirm: function () {
                    var id = $(image).data("imageId");
                    $.ajax({
                        type: "POST",
                        url: "/Account/DeleteImage",
                        data: { "id": id },
                        success: function (result) {
                            if (result.Success) {
                                updateImagesDetail();
                            }
                        }
                    });
                },
                cancel: function () {

                },
            }
        });
    }

    Dropzone.options.dropzoneForm = {
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
        dictDefaultMessage: "请把照片拖到这里或者单击上传!",
        init: function () {
            this.on("success", function (file) {
                updateImagesDetail();
            });

            this.on("error", function (file, response) {
                $(file.previewElement).find('.dz-error-message').text("文件只能是照片格式，谢谢");
            });

            this.on("complete", function (file) {
                setTimeout(function () {
                    Dropzone.forElement("#dropzoneForm").removeFile(file);
                }, 10000)
            });
        }
    };

    function updateImagesDetail() {
        $.ajax({
            type: "GET",
            url: "/Account/_PetImagesDetail",
            data: { "id": petId },
            success: function (result) {
                $('#imagesDetail').html(result);
            }
        });
    }
})