var layer;
var laypage;
var form;
var element;
var rowdata;//某行数据
var assetstable = table.get("assetstable");

layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //查询
    form.on("submit(btnsel)", function (data) {
        var param = data.field;
        assetstable.load(param);
    });
    var param = {};
    param.type = $("#type").val();
    assetstable.load(param);
    //assetstable.load();
//选中某行
//form.on("radio(Choose)", function (data) {
//    rowdata = table.get(data.elem.parentNode.parentNode.id).GetRowData();
//});
    //监听提交
    form.on('submit(exporttable)', function (data) {
        var param = data.field
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/AssetQuery/Export",
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


$(document).ready(function () {
    //判断是否以窗体打开
    if (location.search.indexOf("isrkopen") > 0) {
        $("#isopen").show();
        $("#noopen").hide();

        //$("#btnsel").click();
        //将信息保存父级页面
        $("#btnsave").on("click", function () {
            rowdata = assetstable.GetSelTableRowRadioData();
            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
            var daoru = layer.load(0, {
                shade: [0.1, '#676a6c'] //0.1透明度的白色背景
            });
            if (rowdata.length ==0) {
                layer.msg("请选中一条商品信息！");
                layer.close(daoru);
                return;
            } else {
                //将选中的信息添到父页面中
                parent.$("#asset_class_id").val(rowdata[0].asset_class_id);
                parent.$("#pro_name").attr("data-procode", rowdata[0].prod_code);
                parent.$("#pro_name").attr("data-hidid", rowdata[0].prod_id);
                parent.$('#pro_type option[value="' + rowdata[0].asset_class_id + '"]').attr('selected', true);
                parent.$("#storage_location").val("K"+rowdata[0].storagecode);
                //设置下拉列表框 物资类别
                // parent.$("#pro_type").parent().find("dd").removeClass("layui-this");
                parent.$("#pro_type").parent().find("dd").each(function (index, dditem) {
                    if (dditem.attributes["lay-value"].value == rowdata[0].asset_class_id) {
                        dditem.click();
                    }
                })
                parent.$("#pro_name").val(rowdata[0].prod_name);//物品名称
                parent.$("#pro_standard").val(rowdata[0].prod_spec);//规格
                parent.$("#pro_unit").val(rowdata[0].prod_unit);//单位
                parent.$("#pro_syny").val(rowdata[0].service_life);//使用年限
                parent.$("#AssetClassName").val(rowdata[0].prod_name);//使用年限
                var Num = "0000";
                //资产编码
                var newassetcode = "WZ" + rowdata[0].prod_code + Num;


               parent.$("#stock_code").val(newassetcode);


                //------------------------采购增加代码---------------------------------------
                parent.$("#AssetClassName").val(rowdata[0].prod_name);//物品名称
                parent.$("#AssertSpec").val(rowdata[0].prod_spec);//规格
                parent.$("#AssertMyUnit").val(rowdata[0].prod_unit);//  单位
                parent.$("#pro_syny").val(rowdata[0].service_life);//使用年限
                parent.$("#AssertClass").val(rowdata[0].prod_name);//使用类别
            }
            parent.layer.close(index);

        })
    }
})