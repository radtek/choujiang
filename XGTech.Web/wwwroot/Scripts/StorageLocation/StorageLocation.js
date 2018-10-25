var usertable = table.get("usertable");

layui.use(['form', 'layedit', 'laydate', "layer"], function () {
    var form = layui.form;
    var layer = layui.layer;


    //查询
    form.on('submit(btnsel)', function (data) {
        var param = data.field;
        usertable.load(param);

    });
    //导出
    form.on('submit(btnexport)', function (data) {
        var param = data.field;
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        //var url = "/MonSubScription/ExportData";
        usertable.Export("/StorageLocation/ExportData", param, ["750px", "700px"],
            function () {
                layer.close(daoru);
            });
    });

});

