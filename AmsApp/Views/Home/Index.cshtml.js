$(document).ready(function () {
    if (localStorage.getItem("lookupdata") === null) {
        $.ajax({
            url: './Home/GetLookUpData',
            type: 'GET',
            success: function (result) {
              //  alert(result);
                localStorage.setItem("lookupdata", JSON.stringify(result));
              //  var presentCaller = JSON.parse(localStorage.getItem("lookupdata"));
              //  alert(localStorage.getItem("lookupdata"));
            },
            error: function () {
            }
        });
    }
    else {
        console.log('Data in localStorage persists');
      //  alert(localStorage.getItem("lookupdata"));
    }
});


