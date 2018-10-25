var layer;
var laypage;
var form;
var element;
var estateFilePathlist = [];//文件对象
var rel_id=0;
//关闭窗体页面
var index;
if (typeof (parent.layer) != "undefined") {
    index = parent.layer.getFrameIndex(window.name); //获取窗口索引
}
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
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //自定义验证规则
    form.verify({
        float: function (value) {
            return validationNumber(value, 2);
        }
    });
    //监听提交
    form.on('submit(formDemo)', function (data) {
        if (rel_id <= 0)
        {
            layer.msg("需要录入费用的合同为空");
            return;
        }
        var obj = {};
        obj.costmodel = data.field;
        obj.costmodel.rel_id = rel_id;
        obj.filepathList = estateFilePathlist;
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ResourcManager/SaveContractCost";
        $.ajax({
            url: url,
            data: { model: obj },
            type: "post",
            success: function (e) {

                layer.msg(e.Msg, function () {
                    layer.close(daoru);
                    if (e.State == 1) {
                        if (parseInt(index) > 0) {
                            parent.layer.close(index);
                            parent.$("#btnsel").click();
                        } else {
                            layer.msg(e.Msg);
                        }

                    } else {
                        layer.msg(e.Msg);
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

layui.use('laydate', function () {
    var laydate = layui.laydate;

    var start = {
        max: '2099-06-16 23:59:59'
      , istoday: false
      , choose: function (datas) {
          end.min = datas; //开始日选好后，重置结束日的最小日期
          end.start = datas //将结束日的初始值设定为开始日
      }
    };

    var end = {
        max: '2099-06-16 23:59:59'
      , istoday: false
      , choose: function (datas) {
          start.max = datas; //结束日选好后，重置开始日的最大日期
      }
    };

    document.getElementById('cost_btime').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('cost_etime').onclick = function () {
        end.elem = this
        laydate(end);
    }

});
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
        , elem: '#iptsf' //指定原始元素，默认直接查找class="layui-upload-file"
      , method: 'post' //上传接口的http类型
      , success: function (res) { //上传成功后的回调
          var data = eval(res);
          layer.msg(data.msg);
          if (data.status = 1) {
              $("#water").append('<div style="display: inline-block; "   data-name="' + data.data + '"><img title="' + data.data + '" name="file_path" width="100px" height="100px" src="' + data.url + '" /><i class="layui-icon filedel">&#x1006;</i></div>');
              layer.photos({
                  photos: '#water'
                  , anim: 5
              });
              var estateFilePath = {};
              estateFilePath.file_path = data.url;
              estateFilePath.file_name = data.data;
              estateFilePath.file_type = 1;
              estateFilePathlist.push(estateFilePath);

          }
      }
    });

    layui.upload({
        url: '/FileUpload/GetImportData'
      , elem: '#iptdf' //指定原始元素，默认直接查找class="layui-upload-file"
      , method: 'post' //上传接口的http类型
      , success: function (res) {
          var data = eval(res);
          layer.msg(data.msg);
          if (data.status = 1) {
              $("#electricity").append('<div style="display: inline-block; "   data-name="' + data.data + '"><img title="' + data.data + '" name="file_path" width="100px" height="100px" src="' + data.url + '" /><i class="layui-icon filedel">&#x1006;</i></div>');
              layer.photos({
                  photos: '#electricity'
                  , anim: 5
              });
              var estateFilePath = {};
              estateFilePath.file_path = data.url;
              estateFilePath.file_name = data.data;
              estateFilePath.file_type = 2;
              estateFilePathlist.push(estateFilePath);

          }
      }
    });
});
//显示文件信息和图片信息
function SetFile(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].file_type == 1) {
            $("#water").append('<div style="display: inline-block;"   data-name="' + data[i].file_name + '"><img title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /><i class="layui-icon filedel">&#x1006;</i></div>');
            layer.photos({
                photos: '#water'
                , anim: 5
            });
            var estateFilePath = {};
            estateFilePath.file_path = data[i].file_path;
            estateFilePath.file_name = data[i].file_name;
            estateFilePath.file_type = 1;
            estateFilePathlist.push(estateFilePath);
        } else {
            $("#electricity").append('<div style="display: inline-block; "   data-name="' + data[i].file_name + '"><img title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /><i class="layui-icon filedel">&#x1006;</i></div>');
            layer.photos({
                photos: '#electricity'
                , anim: 5
            });
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
    var $this = $(this)[0].parentNode;
    var filename = $this.attributes["data-name"].value;
    //去除文件集合里面的元素
    estateFilePathlist.forEach(function (item, index) {
        if (item.file_name == filename) {
            estateFilePathlist.splice(index);
        }

    });
    $this.remove();
})
