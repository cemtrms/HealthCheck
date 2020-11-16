var data = { objId: 1 };
$("#btnSubmit").click(function (e) {
    $.ajax({
        url: '@Url.Action("Create", "Url")',
        type: "post",
        contentType: 'application/x-www-form-urlencoded',
        data: data,
        success: function (result) {
            console.log(result);
        }
    });
});