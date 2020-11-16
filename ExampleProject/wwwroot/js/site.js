var data = { objId: 1 };

$.ajax({
    url: '@Url.Action("PassIntFromView", "ControllerName")',
    type: "post",
    contentType: 'application/x-www-form-urlencoded',
    data: data,
    success: function (result) {
        console.log(result);
    }
});