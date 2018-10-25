

$(document).ready(function () {
   
    layui.use(['layer', 'laypage', 'element','form'], function(){
        var layer = layui.layer
        ,laypage = layui.laypage
        , element = layui.element
        ,form = layui.form;

        //layer.load(2);
        //向世界问个好
        //layer.msg('Hello World');

      
        function getlist(curr)
        {
            $.getJSON(
                "/employe/search",
                {
                        username : $("#username").val(),
                        usernub : $("#usernub").val(),
                        userdepart : $("#userdepart").val()
                       
                },
                function (datajson)
                {
                    var norice_content = "";
                    //alert(datajson.pages);
                    $(datajson.rows).each(function (n, Row) {
                        norice_content += "  <div class='panel panel-default'>";
                        norice_content += "      <div class='panel-heading'>";
                        norice_content += "          <h3 class='panel-title'>";
                        norice_content += Row.CreateDate;
                        norice_content += "                        ";
                        norice_content += Row.Creater;
                        norice_content += "          </h3>";
                        norice_content += "      </div>";
                        norice_content += "      <div class='panel-body'>";
                        norice_content += Row.NoticeContent;
                        norice_content += "      </div>";
                        norice_content += "  </div>";
                    });
                    $('#notice_div').html(norice_content);

                    //调用分页
                    laypage({
                        cont: 'pageDemo',//容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                        pages: datajson.pages,//总页数
                        groups: 5, //连续显示分页数
                        skip: false, //是否开启跳页
                        skin: '#AF0000',
                        curr: curr || 1, //当前页,
                        jump: function (obj, first) {//触发分页后的回调
                            if (!first) {//点击跳页触发函数自身，并传递当前页：obj.curr
                                getlist(obj.curr, cid);
                            }
                        }
                    });
                }
            )
        }
     

    })

   
});