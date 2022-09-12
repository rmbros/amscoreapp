$(document).ready(function () {
	
});

$("#btnCall").on('click', function () {
    var leadid = $("#btnCall").attr("data");
    localStorage.setItem("presentnumber", leadid);
    var callUrl = myurl + 'OBCall/OnCall/' + leadid;
    window.location.href = callUrl;
});