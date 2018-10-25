
var layer;
var laypage;
var form;
var element;
var rowdata;//某行数据
var usertable = table.get("usertable");
function InSetOrg(v) {
    v.org_name = "depart";
    v.org_id = "depart_id";
    //v.$("#btnlist").show();
}
layui.use(['layer', 'laypage', 'element', 'form', 'laydate'], function () {
    layer = layui.layer;
    laydate = layui.laydate;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;


    //查询
    form.on("submit(btnsel)", function (data) {
        var param = data.field;
        usertable.load(param, NoData);
        $("#mycount").val(1);
    });


    $("#cleardepart").click(function () {
        $("#depart").val(""); //depart_id
        $("#depart_id").val(""); //
    })

    $("#clearAssert").click(function () {
        $("#pro_name").val("");
    })

    function NoData(data) {
       // $("#mycount").val(0);
        //if (data.list.length == 0) {
        //    //$("tbody").empty();
        //    //$("tbody").append("<tr style='color:red'><td></td><td></td><td></td><td></td><td>该条件下未能检索到任何数据,请切换检索条件后再次尝试！</td><td></td><td></td><td></td><td></td></tr>");
        //    //layer.msg("该条件下未能检索到任何数据,请切换检索条件后再次尝试");
        //    $("#mycount").val(0);
        //}
    }
   


    $(function () {

        //执行一个laydate实例
        //日期时间选择器
        laydate.render({
            elem: '#startTime'
            , type: 'datetime'
        });
        //日期时间选择器
        laydate.render({
            elem: '#endTime'
            , type: 'datetime'
        });


        //新增维修 
        $("#AddAssert").click(function () {
            layer.open({
                type: 2,
                title: "物资报废和处理",
                closeBtn: 1, //不显示关闭按钮
                shade: [0],
                area: ['100%', '100%'],
                offset: 't', //右下角弹出
                //time: 2000, //2秒后自动关闭
                anim: 2,
                maxmin: true, //开启最大化最小化按钮
                content: ['/OutOrder/OpenIndex?isrkopen=1', 'yes'], //iframe的url，no代表不显示滚动条
                end: function () { //此处用于演示
                }
            });
        });

        //选择物资名称
        $("#pro_name").click(function () {
            layer.open({
                type: 2,
                title: "物资编码",
                closeBtn: 1, //不显示关闭按钮
                shade: [0],
                area: ['100%', '100%'],
                offset: 't', //右下角弹出
                //time: 2000, //2秒后自动关闭
                anim: 2,
                maxmin: true, //开启最大化最小化按钮
                content: ['/AssetsManager/GetAssetCode?assetrepair=1&stock_qty_fig=false', 'no'], //链接到其他的控制器
                end: function () { //此处用于演示
                }
            });
        });

        ///导出数据
        form.on('submit(btnExport)', function (data) {
            var param = data.field;
            var count = $("#mycount").val();
            if (0 == count) {
                layer.msg("没有数据可以导出!");
                return;
            }
            //导出
            var daoru = layer.load(0, {
                shade: [0.1, '#676a6c'] //0.1透明度的白色背景
            });

            usertable.Export("/Report/ExportAssetBFBMList", param, ["750px", "700px"],
                function () {
                    layer.close(daoru);
                });
            //$.ajax({
            //    url: "/Report/ExportAssetBFBMList",
            //    data: param,
            //    type: "post",
            //    success: function (e) {
            //        var data = eval(e);
            //        if (data.success) {
            //            location.href = decodeURI(data.filePath);
            //        } else {
            //            layer.msg("导出失败，原因：" + data.msg);
            //        }
            //        layer.close(daoru);

            //    },
            //    error: function () {
            //        layer.close(daoru);
            //        layer.msg("系统异常请稍后再试");
            //    }
            //});
        });
    })
})


