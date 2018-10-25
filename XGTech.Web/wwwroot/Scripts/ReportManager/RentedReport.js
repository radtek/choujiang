
var rel_id = 0;
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

    document.getElementById('contract_effectivedate').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('contract_enddate').onclick = function () {
        end.elem = this
        laydate(end);
    }

});
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;

    //表格单选按钮监听
    form.on('radio(radio)', function (data) {
        rel_id = data.elem.parentNode.parentNode.children[20].innerText;
        if (rel_id <= 0) {
            layer.msg("该物资还没有合同信息")
            return;
        }
        var pageIndex = 1;
        var stock_code = $("#stock_code").val();
        var param = { pageIndex: pageIndex, stock_code: stock_code, rel_id: rel_id };
        SetDataTable("assetstable", "", param);

    });
    //监听提交
    form.on('submit(detailtable)', function (data) {
        var model = data.field;
        var pageIndex = 1;
        var param = { model: model, pageIndex: pageIndex };
        //加载列表
        SetDataTable("assetdetailtable", "assetdetailpage", param);
        rel_id = 0;
        //计算总费用
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/ReportManager/GetList",
            data: param,
            type: "post",
            success: function (e) {
                var data = eval(e);
                $("#assetdetailtable tfoot tr th").each(function (index, item) {
                    var count = 0;
                    for (var i = 0; i < data.length; i++)
                    {
                        if (typeof (item.attributes["field"]) == "undefined") {
                            return true;
                        } else {
                            var fieldname = item.attributes["field"].value;
                            var value = data[i][fieldname];
                            if (value == null) {
                                value = 0;
                            }
                            count = parseFloat(value) + parseFloat(count);
                            item.innerText = count;

                        }
                    }
                });
                layer.close(daoru);

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });
    });
    //监听提交
    form.on('submit(exporttable)', function (data) {
        var model = data.field;
        var filename = "出租盈利报表";
        if (model.contract_type == 1)
        {
            filename = "租入报表";
        } 
        var param = { model: model };
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/ReportManager/ExportRentList",
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
//设置负责人信息
function SetFZREmp(v) {
    v.vemp_id = "emp_id";
    v.vemp_name = "emp_name";
    v.$("#btnlist").show();
    v.$("#export").hide();
}
//设置物资所属单位
function SetAssetOrg(v)
{
    v.org_name = "org_name";
    v.org_id = "org_id";
    v.$("#btnlist").show();
}
//去除条件
$("#clearUser").on("click", function ()
{
    $("#emp_id").val("");
    $("#emp_name").val("");
})
$("#clearOrg").on("click", function () {
    $("#org_id").val("");
    $("#org_name").val("");
})
//请求物资信息
$("#btnselstockcode").on("click", function () {
    if (rel_id <= 0) {
        layer.msg("该物资还没有合同信息")
        return;
    }
    //请求物资信息
    var pageIndex = 1;
    var estate_name = $("#stock_code").val();
    var param = { pageIndex: pageIndex, stock_code: estate_name, rel_id: rel_id };
    SetDataTable("assetstable", "", param);
})