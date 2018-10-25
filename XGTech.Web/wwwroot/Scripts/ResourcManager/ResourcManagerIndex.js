
//窗体打开单位界面
var p_estate_name;
var p_estate_id;
var p_estate_area;
var p_estate_address;
var p_surplus_estate_area;//新增场地剩余面积
var rent_out = "";//只有租赁合同才可以对应多个合同
var vindex = 0;
if (typeof (parent.layer) != "undefined") {
    vindex = parent.layer.getFrameIndex(window.name); //获取窗口索引

}
function vbtnsave() {
    var obj = $("#tabbodyrk tr td input:checked");
    if (obj.length != 1) {
        layer.msg("请选中一条资源信息！");
        return;
    }
    parent.layer.close(vindex);
}
function vbtncancel() {
    parent.layer.close(vindex);
}


var estate_id = "";//资源ID
var rel_id = "";//关系ID
var cust_contract_id = "";
var stateinfo = "";//停用状态
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        estate_id = data.elem.parentNode.parentNode.children[15].innerText;
        rel_id = data.elem.parentNode.parentNode.children[16].innerText;
        cust_contract_id= data.elem.parentNode.parentNode.children[18].innerText;
        stateinfo = data.elem.parentNode.parentNode.children[1].innerText;
        contract_type_name = data.elem.parentNode.parentNode.children[4].innerText;
        if (vindex > 0) {

            if (rent_out != "租赁合同") {
                //2017-08-25租出资源可以存在多个合同
                if (stateinfo == "使用中") {
                    parent.layer.msg("该资源已经存在合同不能再使用");
                    return;
                }

            } else
            {
                if (contract_type_name == "租入合同" && stateinfo == "使用中") {
                    parent.layer.msg("该资源已经存在租入合同不能再使用");
                    return;
                }
            }



            parent.$("#" + p_estate_name + "").val(data.elem.parentNode.parentNode.children[5].innerHTML);
            parent.$("#" + p_estate_id + "").val(data.elem.parentNode.parentNode.children[14].innerHTML);
            parent.$("#" + p_estate_area + "").val(data.elem.parentNode.parentNode.children[10].innerHTML);
            parent.$("#" + p_estate_address + "").val(data.elem.parentNode.parentNode.children[16].innerHTML);
            parent.$("#" + p_surplus_estate_area + "").val(data.elem.parentNode.parentNode.children[18].innerHTML);
        }

    });

})




//查询
$("#btnsel").on("click", function () {
    layer.closeAll();
    //加载表格数据
    btnhtml = '<td style="text-align:center;"><button  class="layui-btn btnxq">详情</button><button  class="layui-btn btnfylr">费用录入</button></td>';
    var pageIndex = 1;
    var estate_name = $("#estate_name").val();
    var param = { pageIndex: pageIndex, estate_name: estate_name };
    SetDataTable("assetstable", "mypage", param, btnhtml);
    estate_id = "";
})


//编辑

$("#btnupdate").on("click", function () {
    if (estate_id == "") {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    Open('编辑', '/ResourcManager/Save', Edit)

})


function Edit(v) {

    //请求数据
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/ResourcManager/GetResourcDetail";
    $.ajax({
        url: url,
        data: { estate_id: estate_id },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            v.SetData(data.estateModel);
            v.SetFile(data.estateFilePath);

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
}
//删除

$("#btnDel").on("click", function () {
    if (estate_id == "") {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    if (rel_id == "" || parseInt(rel_id) > 0) {
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
        data: { estate_id: estate_id },
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

$("#btntz").on("click", function () {
    if (estate_id == "") {
        layer.msg("资源不存在请选中要操作的资源信息");
        return;
    }
    if (rel_id == "" || rel_id == 0) {
        layer.msg("该资源没有合同信息");
        return;
    }
    if (stateinfo == "停用")
    {
        layer.msg("该资源为停用状态");
        return;
    }
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
            data: { estate_id: estate_id, cust_contract_id: cust_contract_id },
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
//费用录入
var rel_id;
$("body").on("click", ".btnfylr", function () {
    var tr = $(this)[0].parentNode.parentNode;
    rel_id = tr.children[16].innerText;
    var status = tr.children[19].innerText;
    if (rel_id == "0" || rel_id == "") {
        layer.msg("该资源暂无合同信息")
        return;
    }
    if (status == "已退租") {
        layer.confirm('该资源已经停用是否继续进行费用录入', {
            btn: ['是', '否'] //按钮
        }, function () {
            Open('费用录入', '/ResourcManager/ContractCost', SetFy);
        }, function () {

        });
    } else {
        Open('费用录入', '/ResourcManager/ContractCost', SetFy)
    }


})
function SetFy(v) {
    v.rel_id = rel_id;
}
//详情页面
var xqestate_id = 0;
$("body").on("click", ".btnxq", function () {
    var tr = $(this)[0].parentNode.parentNode;
    xqestate_id = tr.children[15].innerText;
    if (xqestate_id == "0" || xqestate_id == "") {
        layer.msg("资源信息出错")
        return;
    }
    Open('详情', '/ResourcManager/ResourcRelDetail?estate_id=' + xqestate_id + '');

})
