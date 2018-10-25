var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
}
layui.use('form', function () {
    var form = layui.form;
    //关闭窗体页面

    //监听提交
    form.on('submit(formDemo)', function (data) {
        var obj = data.field;
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/RentalCostManager/SaveCostType";
        $.ajax({
            url: url,
            data: { model: obj },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                layer.msg(e.Msg, function () {
                    if (e.State == 1) {
                       
                        parent.layer.close(index);
                        parent.location.href = parent.location.href;
                    }
                });

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    });
});
$(function () {
    //取消
    $("#btncancel").on("click", function () {
        parent.layer.close(index);
    })

})