var layer;
var laypage;
var form;
var element;
var btnhtml;//定义表格里面的操作按钮
layui.use(['layer', 'laypage', 'element', 'form'], function () {
    layer = layui.layer;
    laypage = layui.laypage;
    element = layui.element;
    form = layui.form;
    //全选
    form.on('checkbox(allChoose)', function (data) {
        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
        child.each(function (index, item) {
            //if (data.elem.checked) {
            //    item.parentNode.parentNode.style["backgroundColor"] = "red";
            //} else
            //{
            //    item.parentNode.parentNode.style["backgroundColor"] = "";
            //}
            item.checked = data.elem.checked;
        });
        form.render('checkbox');
    });
})

//时间格式化
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    if (/(Y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
var pageIndex = 1;
var pageCount = 0;

//设置表格
function SetDataTable(tableid, pageid, param, btntdhtml, loadover) {
    //获取表格对应的数据和需要显示的数据
    var url = $("#" + tableid).attr("url");
    if (url == "" || url == null) {
        layer.msg("请求的地址不能为空");
        return;
    }
    //发送请求得到数据
    //发送保存请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    $.ajax({
        url: url,
        data: param,
        type: "post",
        success: function (e) {
            var data = eval(e);
            SetTableRow(tableid, pageid, data.list, btntdhtml);
            //网格加载之后执行的事件
            if (typeof (loadover) != "undefined") {
                loadover(data); 
            }
            //是否进行分页
            if ($("#" + tableid + "").attr("showpager") == "true") {
                //分页功能
                laypage({
                    cont: '' + pageid + ''
                    , pages: data.pageCount
                    , curr: data.pageIndex
                    , skip: true
                    , jump: function (obj, first) {
                        if (!first) {
                            param.pageIndex = obj.curr;
                            SetDataTable(tableid, pageid, param, btntdhtml);

                        }
                    }
                });
            }
            layer.close(daoru);

        },
        error: function () {
            layer.close(daoru);
            layer.msg("系统异常请稍后再试");
        }
    });
}

