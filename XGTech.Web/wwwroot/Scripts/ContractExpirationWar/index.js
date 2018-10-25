layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //监听提交
    form.on('submit(save)', function (data) {
        var obj = data.field;
        var alarm_day = obj.alarm_day;
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ContractExpirationWar/UpdateContractalarmday";
        $.ajax({
            url: url,
            data: { alarm_day: alarm_day },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                if (eval(e)) {
                    layer.msg("操作成功");
                } else {
                    layer.msg("操作失败");
                }


            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    });

    //监听提交
    form.on('submit(sel)', function (data) {
        var obj = data.field;
        var estate_name = obj.estate_name;
        var pageIndex = 1;
        var param = { estate_name: estate_name, pageIndex: pageIndex };
        SetDataTable("assettable", "page", param);
    });

    form.on('submit(export)', function (data) {
        var obj = data.field;
        var estate_name = obj.estate_name;
        var pageIndex = 1;
        var param = { estate_name: estate_name};
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/ContractExpirationWar/ExportList",
            data: param,
            type: "post",
            success: function (e) {
                var data = eval(e);
                if (data.success) {
                    location.href = decodeURI(data.filePath);
                } else {
                    layer.msg("导出失败，原因：" + data.message);
                }
                layer.close(daoru);

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });
    });

})
