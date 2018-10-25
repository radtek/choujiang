$(document).ready(function ()
{
    //删除表格行事件
    $("#btnDel").click(function () {
        if ($("tbody input[type=checkbox]:checked").length <= 0) {
            layer.msg("您未选中任何数据");
            return;
        }
        layer.confirm('您确定要删除？', {
            btn: ['确定', '取消'] //按钮
        }, function () {

            $("tbody input[type=checkbox]:checked").each(function () {
                $(this).parents("tr").remove();
            });
            layer.msg("删除成功");
        }, function () {

        });
    })
    //给新添的行增加checkbox选中事件(只需要在checkbox输入框后面的div上加入addtd样式)
    $("body").on("click", ".addtd", function () {
        if ($(this).hasClass("layui-form-checked")) {
            $(this).removeClass("layui-form-checked");
            $(this).prev().prop("checked", false);
            $(this)[0].parentNode.parentNode.style["backgroundColor"] = "";
        } else {
            $(this).addClass("layui-form-checked");
            $(this).prev().prop("checked", true);
            $(this)[0].parentNode.parentNode.style["backgroundColor"] = "#1E9FFF";
        }
    })
    //双击表格进行单个修改（只需要在需要修改的表格上加上tdclick）
    $("body").on("dblclick", "tbody .tdclick", function () {
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
    //鼠标离开表格输入框
    $("body").on("blur", "tbody .importtext", function () {
        $(this)[0].parentNode.className = "tdclick ";
        $(this)[0].parentNode.innerHTML = $(this).val();
    });


    layui.use('form', function () {
        var $ = layui.jquery, form = layui.form;

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
        //复选框
        form.on('checkbox(Choose)', function (data) {
            if (data.elem.checked) {
                $(data.elem).parents('tr')[0].style["backgroundColor"] = "#1E9FFF";
            } else
            {
                $(data.elem).parents('tr')[0].style["backgroundColor"] = "";
            }
           
            form.render('checkbox');
        });
        //单选框
        form.on('radio(Choose)', function (data) {
            var child = $(data.elem).parents('table').find('tbody input');
            child.each(function (index,item)
            {
                if (item.checked) {
                    item.parentNode.parentNode.style["backgroundColor"] = "#1E9FFF";
                } else {
                    item.parentNode.parentNode.style["backgroundColor"] = "";
                }
            })
            

            form.render('checkbox');
        });

    });

})