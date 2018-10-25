var layer;
var laypage;
var form;
var element;
var rows;//某行数据
var assetstable = table.get("gid");
function OutSetOrg(v) {
    v.org_name = "out_org_name";
    v.org_id = "out_org_id";
    v.$("#btnlist").show();
}
function InSetOrg(v) {
    v.org_name = "in_org_name";
    v.org_id = "in_org_id";
    v.$("#btnlist").show();
}
$("#outclearOrg").on("click", function () {
    $("#out_org_name").val("");
    $("#out_org_id").val("");
})
$("#inclearOrg").on("click", function () {
    $("#in_org_name").val("");
    $("#in_org_id").val("");
})
$("#clearPro").on("click", function () {
    $("#pro_name").val("");
    document.getElementById("pro_name").setAttribute("data-procode", "");
    document.getElementById("pro_name").setAttribute("data-hidid", "");
})

//点击名称
$("#pro_name").click(function () {
    //iframe窗
    layer.open({
        type: 2,
        title: "商品信息",
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['993px', '600px'],
        offset: 'ct', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/AssetQuery/Index?isrkopen=1', 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示
        }
    });
})
layui.use(['laydate', 'form'], function () {
    var laydate = layui.laydate;

    var start = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
        }
    };

    var end = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
        }
    };

    document.getElementById('start_time').onclick = function () {
        start.elem = this;
        start.istime = true;
        start.format = 'YYYY-MM-DD hh:mm:ss';
        laydate(start);
    }
    document.getElementById('end_time').onclick = function () {
        end.elem = this;
        end.istime = true;
        end.format = 'YYYY-MM-DD hh:mm:ss';
        laydate(end);
    }

});
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //查询

    form.on("submit(btnsel)", function (data) {
        var start_time = $("#start_time").val();
        var end_time = $("#end_time").val();
        var d1 = new Date(start_time.replace(/\-/g, "\/"));
        var d2 = new Date(end_time.replace(/\-/g, "\/"));  
        if (start_time != "" && end_time != "" && d1 >= d2)
        {
            layer.msg("起始时间不得大于结束时间");
            return;
        }
        var param = data.field;
        param.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        assetstable.load(param);
    });

    //监听提交
    form.on('submit(exporttable)', function (data) {
        var start_time = $("#start_time").val();
        var end_time = $("#end_time").val();
        var d1 = new Date(start_time.replace(/\-/g, "\/"));
        var d2 = new Date(end_time.replace(/\-/g, "\/"));
        if (start_time != "" && end_time != "" && d1 >= d2)
        {
            layer.msg("起始时间不得大于结束时间");
            return;
        }
        var param = data.field;
        param.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/AssetTransfers/Export",
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
});