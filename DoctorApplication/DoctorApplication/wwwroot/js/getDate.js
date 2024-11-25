$(document).ready(function () {
    $('#dataInreg').change(function () {
        var selectedValue = $(this).val();
        var idDoc = $("#doctor").val();
        $.ajax({
            url: '/Home/GetTimes',
            type: 'POST',
            data: { date: selectedValue, idDoc: idDoc },
            success: function (data) {
                $("#timp").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#timp").append('<option>' + data[i] + '</option>');
                }
            }
        });
    });
});


$(document).ready(function () {
    $('#dateStart').change(function () {
        var date = $(this).val();
        if (date >= $('#dateEnd').val()) { $('#dateEnd').val(date); }
        $("#dateEnd").attr("min", date);
        var dateCheck = ($("#dateStart").val() >= $("#dateEnd").val());
        if (dateCheck == true) {
            var startTime = $("#timeStart").val();
            $.ajax({
                url: '/Doctor/SetTimeEndLimit',
                type: 'POST',
                data: { time: startTime, dateCheck: dateCheck },
                success: function (time) {
                    var oldVal = $("#timeEnd").val();
                    $("#timeEnd").empty();
                    for (var i = 0; i < time.length; i++) {
                        if (time[i] == oldVal) $("#timeEnd").append('<option selected>' + time[i] + '</option>');
                        $("#timeEnd").append('<option>' + time[i] + '</option>');
                    }
                }
            });
        }
    });
});

$(document).ready(function () {
    $('#timeStart').change(function () {
        var startTime = $(this).val();
        var dateCheck = ($("#dateStart").val() == $("#dateEnd").val());
        $.ajax({
            url: '/Doctor/SetTimeEndLimit',
            type: 'POST',
            data: { time: startTime, dateCheck: dateCheck },
            success: function (time) {
                var oldVal = $("#timeEnd").val();
                $("#timeEnd").empty();
                for (var i = 0; i < time.length; i++) {
                    if (time[i] == oldVal) $("#timeEnd").append('<option selected>' + time[i] + '</option>');
                    $("#timeEnd").append('<option>' + time[i] + '</option>');
                }
            }
        });
    });
});

$(document).ready(function () {
    $('#dateEnd').change(function () {
        var startValue = $("#timeStart").val();
        var dateCheck = ($("#dateStart").val() == $(this).val());
        $.ajax({
            url: '/Doctor/SetTimeEndLimit',
            type: 'POST',
            data: { time: startValue, dateCheck: dateCheck },
            success: function (time) {
                var oldVal = $("#timeEnd").val();
                $("#timeEnd").empty();
                for (var i = 0; i < time.length; i++) {
                    if (time[i] == oldVal) $("#timeEnd").append('<option selected>' + time[i] + '</option>');
                    $("#timeEnd").append('<option>' + time[i] + '</option>');
                }
            }
        });
    });
});

$('#appointmentForm').submit(function (e) {
    $.ajax({
        url: '/Home/ConformationUrl',
        type: 'POST',
        success: function (page) {
            $(location).prop('href', page)
        }
    });
});

var loadFile = function (event) {
    var image = document.getElementById('docImage');
    image.src = URL.createObjectURL(event.target.files[0]);
};


