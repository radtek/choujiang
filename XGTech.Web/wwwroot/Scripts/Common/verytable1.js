/** layui-v1.0.9_rls MIT License By http://www.layui.com */
; !
function (e) {
    var layer;
    var laypage;
    var form;
    var element;
    var initFn;

    layui.use(['layer', 'laypage', 'element', 'form'], function () {
        layer = layui.layer;
        laypage = layui.laypage;
        element = layui.element();
        form = layui.form();

        if (initFn)
        {
            //防止出现layerui.use事件慢于load事件的情况
            initFn();
        }
        //全选
        form.on('checkbox(allChoose)', function (data) {
            var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
            child.each(function (index, item) {
                if (typeof (item.attributes["disabled"]) == "undefined" || item.attributes["disabled"].value == "false") {
                    item.checked = data.elem.checked;
                }

            });
            form.render('checkbox');
        });
    });
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
    var t = function () {
        this.v = "1.0.0_rls"
    };
    t.fn = t.prototype;
    var d = document;
    var sizelistindex = 1;//用户判断是否第一次查询

    $.fn.bindpreload = function (param, success, fail)
    {
        var $table = $(this);
        initFn = function () { $table.load(param, success, fail) };
    }

    $.fn.load = function (param, success, fail) {
        layui.use(['layer', 'laypage', 'element', 'form'], function () {
            layer = layui.layer;
            laypage = layui.laypage;
            element = layui.element();
            form = layui.form();
        })
        var $table = $(this);
        var n = this;
        var url = "";
        if (typeof (n.attr("url")) == "undefined") {
            return;
        }
        url = n.attr("url");
        if (url.length <= 0) {
            layer.msg("请求的地址不能为空");
            return;
        }
        //判断是否需要分页
        if (typeof (n.attr("showpager")) != "undefined") {
            if ($('#' + n[0].id + "_pager" + '').length <= 0) {
                if (typeof (param) == "undefined") {
                    param = { pageIndex: 1 };
                }
            }
            if (typeof (param.pageIndex) == "undefined") {
                if (typeof (n.attr("pageindex")) != "undefined") {
                    param.pageIndex = n.attr("pageindex");
                } else {
                    param.pageIndex = 1;
                }
            }
            //用于控制下拉框选择页码
            if (typeof (param.pageSize) == "undefined") {
                if (typeof (n.attr("sizelist")) != "undefined") {
                    if (sizelistindex == 1) {
                        var sizelist = n.attr("sizelist");
                        var objlist = eval(sizelist);
                        param.pageSize = objlist[0];
                    } else {
                        var selid = n[0].id + "sizelist"
                        param.pageSize = $("#" + selid + " select").val();
                    }
                    sizelistindex++;

                } else {
                    if (typeof (n.attr("pagesize")) != "undefined") {
                        param.pageSize = n.attr("pagesize");
                    } else {
                        param.pageSize = 10;
                    }
                }
            }

        }
        //发送请求得到数据
        var ajaxType = "post";//请求方式
        if (typeof (n.attr("ajaxType")) != "undefined") {
            ajaxType = n.attr("ajaxType");
        }
        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        $.ajax({
            url: url,
            data: param,
            type: ajaxType,
            success: function (e) {
                layer.close(daoru);
                var data = eval(e);
                //请求数据成功时方式的事件
                if (typeof (success) != "undefined") {
                    success(data);
                }
                $table.SetTableRow(data.data);

                //是否进行分页
                if (typeof (n.attr("showpager")) != "undefined") {
                    if ($('#' + n[0].id + "sizelist" + '').length > 0)
                    {
                        SetSelctChexkBox($('#' + n[0].id + "sizelist" + ''), param, n);
                    }
                    if ($('#' + n[0].id + "_pager" + '').length > 0) {
                        //分页功能
                        laypage({
                            cont: '' + n[0].id + "_pager" + ''
                            , pages: data.pageCount
                            , curr: data.pageIndex
                            , skip: true
                            , jump: function (obj, first) {
                                if (!first) {
                                    param.pageIndex = obj.curr;
                                    n.load(param, success, fail);
                                }
                            }
                        });
                        $("#" + n[0].id + "_pagecount")[0].textContent = "每页" + data.pageSize + "条,共" + data.count + "条";
                        return;
                    }
                    var $tool = $('<div></div>');
                    n.after($tool);
                    if (n.attr("showpager") == "true") {
                        //生成分页DIV
                        var $pager = $('<div></div>');
                        $pager.prop('id', n[0].id + "_pager");
                        $tool.append($pager);
                        //分页功能
                        laypage({
                            cont: '' + n[0].id + "_pager" + ''
                            , pages: data.pageCount
                            , curr: data.pageIndex
                            , skip: true
                            , jump: function (obj, first) {
                                if (!first) {
                                    param.pageIndex = obj.curr;
                                    n.load(param, success, fail);
                                }
                            }
                        });
                    }
                    //是否显示每页记录数
                    if (typeof (n.attr("sizelist")) != "undefined") {
                        var sizelist = n.attr("sizelist");
                        var objlist = eval(sizelist);
                        var $pagesize = $("<div></div>");
                        var $pagesel = $("<select lay-ignore></select>");
                        var $pagesizeId = n[0].id + "sizelist";
                        $pagesize.prop("id", $pagesizeId);
                        for (var i = 0; i < objlist.length; i++) {
                            var $pageoption = $("<option value='" + objlist[i] + "'>" + objlist[i] + " 条/页</option>")
                            $pagesel.append($pageoption);
                        }
                        $pagesize.css("margin", "10px")
                        $pagesize.css("line-height", "30px")
                        $pagesel.css("height", "30px")
                        $pagesel.css("display", "inline-block");
                        $pagesize.css("float", "left");
                        $pagesize.append($pagesel);
                        $tool.append($pagesize);
                        if (typeof (param.pageSize) == "undefined") {
                            param.pageSize = objlist[0];
                        }
                        SetSelctChexkBox($pagesize, param, n)
                    }

                    if (typeof (n.attr("showcount")) != "undefined") {
                        if (n.attr("showcount") == "true") {
                            //是否显示总记录数
                            var $pagecount = $("<div>每页" + data.pageSize + "条,共" + data.count + "条</div>")
                            $pagecount.prop("id", n[0].id + "_pagecount");
                            $pager.css("float", "left");
                            $pagecount.css("float", "left");
                            $pagecount.css("line-height", "52px");
                            $pagecount.css("margin-left", "25px");
                            $tool.append($pagecount);
                        }
                    }

                }
            },
            error: function (e) {
                layer.close(daoru);
                //请求失败
                if (typeof (fail) != "undefined") {
                    fail(e);
                }
                layer.msg("系统异常请稍后再试");
            }
        });
    }
    //生成表格行
    $.fn.SetTableRow = function (data) {
        var $table = $(this);
        ////列宽度平均分配
        //$table.css("table-layout", "fixed");
        //清除表格行数据
        $table.find("tbody").remove();

        //获取表格头
        var n = this;
        var thlist = $("#" + $table[0].id + " thead tr th");
        var m$tr = $("<tr style='text-align:left;'></tr>");;//临时tr对象
        var $mtrId = $table[0].id + "mrow" + i;
        m$tr.prop("id", $mtrId);
        $tbody = $("<tbody></tbody>")
        var rowradiofig = false;//控制单选按钮行点击事件
        //遍历得到的结果
        for (var i = 0; i < data.length; i++) {
            var $tr = $("<tr style='text-align:left;'></tr>");
            var $trId = $table[0].id + "row" + i;
            $tr.prop("id", $trId);
            thlist.each(function (index, item) {
                var $td = $('<td></td>')
                var $tdId = $table[0].id + i + "cell" + index;
                $td.prop("id", $tdId);
                if (typeof (item.attributes["very-type"]) != "undefined") {
                    //获取lay-filter
                    var filter = "Choose";
                    if (typeof (item.attributes["lay-filter"]) != "undefined") {
                        filter = item.attributes["lay-filter"].value;
                    }
                    var $input = $('<input title=" " name="sel" type="' + item.attributes["very-type"].value + '"  lay-skin="primary" lay-filter="' + filter + '"></input>');
                    //是否能选中（默认true）
                    if (typeof (item.attributes["allowCellSelect"]) != "undefined" && item.attributes["allowCellSelect"].value == "false") {
                        $input.prop("disabled", "disabled");
                    }
                    //控制容器中的文本是居中,靠左还是靠右。（默认靠左）
                    if (typeof (item.attributes["very-align"]) != "undefined") {
                        var align_value = item.attributes["very-align"].value;
                        $td.css("text-align", align_value);
                    }
                    //行点击时发生
                    if (item.attributes["very-type"].value == "radio") {
                        rowradiofig = true;
                        $tr.click(function () {
                            $(this).trigger("rowclick", [$table, $(this)])
                            if (typeof (n.attr("allowUnselect")) != "undefined") {
                                if (n.attr("allowUnselect") != "false") {

                                    $input.prop("checked", "checked");
                                    if (typeof (m$tr) != "undefined") {
                                        m$tr.css("background-color", "");
                                    }
                                    if (typeof (n.attr("allowunselectcolor")) != "undefined") {
                                        $(this).css("background-color", n.attr("allowunselectcolor"));
                                    } else {
                                        $(this).css("background-color", "#1E9FFF");
                                    }

                                    m$tr = $(this);
                                    form.render();
                                }
                            } else {
                                $input.prop("checked", "checked");
                                if (typeof (m$tr) != "undefined") {
                                    m$tr.css("background-color", "");
                                }
                                if (typeof (n.attr("allowunselectcolor")) != "undefined") {
                                    $(this).css("background-color", n.attr("allowunselectcolor"));
                                } else {
                                    $(this).css("background-color", "#dbd7d7");
                                }

                                m$tr = $(this);
                                form.render();
                            }
                        })
                    }
                    $td.append($input);
                } else if (typeof (item.attributes["field"]) != "undefined") {
                    //绑定数据返回的列
                    var fieldname = item.attributes["field"].value;
                    var value = data[i][fieldname];
                    if (value == null) {
                        value = "";
                    }
                    //根据头格元素隐藏
                    if (item.style.display == "none") {
                        $td[0].style.display = "none";
                    }
                    //判断返回的数据是否为时间
                    if (typeof (item.attributes["very-data-format"]) != "undefined") {
                        var formatvalue = item.attributes["very-data-format"].value;
                        if (value != "" && value.indexOf('Date') > -1) {
                            value = eval('new ' + (value.replace(/\//g, ''))).Format(formatvalue);
                        }
                    }
                    //保留的小数位
                    if (typeof (item.attributes["very-decimalPlaces"]) != "undefined") {
                        var places = 2;
                        if (typeof (item.attributes["very-decimalPlaces"].value) == "number") {
                            places = item.attributes["very-decimalPlaces"].value;
                        }
                        if (typeof (value) == "number") {
                            value = value.toFixed(places);
                        }
                    }

                    if (typeof (item.attributes["very-address"]) != "undefined") {
                        var places = 10;
                        if (typeof (item.attributes["very-address"].value) == "number") {
                            places = item.attributes["very-address"].value;
                        }
                        if (value.length > places) {
                            value = value.substring(0, places)+"...";
                        }
                    }
                    //判断是否为图片显示
                    if (typeof (item.attributes["very-image"]) != "undefined") {
                        var $img = $("<img src='" + value + "'></img>")
                        $td.append($img);
                    } else {
                        $td.html(value);
                    }

                    //双击表格是否进行编辑
                    if (typeof (item.attributes["very-dblclick"]) != "undefined") {
                        $td[0].className = "tddbclick"
                    }
                } else {
                    //单元格自定义显示内容
                    if (typeof (item.attributes["very-create"]) != "undefined") {
                        var tdcontent = eval(item.attributes["very-create"].value);
                        $td.html(tdcontent($tr, data[i]));
                    }
                }
                //控制容器中的文本是居中,靠左还是靠右。（默认靠左）
                if (typeof (item.attributes["very-align"]) != "undefined") {
                    var align_value = item.attributes["very-align"].value;
                    $td.css("text-align", align_value);
                }
                $tr.append($td);
                //单元格事件
                //单元格点击时发生
                $td.click(function () {
                    $(this).trigger("cellclick", [$table, $tr, $(this)])
                })
                //单元格鼠标按下时发生
                $td.mousedown(function () {
                    $(this).trigger("cellmousedown", [$table, $tr, $(this)])
                })
            });
            $tbody.append($tr);
            //单击事件
            if (!rowradiofig)
            {
                $tr.click(function () {
                    $(this).trigger("rowclick", [$table, $(this)])
                });
            }

            //双击事件
            $tr.dblclick(function () {
                $(this).trigger("rowdblclick", [$table, $(this)])
            })
            //行鼠标按下时
            $tr.mousedown(function () {
                $(this).trigger("rowmousedown", [$table, $(this)])
            })
        }
        //数据为空时显示的
        if (data == null || data.length <= 0) {
            if (typeof ($table.attr("showEmptyText") != "undefined")) {
                $thleng= $("th", $table).length;
                if ($table.attr("showEmptyText") == "true") {
                    $tbody.append("<tr><td colSpan='" + $thleng + "' style='text-align:center;color:#F00'>"+$table.attr("emptyText")+"</td></tr>");
                }
            }
        }
        //添加表格行
        $table.append($tbody);
        layui.use(['layer', 'laypage', 'element', 'form'], function () {
            layer = layui.layer;
            laypage = layui.laypage;
            element = layui.element();
            form = layui.form();
            form.render();
        })
      
        $table.loadover($table, data);
        if (__customerFunc != null) {
            __customerFunc();
        }
    }

    __customerFunc = null;

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
    //修改输入框数据之后鼠标离开时自动关闭
    $("body").on("blur", ".importtext", function () {
        $(this).removeClass("addinput");
        $(this).html = $(this)[0].parentNode.innerHTML = $(this).val();
    })
    //添加每页记录数下拉框选中事件
    function SetSelctChexkBox($pagesize, param, $table) {
        $pagesize[0].onchange = function (e) {
            param.pageIndex = 1;
            param.pageSize = e.target.value;
            $table.load(param);
        };

    }


    //获取表格所有行的数据
    $.fn.GetTableRowData = function () {
        var $table = $(this);
        var data = [];
        //获取表格所有的行
        if (typeof ($("#" + $table[0].id).find("tbody")[0]) == "undefined") {
            return data;
        }
        for (var i = 0; i < $("#" + $table[0].id).find("tbody")[0].children.length; i++) {
            var rowdata = t.fn.get($("#" + $table[0].id).find("tbody")[0].children[i].id).GetRowData();
            data.push(rowdata);
        }

        return data;
    }
    //获取表格所有选中的行的数据
    $.fn.GetSelTableRowData = function () {
        var $table = $(this);
        var data = [];
        //获取表格所有的行
        if (typeof ($("#" + $table[0].id).find("tbody")[0]) == "undefined") {
            return data;
        }
        for (var i = 0; i < $("#" + $table[0].id).find("tbody")[0].children.length; i++) {
            var objrow = $("#" + $("#" + $table[0].id).find("tbody")[0].children[i].id + " input:checkbox:checked");
            if (objrow.length > 0) {
                var rowdata = t.fn.get($("#" + $table[0].id).find("tbody")[0].children[i].id).GetRowData();
                data.push(rowdata);
            }

        }

        return data;
    }
    //获取表格单选选中的行的数据
    $.fn.GetSelTableRowRadioData = function () {
        var $table = $(this);
        var data = [];
        //获取表格所有的行
        if (typeof ($("#" + $table[0].id).find("tbody")[0]) == "undefined") {
            return data;
        }
        for (var i = 0; i < $("#" + $table[0].id).find("tbody")[0].children.length; i++) {
            var objrow = $("#" + $("#" + $table[0].id).find("tbody")[0].children[i].id + " input:radio:checked");
            if (objrow.length > 0) {
                var rowdata = t.fn.get($("#" + $table[0].id).find("tbody")[0].children[i].id).GetRowData();
                data.push(rowdata);
            }

        }

        return data;
    }
    //获取行数据(row:行对象)
    $.fn.GetRowData = function () {
        var row = $(this)[0];
        var rowdata = {};
        var tdchildrens = row.children;
        //获取表格header用于给数据赋值
        var theadlist = $("#" + row.parentNode.parentNode.id + " thead tr th");
        for (var i = 0; i < tdchildrens.length; i++) {
            //排除没有定义的列名
            if (typeof (theadlist[i].attributes["field"]) == "undefined") {
                continue;
            }
            var attrname = theadlist[i].attributes["field"].value;
            rowdata[attrname] = tdchildrens[i].innerText;
        }
        return rowdata;
    }

    $.fn.Export = function (exporturl, exportparam, area, colseiframfn, successCallBack) {
        var $table = $(this);
        var columns = [];
        var theadlist = $("#" + $table.attr("id") + " thead tr th");
        for (var i = 0; i < theadlist.length; i++) {
            //排除没有定义的列名
            if (typeof (theadlist[i].attributes["field"]) == "undefined") {
                continue;
            }
            //隐藏列不导出
            if (theadlist[i].style.display == "none") {
                continue;
            }
            var attrname = theadlist[i].attributes["field"].value;

            columns.push({ "displayname": theadlist[i].innerText, "field": attrname });
        }

        var param = {
            type: 2,//0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）
            title: "导出",
            closeBtn: 1, //不显示关闭按钮
            shade: [0],
            area: ['100%', '100%'],
            maxmin: false, //开启最大化最小化按钮
            //time: 2000, //2秒后自动关闭
            anim: 2,
            content: ['/Export/Index', 'yes'], //iframe的url，no代表不显示滚动条
            success: function (obj, index) {
                var iframe = "layui-layer-iframe" + index;
                var v = window[iframe];
                v.SetColumn(columns);
                v.registerExportEvent(function (cels) {
                    exportparam.columns = cels;
                    $.ajax({
                        url: exporturl,
                        data: exportparam,
                        type: "post",
                        success: function (e) {
                            var data = eval(e);
                            if (data.success) {
                               location.href = data.filePath;

                                if (typeof (successCallBack) != "undefined" && successCallBack != null) {
                                    successCallBack();
                                }
                                layer.closeAll();
                              
                            } else {
                                layer.msg("导出失败，原因：" + data.msg);
                            }
                            //layer.close(daoru);

                        },
                        error: function () {
                            //layer.close(daoru);
                            layer.msg("系统异常请稍后再试");
                        }
                    });
                });

                //操作子窗体
                if (typeof (setdata) != "undefined" && setdata != null) {
                    setdata(v);
                }

            },
            cancel: function () {
                if (typeof (colseiframfn) != "undefined" && colseiframfn != null) {
                    colseiframfn();
                }
            },
            yes: function () {

            },
            end: function () {

                if (typeof (colseiframfn) != "undefined" && colseiframfn != null) {
                    colseiframfn();
                }
            }
        };

        if (area) {
            param.area = area;
        }

        layer.open(param);
    }

    t.fn.get = function (id) {
        var obj = $("#" + id + "");
        return obj;
    };
    //表格事件
    $.fn.extend({
        loadover: function ($table, data) {
            $(this).trigger("load", [$table, data]);
        },
        regCustomerFunc: function (fn) {
            __customerFunc = fn;
        }
    })
    //行单击事件
    $.fn.on("rowclick", function (event, sender, record) {
    });
    //行双击事件
    $.fn.on("rowdblclick", function (event, sender, record) {
    })
    //行鼠标按下时
    $.fn.on("rowmousedown", function (event, sender, record) {

    })
    //单元格点击时发生
    $.fn.on("cellclick", function (event, sender, record, column) {

    })
    //单元格鼠标按下时发生
    $.fn.on("cellmousedown", function (event, sender, record, column) {

    })
    //表格加载完事件
    $.fn.on("load", function (event, sender, data) {

    })
    //行编辑
    $.fn.enitrow = function () {
        var row = $(this);
        row.find("td").each(function () {
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
        })

    }
    //拖拽表格
    $.fn.tableresize = function () {
        var _document = $("body");
        $(this).each(function () {
            if (!$.tableresize) {
                $.tableresize = {};
            }
            var _table = $(this);
            //设定ID
            var id = _table.attr("id") || "tableresize_" + (Math.random() * 100000).toFixed(0).toString();
            var tr = _table.find("tr").first(), ths = tr.children(), _firstth = ths.first();
            //设定临时变量存放对象
            var cobjs = $.tableresize[id] = {};
            cobjs._currentObj = null, cobjs._currentLeft = null;
            ths.mousemove(function (e) {
                var _this = $(this);
                var left = _this.offset().left,
                    top = _this.offset().top,
                    width = _this.width(),
                    height = _this.height(),
                    right = left + width,
                    bottom = top + height,
                    clientX = e.clientX,
                    clientY = e.clientY;
                var leftside = !_firstth.is(_this) && Math.abs(left - clientX) <= 5,
                    rightside = Math.abs(right - clientX) <= 5;
                if (cobjs._currentLeft || clientY > top && clientY < bottom && (leftside || rightside)) {
                    _document.css("cursor", "col-resize");
                    if (!cobjs._currentLeft) {
                        if (leftside) {
                            cobjs._currentObj = _this.prev();
                        }
                        else {
                            cobjs._currentObj = _this;
                        }
                    }

                }
                else {
                    cobjs._currentObj = null;
                }

            });
            ths.mouseout(function (e) {
                if (!cobjs._currentLeft) {
                    cobjs._currentObj = null;
                    _document.css("cursor", "auto");
                }
            });
            _document.mousedown(function (e) {
                if (cobjs._currentObj) {
                    cobjs._currentLeft = e.clientX;
                }
                else {
                    cobjs._currentLeft = null;
                }
            });
            _document.mouseup(function (e) {
                if (cobjs._currentLeft) {
                    cobjs._currentObj.width(cobjs._currentObj.width() + (e.clientX - cobjs._currentLeft));
                }
                cobjs._currentObj = null;
                cobjs._currentLeft = null;
                _document.css("cursor", "auto");
            });

        });
    };

    e.table = new t;
}(window);