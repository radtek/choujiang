// JavaScript Document

var turnplate = {
    restaraunts: [],				//大转盘奖品名称
    colors: [],	                //大转盘奖品区块对应背景颜色
    //fontcolors:[],				//大转盘奖品区块对应文字颜色
    outsideRadius: 222,			//大转盘外圆的半径
    textRadius: 165,				//大转盘奖品位置距离圆心的距离
    insideRadius: 65,			//大转盘内圆的半径
    startAngle: 0,				//开始角度
    bRotate: false				//false:停止;ture:旋转
};

var Mar = document.getElementById("Marquee");
var child_div = Mar.getElementsByTagName("div");
var picH = 35;//移动高度 
var scrollstep = 3;//移动步幅,越大越快 
var scrolltime = 50;//移动频度(毫秒)越大越慢 
var stoptime = 3000;//间断时间(毫秒) 
var tmpH = 0;
Mar.innerHTML += Mar.innerHTML;
function start() {
    if (tmpH < picH) {
        tmpH += scrollstep;
        if (tmpH > picH) tmpH = picH;
        Mar.scrollTop = tmpH;
        setTimeout(start, scrolltime);
    } else {
        tmpH = 0;
        Mar.appendChild(child_div[0]);
        Mar.scrollTop = 0;
        setTimeout(start, stoptime);
    }
}

