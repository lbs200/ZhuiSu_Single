var objects;
$.get("/ManageOrg_Info/GetAutoComplete", function (data, textStatus) {
    objects = data;
});