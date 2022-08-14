$(document).ready(function () {
    //var mobile = localStorage.getItem("presentnumber");
    var mobile = $('#Mobile').val();
    //alert(mobile);
    LoadData();
    $('.preappt').hide();
    $('.calba').hide();
    var link = "tel:" + mobile;
    window.location.href = link;
});

function LoadData() {
    var data = JSON.parse(localStorage.getItem("lookupdata"));
    //Disposition
    $.each(data.disposition, function (index, value) {
        $('#Disposition').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //Clinic Branch
    $.each(data.clinicBranch, function (index, value) {
        $('#ClinicBranch').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //Gender
    $.each(data.gender, function (index, value) {
        $('#Gender').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //City
    $.each(data.city, function (index, value) {
        $('#City').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //State
    $.each(data.state, function (index, value) {
        $('#State').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //Country
    $.each(data.country, function (index, value) {
        $('#Country').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //City
    $.each(data.city, function (index, value) {
        $('#City').append('<option value="' + value.id + '">' + value.title + '</option>');
    });

    //Main Disease
    $.each(data.mainDisease, function (index, value) {
        $('#MainDisease').append('<option value="' + value.id + '">' + value.title + '</option>');
    });
    //Sub Disease
    $.each(data.subDisease, function (index, value) {
        $('#SubDisease').append('<option value="' + value.id + '">' + value.title + '</option>');
    });
}
$('#Disposition').change(function () {
    if (this.value == 1) {
        $('.preappt').show();
        $('.calba').show();
    }
    else if (this.value == 5 || this.value == 6 || this.value == 7 || this.value == 8 || this.value == 9 || this.value == 10) {
        $('.preappt').hide();
        $('.calba').hide();
    }
    else {
        $('.preappt').hide();
        $('.calba').show();
    }
});
/*
$('#MainDisease').change(function () {
    $('#SubDisease').html('<option value="" selected>Choose...</option>');
    if ($(this).val() !== "" && $(this).val() > 0) {
        var maindisease = $(this).val();
        var data = JSON.parse(localStorage.getItem("lookupdata"));
        $.each(data.subDisease, function (index, value) {
            if (value.parentId == maindisease) {
                $('#SubDisease').append('<option value="' + value.id + '">' + value.title + '</option>');
            }
        });
    }
});
*/
$("#btnSaveNext").on('click', function () {
    $("#lblError").empty();
    $('#alertError').hide();
    var data = {
        Id: $('#Id').val(),
        Mobile: $('#Mobile').val(),
        Disposition: $('#Disposition').val(),
        Name: $('#Name').val(),
        Gender: $('#Gender').val(),
        Age: $('#Age').val(),
        MainDisease: $('#MainDisease').val(),
        SubDisease: $('#SubDisease').val(),
        ClinicBranch: $('#ClinicBranch').val(),
        Email: $('#Email').val(),
        Address: $('#Address').val(),
        City: $('#City').val(),
        State: $('#State').val(),
        Country: $('#Country').val(),
        Pin: $('#Pin').val(),
        NextCallDate: $('#NextCallDate').val(),
        Notes: $('#Notes').val(),
        OnHold: $("#OnHold")[0].checked,
    };
    var saveUrl = myurl + 'OBCall/SaveCall';
    var homeUrl = myurl + 'Home';
    var callUrl = myurl + 'OBCall/OnCall/';
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
                if (result.toUpperCase().includes("DONE")) {
                    window.location.href = homeUrl;
                }
                else {
                    localStorage.setItem("presentnumber", result);
                    window.location.href = callUrl + result;
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#alertError').show();
            $("#lblError").text(textStatus);
        }
    });
});