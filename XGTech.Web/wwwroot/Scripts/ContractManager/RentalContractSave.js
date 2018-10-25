var layer;
var laypage;
var form;
var element;
//关闭窗体页面
var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
}
wooHideAll();
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var regj = /^\d+(\.\d{1,2})?$/
        var estate_area = $.trim($("#estate_area").val());
        //var surplus_estate_area = $.trim($("#surplus_estate_area").val());
        //if (surplus_estate_area <= 0) {
        //    layer.msg("该场地无剩余面积");
        //    return;
        //}

        if (estate_area.length < 0) {
            layer.msg("请输入面积");
            return;
        }
        if (parseFloat(estate_area) <= 0) {
            layer.msg("面积要大于0");
            return;
        }
        if (!regj.test(estate_area)) {
            layer.msg("面积请输入浮点数,精确到小数点后面两位");
            return;
        }
        var cust_contract_id = $("#cust_contract_id").val();

        //if (cust_contract_id == "" || typeof (cust_contract_id) == "undefined") {
        //    if (parseFloat(estate_area) > parseFloat(surplus_estate_area)) {
        //        layer.msg("该场地剩余面积不足");
        //        return;
        //    }
        //}
        var obj = {};
        var rel = {};
        rel.estate_id = data.field.estate_id;
        rel.rel_id = data.field.rel_id;
        obj.contract = data.field;
        obj.filelist = estateFilePathlist;
        obj.rel = rel;
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ContractManager/RentalContractSave";
        $.ajax({
            url: url,
            data: { model: obj },
            dataType: "json",
            type: "post",
            success: function (e) {
                layer.close(daoru);
                layer.msg(e.Msg);
                if (e.State == 1) {
                    if (parseInt(index) > 0) {
                        parent.layer.close(index);
                        parent.$("#btnSel").click();
                    }
                }
            },
            error: function (e) {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });

    });
    //自定义验证规则
    form.verify({
        float: function (value) {
            return validationNumber(value, 2);
        }
    });


})
//验证字符串数字
function validationNumber(value, num) {
    var regu = /^[0-9]+\.?[0-9]*$/;
    if (value != "") {
        if (!regu.test(value)) {
            return "请输入正确的数字";
        }
    } else {
        return '必填项不能为空';
    }
}
function wooShow(className, type) {
    if (type == 1)
    { $("#" + className).css("visibility", "visible"); }
    else {
        $("#" + ($("#" + className).parents().next()[0].id)).css("visibility", "visible");
        //$("#" + className).nextSbiling.css("visibility", "visible");
    }

}
function wooHide(className, type) {
    if (type == 1)
    { $("#" + className).css("visibility", "hidden"); }
    else
    { $("#" + ($("#" + className).parents().next()[0].id)).css("visibility", "hidden"); }

}
function wooHideAll() {
    var cnList = ['woo-qs', 'woo-zj', 'woo-yj', 'woo-khmc', 'woo-khsj', 'woo-zymc', 'woo-mj'];
    for (var i = 0; i < cnList.length; i++) {
        wooHide(cnList[i], 1);
    }
}

function onblur1(e) {
    if (!amount(e)) {
        $("#" + e.id).focus();
        wooShow(e.id, 0);
    }
    else {
        wooHide(e.id, 0);
    }
}

