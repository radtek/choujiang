$(document).ready(function () {
    //获取图形验证码
    $("#valid_img").click(function () {
        $("#valid_img").attr("src", "/VerifyCode/index?rnd=" + Math.random());
    });
    // 回车键事件 
    // 绑定键盘按下事件  
    $(document).keypress(function (e) {
        // 回车键事件  
        if (e.which == 13) {
            $("#loginBtn").click();
        }
    });
});
layui.use('form', function () {
    var form = layui.form;
    form.on('submit(sub_login)', function (data) {
        //发送保存请求
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/User/UserLogin";
        $.ajax({
            url: url,
            data: { Net_no: data.field.Net_no, UserName: data.field.UserName, Pwd: data.field.Pwd, verifyCode: data.field.verifyCode },
            type: "post",
            success: function (data) {
                layer.close(daoru);
                var e = eval(data);
                if (e.status == "200") {
                    location.href = e.url;
                } else {
                    layer.msg(e.msg);
                    $("#valid_img").click();
                }
            }
        });
    });
});