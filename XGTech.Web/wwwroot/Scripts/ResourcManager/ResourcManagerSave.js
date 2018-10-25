var layer;
var laypage;
var form;
var element;
var estateFilePathlist = [];//文件对象
//关闭窗体页面
var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
}
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //监听提交
    form.on('submit(formDemo)', function (data) {
        var obj = {};
        obj.estateModel = data.field;
        obj.estateFilePath = estateFilePathlist;

        if (!verify("#estate_name")) {
            return;
        }
        if (!verify("#emp_name"))
        {
            return;
        }
        if (!verify("#emp_mobile")) {
            return;
        }
        if (!verify("#estate_address")) {
            return;
        }
        if (!verify("#estate_area")) {
            return;
        }


        var reg = /^[0-9]+$/;
        if (!reg.test(data.field.emp_mobile) || data.field.emp_mobile.length != 11)
        {
            layer.msg("负责人手机只允许允许输入11位数字");
            return;
        }
        if (data.field.emp_phone!="")
        {
            if (!reg.test(data.field.emp_phone)) {
                layer.msg("联系电话只允许输入数字");
                return;
            }
        }
        if (data.field.estate_per_num!="")
        {
            if (!reg.test(data.field.estate_per_num)) {
                layer.msg("驻点操作员数只允许输入数字");
                return;
            }
        }

        
        if (!reg.test(data.field.estate_area)) {
            layer.msg("面积只允许输入数字");
            return;
        }
        if (data.field.fire_emp_phone!="")
        {
            if (!reg.test(data.field.fire_emp_phone) || data.field.fire_emp_phone.length != 11) {
                layer.msg("消防责任人电话只允许允许输入11位数字");
                return;
            }
        }


        //if (data.field.estate_per_num.length > 8)
        //{
        //    layer.msg("操作员数过大不能超过8位数");
        //    return;
        //}
        if (parseFloat(data.field.estate_area) <= 0)
        {
            layer.msg("面积要大于0");
            return;
        }
        //if (data.field.estate_area.length > 8) {
        //    layer.msg("面积过大不能超过8位数");
        //    return;
        //}
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ResourcManager/Save";
        $.ajax({
            url: url,
            data: { model: obj },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                layer.msg(e.Msg, function () {
                    
                    if (e.State == 1) {
                        if (parseInt(index) > 0) {
                            parent.layer.close(index);
                            parent.$("#btnsel").click();
                        } else {
                           // layer.msg(e.Msg);
                        }

                    } else {
                       // layer.msg(e.Msg);
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


$(function () {
    //取消
    $("#btncancel").on("click", function () {
        parent.layer.close(index);
    })

})

//上传功能
layui.use('upload', function (e) {
    layui.upload({
        url: '/FileUpload/GetImportData' //上传接口
        , elem: '#imgfile' //指定原始元素，默认直接查找class="layui-upload-file"
      , method: 'post' //上传接口的http类型
      , success: function (res) { //上传成功后的回调
          var data = eval(res);
          layer.msg(data.msg);
          if (data.status = 1) {

              $("#assetimg").append('<div class="gallery-item" style="display: inline-block;margin-right:10px; "   data-name="' + data.url + '"><img layer-src="' + data.url + '" title="' + data.othermsg + '" name="file_path" width="100px" height="100px" src="' + data.url + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
              //layer.photos({
              //    photos: '#assetimg'
              //     , anim: 5,
              //     closeBtn: 1
              //});
              $('.gallery-item').ma5gallery({
                  preload: true,
                  fullscreen: false
              });
              var estateFilePath = {};
              estateFilePath.file_path = data.url;
              estateFilePath.file_name = data.othermsg;
              estateFilePath.file_type = 1;
              estateFilePathlist.push(estateFilePath);

          }
      }
    });

    layui.upload({
        url: '/FileUpload/GetImportData'
      , elem: '#yewufile' //指定原始元素，默认直接查找class="layui-upload-file"
      , method: 'post' //上传接口的http类型
      , success: function (res) {
          var data = eval(res);
          layer.msg(data.msg);
          if (data.status = 1) {
              $("#fileslist").append('<div style="display:inline-block; margin-top:10px;"   data-name="' + data.url + '"><input type="hidden" name="file_path"  value="' + data.url + '" /><i title="' + data.othermsg + '" class="layui-icon">&#xe621;</i><i class="layui-icon filede2">&#x1006;</i></div>');
              var estateFilePath = {};
              estateFilePath.file_path = data.url;
              estateFilePath.file_name = data.othermsg;
              estateFilePath.file_type = 2;
              estateFilePathlist.push(estateFilePath);
          }
      }
    });
});
//操作子页面 v子页面对象
//机构信息
function SetOrgInfo(v) {
    v.org_name = "org_name"
    v.org_id = "org_id";
}
//负责人
function SetZREmp(v) {
    v.$("#btnlist").show();
    v.vemp_name = "emp_name";
    v.vemp_id = "emp_id";
    v.vorg_name = "org_name";
    v.vorg_id = "org_id";
    v.$("#export").hide();
    v.$("#btnlist").show();
    v.$("#btnsave").hide();
    v.$("#resetpwd").hide();
}
//负责人回调函数
function colseifram()
{
    verify("#emp_name");
}
//消防负责人
function SetXFEmp(v) {
    v.$("#btnlist").show();
    v.vemp_name = "fire_emp_name";
    v.vemp_id = "fire_emp_id";
    v.$("#export").hide();
    v.$("#btnlist").show();
    v.$("#btnsave").hide();
    v.$("#resetpwd").hide();
}
//显示文件信息和图片信息
function SetFile(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].file_type == 1) {
            $("#assetimg").append('<div class="gallery-item" style="display:inline-block;margin-right:10px;"  data-name="' + data[i].file_path + '"><img title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /></div><i class="layui-icon filedel">&#x1006;</i>');
            //layer.photos({
            //    photos: '#assetimg'
            //    , anim: 5
            //});
            $('.gallery-item').ma5gallery({
                preload: true,
                fullscreen: false
            });
            var estateFilePath = {};
            estateFilePath.file_path = data[i].file_path;
            estateFilePath.file_name = data[i].file_name;
            estateFilePath.file_type = 1;
            estateFilePathlist.push(estateFilePath);
        } else {
            $("#fileslist").append('<div  style="display:inline-block;margin-top:10px;"  data-name="' + data[i].file_path + '"><input type="hidden" name="file_path"  value="' + data[i].file_path + '" /><i title="' + data[i].file_name + '" class="layui-icon">&#xe621;</i><i class="layui-icon filede2">&#x1006;</i></div>');
            var estateFilePath = {};
            estateFilePath.file_path = data[i].file_path;
            estateFilePath.file_name = data[i].file_name;
            estateFilePath.file_type = 2;
            estateFilePathlist.push(estateFilePath);
        }
    }
}
//删除文件信息
$("body").on("click", ".filedel", function () {
    var $this = $(this)[0].previousSibling;
    var file_path = $this.attributes["data-name"].value;
    //去除文件集合里面的元素
    estateFilePathlist.forEach(function (item, index) {
        if (item.file_path == file_path) {
            estateFilePathlist.splice(index);
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
            estateFilePathlist.splice(index);
        }

    });
    $this.remove();
})

//验证方法
function verify(a) {
    var sss = $(a).next();
    if ($(a).val().trim() == "") {
        $(sss).removeAttr("hidden");
        return false;
    } else {
        $(sss).attr("hidden", "hidden");
        return true;
    }
}
