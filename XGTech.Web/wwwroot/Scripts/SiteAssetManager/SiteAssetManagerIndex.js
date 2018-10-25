//查询
$("#btnsel").on("click", function () {
    //加载表格数据
    var pageIndex = 1;
    var estate_name = $("#estate_name").val();
    var param = { pageIndex: pageIndex, estate_name: estate_name };
    SetDataTable("contracttable", "mypage", param);
    rel_id = 0;
})
//选中某个合同信息
var rel_id = 0;//关系ID
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        rel_id = data.elem.parentNode.parentNode.children[9].innerText;
        if (rel_id<=0)
        {
            layer.msg("该物资还没有合同信息不能添加物资")
            return;
        }
        //请求物资信息
        btnhtml = '<td style="text-align:center;"><button  class="layui-btn btnEdit">编辑</button></td>';
        var pageIndex = 1;
        var stock_code = $("#stock_code").val();
        var param = { pageIndex: pageIndex, stock_code: stock_code, rel_id: rel_id };
        SetDataTable("assetstable", "", param, btnhtml, test);
    });

})

function test(data)
{
    if (data.count==0)
    {
        layer.msg("该场地没有任何物资")
        return;
    }
}
//新建
$("#btncreate").on("click", function ()
{
    if (rel_id <= 0) {
        layer.msg("该物资还没有合同信息不能添加物资")
        return;
    }
    Open('新建', '/SiteAssetManager/AddSiteAssetManager', SetCreate)
})
function SetCreate(v)
{
    v.G_rel_id = rel_id;
}
//请求物资信息
$("#btnselstockcode").on("click", function ()
{
    if (rel_id <= 0) {
        layer.msg("该物资还没有合同信息不能添加物资")
        return;
    }
    //请求物资信息
    btnhtml = '<td style="text-align:center;"><button  class="layui-btn btnEdit">编辑</button></td>';
    var pageIndex = 1;
    var estate_name = $("#stock_code").val();
    var param = { pageIndex: pageIndex, stock_code: estate_name, rel_id: rel_id };
    SetDataTable("assetstable", "", param, btnhtml);
})
//编辑通过合同物资关系ID
var field_asset_id = 0;
$("body").on("click", ".btnEdit", function ()
{
    field_asset_id = $(this)[0].parentNode.parentNode.children[9].innerText;
    if (field_asset_id <= 0)
    {
        layer.msg("信息出错不能进行编辑")
        return;
    }
    Open('编辑', '/SiteAssetManager/UpdateCode', SetUpdateCode)
})
function SetUpdateCode(v)
{
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    $.ajax({
        url: "/SiteAssetManager/GetStieAssetById",
        data: { field_asset_id: field_asset_id },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            v.SetData(data);
        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
}
//删除
var field_asset_idlist = [];
$("#btnDel").on("click", function ()
{
    if ($("#assetstable tbody input[type=checkbox]:checked").length <= 0) {
        layer.msg("您未选中任何数据");
        return;
    }
    layer.confirm('您确定要删除？', {
        btn: ['确定', '取消'] //按钮
    }, function () {

        $("#assetstable tbody input[type=checkbox]:checked").each(function () {
            $(this).parents("tr").remove();
            //去除集合中的数据
            var field_asset_id = $(this).parents("tr")[0].children[9].innerHTML;
            field_asset_idlist.push(field_asset_id);

        });
        //删除
        //发送请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: "/SiteAssetManager/DelStieAssetById",
            data: { field_asset_id: field_asset_idlist },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                if(eval(e))
                {
                    layer.msg("删除成功");
                    $("#btnselstockcode").onclick();
                } else
                {
                    layer.msg("删除失败");
                }
            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    }, function () {

    });


})