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

    document.getElementById('start_time').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('end_time').onclick = function () {
        end.elem = this
        laydate(end);
    }

});


var estate_id = "";//资源ID
var rel_id = "";
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        estate_id = data.elem.parentNode.parentNode.children[5].innerText;
        rel_id = data.elem.parentNode.parentNode.children[6].innerText;
    });

})

//查询
$("#btnsel").on("click", function () {
    //加载表格数据
    var pageIndex = 1;
    var estate_name = $("#estate_name").val();
    var param = { pageIndex: pageIndex, estate_name: estate_name };
    SetDataTable("assetstable", "mypage", param);
    estate_id = 0;
    rel_id = 0;
})
$("#btndetailsel").on("click", function ()
{
    if (rel_id == "" || rel_id==0)
    {
        layer.msg("该资源暂无合同信息");
        return;
    }
    var start_time = $("#start_time").val();
    var end_time = $("#end_time").val();
    var param = {start_time:start_time,end_time:end_time, rel_id: rel_id};
    SetDataTable("costtable", "", param);
})