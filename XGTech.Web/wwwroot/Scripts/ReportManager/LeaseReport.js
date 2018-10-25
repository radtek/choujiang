
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

    document.getElementById('contract_effectivedate').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('contract_enddate').onclick = function () {
        end.elem = this
        laydate(end);
    }

});


function getbtn(t, data) {
    return '<input estate_id="' + data.estate_id + '" estate_name="' + data.estate_name + '" type="button"  class="layui-btn lookdetailsc"  value="查看" />';
}

var estate_name = "";
var estate_id = 0;
$("body").on("click", ".lookdetailsc", function () {
    estate_name = $(this).attr("estate_name");
    estate_id = $(this).attr("estate_id");
    Open("物资明细", "/SiteAssetsDetail/Index", setlookdetailsclick)
})


//查看按钮
function setlookdetailsclick(win) {

    win.$("#estate_name").val(estate_name);
    win.$("#estate_id").val(estate_id);
    win.$("#btnsel").click();
}

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


    //监听提交
    form.on('submit(exporttable)', function (data) {
        var param = data.field
     
        //导出
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });


        var usertable = table.get("usertable");
        usertable.Export("/ReportManager/ExportLeasList", param, ["750px", "500px"],
        function () {
            layer.close(daoru);
        });
    });

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

//设置负责人信息
function SetFZREmp(v) {
    v.vemp_id = "emp_id";
    v.vemp_name = "emp_name";
    v.$("#btnlist").show();
    v.$("#export").hide();
}
//设置物资所属单位
function SetAssetOrg(v) {
    v.org_name = "org_name";
    v.org_id = "org_id";
    v.$("#btnlist").show();
}
//去除条件
$("#clearUser").on("click", function () {
    $("#cus_name").val("");
    $("#cus_id").val("");
})
$("#clearOrg").on("click", function () {
    $("#org_id").val("");
    $("#org_name").val("");
})
