// Write your Javascript code.
$(document).ready(function () {
    $("#upload").click(function (e) {
        var fileUpload = $("#files").get(0);
        var files = fileUpload.files;
        var data = new FormData();
        for (var i = 0; i < files.length ; i++) {
            data.append(files[i].name, files[i]);
        }
        $.ajax({
            type: "POST",
            url: "/home/UploadFilesAjax",
            contentType: false,
            processData: false,
            data: data,
            success: function (message) {
                $('.messagebox').html(message);
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    });
    $("#videoSearch").submit(function (e) {
        e.preventDefault();
        var data = new FormData();
        $('input[type=text],select').each(function( index ) {
            data.append($( this ).attr("name"), $( this ).val());
        });
        
        $.ajax({
            type: "POST",
            url: "/Youtube/YoutubeSearchAjax",
            contentType: false,
            processData: false,
            data: data,
            success: function (message) {
                console.log(message);
                $('#SearchResults > div').html(message);
            },
            error: function (message) {
                alert("Search Didn't work<br />" + message);
            }
        });
    });
});