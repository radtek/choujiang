
var layer;
var laypage;
var form;
var element;
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
})


var G_rel_id = 0;//关系ID
var rowdata;
var rowdatalist = [];
//选中物资信息
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        rowdata = GetRowData(data.elem.parentNode.parentNode);
       
    });

})

//查询编码
$("#btnsel").on("click", function ()
{
    var pageIndex = 1;
    var assetname = $("#assetname").val();
    var param = { pageIndex: pageIndex, assetname: assetname };
    SetDataTable("assetstable", "mypage", param);
    stock_id = 0;
})

//设置责任人
function SetEmp(v)
{
    v.vemp_name = "emp_name";
    v.vemp_id = "emp_id";
    v.$("#btnlist").show();
    v.$("#export").hide();
}
//添加物资
$("#btnadd").on("click", function ()
{
    if (typeof (rowdata) == "undefined")
    {
        layer.msg('选中要添加的物资信息');
        return;
    }
    //判断是否选中物资
    layer.open({
        type: 1,
        offset: 'c',
        title: '添加',
        content: $('#addhtml') //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
    });
})
//添加物资行
$("#btnqr").on("click", function ()
{
    //数量
    var lease_numb = $("#lease_numb").val();
    var lease_price = $("#lease_price").val();
    var numbstr = validationNumber(lease_numb);
    var lease_pricestr = validationNumber(lease_price);
    if (typeof (numbstr) != "undefined")
    {
        layer.msg(numbstr);
        $("#lease_numb").focus();
        return;
    }
    if (typeof (lease_pricestr) != "undefined") {
        layer.msg(lease_price);
        $("#lease_price").focus();
        return;
    }
    var reg = /^[1-9]\d*$/;
    //判断数量是否大于0
    if (!reg.test(lease_numb))
    {
        layer.msg("数量必须是正整数");
        return;
    }
    //判断金额是否大于0
    if (parseInt(lease_price) < 0) {
        layer.msg("金额不能小于0");
        return;
    }
    //判断库存是否充足
    if (parseInt(lease_numb) > parseInt(rowdata.stock_qty)) {
        layer.msg("数量不能大于库存数");
        return;
    }
    //添加数量和出租单价
    rowdata.lease_numb = lease_numb;
    rowdata.lease_price = lease_price;
    rowdata.lease_money = lease_numb * lease_price;
    rowdatalist.push(rowdata);
    //添加表格详细
    SetTableRow("detailtable", '', rowdatalist)
    //关闭弹出层
    layer.closeAll();

})
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
//删除表格行事件
$("#btndel").on('click', function () {
    if ($("#detailtable tbody input[type=checkbox]:checked").length <= 0) {
        layer.msg("您未选中任何数据");
        return;
    }
    layer.confirm('您确定要删除？', {
        btn: ['确定', '取消'] //按钮
    }, function () {

        $("#detailtable tbody input[type=checkbox]:checked").each(function () {
            $(this).parents("tr").remove();
            //去除集合中的数据
            var stock_id = $(this).parents("tr")[0].children[7].innerHTML;
            rowdatalist.forEach(function (item,index)
            {
                if (item.stock_id == stock_id)
                {
                    rowdatalist.splice(index, 1);
                }
            })

        });
        layer.msg("删除成功");
    }, function () {

    });
})
//保存
$("#btnsave").on("click", function ()
{
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    var emp_id=$("#emp_id").val();
    if (parseInt(emp_id) <= 0)
    {
        layer.msg("请选择负责人");
        return;
    }
    if (rowdatalist.length <= 0)
    {
        layer.msg("请添加物资信息后再操作")
        return;
    }
    //提交信息
    rowdatalist.forEach(function (item,index)
    {
        item.rel_id = G_rel_id;
        item.emp_id = emp_id;
    })
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    $.ajax({
        url: "/SiteAssetManager/AddSiteAsset",
        data: { model: rowdatalist },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            if (data.status == 200)
            {
                layer.msg("保存成功", function ()
                {
                    parent.layer.close(index);
                });
               

            } else
            {
                layer.msg(data.msg);
            }
           

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
})