layui.use('laydate', function () {
    var laydate = layui.laydate;

    var start = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , isclear: false
        , choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
        }
    };

    var end = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , isclear: false
        , choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
        }
    };

    document.getElementById('contract_effectivedate').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('contract_enddate').onclick = function () {
        end.elem = this
        laydate(end);
    }

});
//上传功能
var estateFilePathlist = [];//文件对象
layui.use('upload', function (e) {
    layui.upload({
        url: '/FileUpload/GetImportData' //上传接口
        , elem: '#contractfile' //指定原始元素，默认直接查找class="layui-upload-file"
        , method: 'post' //上传接口的http类型
        , success: function (res) { //上传成功后的回调
            var data = eval(res);
            layer.msg(data.msg);
            if (data.status = 1) {
                $("#contractimg").append('<div class="gallery-item" style="display: inline-block; margin-right:10px;"   data-name="' + data.url + '"><img title="' + data.othermsg + '" name="file_path" width="100px" height="100px" src="' + data.url + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
                //layer.photos({
                //    photos: '#contractimg'
                //    , anim: 5
                //});
                $('.gallery-item').ma5gallery({
                    preload: true,
                    fullscreen: false
                });
                var estateFilePath = {};
                estateFilePath.file_path = data.url;
                estateFilePath.file_name = data.othermsg;
                estateFilePath.file_type = 1;//合同照片
                estateFilePathlist.push(estateFilePath);

            }
        }
    });

    layui.upload({
        url: '/FileUpload/GetImportData'
        , elem: '#oldfile' //指定原始元素，默认直接查找class="layui-upload-file"
        , method: 'post' //上传接口的http类型
        , success: function (res) {
            var data = eval(res);
            layer.msg(data.msg);
            if (data.status = 1) {
                $("#oldtimg").append('<div class="gallery-item" style="display: inline-block; margin-right:10px;"   data-name="' + data.url + '"><img title="' + data.othermsg + '" name="file_path" width="100px" height="100px" src="' + data.url + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
                //layer.photos({
                //    photos: '#oldtimg'
                //    , anim: 5
                //});
                $('.gallery-item').ma5gallery({
                    preload: true,
                    fullscreen: false
                });
                var estateFilePath = {};
                estateFilePath.file_path = data.url;
                estateFilePath.file_name = data.othermsg;
                estateFilePath.file_type = 2;//使用前照片
                estateFilePathlist.push(estateFilePath);
            }
        }
    });
});
//删除文件信息
$("body").on("click", ".filedel", function () {
    var $this = $(this)[0].previousSibling;
    var file_path = $this.attributes["data-name"].value;
    //去除文件集合里面的元素
    estateFilePathlist.forEach(function (item, index) {
        if (item.file_path == file_path) {
            estateFilePathlist.splice(index, index + 1);
        }

    });
    $(this)[0].remove();
    $this.remove();
})

$("body").on("click", ".filede2", function () {
    var $this = $(this)[0].parentNode;
    var file_path = $this.attributes["data-name"].value;
    //去除文件集合里面的元素
    estateFilePathlist.forEach(function (item, index) {
        if (item.file_path == file_path) {
            estateFilePathlist.splice(index, index + 1);
        }

    });
    $this.remove();
})


$(function () {
    //取消
    $("#btncancel").on("click", function () {
        parent.layer.close(index);
    })

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
//资源界面
function estate(v) {
    v.$("#btnplay").hide();
    v.$("#btnlist").show();
    v.$("#btnshow").hide();
    v.p_estate_name = "estate_name";
    v.p_estate_id = "estate_id";
    v.status = 1;
    //v.p_surplus_estate_area = "surplus_estate_area";
    v.p_estate_address = "estate_address";
    v.rent_out = "租赁合同";
    var param = { status: 1 };

    v.gid.load(param);
}

//显示文件信息和图片信息
function SetFile(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].file_type == 1) {
            $("#contractimg").append('<div class="gallery-item" style="display: inline-block;margin-right:10px; "   data-name="' + data[i].file_path + '"><img title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
            //layer.photos({
            //    photos: '#contractimg'
            //    , anim: 5
            //});
            $('.gallery-item').ma5gallery({
                preload: true,
                fullscreen: false
            });
            var estateFilePath = {};
            estateFilePath.file_path = data[i].file_path;
            estateFilePath.file_name = data[i].file_name;
            estateFilePath.file_type = 1;//合同照片
            estateFilePathlist.push(estateFilePath);
        } else {
            $("#oldtimg").append('<div class="gallery-item" style="display: inline-block;margin-right:10px; "   data-name="' + data[i].file_path + '"><img title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
            //layer.photos({
            //    photos: '#oldtimg'
            //    , anim: 5
            //});
            $('.gallery-item').ma5gallery({
                preload: true,
                fullscreen: false
            });
            var estateFilePath = {};
            estateFilePath.file_path = data[i].file_path;
            estateFilePath.file_name = data[i].file_name;
            estateFilePath.file_type = 2;//使用前照片
            estateFilePathlist.push(estateFilePath);
        }
    }
}
function amount(th) {
    if (th.value == "")
    { return true; }
    var regStrs = [
        ['^0(\\d+)$', '$1'], //禁止录入整数部分两位以上，但首位为0
        ['[^\\d\\.]+$', ''], //禁止录入任何非数字和点
        ['\\.(\\d?)\\.+', '.$1'], //禁止录入两个以上的点
        ['^(\\d+\\.\\d{2}).+', '$1'] //禁止录入小数点后两位以上
    ];
    for (i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        th.value = th.value.replace(reg, regStrs[i][1]);
    }
    if (th.value == "")
    { return false; }

    return true;

}