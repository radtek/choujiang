var cost_type_id = 0;
var cost_type_name = "";
var rel_id = 0;
var vcontract_no = "";
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //加载成本类型列表
    SetDataTable("assetstable");
    //表格单选按钮监听
    form.on('radio(radio)', function (data) {
        cost_type_id = data.elem.parentNode.parentNode.children[3].innerText;
        cost_type_name = data.elem.parentNode.parentNode.children[2].innerText;

    });
    //表格单选按钮监听合同成本
    form.on('radio(costradio)', function (data) {
        rel_id = data.elem.parentNode.parentNode.children[5].innerText;
        vcontract_no = data.elem.parentNode.parentNode.children[1].innerText;
    });

})

//成本类型编辑功能
$("#btncosttypeupdate").on("click",function () {
    if (cost_type_id == 0) {
        layer.msg("请选中要编辑的信息！")
        return;
    }
    Open('新增', '/RentalCostManager/CostType', SetForm);
})
function SetForm(v) {

    v.$("#cost_type_id").val(cost_type_id);
    v.$("#cost_type_name").val(cost_type_name);

};
//成本类型删除功能
function DelCostType() {
    if (cost_type_id == 0) {
        layer.msg("请选中要删除的信息！")
        return;
    }
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/RentalCostManager/DelCostType";
    $.ajax({
        url: url,
        data: { cost_type_id: cost_type_id },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            if (eval(e)) {
                layer.msg("删除成功");
                location.href = location.href;
            } else {
                layer.msg("删除失败");
            }

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
}
//合同成本 查询
$("#btnSel").on("click", function ()
{
    var param = { pageIndex: pageIndex, content: $("#content").val() };
    SetDataTable("tblassetcost", "mypage", param);
    rel_id = 0;
})
//编辑合同成本
$("#btnEdit").on("click", function () {
    if (rel_id <= 0)
    {
        layer.msg("请选择要编辑的信息");
        return;
    }
    Open('成本编辑', '/RentalCostManager/RentalCostSave', EditOpen);


});
function EditOpen(v)
{
    v.SetDetail(rel_id);
    v.$("#contract_no").val(vcontract_no);
    v.$("#rel_id").val(rel_id);
}
