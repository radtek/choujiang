

layui.use(['laydate', 'form'], function () {
    var laydate = layui.laydate;
    laydate.render({
        elem: '#startmoy'
        , type: 'datetime'
    });
    //日期时间选择器
    laydate.render({
        elem: '#endmoy'
        , type: 'datetime'
    });

});

var usertable = table.get("usertable");

layui.use(['form', 'layedit', 'laydate', "layer"], function () {
    var form = layui.form;
    var layer = layui.layer
     , layedit = layui.layedit
     , laydate = layui.laydate;


    $("#clearproname").click(function () {
        $("#pro_name").val("");
    })
    //查询
    form.on('submit(btnsel)', function (data) {
        var moy = $("#startmoy").val();
        var end_moy = $("#endtmoy").val();
        var d1 = new Date(moy.replace(/\-/g, "\/"));
        var d2 = new Date(end_moy);
        if (moy != "" && end_moy != "" && d1 >= d2) {
            layer.msg("起始月份不得大于结束月份");
            return;
        }
        //var model = data.field;
        //model.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        //var param = { model: model, pageIndex: 1 };
        //SetDataTable("apptable", "mypage", param);
        var param = data.field;
        usertable.load(param);

    });
    //导出
    form.on('submit(btnexport)', function (data) {
        var param = data.field;
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        //var url = "/MonSubScription/ExportData";
        usertable.Export("/MonSubScription/ExportData", param, ["750px", "700px"],
            function () {
                layer.close(daoru);
            });
    });
    //导出
    form.on('submit(btnexports)', function (data) {
        var moy = $("#moy").val();
        var end_moy = $("#end_moy").val();
        var d1 = new Date(moy.replace(/\-/g, "\/"));
        var d2 = new Date(end_moy.replace(/\-/g, "\/"));
        if (moy != "" && end_moy != "" && d1 >= d2) {
            layer.msg("起始月份不得大于结束月份");
            return;
        }
        var model = data.field;
        model.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        ExportSh(model);
    });
    //审核状态
    form.on('select(status)', function (data) {
        state = data.value;
    });

});

function contains(arr, obj) {
    var i = arr.length;
    while (i--) {
        if (arr[i] === obj) {
            return true;
        }
    }
    return false;
} 