//生成表格行
function SetTableRow(tableid, pageid, data, btntdhtml) {

    //清除表格行数据
    $("#" + tableid + " tbody").html("");
    //获取表格头
    var thlist = $("#" + tableid + " thead th");
    var html = "";
    //遍历得到的结果
    for (var i = 0; i < data.length; i++) {
        html += "<tr style='text-align:left;'>"
        thlist.each(function (index, item) {
            //生成操作列
            if (typeof (item.attributes["name"]) != "undefined" && item.attributes["name"].value == "btnplay" && item.style.display != "none") {
                html += btntdhtml;
            }
            //生成checkbox
            if (typeof (item.attributes["name"]) != "undefined" && item.attributes["name"].value == "checkbox") {

                //获取lay-filter
                var filter = "Choose";
                if (typeof (item.attributes["lay-filter"]) != "undefined") {
                    filter = item.attributes["lay-filter"].value;
                }
                if (typeof (item.attributes["data-align"]) != "undefined") {
                    if (item.attributes["data-align"].value == "center") {
                        html += '<td style="text-align:center;"><input   type="checkbox" name="" lay-skin="primary" lay-filter="' + filter + '"></td>';

                    } else if (item.attributes["data-align"].value == "right") {
                        html += '<td  style="text-align:right;"><input  type="checkbox" name="" lay-skin="primary" lay-filter="' + filter + '"></td>';

                    } else if (item.attributes["data-align"].value == "left") {
                        html += '<td style="text-align:left;"><input   type="checkbox" name="" lay-skin="primary" lay-filter="' + filter + '"></td>';
                    }
                } else {
                    html += '<td><input  type="checkbox" name="" lay-skin="primary" lay-filter="' + filter + '"></td>';
                }

            }
            //生成ridao
            if (typeof (item.attributes["name"]) != "undefined" && item.attributes["name"].value == "radio") {
                //获取lay-filter
                var filter = "radio";
                if (typeof (item.attributes["lay-filter"]) != "undefined") {
                    filter = item.attributes["lay-filter"].value;
                }
                if (typeof (item.attributes["data-align"]) != "undefined") {
                    if (item.attributes["data-align"].value == "center") {
                        html += '<td style="text-align:center;"><input id="choose" lay-filter="' + filter + '" type="radio" name="" title=" " lay-skin="primary" lay-filter="Choose"></td>';

                    } else if (item.attributes["data-align"].value == "right") {
                        html += '<td style="text-align:right;"><input id="choose" lay-filter="' + filter + '" type="radio  name="" title=" " lay-skin="primary" lay-filter="Choose"></td>';

                    } else if (item.attributes["data-align"].value == "left") {
                        html += '<td style="text-align:left;"><input id="choose" type="radio" lay-filter="' + filter + '"   name="" title=" " lay-skin="primary" lay-filter="Choose"></td>';
                    }
                } else {
                    html += '<td><input id="choose" type="radio" name="" title=" " lay-filter="' + filter + '" lay-skin="primary" lay-filter="Choose"></td>';

                }

            }
            //生成表格内容信息
            if (typeof (item.attributes["field"]) == "undefined") {
                return true;
            } else {
                var fieldname = item.attributes["field"].value;
                var value = data[i][fieldname];
                if (value == null) {
                    value = "";
                }
                //根据头格元素隐藏
                if (item.style.display == "none") {
                    //判断是否为时间格式
                    if (typeof (item.attributes["format"]) != "undefined") {
                        var formatvalue = item.attributes["format"].value;
                        html += "<td  field='" + fieldname + "' style='display:none'>" + eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue) + "</td>"
                    }
                    else if (typeof (item.attributes["data-image"]) != "undefined") {
                        //生成图片
                        html += "<td  field='" + fieldname + "' style='display:none'><img src='" + value + "' style='width:100%;height:100%;'></td>"
                    } else {
                        html += "<td  field='" + fieldname + "' style='display:none'>" + value + "</td>"
                    }

                } else {

                    //判断是否为时间格式
                    if (typeof (item.attributes["format"]) != "undefined") {
                        var formatvalue = item.attributes["format"].value;
                        if (value == "")
                        {
                            html += "<td  field='" + fieldname + "' >" + value + "</td>"
                        } else
                        {
                            if (typeof (item.attributes["data-align"]) != "undefined") {
                                if (item.attributes["data-align"].value == "center") {
                                    html += "<td style='text-align:center;'  field='" + fieldname + "' >" + eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue) + "</td>"

                                } else if (item.attributes["data-align"].value == "right") {
                                    html += "<td style='text-align:right;'  field='" + fieldname + "' >" + eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue) + "</td>"

                                } else if (item.attributes["data-align"].value == "left") {
                                    html += "<td  style='text-align:left;' field='" + fieldname + "' >" + eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue) + "</td>"
                                }

                            } else {
                                html += "<td  field='" + fieldname + "' >" + eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue) + "</td>"
                            }
                        }
                        
                    } else if (typeof (item.attributes["data-image"]) != "undefined") {
                        //生成图片
                        if (typeof (item.attributes["data-align"]) != "undefined") {
                            if (item.attributes["data-align"].value == "center") {
                                html += "<td style='text-align:center;'  field='" + fieldname + "' ><img src='" + value + "' style='width:100%;height:100%;'></td>"

                            } else if (item.attributes["data-align"].value == "right") {
                                html += "<td style='text-align:right;'  field='" + fieldname + "' ><img src='" + value + "' style='width:100%;height:100%;'></td>"

                            } else if (item.attributes["data-align"].value == "left") {
                                html += "<td  style='text-align:left;' field='" + fieldname + "' ><img src='" + value + "' style='width:100%;height:100%;'></td>"
                            }

                        } else {
                            html += "<td  field='" + fieldname + "' ><img src='" + value + "' style='width:100%;height:100%;'></td>"
                        }
                    } else {
                        if (typeof (item.attributes["data-align"]) != "undefined") {
                            if (item.attributes["data-align"].value == "center") {
                                html += "<td style='text-align:center;'  field='" + fieldname + "' >" + value + "</td>"

                            } else if (item.attributes["data-align"].value == "right") {
                                html += "<td style='text-align:right;'  field='" + fieldname + "' >" + value + "</td>"

                            } else if (item.attributes["data-align"].value == "left") {
                                html += "<td  style='text-align:left;' field='" + fieldname + "' >" + value + "</td>"
                            }

                        } else {
                            html += "<td  field='" + fieldname + "' >" + value + "</td>"
                        }
                    }




                }

            }

        });
        html += "</tr>"
    }
    //添加表格行
    $("#" + tableid + " tbody").append(html);
    //生成自定义属性操作
    thlist.each(function (index, item) {
        //增加表格双击事件
        if (typeof (item.attributes["data-dblclick"]) != "undefined") {
            $("#" + tableid + " tbody tr").each(function (i,items)
            {
                items.cells[index].className = "tddbclick";
            })
        }
        //单元格里面的操作按钮
        if (typeof (item.attributes["data-create"]) != "undefined") {
            $("#" + tableid + " tbody tr").each(function (i, items) {
                var html = eval(item.attributes["data-create"].value);
                items.cells[index].innerHTML = html;
            })
        }
    });
    form.render();
}
//获取表格所有行的数据
function GetTableRowData(e) {
    var data = [];
    //获取表格所有的行
    $("#" + e + " tbody tr").each(function (index, item) {
        var rowdata = GetRowData(item);
        data.push(rowdata);

    });
    return data;
}

//获取行数据(row:行对象)
function GetRowData(row) {
    var rowdata = {};
    var tdchildrens = row.children;
    for (var i = 0; i < tdchildrens.length; i++) {
        //排除没有定义的列名
        if (typeof (tdchildrens[i].attributes["field"]) == "undefined") {
            continue;
        }
        var attrname = tdchildrens[i].attributes["field"].value
        rowdata[attrname] = tdchildrens[i].innerText;
    }
    return rowdata;
}

//双击表格进行单个修改（只需要在需要修改的表格上加上tdclick）
$("body").on("dblclick", "tbody .tddbclick", function () {

    if ($(this).hasClass("addinput")) {
        $(this).removeClass("addinput");
        $(this).html = $(this).html($(this).find(".importtext").val());

    } else {
        if ($(this).find("input").length <= 0) {
            $(this).addClass("addinput");
            $(this).html = $(this).html("<input type='text' class='importtext' value='" + $(this).html() + "' />")
        } else {
            $(this).html = $(this).html($(this).find(".importtext").val());
        }
    }

});
////修改输入框数据之后鼠标离开时自动关闭
//$("body").on("blur", ".importtext", function ()
//{
//    $(this).removeClass("addinput");
//    $(this).html = $(this)[0].parentNode.innerHTML=$(this).val();
//})

