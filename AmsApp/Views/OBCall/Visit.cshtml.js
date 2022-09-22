$(document).ready(function () {
    $('#alertError').hide();
    $("#btnSave").on('click', function () {
        var _form = $("#frmVisit");
        var isvalid = _form.valid();
        if (isvalid) {
            $("#lblError").empty();
            $('#alertError').hide();
            var data = _form.serialize();
            //  alert(data);
            $.ajax({
                url: '/OBCall/Visit',
                type: 'POST',
                data: data,
                success: function (result) {
                    if (result.toUpperCase().includes("ERROR")) {
                        $('#alertError').show();
                        $("#lblError").text(result);
                    }
                    else {
                        var url = '/OBCall/Appointments';
                        window.location.href = url;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#alertError').show();
                    $("#lblError").text(textStatus);
                }
            });
        }

    });
});
