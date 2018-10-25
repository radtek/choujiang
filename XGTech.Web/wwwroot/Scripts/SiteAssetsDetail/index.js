var form;
var gid = table.get("gid");
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;


    //监听提交
    form.on('submit(exporttable)', function (data) {
        var param = data.field;

        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/AssetInventorySummary/Export",
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

$("#btnsel").on("click", function () {
    var estate_id = $("#estate_id").val();
    var estate_name = $("#estate_name").val();
    if (estate_id <= 0 || estate_id == "" || estate_name == "") {
        $("#estate_name_verify").removeAttr("hidden");

        return;
    } else {
        $("#estate_name_verify").attr("hidden", "hidden");
    }
    var param = {};
    param.estate_id = estate_id;
    gid.load(param);


})
//点击名称
$("#estate_name").click(function () {
    //iframe窗
    layer.open({
        type: 2,
        title: "场地信息",
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['993px', '600px'],
        offset: 'ct', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/ResourcManager/SiteBounced', 'yes'], //iframe的url，no代表不显示滚动条
        success: function (obj, index) {
            var iframe = "layui-layer-iframe" + index;
            var v = window[iframe];
            v.$("#btnshow").hide();
            v.$("#btnlist").show();
            v.parent_from = 2;
        },
        end: function () { //此处用于演示
        }
    });
})
$("#clearEstate").on("click", function () {
    $("#estate_name").val("");
    $("#estate_id").val("");
})