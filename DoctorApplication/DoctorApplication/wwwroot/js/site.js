var coll = document.getElementsByClassName("collapsible");
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var content = this.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
        } else {
            content.style.display = "block";
        }
    });
}

$(function () {
    $('#docImageFileButton').click(function (e) {
        e.preventDefault();
        $('#docImageFile').click();
    }
    );
});

function showUncheckedProfile(docId, page) {
        $(location).prop('href', 'DoctorProfileCheck?idDoc=' + docId + '&page=' + page);
};

function AreYouSure(idPhase1, idPhase2, phase) {
    if (phase == 1)
    {
        $("#" + idPhase1).hide();
        $("#" + idPhase2).show();
    }
    if (phase == 2)
    {
        $("#" + idPhase1).show();
        $("#" + idPhase2).hide();
    }
};