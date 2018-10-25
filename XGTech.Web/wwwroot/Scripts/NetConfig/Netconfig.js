


var tables = table.get("netstable");
function getbtn(v) {
    return "<input  type='button' class='layui-btn' value='分配' onclick='openwindow(this)'/>";  //onclick='Open('权限分配', '/user/UserManager')'
}

function openwindow(e) {
    var net_no = $(e).parent().parent().children('td').eq(1).text();
    var openedit = layer.open({
        type: 2,
        title: "",
        anim: 2,
        maxmin: false, //开启最大化最小化按钮
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        content: ['/NetConfig/ConfigNet?net_no=' + net_no, 'yes'],
        area: ['80%', '80%'],
    });
}

function GetCheckItems() {
    var list = new Array();
    $("#netstable [type=checkbox]:checked").each(function () {
        var item = $(this).parent().parent().children('td').eq(1).text();
        list.push(item);
    })
    return list;
}

//删除网点
function DeleteNet()
{
    //先判断是否选中一条数据
    var rowdata = tables.GetSelTableRadioRowData();
    if (rowdata.length==0)
    {
        layer.msg("请先选中一行");
        return false;
    }
    layer.msg('确定要删除吗？', {
        time: 0 //不自动关闭
                 , btn: ['确认', '取消']
                 , yes: function (index) {
                     layer.close(index);
                     $.post("/NetConfig/ConfirmDelNet", { net_no: rowdata[0].net_no }, function (data) {
                         if (data.State == 1) {
                             layer.msg("删除成功", { icon: 6 });
                             parent.$("#btnSel").click();
                             parent.layer.closeAll();
                         } else {
                             layer.msg(data.Msg);
                         }
                     })
                 }
    });
}


$(function () {
   
    
    $("#btnSel").on("click", function () {
        var netinfo = $("#netinfo").val();
        var param = { netinfo: netinfo };
        tables.load(param);
       
    })

    $('#pro_create').click(function open() {
        var openedit = layer.open({
            type: 2,
            title: "新增",
            anim: 2,
            content: ['/NetConfig/AddNet', 'yes'],
            area: ['800px', '450px']
        });
    })

    $('#pro_edit').click(function open() {
        //先判断是否选中一条数据
        var rowdata = tables.GetSelTableRadioRowData();
        if (rowdata.length == 0) {
            layer.msg("请先选中一行");
            return false;
        }
        var net_no = rowdata[0].net_no;
        var openedit = layer.open({
            type: 2,
            title: "编辑",
            anim: 2,
            //content: $('#layersupp'),
            content: ['/NetConfig/EditNet?net_no=' + net_no, 'yes'],
            area: ['800px', '450px']
        });
    })
  
});