///新增采购
$("#pro_create").click(function () {
    layer.open({
        type: 2,
        title: "新增申购",
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['100%', '100%'],
        offset: 't', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/MonSubScription/AddNewMonSub?isrkopen=1', 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示
        }
    });
})
//审核
$("#btnconfirm").on("click", function () {
    var GetSelectRows = usertable.GetSelTableRowData();
    if (GetSelectRows.length == 0) {
        layer.msg("您未选中任何数据");
        return;
    }
    var ids = [];
    var statuslist = [];
    var fig = false;

    $.each(GetSelectRows, function (index, item) {
        var id = item["plan_puch_id"];
        var status = item["review_status_name"];
        statuslist.push(status);
        ids.push(id);
    })
    if (contains(statuslist, "通过")) {
        layer.msg("所选数据中存在已经审核不能再次审核");
        return false;
    }
    if (contains(statuslist, "不通过")) {
        layer.msg("所选数据中存在已经审核不能再次审核");
        return false;
    }
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/MonSubScription/UpdateState";
    $.ajax({
        url: url,
        data: { ids: ids,state:10 },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            layer.msg(e.Msg, function () {
                if (e.State == 1) {
                    $("#btnsel").click();
                }
            });

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
})

//删除 
$("#btnDel").on("click", function () {
    var GetSelectRows = usertable.GetSelTableRowData();
    if (GetSelectRows.length == 0) {
        layer.msg("您未选中任何数据");
        return;
    }

    var ids = [];
    var statuslist = [];
    var fig = false;
    layer.confirm('您确定要删除？', {
        btn: ['确定', '取消'] //按钮
    }, function () {
        $.each(GetSelectRows, function (index, item) {
            var id = item["plan_puch_id"];
            var status = item["review_status_name"];
            //if (status != "初始") {
            //    fig = true;
            //    layer.msg("所选数据中存在已经审核不能删除");
            //    return false;
            //}
            statuslist.push(status);
            ids.push(id);
        })
        if (contains(statuslist, "通过")) {
            layer.msg("所选数据中存在已经审核不能删除");
            return false;
        }
        if (contains(statuslist, "不通过")) {
            layer.msg("所选数据中存在已经审核不能删除");
            return false;
        }

        //发送请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/MonSubScription/UpdateDelfig";
        $.ajax({
            url: url,
            data: { ids: ids },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                layer.msg(e.Msg, function () {
                    if (e.State == 1) {
                        $("#btnsel").click();
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

//导出
function Export(model) {
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    //var url = "/MonSubScription/ExportData";
    usertable.Export("/MonSubScription/ExportData", param, ["750px", "700px"],
        function () {
            layer.close(daoru);
        });
    //$.ajax({
    //    url: url,
    //    data: { model: model },
    //    type: "post",
    //    success: function (e) {
    //        var data = eval(e);
    //        if (data.success) {
    //            location.href = decodeURI(data.filePath);
    //        } else {
    //            layer.msg("导出失败，原因：" + data.msg);
    //        }
    //        layer.close(daoru);
    //    },
    //    error: function () {
    //        layer.close(daoru);
    //        layer.msg("系统异常请稍后再试");
    //    }
    //});
}

//导出
function ExportSh(model) {

    gid.Export("/MonSubScription/ExportDataSh", model, ["750px", "700px"],
        function () {

        }, function () {
            layer.closeAll();
        });
}
//导入
function doImport() {
    $("#fileupload").val("");
    $("#fileupload").click();
}
var ld;
var daoru;
function dofileUpload() {
    var val = $("#fileupload").val();
    if (val == "") {
        layer.alert("请先选择文件！", { icon: 2 });
        return;
    }
    daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    $("#excelurl").val();//excelurl用法存放上传后的地址
    var fileObj = document.getElementById("fileupload").files[0]; // 获取文件对象
    var FileController = "/MonSubScription/GetImportData";                    // 接收上传文件的后台地址
    var form = new FormData();
    form.append("file", fileObj);                           // 文件对象
    xhr = new XMLHttpRequest();
    xhr.open("post", FileController, true);
    xhr.onreadystatechange = zswFun;
    xhr.send(form);
};
//导入文件后操作
var rowdatalist;
function zswFun() {
    if ((xhr.readyState == 4) && xhr.status == 200) {
        layer.close(daoru);
        var b = xhr.responseText;
        var result = eval("(" + b + ")");
        if (result.status == 1) {
            rowdatalist = result.data;
            //打开导入的详细信息页面
            Open('导入', '/MonSubScription/AddMonSub', SetDRInfo);

        } else {
            layer.msg(result.Msg);
        }
    }

}
//打开导入详细页面后的操作
function SetDRInfo(v)
{
    v.$("#selset").hide();
    //添加表格详细
    if (typeof (rowdatalist) != "undefined")
    {
        v.rowdatalist = rowdatalist;
        v.SetTableRow("detailtable", '', rowdatalist)
    }
    
}
//设置申请人
function SetEmp(v) {
    v.vemp_name = "app_person_name";
    v.vuser_id = "app_person_id";
    v.$("#btnlist").show();
    v.$("#export").hide();
}
function SetOrg(v) {
    v.org_name = "app_dept_name";
    v.org_id = "app_dept_id";
    v.$("#btnlist").show();
}

//去除条件
$("#clearUser").on("click", function () {
    $("#app_person_id").val("");
    $("#app_person_name").val("");
})
$("#clearOrg").on("click", function () {
    $("#app_dept_name").val("");
    $("#app_dept_id").val("");
})
//审核
var state = 10;
$("#btnsh").on("click", function () {
    if ($("#apptable tbody input[type=checkbox]:checked").length <= 0) {
        layer.msg("您未选中任何数据");
        return;
    }
    var ids = [];
  
    $("#apptable tbody input[type=checkbox]:checked").each(function (index, item) {
        var id = item.parentNode.parentNode.children[10].innerHTML;
        ids.push(id);
    });
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/MonSubScription/UpdateState";
    $.ajax({
        url: url,
        data: { ids: ids, state: state },
        type: "post",
        success: function (e) {

            layer.msg(e.Msg, function () {
                if (e.State == 1) {
                    layer.close(daoru);
                    $("#btnsel").click();
                }
            });

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });

})
$("#clearPro").on("click", function () {
    $("#pro_name").val("");
    document.getElementById("pro_name").setAttribute("data-procode", "");
    document.getElementById("pro_name").setAttribute("data-hidid", "");
})

//点击名称
$("#pro_name").click(function () {
    //iframe窗
    layer.open({
        type: 2,
        title: "商品信息",
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['100%', '100%'],
        offset: 't', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/AssetQuery/Index?isrkopen=1', 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示
        }
    });
})