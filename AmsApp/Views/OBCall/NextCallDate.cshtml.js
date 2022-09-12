$(document).ready(function () {
	
});

function call(leadid) {
    localStorage.setItem("presentnumber", leadid);
    var callUrl = myurl + 'OBCall/OnCallNC/' + leadid;
    window.location.href = callUrl;
}

$('#CallDate').change(function () {
    var date = $(this).val();
    var callUrl = myurl + 'OBCall/NextCallDate/' + date;
    window.location.href = callUrl;
});
