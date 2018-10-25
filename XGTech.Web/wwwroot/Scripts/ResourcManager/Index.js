var gid = table.get("gid");
var rowdata;//某行数据
var rel_id;
//编辑按钮
$("#btnupdate").on("click", function () {
    rowdata = gid.GetSelTableRowRadioData();
    if (rowdata[0].estate_id == "") {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    Open('编辑', '/ResourcManager/Save', rowsEdit)
})
function rowsEdit(v) {
    //请求数据
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/ResourcManager/GetResourcDetail";
    $.ajax({
        url: url,
        data: { estate_id: rowdata[0].estate_id },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            v.SetData(data.estateModel);
            v.$("#status").val(data.estateModel.status);
            v.form.render('select');
            v.SetFile(data.estateFilePath);

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
}
//删除按钮
$("#btnDel").on("click", function () {
    rowdata = gid.GetSelTableRowRadioData();
    if (rowdata[0].estate_id == "") {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    if (rowdata[0].rel_id == "" || parseInt(rowdata[0].rel_id) > 0) {
        layer.msg("该资源已有合同不能进行删除");
        return;
    }
    //请求数据
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/ResourcManager/DelResourc";
    $.ajax({
        url: url,
        data: { estate_id: rowdata[0].estate_id },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var fig = eval(e);
            if (fig) {
                layer.msg("操作成功");
                $("#btnsel").click();
            } else {
                layer.msg("操作失败");

            }

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });

})
//退租按钮
$("#btntz").on("click", function () {
    rowdata = gid.GetSelTableRowRadioData();
    if (rowdata.length<=0)
    {
        layer.msg("请选择要退租的场地");
        return;
    }
    if (rowdata[0].estate_status_name=="闲置")
    {
        layer.msg("该场地没有合同无法操作");
        return;
    }

    //if (rowdata[0].estate_id == "") {
    //    layer.msg("场地不存在请选中要操作的资源信息");
    //    return;
    //}
    //if (rowdata[0].rel_id == "" || rowdata[0].rel_id == 0) {
    //    layer.msg("该资源没有合同信息");
    //    return;
    //}
    //if (rowdata[0].estate_status_name == "停用") {
    //    layer.msg("该资源为停用状态");
    //    return;
    //}
    layer.confirm('是否退租', {
        btn: ['是', '否'] //按钮
    }, function () {
        //请求数据
        //发送请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ResourcManager/TZResourc";
        $.ajax({
            url: url,
            data: { estate_id: rowdata[0].estate_id, cust_contract_id: rowdata[0].cust_contract_id ,rel_id: rowdata[0].rel_id},
            type: "post",
            success: function (e) {
                layer.close(daoru);
                var fig = eval(e);
                if (fig) {
                    layer.msg("操作成功");
                    $("#btnsel").click();
                } else {
                    layer.msg("操作失败");

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

//编辑列
function Edit() {
    //var row_id = $(this)[0].$tbody[0].id;
    return "<a style='color:#337ab7 !important;cursor:pointer' onclick='btnxq(this)' >详情</a> &nbsp;&nbsp;<a style='color:#337ab7 !important;cursor:pointer' onclick='btnfylr(this)' >费用录入</a> &nbsp;&nbsp;<a style='color:#337ab7 !important;cursor:pointer' onclick='btnzcmx(this)' >物资明细</a>";
}
//详情
function btnxq(a) {
    var row = a.parentNode.parentNode;
    var row_model = $(row).GetRowData();
    if (row_model.estate_id == "" || row_model.estate_id==0)
    {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    Open('详情', '/ResourcManager/ResourcRelDetail?estate_id=' + row_model.estate_id + '');
}
//费用录入
function btnfylr(a) {
    var row = a.parentNode.parentNode;
    var row_model = $(row).GetRowData();
    if (row_model.rel_id == "" || row_model.rel_id == 0)
    {
        layer.msg("该资源暂无合同信息")
        return;
    }
    rel_id = row_model.rel_id;
    if (row_model.estate_status_name =="停用")
    {
        layer.confirm('该资源已经停用是否继续进行费用录入', {
            btn: ['是', '否'] //按钮
        }, function (index) {
            Open('费用录入', '/ResourcManager/CostContract', SetFy);
            layer.close(index);
            }, function (index) {
                layer.close(index);
        });
    } else {
        Open('费用录入', '/ResourcManager/CostContract', SetFy)
    }
}
function SetFy(v) {
    v.rel_id = rel_id;
}
//物资明细
var estate_name = "";
var estate_id = 0;
function btnzcmx(a) {
    var row = a.parentNode.parentNode;
    var row_model = $(row).GetRowData();

    estate_name = row_model.estate_name;
    estate_id = row_model.estate_id;
    Open("物资明细", "/SiteAssetsDetail/Index", setlookdetailsclick)
}
function setlookdetailsclick(win) {

    win.$("#estate_name").val(estate_name);
    win.$("#estate_id").val(estate_id);
    win.$("#btnsel").click();
}



layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //查询
    form.on("submit(btnsel)", function (data) {

        var param = data.field;
        gid.load(param);
        rowdata = {};
    });
})
//单位界面操作
function cus(v) {
    v.$("#btnlist").show();
    v.$("#btnshow").hide();
    v.$("#btnplay").hide();
    v.cus_name = "cus_name";
    v.cus_id = "cus_id";
    v.cus_mobile = "cus_mobile";
}
//去除条件
$("#clearCus").on("click", function () {
    $("#cus_name").val("");
    $("#cus_id").val("");
})

//窗体打开单位界面
var parent_from;
var p_estate_name;
var p_estate_id;
var p_estate_area;
var p_estate_address;
var p_surplus_estate_area;//新增场地剩余面积
var rent_out = "";//只有出租合同才可以对应多个合同

//弹出框
var vindex = 0;
if (typeof (parent.layer) != "undefined") {
    vindex = parent.layer.getFrameIndex(window.name); //获取窗口索引

}
//保存按钮
function vbtnsave() {
    rowdata = gid.GetSelTableRowRadioData();
    if (rowdata.length != 1) {
        layer.msg("请选中一条资源信息！");
        return;
    }
    if (parent_from==2)
    {
        parent.$("#estate_name").val();
        parent.$("#field_asset_id").val();

    } else
    {
        if (rent_out != "出租合同") {
            //2017-08-25租出资源可以存在多个合同
            if (rowdata[0].estate_status_name == "使用中") {
                parent.layer.msg("该资源已经存在合同不能再使用");
                return;
            }

        } else {
            if (rowdata[0].contract_type_name == "租入合同" && rowdata[0].estate_status_name == "使用中") {
                parent.layer.msg("该资源已经存在租入合同不能再使用");
                return;
            }
        }
        parent.$("#" + p_estate_name + "").val(rowdata.estate_name);
        parent.$("#" + p_estate_id + "").val(rowdata.estate_id);
        parent.$("#" + p_estate_area + "").val(rowdata.estate_area);
        parent.$("#" + p_estate_address + "").val(rowdata.estate_address);
        parent.$("#" + p_surplus_estate_area + "").val(rowdata.surplus_estate_area);

    }
    parent.layer.close(vindex);
}
//关闭按钮
function vbtncancel() {
    parent.layer.close(vindex);
}