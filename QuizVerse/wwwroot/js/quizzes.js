$(function () {
    var token = $('input[name="__RequestVerificationToken"]').val();

    function loadTable() {
        var data = $("#searchBox").val();
        $.get("/Quizzes/Search", { term: data }, function (response) {
            $("#quizTable").html(response);
        });
    }

    // $.get : live search
    $("#searchBox").on("keyup", loadTable);

    // $.post : publish or unpublish
    $("#quizTable").on("click", ".btn-toggle", function () {
        var id = $(this).data("id");
        var publish = !$(this).data("published");

        $.post("/Quizzes/TogglePublish",
            { id: id, publish: publish, __RequestVerificationToken: token },
            function (res) {
                if (res.success) loadTable();
            });
    });

    // $.ajax : delete
    $("#quizTable").on("click", ".btn-delete", function () {
        var id = $(this).data("id");
        if (!confirm("Delete this quiz with its questions and attempts?")) return;

        $.ajax({
            url: "/Quizzes/Delete/" + id,
            type: "DELETE",
            headers: { "RequestVerificationToken": token },
            success: function (res) {
                if (res.success) {
                    $("#row-" + id).fadeOut(300, function () { $(this).remove(); });
                }
            },
            error: function () { alert("Delete failed."); }
        });
    });
});