/*
 * @Author: Paco
 * @Date:   2017-01-31 
 * +----------------------------------------------------------------------
 * | jqadmin [ jq酷打造的一款懒人后台模板 ]
 * | Copyright (c) 2017 http://jqadmin.jqcool.net All rights reserved.
 * | Licensed ( http://jqadmin.jqcool.net/licenses/ )
 * | Author: Paco <admin@jqcool.net>
 * +----------------------------------------------------------------------
 */

layui.define(['jquery', 'elem', 'tabmenu'], function(exports) {
    var $ = layui.jquery,
        element = layui.elem(),
        tabmenu = layui.tabmenu(),
        jqIndex = function() {};
    /**
     *@todo 初始化方法
     */
    jqIndex.prototype.init = function(options) {
        this.showMenu();
        this.resize();
        this.menuBind();
        this.refresh();
    }

    /**
     *@todo 绑定刷新按钮单击事件
     */
    jqIndex.prototype.refresh = function() {
        $('.fresh-btn').bind("click", function() {
            $('.jqadmin-body .layui-show').children('iframe')[0].contentWindow.location.reload(true);
        })
    }

    /**
     *@todo 绑定左侧菜单显示隐藏按钮单击事件
     */
    jqIndex.prototype.showMenu = function() {
        $('.display-arrow').bind("click", function() {
            var bar = $('.jqamdin-left-bar'),
                w = bar.width();
            if (w > 0) {
                $('.jqadmin-body').animate({
                    left: '0'
                });
                bar.animate({
                    width: '0'
                });
                $('.jqadmin-foot').animate({
                    left: '0'
                });
                $(this).find('i').html('&#xe618;');
            } else {
                $('.jqadmin-body').animate({
                    left: '200'
                });
                bar.animate({
                    width: '200'
                });
                $('.jqadmin-foot').animate({
                    left: '200'
                });

                $(this).find('i').html('&#xe616;');
            }
        })
    }

    /**
     *@todo 自适应窗口
     */
    jqIndex.prototype.resize = function() {
        $(window).on('resize', function() {
            tabmenu.init();
            tabmenu.tabMove(0, 1);
        }).resize();
    }

    /**
     *@todo 初始化菜单 
     */
    jqIndex.prototype.menuBind = function() {
        var _this = this;
        //初始化时显示第一个菜单
        $('.sub-menu').eq(0).slideDown();

        //绑定左侧树菜单的单击事件
        $('.sub-menu .layui-nav-item,.tab-menu').bind("click", function() {
            var obj = $(this);
            if (obj.find('dl').length > 0) {
                if (obj.attr('data-bind') == 1) {
                    return;
                }
                obj.attr('data-bind', '1');
                obj.find('dd').bind("click", function() {
                    _this.menuSetOption($(this));
                });
            } else {
                _this.menuSetOption(obj);
            }
        });
        $('.sub-menu .layui-nav-item,.tab-menu').find('dd').bind("click", function () {
            _this.menuSetOption($(this));
        });

        //绑定主菜单单击事件，点击时显示相应的菜单
        element.on('nav(main-menu)', function(elem) {
            var index = (elem.index())
            $('.sub-menu').slideUp().eq(index).slideDown();
        });

        //绑定tag菜单向左滚动按钮事件
        $('span.move-left-btn').bind("click", function() {
            var item = tabmenu.config.item,
                ml = parseInt($('' + item + '').children('ul.layui-tab-title').css("margin-left"));
            if (ml < 0) {
                ml = ml + 130;
                if (ml > 0) {
                    ml = 0;
                }
                $('' + item + '').children('ul.layui-tab-title').css("margin-left", ml);
            }
        });

        //绑定tag菜单向右滚动按钮事件
        $('span.move-right-btn').bind("click", function() {
            var obj = $('' + tabmenu.config.item + '').children('ul.layui-tab-title'),
                ml = parseInt(obj.css("margin-left")),
                tab_all_width = parseInt(obj.prop('scrollWidth')),
                navWidth = parseInt(obj.parent('div').width());
            if (ml - navWidth > -tab_all_width) {
                ml = ml - 130;
                if (ml <= parseInt(navWidth - tab_all_width - 54)) {
                    ml = navWidth - tab_all_width - 54;
                }
                obj.css("margin-left", ml);
            }
        });

        //禁止双击选中
        $('span.move-left-btn,span.move-right-btn').bind("selectstart", function() {
            return false;
        })

    }

    /**
     *@todo 设置菜单项，并创建tab菜单
     */
    jqIndex.prototype.menuSetOption = function(obj) {
        var $a = obj.children('a'),
            href = $a.data('url'),
            icon = $a.children('i:first').data('icon'),
            title = $a.data('title'),
            data = {
                href: href,
                icon: icon,
                title: title
            }
        tabmenu.tabAdd(data);
    }

    var index = new jqIndex();
    index.init();
    exports('index', {});
});