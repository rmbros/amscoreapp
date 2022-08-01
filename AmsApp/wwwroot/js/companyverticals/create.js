jQuery("#btnSubmit").on("click", function () {
    if ($("#frmcompanyverticalcreate").valid()) {
        console.log("Validation success");
    }
    else {
        console.log("Validation not success");
    }
});
jQuery("#frmcompanyverticalcreate").validate({
    rules: {
        "vertical": {
            required: !0,
            minlength: 3
        },
        "company": {
            required: !0
        }
    },
    messages: {
        "vertical": {
            required: "Please enter a vertical",
            minlength: "Your vertical must consist of at least 3 characters"
        },
        "company": {
            required: "Please select a company"
        }
    },

    ignore: [],
    errorClass: "invalid-feedback animated fadeInUp",
    errorElement: "div",
    errorPlacement: function (e, a) {
        jQuery(a).parents(".form-group > div").append(e)
    },
    highlight: function (e) {
        jQuery(e).closest(".form-group").removeClass("is-invalid").addClass("is-invalid")
    },
    success: function (e) {
        jQuery(e).closest(".form-group").removeClass("is-invalid"), jQuery(e).remove()
    },
    submitHandler: function (form) {
        //$.ajax({
        //    url: form.action,
        //    type: form.method,
        //    data: $(form).serialize(),
        //    success: function (response) {
        //        $('#answers').html(response);
        //    }
        //});
    }
});



