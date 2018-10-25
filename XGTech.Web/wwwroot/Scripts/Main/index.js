var yzcount = 0;
var dzcount = 0;

layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //加载比例
    var param = { pageIndex: pageIndex, orgId: 0 };
    SetDataTable("tjtable", "",param, "", TJ);
    //加载合同
    var pageIndex = 1;
    var estate_name = $("#estate_name").val();
     param = { pageIndex: pageIndex, estate_name: estate_name };
    SetDataTable("assetstable", "mypage", param);
    //加载即将到期
    param = { estate_name: estate_name, pageIndex: pageIndex };
    SetDataTable("dqtable", "page", param, "", dq);
    //下拉框切换
    form.on("select(qiehuan)", function (e) {

        var param = { pageIndex: pageIndex, orgId: e.value };
        SetDataTable("tjtable", "", param, "", TJ);
    });
})
//统计
function TJ(data) {
    yzcount = data.list[0].yzcount;
    dzcount = data.list[1].dzcount;
    var data = [
{
    value: dzcount,
    color: "#F38630"
},
{
    value: yzcount,
    color: "#E0E4CC"
}
    ]
    var height = 500 * window.devicePixelRatio;
    var width = 500 * window.devicePixelRatio;
    var ctx = document.getElementById("myChart").getContext("2d");
    var myNewChart = new Chart(ctx).PolarArea(data);
    document.getElementById("myChart").height = height;
    document.getElementById("myChart").width  = width ;
    new Chart(ctx).Pie(data);
}

//到期
function dq(data) {
    if (data.list.length > 0) {
        $("#tx").show();
    }
}
//查询
$("#btnsel").on("click", function () {
    //加载表格数据
    var pageIndex = 1;
    var estate_name = $("#estate_name").val();
    var param = { pageIndex: pageIndex, estate_name: estate_name };
    SetDataTable("assetstable", "mypage", param);
})
