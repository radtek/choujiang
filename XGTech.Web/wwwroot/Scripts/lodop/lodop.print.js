//var head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;
//var oscript = document.createElement("script");
//oscript.src = "/Scripts/jquery.qrcode.min.js";
//head.appendChild(oscript);
//var oscript2 = document.createElement("script");
//oscript2.src = "/Scripts/qrcode.min.js";
//head.appendChild(oscript2);
//var oscript3 = document.createElement("script");
//oscript3.src = "/Scripts/lodop/LodopFuncs.js";
//head.appendChild(oscript3);

/*
二维码打印方法

onlyCode :wz_tbl_hr_org.only_code 组织唯一编码  5  'YWSTO'
stockCode:wz_tbl_wh_stock.stock_code 物资编号   20
assetClassName: wz_tbl_base_asset_class.asset_class_name 类型编号 20
empNo:wz_tbl_hr_employee.emp_no 入库员编号  10
createTime:wz_tbl_wh_stock.create_time 入库时间 字符串类型，格式'yyMMdd'
desc:  条码说明 

不支持中文
*/
function printQrCode(onlyCode, stockCode, assetClassName, empNo, createTime, desc) {
    debugger;
    var LODOP = getLodop();
    var test = $("body").append('<div id="printUnit" style="height:1mm;display:none;"></div>');
    var width = test.find("#printUnit").height() * 19;
    LODOP.PRINT_INIT("demo");
    LODOP.SET_PRINT_PAGESIZE(0, 480, 250, "");
    LODOP.ADD_PRINT_TEXT('3mm', '2.5mm', '24mm', '4mm', onlyCode);
    LODOP.ADD_PRINT_HTM('7mm', '2mm', '24mm', '4mm', "<p style='font-size:2.3mm;'>" + stockCode + "</p>");
    LODOP.ADD_PRINT_HTM('12mm', '5mm', '24mm', '4mm', "<p style='font-size:2.3mm;'>" + assetClassName + "</p>");
    LODOP.ADD_PRINT_TEXT('17mm', '5mm', '24mm', '4mm', empNo);
    LODOP.ADD_PRINT_HTM('21mm', '3mm', '24mm', '4mm', "<p style='font-size:2.4mm;'>" + createTime + "</p>");
    LODOP.ADD_PRINT_IMAGE('3.5mm', '28mm', '20mm', '20mm',
        $("<div id='qarea'></div>").qrcode({
            width: width,
            height: width,
            text: desc
            //correctLevel: QRErrorCorrectLevel.M
        }).find("canvas")[0].toDataURL("image/jpeg", 1.0));
    LODOP.PRINT();
    $("#printUnit").remove();
}

function printQrCode_New_old(item) {
    var LODOP = getLodop();
    var test = $("body").append('<div id="printUnit" style="height:1mm;display:none;"></div>');
    var width = test.find("#printUnit").height() * 19;
    LODOP.PRINT_INIT("rk");
    LODOP.SET_PRINT_PAGESIZE(0, 480, 250, "");
    LODOP.ADD_PRINT_HTM('5.5mm', '45mm', '40mm', '8mm', "<p style='font-size:4.3mm;'>物资标签</p>");
    LODOP.ADD_PRINT_HTM('10.5mm', '5mm', '40mm', '8mm', "<p style='font-size:4.3mm;'>物资名称:" + item.prod_name + "</p>");
    LODOP.ADD_PRINT_HTM('20.5mm', '5mm', '40mm', '8mm', "<p style='font-size:4.3mm;'>物资类别:" + item.asset_class_name + "</p>");
    LODOP.ADD_PRINT_HTM('20.5mm', '50mm', '40mm', '8mm', "<p style='font-size:4.3mm;'>责任人:</p>");
    LODOP.ADD_PRINT_HTM('30.5mm', '5mm', '40mm', '8mm', "<p style='font-size:4.3mm;'>使用单位:</p>");

    var date = new Date(item.storage_time);
    LODOP.ADD_PRINT_HTM('30.5mm', '50mm', '80mm', '8mm', "<p style='font-size:4.3mm;'>入库时间:" + date.Format("yyyy-MM-dd") + "</p>");
   // LODOP.ADD_PRINT_BARCODE(150, 34, 307, 47, "128A", item.stock_code);
    LODOP.ADD_PRINT_BARCODE("35.97mm", "5mm", "65.01mm", "10.05mm", "128A", item.stock_code);
    //LODOP.ADD_PRINT_IMAGE('45.5mm', '3mm', '20mm', '40mm',
    //    $("<div id='qarea'></div>").qrcode({
    //        width: width,
    //        height: width,
    //        text: item.stockCode
    //        //correctLevel: QRErrorCorrectLevel.M
    //    }).find("canvas")[0].toDataURL("image/jpeg", 1.0));//
   // LODOP.PREVIEW();
    LODOP.PRINT();
    $("#printUnit").remove();
}

function printQrCode_New(item) {
    var LODOP = getLodop();
    LODOP.PRINT_INIT("小标签");
    LODOP.SET_PRINT_PAGESIZE(1, '24mm', '85mm', "小标签");
    LODOP.SET_PRINT_MODE("PRINT_NOCOLLATE", 1);


    LODOP.ADD_PRINT_TEXT(2, 4, 50, 20, "物品名称:");
    LODOP.SET_PRINT_STYLEA(0, "AngleOfPageInside", 90);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 7);
    LODOP.ADD_PRINT_TEXT(2, 52, 60, 20, item.prod_name);
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 7);
    LODOP.ADD_PRINT_TEXT(2, 124, 41, 20, "责任人:");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 7);
    LODOP.ADD_PRINT_TEXT(2, 140, 63, 20, "");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 7);
    LODOP.ADD_PRINT_BARCODE(26, 6, "60.01mm", "10.05mm", "128A", item.stock_code);
   // LODOP.ADD_PRINT_BARCODE(26, 6, 174, 30, "Code39", item.stock_code);
    //LODOP.PREVIEW();
    LODOP.PRINT();
    $("#printUnit").remove();
}

//打印H5
function printQrCode_H5(text,table) {

    var LODOP = getLodop();
   // LODOP.ADD_PRINT_HTM('0mm', '0mm', '100mm', '500mm', "");
    ADD_PRINT_TABLE("50mm", "50mm", "100mm", "500", table);
    LODOP.PREVIEW();
   // LODOP.PRINT();
    $("#printUnit").remove();
}
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

function utf16to8(str) { //转码 
    var out, i, len, c;
    out = "";
    len = str.length;
    for (i = 0; i < len; i++) {
        c = str.charCodeAt(i);
        if ((c >= 0x0001) && (c <= 0x007F)) {
            out += str.charAt(i);
        } else if (c > 0x07FF) {
            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        } else {
            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
        }
    }
    return out;
}