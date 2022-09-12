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
    var hvDisposition = $('#hvDisposition').val();
   
    $.each(data.disposition, function (index, value) {
        if (value.id == hvDisposition) {
            $('#Disposition').append('<option value="' + value.id + '" selected>' + value.title + '</option>');
        }
        else {
            $('#Disposition').append('<option value="' + value.id + '">' + value.title + '</option>');
        }
    });
    $('#Disposition').selectpicker('refresh');
    $('#Disposition').trigger('change');

    //Clinic Branch
    var hvClinicBranch = $('#hvClinicBranch').val();
    $.each(data.clinicBranch, function (index, value) {
        if (value.id == hvClinicBranch) {
            $('#ClinicBranch').append('<option value="' + value.id + '" selected>' + value.title + '</option>');
        }
        else {
            $('#ClinicBranch').append('<option value="' + value.id + '">' + value.title + '</option>');
        }
    });
    $('#ClinicBranch').selectpicker('refresh');

    //Gender
    var hvGender = $('#hvGender').val();
    $.each(data.gender, function (index, value) {
        if (value.id == hvGender) {
            $('#Gender').append('<option value="' + value.id + '" selected>' + value.title + '</option>');
        }
        else {
            $('#Gender').append('<option value="' + value.id + '">' + value.title + '</option>');
        }
    });
    $('#Gender').selectpicker('refresh');

    //Main Disease
    var hvMainDisease = $('#hvMainDisease').val();
    $.each(data.mainDisease, function (index, value) {
        if (value.id == hvMainDisease) {
            $('#MainDisease').append('<option value="' + value.id + '" selected>' + value.title + '</option>');
        }
        else {
            $('#MainDisease').append('<option value="' + value.id + '">' + value.title + '</option>');
        }
    });
    $('#MainDisease').selectpicker('refresh');
    $('#MainDisease').trigger('change');
  
   
}
$('#Disposition').change(function () {
    if (this.value == 1) {
        $('.preappt').show();
        $('.calba').show();
    }
    else if (this.value == 5 || this.value == 6 || this.value == 7 || this.value == 8 || this.value == 9 || this.value == 10 || this.value == 11) {
        $('.preappt').hide();
        $('.calba').hide();
    }
    else {
        $('.preappt').hide();
        $('.calba').show();
    }
});

$('#MainDisease').change(function () {
    $('#SubDisease').html('<option value="" selected>Choose...</option>');
    if ($(this).val() !== "" && $(this).val() > 0) {
        var maindisease = $(this).val();
        var data = JSON.parse(localStorage.getItem("lookupdata"));
        var hvSubDisease = $('#hvSubDisease').val();
        $.each(data.subDiseases, function (index, value) {
            if (value.parentId == maindisease) {
                if (value.id == hvSubDisease) {
                    $('#SubDisease').append($('<option style="word-wrap:break-word" selected></option>').val(value.id).html(value.title));
                }
                else {
                    $('#SubDisease').append($('<option style="word-wrap:break-word"></option>').val(value.id).html(value.title));
                }
            }
        });
        $('#SubDisease').selectpicker('refresh');
    }
});

function SaveData(bClose) {
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
      /*  Email: $('#Email').val(),*/
        Address: $('#Address').val(),
        //City: $('#City').val(),
        //State: $('#State').val(),
        //Country: $('#Country').val(),
        //Pin: $('#Pin').val(),
        NextCallDate: $('#NextCallDate').val(),
        AppointmentDate: $('#AppointmentDate').val(),
        //Notes: $('#Notes').val(),
        //OnHold: $("#StartTime")[0].checked,
        StartTime: $('#StartTime').val(),
        SaveAndClose: bClose,
    };
    var saveUrl = myurl + 'OBCall/SaveCall';
    var homeUrl = myurl + 'OBCall/NextCallDate/' + $('#hvNextCallDate').val();
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
    if ($('#Disposition').val() == 0) {
        alert("Please select disposition")
    }
    else {
        SaveData(true);
    }
    
});
