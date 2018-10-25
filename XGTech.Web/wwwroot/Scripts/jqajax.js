/**
 @Name：layui.jqajax 异步提交插件
 @Author：Paco
 @date: 2016-12-03
 @web: www.jqcool.net
 */
layui.define(['jquery', 'layer'], function(exports) {
    var $ = layui.jquery,
        layer = layui.layer;
    var ajax = {
        //定义参数
        options: {
            'method': 'POST',
            'data': '',
            'dataType': 'json',
            'timeout': 50000,
            'cache': false,
            'loading': true //是否显示loading图片
        },

        //初始化，按属性绑定ajax事件给元素
        init: function() {
            //对有.ajax的元素绑定单击ajax事件
            $(".ajax:not([ajax-bind])").each(function() {
                ajax.setBind($(this));
                $(this).bind('click', ajax.ajaxClick);
            });

        },


        /**
         * 为已绑定事件的元素加上绑定属性，标识已绑定，当重新加载ajax.init()避免重复绑定
         * 原因：
         * 1 因為使用live無效，ajax加載的元素，無法使用jx，所以只能再加載一邊ajax.init()，這裡是為了避免重複綁定統一元素
         * 2 获取options
         **/
        setBind: function($obj) {
            $obj.attr("ajax-bind", 1);
        },
        /**
         * 赋值
         * @return $this 当前对象
         **/
        setOptions: function($this, option) {
            if (undefined == option || null == option) {
                var method = $this.attr("data-method"),
                    loading = $this.attr("data-loading");
                if (undefined != method || null != method) {
                    ajax.options.method = method;
                }

                if (undefined != loading || null != loading) {
                    ajax.options.loading = loading;
                }
                ajax.options = $.extend({}, ajax.options, {
                    "url": $this.data("href"),
                    "complete": $this.data("complete"),
                    "params": $this.data("params")
                });
            } else {
                ajax.options = $.extend({}, ajax.options, option);
            }

            if (undefined === ajax.options.url || null === ajax.options.url) {
                ajax.options.url = window.location.pathname;
            }

            return $this;

        },

        /**
         * ajax 事件  这里callback特意采用json格式，如果有需要可以在这里修改
         * 
         **/
        ajaxClick: function(event, obj, option) {
            if (undefined == obj || null == obj) {
                obj = $(this);
            }
            //获取当前对象
            $this = ajax.setOptions(obj, option);

            if ($this == false) {
                return;
            }
            //变为大写
            ajax.options.method = ajax.options.method.toUpperCase();
            /**
             * 设置参数
             * v value;t text
             **/
            var _paramsArr = "";

            if (ajax.options.params) {
                _paramsArr = ajax.attr2param();
                if ("GET" == ajax.options.method) {
                    ajax.options.url += "?" + $.param(_paramsArr);
                } else {
                    ajax.options.data = $.param(_paramsArr);
                }
            }
            if (ajax.options.loading) {
                var l = layer.load(1);
            }

            $.ajax({
                type: ajax.options.method,
                url: ajax.options.url,
                dataType: ajax.options.dataType,
                data: ajax.options.data,
                timeout: ajax.options.timeout,
                cache: ajax.options.cache,
                error: function(XMLHttpRequest, status, thrownError) {
                    if (ajax.options.loading) {
                        layer.close(l);
                    }
                    layer.msg('网络繁忙，请稍后重试...')
                },
                success: function(msg) {
                    if (ajax.options.loading) {
                        layer.close(l);
                    }
                    if (!ajax.callBackFunction(msg)) return;
                }
            });
        },

        /**
         * ajax成功后执行方法
         *
         **/
        callBackFunction: function(ret) {
            if ((undefined == ret) || (null == ret))
                return false;

            if (ajax.options.complete) {
                //自定义方法
                eval(ajax.options.complete + "(ret)");
                return;
            }
            if (200 == ret.status) {

                if ((undefined == ret.url) || (null == ret.url)) {
                    location.reload();
                } else {
                    location.href = ret.url;
                }
            } else {
                layer.msg(ret.msg)
            }
            return true;
        },

        /**
         * 属性转换为参数
         **/
        attr2param: function() {
            var _paramsArr = {};
            //eval('var _eval_params = (' + ajax.options.params + ')');
            // console.log(ajax.options.params);
            // var _eval_params = {};
            // alert(typeof ajax.options.params);
            // if (typeof ajax.options.params === 'string') {
            //     params = JSON.parse(ajax.options.params);
            // }
            // return;
            $.each(ajax.options.params, function(param, type) {
                var $temp = $("#" + param); //临时对象
                var _val = ""; //获取值
                switch (type) {
                    case "t":
                        _val = $temp.text();
                        break;

                    case "v":
                        _val = $temp.val();
                        break;

                    default:
                        _val = type;

                }
                _paramsArr[param] = _val;
            });
            return _paramsArr;
        }
    };

    //ajax.init();
    exports('jqajax', ajax);
});