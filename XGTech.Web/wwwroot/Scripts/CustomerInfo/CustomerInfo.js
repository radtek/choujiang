$(function () {
    if (location.search.indexOf("islyopen") > 0) {
        $(".diglog").show();
        $(".ndiglog").hide();

    } else {
        $(".ndiglog").show();
        $(".diglog").hide();
    }

})


var gid = table.get("usertable");

layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    gid.load();

    //查询
    form.on("submit(detailtable)", function (data) {

        var param = data.field;
        gid.load(param);
        rowdata = {};
    });
    //保存
    form.on("submit(save)", function (data) { 
        var rowdata = gid.GetSelTableRowData();
        if (rowdata.length == 0) {
            layer.msg("请选择要的数据");
            return;
        }
        parent.$("#cus_name").val(rowdata[0].cus_name); 
        parent.$("#cus_id").val(rowdata[0].cus_id);
        parent.$("#cus_mobile").val(rowdata[0].cus_mobile);

        parent.layer.close(parent.layer.getFrameIndex(window.name));
      
    });
    //删除
    form.on("submit(cancel)", function (data) {
        parent.layer.close(parent.layer.getFrameIndex(window.name));
       
    });


    //监听提交
    form.on('submit(exporttable)', function (data) {
        var param = data.field

        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });


        var usertable = table.get("usertable");
        usertable.Export("/CustomerInfo/ExportData", param, ["750px", "500px"],
            function () {
                layer.close(daoru);
            });
    });


})


$("#btncustomeradd").click(function () {
    //iframe窗
    layer.open({
        type: 2,
        title: "新增单位",
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['670px', '449px'],
        offset: 'ct', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/CustomerInfo/Save?sessionID='+sessionID, 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示

        },
        success: function (obj, index) {
        }
    });
})


function openedit(a) {
    var row = a.parentNode.parentNode;
    var rowobj = $(row).GetRowData();
    //iframe窗
    layer.open({

        title: "编辑单位",
        type: 2,
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['646px', '449px'],
        offset: 'ct', //右下角弹出
        //time: 2000, //2秒后自动关闭
        anim: 2,
        maxmin: true, //开启最大化最小化按钮
        content: ['/CustomerInfo/Save?sessionID=' + sessionID, 'yes'], //iframe的url，no代表不显示滚动条
        end: function () { //此处用于演示

        },
        success: function (obj, index) {
            var iframe = "layui-layer-iframe" + index;
            var v = window[iframe];
            v.SetData(rowobj);
        }
    });
}

function del(a) {
    var row = a.parentNode.parentNode;
    var rowobj = $(row).GetRowData();
    layer.open({
        title: '提示'
        , content: "确认删除数据"
        , btn: ['确定', '返回']
        //确定恢复原先的菜单数据
        , yes: function () {
            $.post('/CustomerInfo/DelCustomer', { ids: rowobj.cus_id }, function (data) {
                if (data.State==1) {
                    layer.msg("删除成功！");
                    $("#btnsel").click();

                } else {
                    layer.msg(data.Msg);
                }
            })
        }

    });
}


layui.use('table', function () {
    table = layui.table;
    //监听工具条
    table.on('tool(assetstable)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        rowdata = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象

        if (layEvent === 'detail') { //查看
            //do somehing
        } else if (layEvent === 'del') { //删除
            layer.confirm('真的删除行么', function (index) {
                obj.del(); //删除对应行（tr）的DOM结构，并更新缓存
                layer.close(index);
                var ids = rowdata.supp_id;

                var param = { ids: ids };

                $.ajax({
                    url: "/CustomerInfo/DelCustomer",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (e) {
                        if (e.State == 1) {
                            //layer.msg(e.Msg, function () {
                            //    $("#btnsel").click();
                            //});
                            layer.msg("删除成功！");
                            $("#btnsel").click();
                        } else {
                            layer.msg("删除失败！");
                        }

                        //layer.msg(e.Msg, function () {
                        //    $("#btnsel").click();
                        //});
                    },
                    error: function () {
                        layer.msg("系统异常请稍后再试");
                    }
                });

            });
        } else if (layEvent === 'edit') { //编辑
            //do something

            ////同步更新缓存对应的值
            //obj.update({
            //    username: '123'
            //    , title: 'xxx'
            //});
            Open('编辑', '/CustomerInfo/Save?sessionID=' + sessionID, Edit);
        }
    });
});

function Edit(v)
{
    v.SetData(rowdata);
}

$("#btnDel").on("click", function () {


    var rowdata = gid.GetSelTableRowData();
    if (rowdata.length == 0) {
        layer.msg("请选择要删除的数据");
        return;
    }

    layer.confirm('您确定要删除吗？', {
        btn: ['确定', '取消'] //按钮
    }, function () {

        var ids = [];

        $.each(rowdata, function (index, value) {
            ids.push(value.cus_id);
        })
        var param = { ids: ids };

        $.ajax({
            url: "/CustomerInfo/DelCustomer",
            data: param,
            type: "post",
            dataType:"json",
            success: function (e) {
                if (e.State == 1) {
                    //layer.msg(e.Msg, function () {
                    //    $("#btnsel").click();
                    //});
                    layer.msg("删除成功！");
                    $("#btnsel").click();
                } else {
                    layer.msg("删除失败！");
                }

                //layer.msg(e.Msg, function () {
                //    $("#btnsel").click();
                //});
            },
            error: function () {
                layer.msg("系统异常请稍后再试");
            }
        });
        }, function () { })

})
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
    var FileController = "/CustomerInfo/GetImportData";                    // 接收上传文件的后台地址
    var form = new FormData();
    form.append("file", fileObj);                           // 文件对象
    xhr = new XMLHttpRequest();
    xhr.open("post", FileController, true);
    xhr.onreadystatechange = zswFun;
    xhr.send(form);
};
//导入文件后操作
function zswFun() {
    if ((xhr.readyState == 4) && xhr.status == 200) {
        layer.close(daoru);
        var b = xhr.responseText;
        var result = eval("(" + b + ")");
        layer.msg(result.msg, function ()
        {
            $("#btnsel").click();
        });
    }

}


//窗体打开单位界面
var cus_name;
var cus_id;
var cus_mobile;
var vindex;
if (typeof (parent.layer) != "undefined") {
     vindex = parent.layer.getFrameIndex(window.name); //获取窗口索引

}
function vbtnsave() {
    var obj = $("#cusbody tr td input:checked");
    if (obj.length != 1) {
        layer.msg("请选中一位单位信息！");
        return;
    } else {
        $("#cusbody tr").each(function (index, item) {
            if ($(this).find("input:checked").length > 0) {
                parent.$("#" + cus_name + "").val(item.children[1].innerHTML);
                parent.$("#" + cus_id + "").val(item.children[8].innerHTML);
                parent.$("#" + cus_mobile + "").val(item.children[5].innerHTML);
            }
        });

    }
    parent.layer.close(vindex);
}
function vbtncancel() {
    parent.layer.close(vindex);
}

function getbtn() {
    return "<a class=\"layui-btn\" onclick=\"openedit(this)\">编辑</a>  <a class=\"layui-btn\" onclick='del(this)'>删除</a>";
}

function getaddress(e) {
    console.log(e);
}
