$(function () {
    $('input[type="file"]').change(function () {
        var type = $(this).data('type') || null;
        var file = this.files[0];
        var fd = new FormData();
        fd.append("file", file);
        $.ajax({
            type: 'post',
            url: '/home/upload/?name=' + file.name,
            data: fd,
            contentType: false,
            processData: false,
            success: function (data) {
                // do something
                if (typeof (data) == "string" && data.length > 0) {
                    var result;
                    if (typeof (type) == 'string' && data.length > 0) {
                        result = $('input[name="' + type + '"]');
                    } else {
                        result = $('#result');
                        $('label[for="uploadInput"]>img').attr("src", "/content/upload/" + data);
                    }
                    result.attr("value", data);
                }
            },
            error: function () { }
        });
    })
});