var layer;
var laypage;
var form;
var element;
//成本
var cost_type_id ;
var cost_type_name;
//关闭窗体页面
var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
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
    //选中成本类型
    form.on('select(cbtype)', function (data) {
        cost_type_id = data.value;
        cost_type_name =$("#cbtype option:checked").text();
        
    });
    //单选按钮
    form.on('radio(radio)', function (data) {
        delrowIndex = data.elem.parentNode.parentNode.attributes["rowIndex"].value;
    });
    //监听提交
    form.on('submit(formDemo)', function (data) {
        if (detaillist.length <= 0)
        {
            layer.msg("请添加成本后操作");
            return;
        }
        if (data.field.rel_id == "")
        {
            layer.msg("请选择合同后操作");
            return;
        }
        detaillist.forEach(function (item,index) {
            item.rel_id = data.field.rel_id;
        });
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/RentalCostManager/AssetCost";
        $.ajax({
            url: url,
            data: { model:detaillist },
            type: "post",
            success: function (e) {
                layer.msg(e.Msg, function () {
                    layer.close(daoru);
                    if (e.State == 1) {
                        if (parseInt(index) > 0) {
                            parent.layer.close(index);
                            parent.$("#btnSel").click();

                        } else {
                            layer.msg(e.Msg);
                        }

                    } else {
                        layer.msg(e.Msg);
                    }
                });

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    });

})
//取消
$("#btncancel").on("click", function ()
{
    parent.layer.close(index);
})
//打开租出合同页面
function SetContract(v)
{
    v.$("#btnlist").show();
    v.$("#btnshow").hide();
    v.vid = "rel_id";
    v.vname = "contract_no";
}
//租出合同页面关闭时
function CloseContract()
{
    if ($("#rel_id").val() == "")
    {
        return;
    }
    SetDetail($("#rel_id").val());
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
//添加明细
var detaillist = [];
var rowIndex = 1;
$("#btnaddrow").on("click", function () {
    if (typeof (cost_type_id) == "undefined" || typeof (cost_type_name) == "cost_type_id" || cost_type_id==-1)
    {
        layer.msg("请选择成本类型");
        return;
    }
    var asset_cost_money = $("#asset_cost_money").val();
    var valida = validationNumber(asset_cost_money);
    if (typeof (valida) != "undefined")
    {
        layer.msg(valida);
        return;
    }

    //添加明细
    var html = "<tr rowIndex='" + rowIndex + "'><td style='text-align: center;'><input id='choose' name='radio' lay-filter='radio' title=' ' type='radio'lay-skin='primary' lay-filter='Choose'></td>" +
        "<td name='cost_type_name'>" + cost_type_name + "</td><td   style='text-align:right' name='asset_cost_money'>" + asset_cost_money + "</td><td style='display:none' name='cost_type_id'>" + cost_type_id + "</td>/tr>";
    $("#tabbodyrk").append(html);
    form.render();
    var detail = {};
    detail.asset_cost_money = asset_cost_money;
    detail.cost_type_id = cost_type_id;
    detail.rowIndex = rowIndex;
    detaillist.push(detail);
    rowIndex++;
    var hj = parseFloat($("#ipthj").val())+ parseFloat(asset_cost_money);
    $("#ipthj").val(hj);
});
//删除明细
var delrowIndex = 0;
$("#btndelrow").on("click", function ()
{
    if (delrowIndex == 0)
    {
        layer.msg("请选中要删除的明细")
        return;
    }
    $("#tabbodyrk tr").each(function (index,item)
    {
        if (item.attributes["rowIndex"].value == delrowIndex)
        {
            item.remove();
            for (var i = 0; i < detaillist.length; i++) {
                if (detaillist[i].rowIndex == delrowIndex) {

                    var hj = parseFloat($("#ipthj").val()) - parseFloat(detaillist[i].asset_cost_money);
                    $("#ipthj").val(hj);
                    detaillist.splice(index, 1);
                }
            }
        }
       
       
    })
    delrowIndex = 0;
})
//从数据库获取明细
function SetDetail(rel_id)
{
    var param = { rel_id: rel_id};
    SetDataTable("assetstable", "", param,"",loadover);

}
//网格加载后的事件
function loadover(data)
{
    for (var i = 0; i < data.list.length; i++)
    {
        var item = data.list[i];
        var detail = {};
        detail.asset_cost_money = item.asset_cost_money;
        detail.cost_type_id = item.cost_type_id;
        detail.rowIndex = rowIndex;
        detaillist.push(detail);
        rowIndex++;
        var hj = parseFloat($("#ipthj").val()) + parseFloat(item.asset_cost_money);
        $("#ipthj").val(hj);
    }
    //获取所有新增的网格
    var rindex = 1;
    $("#tabbodyrk tr").each(function ()
    {
        $(this).attr("rowIndex", rindex);
        rindex++;
    })
}
