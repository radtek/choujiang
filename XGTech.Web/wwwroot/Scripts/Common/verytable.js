/** layui-v1.0.9_rls MIT License By http://www.layui.com */
; !
    function (e) {
        var layer;
        var laypage;
        var form;
        var element;
        var initFn;
        var table;
        layui.use(['layer', 'laypage', 'element', 'form'], function () {
            layer = layui.layer;
            laypage = layui.laypage;
            element = layui.element;
            form = layui.form;
        });

        layui.use('table', function () {
            table = layui.table;

        });
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
        $.fn.load = function (param, success, fail) {
            layui.use('table', function () {
                table = layui.table;

            });

            var $table = $(this);
            $table.css("display", "none");
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

            var thlist = $("#" + $table[0].id + " thead tr th");
            var cols = [];
            thlist.each(function (index, item) {
                if (typeof (item.attributes["very-type"]) != "undefined") {
                    if (item.attributes["very-type"].value == "radio") {
                        var fields = {
                            templet: function () {
                                return '<input title=" " name="sel" type="' + item.attributes["very-type"].value + '"  lay-skin="primary" lay-filter="Choose"></input>';
                            }
                        };
                        cols.push(fields);
                    } else {
                        var fields = { type: item.attributes["very-type"].value };
                        cols.push(fields);
                    }


                } else if (typeof (item.attributes["field"]) != "undefined") {
                    if (item.style.display != "none") {
                        var fields = { field: item.attributes["field"].value, title: item.innerText, sort: true };

                        cols.push(fields);
                    }

                }
                else if (typeof (item.attributes["very-create"]) != "undefined") {
                    var templets = item.attributes["very-create"].value;
                    if (typeof (item.innerText != "undefined") && item.innerText != "") {
                        var fields = {
                            title: item.innerText, toolbar: "#" + templets + ""
                        };
                        cols.push(fields);
                    } else
                    {
                        var fields = {
                            title: "操作", toolbar: "#" + templets + ""
                        };
                        cols.push(fields);
                    }

                   
                }
            })
            table.render({
                elem: $table.selector
                , url: url
                , where: param
                , method: "post"
                , cellMinWidth: 150 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
                , cols: [cols]
                , page: true
                , text: {
                    none: '当前检索条件下未能检索到数据，请切换检索条件后重试' //默认：无数据。注：该属性为 layui 2.2.5 开始新增
                }
                , done: function (res, curr, count) {
                    //请求数据成功时方式的事件
                    if (typeof (success) != "undefined") {
                        success(res);
                    }
                    //如果是异步请求数据方式，res即为你接口返回的信息。
                    //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                }
            });
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
            var checkStatus = table.checkStatus($table[0].id);
            return checkStatus.data;
        }
        //获取表格单选选中的行的数据
        $.fn.GetSelTableRowRadioData = function () {
            var $table = $(this);
            var checkStatus = table.checkStatus($table[0].id);
            if (checkStatus.data.length > 1) {
                return [];

            }
            return checkStatus.data;
        }

        t.fn.get = function (id) {
            var obj = $("#" + id + "");
            return obj;
        };

        $.fn.SetTableRow = function (datalist, ispage, success, fail) {

            if (typeof (ispage) == "undefined") {
                ispage = false;
            }
            layui.use('table', function () {
                table = layui.table;

            });
            var $table = $(this);
            $table.css("display", "none");
            var thlist = $("#" + $table[0].id + " thead tr th");
            var cols = [];
            thlist.each(function (index, item) {
                if (typeof (item.attributes["very-type"]) != "undefined") {
                    if (item.attributes["very-type"].value == "radio") {
                        var fields = {
                            templet: function () {
                                return '<input title=" " name="sel" type="' + item.attributes["very-type"].value + '"  lay-skin="primary" lay-filter="Choose"></input>';
                            }
                        };
                        cols.push(fields);
                    } else {
                        var fields = { type: item.attributes["very-type"].value };
                        cols.push(fields);
                    }


                } else if (typeof (item.attributes["field"]) != "undefined") {
                    if (item.style.display != "none") {
                        var fields = { field: item.attributes["field"].value, title: item.innerText };
                        cols.push(fields);
                    }

                }
                else if (typeof (item.attributes["very-create"]) != "undefined") {
                    var templets = item.attributes["very-create"].value;
                    if (typeof (item.innerText) != "undefined" && item.innerText!="")
                    {
                        var fields = {
                            title: item.innerText, toolbar: "#" + templets + ""
                        };
                        cols.push(fields);
                    } else
                    {
                        var fields = {
                            title: "操作", toolbar: "#" + templets + ""
                        };
                        cols.push(fields);

                    }

                }
            })
            table.render({
                elem: $table.selector
                , data: datalist
                , cellMinWidth: 150 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
                , cols: [cols]
                , page: ispage
                , done: function (res, curr, count) {
                    //请求数据成功时方式的事件
                    if (typeof (success) != "undefined") {
                        success(res);
                    }
                    //如果是异步请求数据方式，res即为你接口返回的信息。
                    //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                }
            });
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
                                    layer.msg("导出失败，原因：" + data.Msg);
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

        e.table = new t;
    }(window);