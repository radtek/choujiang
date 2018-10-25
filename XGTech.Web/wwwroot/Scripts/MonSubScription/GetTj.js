var gid = table.get("gid");

layui.use('laydate', function () {
    var laydate = layui.laydate;

    //执行一个laydate实例
    laydate.render({
        elem: '#moy'//指定元素
        , type: 'month'
    });

    //执行一个laydate实例
    laydate.render({
        elem: '#end_moy'//指定元素
        , type: 'month'
    });
});



layui.use(['form', 'layedit', 'laydate', "layer"], function () {
    var form = layui.form;
    var layer = layui.layer
     , layedit = layui.layedit
     , laydate = layui.laydate;



    //查询
    form.on('submit(btnsel)', function (data) {
        var moy = $("#moy").val();
        var end_moy = $("#end_moy").val();
        var d1 = new Date(moy.replace(/\-/g, "\/"));
        var d2 = new Date(end_moy.replace(/\-/g, "\/"));
        if (moy != "" && end_moy != "" && (d1.getYear() > d2.getYear() || d1.getMonth() > d2.getMonth())) {
            layer.msg("起始月份不得大于结束月份");
            return;
        }
        var model = data.field;
        model.startmoy = moy;
        model.endmoy = end_moy;
        model.prod_id = document.getElementById("pro_name").attributes["data-hidid"].value;
        gid.load(model);
    });
    //导出
    form.on('submit(btnexport)', function (data) {
        var model = data.field;
        model.startmoy = moy;
        model.endmoy = end_moy;
        Export(model);
    });
    //导出
    form.on('submit(exporttable)', function (data) {
        var moy = $("#moy").val();
        var end_moy = $("#end_moy").val();
        var d1 = new Date(moy.replace(/\-/g, "\/"));
        var d2 = new Date(end_moy.replace(/\-/g, "\/"));
        if (moy != "" && end_moy != "" && d1 > d2) {
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
//删除
$("#btnDel").on("click", function () {
    if ($("#apptable tbody input[type=checkbox]:checked").length <= 0) {
        layer.msg("您未选中任何数据");
        return;
    }
    var ids = [];
    var fig = false;
    $("#apptable tbody input[type=checkbox]:checked").each(function (index, item) {
        var id = item.parentNode.parentNode.children[10].innerHTML;
        var status = item.parentNode.parentNode.children[9].innerHTML;
        if (status != "初始")
        {
            fig = true;
            layer.msg("所选数据存在已经审核不能删除");
            return false;
        }
        ids.push(id);
    });
    if (fig)
    {
        return;
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

})

//导出
function Export(model) {
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/MonSubScription/ExportData";
    $.ajax({
        url: url,
        data: { model: model },
        type: "post",
        success: function (e) {
            layer.msg(e.msg, function () {
                layer.close(daoru);
                if (e.status == 200) {
                  
                    var filename = e.data;
                    var durl = "/DownExcel/" + filename;
                    location.href = durl;
                }
            });

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
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
            layer.msg(result.msg);
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
        area: ['993px', '600px'],
        offset: 'ct', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/AssetQuery/Index?isrkopen=1', 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示
        }
    });
})