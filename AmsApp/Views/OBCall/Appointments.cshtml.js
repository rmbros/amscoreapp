$(document).ready(function () {
	
});

function call(leadid) {
    localStorage.setItem("presentnumber", leadid);
    var callUrl = myurl + 'OBCall/OnCallApp/' + leadid;
    window.location.href = callUrl;
}

$('#CallDate').change(function () {
    var date = $(this).val();
    var callUrl = myurl + 'OBCall/Appointments/' + date;
    window.location.href = callUrl;
});
