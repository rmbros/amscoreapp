var dtable;
function loadTable(fData) {
    dtable = $("#patientlist").DataTable({
        "searching": false,
        "dom": '<"top"i>rt<"bottom"flp><"clear">',
        "paging": true,
        "lengthChange": true,
        "select": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/OBCall/GetVisitsList",
            "type": "POST",
            "data": fData
        },
        "columns": [
            { "data": "mobile", "name": "Mobile", "autoWidth": true },
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "agent", "name": "Agent", "autoWidth": true },
            { "data": "visitDate", "autoWidth": true },
            { "data": "patientId", "name": "Status", "autoWidth": true }       
        ],
    });
}
$(document).ready(function () {
    var fData = {};
    fData.Mobile = $('#Mobile').val();
    fData.FromDate = $('#FromDate').val();
    fData.ToDate = $('#ToDate').val();
    loadTable(fData);
});
$("#btnSearch").on("click", function () {
    var fData = {};
    fData.Mobile = $('#Mobile').val();
    fData.FromDate = $('#FromDate').val();
    fData.ToDate = $('#ToDate').val();
    dtable.destroy();
    loadTable(fData);
});