$(document).ready(function () {

    setTimeout(start, stoptime);

    //动态添加大转盘的奖品与奖品区域背景颜色
    var num = location.hash;//根据接收过来的值判断概率。
    //	alert(num.substr(1));

    var aid=$('#AId').val();

    $.ajax({
        type: "get",
        async: false,
        url: "/default/GetPrizeItem/" + aid,
        data: "",
        dataType: "json",
        success: function (data) {
            var arr = new Array();
            for (var i = 0; i < data.length; i++) {
                arr[i] = data[i];
            }
            turnplate.restaraunts = arr;
            drawRouletteWheel();
        },
        error: function (data) {
            console.log("error:" + data + "GetPrizeItem");
            return;
        }
    });



    //turnplate.restaraunts = ["奖品1", "奖品2", "奖品3", "奖品4", "奖品5", "奖品6", "奖品7", "奖品8"];
    turnplate.colors = ["#FBDB00", "#FACA00", "#FBDB00", "#FACA00", "#FBDB00", "#FACA00", "#FBDB00", "#FACA00"];
    //turnplate.fontcolors = ["#CB0030", "#FFFFFF", "#CB0030", "#FFFFFF","#CB0030", "#FFFFFF"];

    var rotateTimeOut = function () {
        $('#wheelcanvas').rotate({
            angle: 0,
            animateTo: 2160,
            duration: 6000,
            callback: function () {
                alert('网络超时，请检查您的网络设置！');
            }
        });
    };


    //旋转转盘 item:奖品位置; txt：提示语;
    var rotateFn = function (item, txt, data) {
        var angles = item * (360 / turnplate.restaraunts.length) - (360 / (turnplate.restaraunts.length * 2));
        if (angles < 270) {
            angles = 270 - angles;
        } else {
            angles = 360 - angles + 270;
        }
        $('#wheelcanvas').stopRotate();
        $('#wheelcanvas').rotate({
            angle: 0,
            animateTo: angles + 1800,
            duration: 6000,
            callback: function () {

                console.log(txt);
                //中奖页面与谢谢参与页面弹窗
                if (txt.indexOf("谢谢参与") >= 0) {
                    $(".xxcy_text").html(data.msg);
                    $("#xxcy-main").fadeIn();
                    console.log("1:" + data.msg);
                    save();
                } else {
                    $("#zj-main").fadeIn();
                    var resultTxt = txt.replace(/[\r\n]/g, "");//去掉回车换行
                    $("#jiangpin").html(data.msg);
                    console.log("2:" + data.msg);
                    save();
                }
                turnplate.bRotate = !turnplate.bRotate;
            }
        });
    };

    /********弹窗页面控制**********/

    $('.close_zj').click(function () {
        window.location.reload();
        $('#zj-main').fadeOut();
        $('#tx-main').fadeIn();//提醒框显示
        //判断用户是否确认放弃
        $(".do").click(function () {//点确认就默认放弃
            $('#tx-main').fadeOut();
            theEnd();
            save();
        });
        $(".not_do").click(function () {//点取消就回到提交页面
            $('#tx-main').fadeOut();
            $('#zj-main').fadeIn();
        });

        $('#ml-main').fadeIn();

    });

    $('.close_xxcy').click(function () {
        $('#xxcy-main').fadeOut();
        window.location.reload();
        //		theEnd();
        //		save();
    });


    function getUrlParam(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }


    $(function ()
    {
        var id = getUrlParam("id");

        console.log(id);

        $.ajax({
            type: "post",
            url: "/Default/ConditionActiveStatus/" + id,
            data: "",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == 0) {
                    $('#tupBtn').attr("flag", "0");
                    //location.href = '/Login/index?id=' + $("#AId").val();
                } else {
                    $('#tupBtn').attr("flag", "1");
                    $('#tupBtn').attr("msg", data.msg);

                    var code = data.code;

                    if (code == 1 || code == 2) {
                        $('.button2').attr("href", "javascript:void(0);");
                    }

                    //$('#tupBtn').attr("src", "/Turn/img/turnplate-pointer_3.png");
                }
            },
            error: function (data) {
                console.log("error:" + data + ",GetProb");
                return;
            }
        });

    });

    /********抽奖开始**********/
    $('#tupBtn').click(function () {
        var gamed = $("#gamed").val();
        var gameState = $("#gameState").val();
        var cardCode = $("#cardCode").val();
        var mId = $("#mId").val();


        if ($('#tupBtn').attr('flag') == "1") {
            layer.alert($('#tupBtn').attr("msg"));
            return;
        }


        //		if(gameState != 0){
        //			$(".xxcy_text").html("活动时间：01.13-03.04");
        //			$("#xxcy-main").fadeIn();
        //			return;	
        //		}


      


        //扣除次数、积分

       

        //$.ajax({
        //    type: "post",
        //    url: "/Login/CheckLogin",
        //    data: "",
        //    dataType: "json",
        //    async: false,
        //    success: function (data) {
        //        if (data.code == 0) {
        //            location.href = '/Login/index?id=' + $("#AId").val();
        //        }

        //    },
        //    error: function (data) {
        //        console.log("error:" + data + ",GetProb");
        //        return;
        //    }
        //});


        $.ajax({
            type: "post",
            url: "/Default/ConditionCount",
            data: "",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == 0) {
                    $(".xxcy_text").html("抽奖次数已用完<br>");
                    $("#xxcy-main").fadeIn();

                } else if (data.code == 1) {
                    Choujiang();
                } else {
                    location.href = data.data;
                }
            },
            error: function (data) {
                console.log("error:" + data + ",GetProb");
                return;
            }
        });

        //抽奖
        function Choujiang() {
           

            //抽奖概率计算
            $.ajax({
                type: "post",
                url: "/Default/GetProb",
                data: "",
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    var textVal;
                    if (data.code == 0) {
                         textVal = { "msg": data.msg };
                    } else {
                        textVal = { "msg": data.msg };
                    }

                    if (turnplate.bRotate) return;
                    turnplate.bRotate = !turnplate.bRotate;
                    var item = data.prizeIndex + 1;
                   
                    rotateFn(item, turnplate.restaraunts[item - 1], textVal);

                },
                error: function (data) {
                    console.log("error:" + data + ",GetProb");
                    return;
                }
            });
        }


    })

});

function rnd(n, m) {
    var random = Math.floor(Math.random() * (m - n + 1) + n);
    return random;
}

//页面所有元素加载完毕后执行drawRouletteWheel()方法对转盘进行渲染
window.onload = function () {
    drawRouletteWheel();
};

