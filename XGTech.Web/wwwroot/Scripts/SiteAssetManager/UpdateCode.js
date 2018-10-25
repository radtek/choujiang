//关闭窗体页面
var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
}
//验证字符串数字
function validationNumber(value) {
    var regu = /^[0-9]+\.?[0-9]*$/;
    if (value != "") {
        if (!regu.test(value)) {
            return "请输入正确的数字";
        }
    } else {
        return '必填项不能为空';
    }
}

layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //自定义验证规则
    form.verify({
        float: function (value) {
            return validationNumber(value, 2);
        }
    });
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var obj = data.field;
        if (parseInt(obj.lease_numb) <= 0)
        {
            layer.msg("数量必须大于0");
            return;
        }
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/SiteAssetManager/UpdateSiteAsset";
        $.ajax({
            url: url,
            data: { model: obj },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                if(eval(e))
                {
                    layer.msg("操作成功");
                    parent.layer.close(index);
                    parent.$("#btnselstockcode").click();
                } else
                {
                    layer.msg("操作失败,请检查库存数");
                }
              

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    });

})

//取消
$("#btncancel").on("click", function () {
    parent.layer.close(index);
})

$("#lease_price").on("blur", function ()
{
    $("#lease_money").val(parseInt($(this).val()) * parseInt($("#lease_numb").val()))
})

$("#lease_numb").on("blur", function () {
    $("#lease_money").val(parseInt($(this).val()) * parseInt($("#lease_price").val()))
})
//设置责任人
function SetEmp(v) {
    v.vemp_name = "emp_name";
    v.vemp_id = "emp_id";
    v.$("#btnlist").show();
    v.$("#export").hide();
}