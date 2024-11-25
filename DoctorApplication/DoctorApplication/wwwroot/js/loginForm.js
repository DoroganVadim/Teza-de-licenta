$(document).ready(function () {
    $('#accTypeSelect').change(function () {
        var type = $("#accTypeSelect").val();
        if (type == "User") {
            $("#doctorAply").hide();
            $("#email").show();
            $("#password").show();
            $("#repPassword").show();
            $("#submit").show();
        }
        if (type == "Doctor") {
            $("#doctorAply").show();
            $("#email").hide();
            $("#password").hide();
            $("#repPassword").hide();
            $("#submit").hide();
        }
    });
});

$(document).ready(function () {
    $('#accPresence').change(function () {
        var val = $("#accPresence").val();
        if (val == "Have") {
            $("#password").hide();
            $("#repPassword").hide();
            $("#HasAccount").val(true);   
        }
        if (val == "DontHave") {
            $("#password").show();
            $("#repPassword").show();
            $("#HasAccount").val(false);
        }
    });
});