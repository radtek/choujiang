

$(function () {

    layui.use(['layer', 'laypage', 'element','form'], function(){
        var layer = layui.layer
        ,laypage = layui.laypage
        , element = layui.element
        , form = layui.form;

        //初始化密码  
        $("#InitPwd").on("click", function () {
            var net_no = $("#mynet_no").text();
            var param = { net_no: net_no };
            $.post("/NetConfig/InitPwd", param, function (data) {
                if (data.State == 1) {
                    layer.msg(data.Msg, { icon: 6 });
                } else {
                    layer.msg(data.Msg);
                }
            })
        })

        //单机组，全选或者取消
        $(".menugroup").on("click", function () {
            var obj = $(this);
            $(this).parent().parent().find(".menuid input").each(function (index, item) {
                item.checked = obj[0].checked;
            })
        })

        var usermenulist = [];
        //保存
        $("#SavePermision").on("click", function () {
           
            var net_no = $("#mynet_no").text();
            //重新生成权限
            $(".menuid input[type=checkbox]:checked").each(function (index, item) {
                var usermen = {};
                usermen["user_id"] = $("#empno").val();
                usermen["menu_id"] = item.attributes["data-id"].value;
                usermen["net_no"] = $("#mynet_no").text();
                usermenulist.push(usermen);
            })
            //发送保存请求
            var daoru = layer.load(0, {
                shade: [0.1, '#676a6c'] //0.1透明度的白色背景
            });
            var url = "/NetConfig/AddUserMenu";
            $.ajax({
                url: url,
                data: { usermenulist: JSON.stringify(usermenulist), net_no: net_no },
                type: "post",
                success: function (e) {
                    layer.close(daoru);
                    layer.msg(e.Msg);
                    parent.$("#btnSel").click();
                }
            });
        })

        $("#CancelAdd").on("click", function () {
           parent.layer.closeAll();
        })


    })
  
});

