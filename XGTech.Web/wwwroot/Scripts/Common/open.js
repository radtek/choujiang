
var layer;
var laypage;
var form;
var element;
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
})

//弹出层
function Open(title,content,setdata,colseifram, area)
{
    var param = {
        type: 2,//0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）
        title: title,
        closeBtn: 1, //不显示关闭按钮
        shade: [0],
        area: ['100%', '100%'],
        offset: 't', //右下角弹出
        maxmin: false, //开启最大化最小化按钮
        //time: 2000, //2秒后自动关闭
        anim: 2,
        content: ['' + content + '', 'yes'], //iframe的url，no代表不显示滚动条
        success: function (obj, index) {
            var iframe = "layui-layer-iframe" + index;
            var v = window[iframe];
            //操作子窗体
            if (typeof (setdata) != "undefined" && setdata != null) {
                setdata(v);
            }

        },
        cancel: function () {

        },
        yes: function () {

        },
        end: function () {

            if (typeof (colseifram) != "undefined" && colseifram != null) {
                colseifram();
            }
        }
    };
    if (area)
    {
        param.area = area;
    }
    layer.open(param);
}


