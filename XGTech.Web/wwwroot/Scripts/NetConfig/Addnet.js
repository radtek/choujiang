
$(function () {

    //新增网点  
    $("#AddNetbtn").on("click", function () {
        var net_no=$("#net_no").val();
        var net_name = $("#net_name").val();
        var contact_name = $("#contact_name").val();
        var net_phone = $("#net_phone").val();
        var address = $("#address").val();
        var del_flag = $("#delfig option:selected").val();
        var fig = false;
        if (parseInt(del_flag) == 1)
        {
            fig = true;
        }
        if ("" == net_no || "" == net_name || contact_name==""||net_phone==""||address=="")
        {
            layer.msg("必填项不能为空！");
            return false;
        }
        var param = { net_no: net_no, net_name: net_name, contact_name: contact_name, net_phone: net_phone, address: address, del_flag: fig };
        $.post("/NetConfig/AddNetcom", param, function (data) {
            if (data.State == 1) {
                layer.msg(data.Msg, { icon: 6 });
                parent.$("#btnSel").click();
                parent.layer.closeAll();
            } else {
                layer.msg(data.Msg);
            }
        })
    })


    $("#CancelAdd").on("click", function () {
        parent.layer.closeAll();
    })
});
