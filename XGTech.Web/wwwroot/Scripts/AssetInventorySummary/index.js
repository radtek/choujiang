var form;
var gid = table.get("gid");
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;

    form.on("submit(btnsel)", function (data) {

        var param = data.field;
        param.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        gid.load(param);

    });
    var param = {};
    param.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
    gid.load(param);
    //监听提交
    form.on('submit(exporttable)', function (data) {
        var param = data.field;
        param.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        //导出
        gid.Export("/AssetInventorySummary/Export", param, ["750px", "700px"],
            function () {

            }, function () {
                layer.closeAll();
            });
    });
})

$("#clearPro").on("click", function () {
    $("#pro_name").val("");
    document.getElementById("pro_name").setAttribute("data-procode", "");
    document.getElementById("pro_name").setAttribute("data-hidid", "");
})