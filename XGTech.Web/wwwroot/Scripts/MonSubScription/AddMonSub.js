//查询
$("#btnsel").on("click", function () {
    var proname = $("#proname").val();
    var param = { proname: proname, pageIndex: 1 };
    SetDataTable("protable", "mypage", param);
    prod_id = 0;
})
var rowdata;
var rowdatalist = [];
var prod_id = 0;
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        rowdata = GetRowData(data.elem.parentNode.parentNode);
        prod_id = rowdata.prod_id;
    });

})
//添加选中的行到详细列表中
$("#btnadd").on("click", function () {
    if (typeof (rowdata) == "undefined" || prod_id == 0) {
        layer.msg('请选择商品后操作');
        return;
    }
    var fig = false;
    //判断相同商品是否存在如果存在则数量加1
    //更改倒序排列
    //for (var i = rowdatalist.length; i > 0; i--)
    //{
    //    if (rowdatalist[i].prod_id == rowdata.prod_id) {
    //        rowdatalist[i].app_numb += 1;
    //        fig = true;
    //    }
    //}

    for (var i = 0; i < rowdatalist.length;i++) {
        if (rowdatalist[i].prod_id == rowdata.prod_id) {
            rowdatalist[i].app_numb += 1;
            fig = true;
        }
    }
    if (!fig) {
        rowdata.app_numb = 1;
        
        rowdatalist.unshift(rowdata);
        //rowdatalist.push(rowdata);
    }
    //添加表格详细
    SetTableRow("detailtable", '', rowdatalist)
});
//删除
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
            var prod_id = $(this).parents("tr")[0].children[6].innerHTML;
            rowdatalist.forEach(function (item, index) {
                if (item.prod_id == prod_id) {
                    rowdatalist.splice(index, 1);
                }
            })

        });
        layer.msg("删除成功");
    }, function () {

    });
})
//保存
$("#btnsave").on("click", function () {
    var fig = false;
    if (rowdatalist.Orderlength <= 0) {
        layer.msg("数据为空不能保存");
        return;
    }
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引

    $("#detailtable tbody input").each(function () {
        var prod_id = $(this).parents("tr")[0].children[6].innerHTML;
        var appnumb = $(this).parents("tr")[0].children[5].innerHTML;
        var r = /^[0-9]*[1-9][0-9]*$/　　//正整数 
        if (!r.test(appnumb)) {
            fig = true;
            return false;

        }

        rowdatalist.forEach(function (item, index) {
            if (item.prod_id == prod_id) {
                item.app_numb = appnumb;
            }
        })

    });
    if (fig) {
        layer.msg("检查表格数量是否正确");
        return;
    }
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    $.ajax({
        url: "/MonSubScription/Add",
        data: { list: rowdatalist },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            if (data.State == 1) {
                layer.msg("保存成功", function () {
                    parent.layer.close(index);
                    parent.$("#btnsel").click();
                });


            } else {
                layer.msg(data.Msg);
            }


        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
})

//修改输入框数据之后鼠标离开时自动关闭
$("body").on("blur", ".importtext", function ()
{
    var row = $(this)[0].parentNode.parentNode;
    var rowdatas = GetRowData(row);

    //判断相同商品是否存在如果存在则数量加1
    for (var i = 0; i < rowdatalist.length; i++) {
        if (rowdatalist[i].prod_id == rowdatas.prod_id) {
            rowdatalist[i].app_numb = parseInt($(this).val());
            fig = true;
        }
    }
    $(this).removeClass("addinput");
    $(this).html = $(this)[0].parentNode.innerHTML=$(this).val();
})

