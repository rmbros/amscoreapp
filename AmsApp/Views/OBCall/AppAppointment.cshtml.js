$(document).ready(function () {
    LoadData();
});

function LoadData() {
    var data = JSON.parse(localStorage.getItem("lookupdata"));

    //Clinic Branch
    $.each(data.clinicBranch, function (index, value) {
        $('#ClinicBranch').append('<option value="' + value.id + '">' + value.title + '</option>');
    });
    $('#ClinicBranch').selectpicker('refresh');

    //Gender
    $.each(data.gender, function (index, value) {
        $('#Gender').append('<option value="' + value.id + '">' + value.title + '</option>');
    });
    $('#Gender').selectpicker('refresh');

    //Main Disease
    $.each(data.mainDisease, function (index, value) {
        $('#MainDisease').append('<option value="' + value.id + '">' + value.title + '</option>');
    });
    $('#MainDisease').selectpicker('refresh');
}

$('#MainDisease').change(function () {
    $('#SubDisease').html('<option value="" selected>Choose...</option>');
    if ($(this).val() !== "" && $(this).val() > 0) {
        var maindisease = $(this).val();
        var data = JSON.parse(localStorage.getItem("lookupdata"));
        $.each(data.subDiseases, function (index, value) {
            if (value.parentId == maindisease) {
                $('#SubDisease').append($('<option style="word-wrap:break-word"></option>').val(value.id).html(value.title));
            }
        });
        $('#SubDisease').selectpicker('refresh');
    }
});

function SaveData(bClose) {
    $("#lblError").empty();
    $('#alertError').hide();
    var data = {
        Mobile: $('#Mobile').val(),
        Name: $('#Name').val(),
        Gender: $('#Gender').val(),
        Age: $('#Age').val(),
        MainDisease: $('#MainDisease').val(),
        SubDisease: $('#SubDisease').val(),
        ClinicBranch: $('#ClinicBranch').val(),
        Location: $('#Location').val(),
        AppointmentDate: $('#AppointmentDate').val(),
    };
    var saveUrl = myurl + 'OBCall/SaveAppointment';
    var homeUrl = myurl + 'OBCall/AppAppointments';
    $.ajax({
        url: saveUrl,
        type: 'POST',
        data: data,
        success: function (result) {
            if (result.toUpperCase().includes("ERROR")) {
                $('#alertError').show();
                $("#lblError").text(result);
            }
            else {
                window.location.href = homeUrl;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#alertError').show();
            $("#lblError").text(textStatus);
        }
    });
}

$("#btnSave").on('click', function () {
    SaveData(true);    
});