function drawRouletteWheel() {
    var canvas = document.getElementById("wheelcanvas");
    if (canvas.getContext) {
        //根据奖品个数计算圆周角度
        var arc = Math.PI / (turnplate.restaraunts.length / 2);
        var ctx = canvas.getContext("2d");
        //在给定矩形内清空一个矩形
        ctx.clearRect(0, 0, 516, 516);
        //strokeStyle 属性设置或返回用于笔触的颜色、渐变或模式  
        ctx.strokeStyle = "#FFBE04";
        //font 属性设置或返回画布上文本内容的当前字体属性
        ctx.font = 'bold 22px Microsoft YaHei';
        for (var i = 0; i < turnplate.restaraunts.length; i++) {
            var angle = turnplate.startAngle + i * arc;
            ctx.fillStyle = turnplate.colors[i];
            ctx.beginPath();
            //arc(x,y,r,起始角,结束角,绘制方向) 方法创建弧/曲线（用于创建圆或部分圆）    
            ctx.arc(258, 258, turnplate.outsideRadius, angle, angle + arc, false);
            ctx.arc(258, 258, turnplate.insideRadius, angle + arc, angle, true);
            ctx.stroke();
            ctx.fill();
            //锁画布(为了保存之前的画布状态)
            ctx.save();

            //----绘制奖品开始----
            ctx.fillStyle = "#E83800";
            //ctx.fillStyle = turnplate.fontcolors[i];
            var text = turnplate.restaraunts[i];
            var line_height = 30;
            //translate方法重新映射画布上的 (0,0) 位置
            ctx.translate(258 + Math.cos(angle + arc / 2) * turnplate.textRadius, 258 + Math.sin(angle + arc / 2) * turnplate.textRadius);

            //rotate方法旋转当前的绘图
            ctx.rotate(angle + arc / 2 + Math.PI / 2);

            /** 下面代码根据奖品类型、奖品名称长度渲染不同效果，如字体、颜色、图片效果。(具体根据实际情况改变) **/
            if (text.indexOf("\n") > 0) {//换行
                var texts = text.split("\n");
                for (var j = 0; j < texts.length; j++) {
                    ctx.font = j == 0 ? '22px Microsoft YaHei' : '22px Microsoft YaHei';
                    //ctx.fillStyle = j == 0?'#FFFFFF':'#FFFFFF';
                    if (j == 0) {
                        //ctx.fillText(texts[j]+"M", -ctx.measureText(texts[j]+"M").width / 2, j * line_height);
                        ctx.fillText(texts[j], -ctx.measureText(texts[j]).width / 2, j * line_height);
                    } else {
                        ctx.fillText(texts[j], -ctx.measureText(texts[j]).width / 2, j * line_height);
                    }
                }
            } else if (text.indexOf("\n") == -1 && text.length > 6) {//奖品名称长度超过一定范围 
                text = text.substring(0, 6) + "||" + text.substring(6);
                var texts = text.split("||");
                for (var j = 0; j < texts.length; j++) {
                    ctx.fillText(texts[j], -ctx.measureText(texts[j]).width / 2, j * line_height);
                }
            } else {

                //在画布上绘制填色的文本。文本的默认颜色是黑色
                //measureText()方法返回包含一个对象，该对象包含以像素计的指定字体宽度
                ctx.fillText(text, -ctx.measureText(text).width / 2, 0);
            }

            //把当前画布返回（调整）到上一个save()状态之前 
            ctx.restore();
            //----绘制奖品结束----
        }
    }


    // 对浏览器的UserAgent进行正则匹配，不含有微信独有标识的则为其他浏览器
    /*var useragent = navigator.userAgent;
    if (useragent.match(/MicroMessenger/i) != 'MicroMessenger') {
        // 这里警告框会阻塞当前页面继续加载
        alert('已禁止本次访问：您必须使用微信内置浏览器访问本页面！');
        // 以下代码是用javascript强行关闭当前页面
        var opened = window.open('about:blank', '_self');
        opened.opener = null;
        opened.close();
    }*/


}

function showDialog(id) {
    document.getElementById(id).style.display = "-webkit-box";
}

function showID(id) {
    document.getElementById(id).style.display = "block";
}
function hideID(id) {
    document.getElementById(id).style.display = "none";
}

//缓存函数
function save() {
    localStorage.end = theEnd();
    localStorage.gifts = $(".cjjg").text();
}

//提示抽奖结束
function theEnd() {
    $('#tupBtn').unbind('click');//提交成功解除点击事件。   
    return 2;
}

