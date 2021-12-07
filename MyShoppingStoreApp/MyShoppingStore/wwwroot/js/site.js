$(function () {

    if ($("a.Deletion").length) {
        $("a.Deletion").click(() => {
            if (!confirm("Are you sure you want to Delete?")) return false;

        });
    }
    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2020);
    }

});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreview").attr("src", e.target.result).width(200).height(200);

        };

        reader.readAsDataURL(input.files[0]);
    }
}