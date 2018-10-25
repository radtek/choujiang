layui.use('laydate', function () {
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
        laydate(start);
    }
    document.getElementById('end_time').onclick = function () {
        end.elem = this
        laydate(end);
    }

});
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //监听提交
    form.on('submit(detailtable)', function (data) {
        var model = data.field;
        var pageIndex = 1;
        var param = { model: model, pageIndex: pageIndex };
        //加载成本类型列表
        SetDataTable("assetstable", "mypage", param);

    });

    //监听提交
    form.on('submit(exporttable)', function (data) {
        var model = data.field;
        var param = { model: model};
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/ReportManager/ExportRentalCostReportList",
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
//设置物资所属单位
function SetAssetOrg(v) {
    v.org_name = "org_name";
    v.org_id = "org_id";
    v.$("#btnlist").show();
}