

var formclass = "";//需要获取哪个DIV下的元素
//获取页面所有input
var inputlist;
//获取页面所有textarealist
var textarealist;
if (formclass != "") {
    //获取页面所有input
    inputlist = $("." + formclass + " input");
    //获取页面所有textarealist
    textarealist = $("." + formclass + " textarea");
} else {
    //获取页面所有input
    inputlist = $("input");
    //获取页面所有textarealist
    textarealist = $("textarea");
}

//设置表单值
function SetData(data) {
    //设置表单input值
    SetValue(inputlist, data);
    //设置表单下所有textarea值
    SetValue(textarealist, data);
}

function SetValue(e, data) {
    e.each(function (index, item) {
        var attrname = item.attributes["name"].value;
        if (typeof(item.attributes["vary-data-format"]) != "undefined") {
            formatvalue = item.attributes["vary-data-format"].value;
            var time = eval('new ' + (data[attrname].replace(/\//g, ''))).Format(formatvalue);
            $("*[name='" + attrname + "']").val(time);
        } else
        {
            $("*[name='" + attrname + "']").val(data[attrname]);
        }
      
     
    })
}
//获取表单里面的值
function GetData() {
    var obj = {};
    //获取表单下所有的Input值
    GetValue(inputlist, obj);
    //获取表单下所有textarea值
    GetValue(textarealist, obj);
    return obj;
}

function GetValue(e, obj) {
    e.each(function (index, item) {
        var attrname = item.attributes["name"].value;
        obj[attrname] = $("*[name='" + attrname + "']").val();
    })
    return obj;
}