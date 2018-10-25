var type;//判断页面
layui.use('laydate', function () {
    var laydate = layui.laydate;

    var start = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , choose: function (datas) {
            end.min = datas; //开始日选好后，重置结束日的最小日期
            end.start = datas //将结束日的初始值设定为开始日
        }
    };

    var end = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , choose: function (datas) {
            start.max = datas; //结束日选好后，重置开始日的最大日期
        }
    };

    document.getElementById('contract_effectivedate').onclick = function () {
        start.elem = this;
        laydate(start);
    }
    document.getElementById('contract_enddate').onclick = function () {
        end.elem = this
        laydate(end);
    }

});
var cust_contract_id = "";//合同ID
layui.use('form', function () {
    var forms = layui.form;

    forms.on('radio(radio)', function (data) {
        if ($("#btnupdate").attr("data-id") == "1") {
            cust_contract_id = data.elem.parentNode.parentNode.children[14].innerText;
        } else {
            cust_contract_id = data.elem.parentNode.parentNode.children[16].innerText;
            vrel_id = data.elem.parentNode.parentNode.children[17].innerText;
            vcontract_no = data.elem.parentNode.parentNode.children[1].innerText;
        }


    });

})
function look() {
    return '<button  class="layui-btn btn lookimg">查看</button>';
}

function update() {
    var type = $("#contract_type").val();
    return '<button  class="layui-btn btn update"  data-id="' + type + '">编辑</button>';
}
var assettable = table.get("assetstable");
$(function () {
    //查询
    $("#btnSel").on("click", function () {
        var contract_effectivedate = $("#contract_effectivedate").val();
        var contract_enddate = $("#contract_enddate").val();
        var contract_no = $("#contract_no").val();
        var pageIndex = 1;
        var contract_type = $("#contract_type").val();
        var pay_type = $("#pay_type").val();
        var cus_id;
        if (contract_type == 2) {
            cus_id = $("#cus_id").val();
        }
        var estate_id = $("#estate_id").val();
        var param = { contract_effectivedate: contract_effectivedate, pageIndex: pageIndex, contract_enddate: contract_enddate, contract_no: contract_no, contract_type: contract_type, pay_type: pay_type, cus_id: cus_id, estate_id: estate_id };
        assettable.load(param);
    })
    //单元格编辑
    $("body").on("click", ".update", function () {
        cust_contract_id = 0;

        type = $(this).attr("data-id");

        if (type == 1) {
            cust_contract_id = $(this)[0].parentNode.parentNode.children[15].innerText;
            if (cust_contract_id == "") {
                layer.msg("资源不存在请选中要操作的资源信息");
                return;
            }
            Open('编辑', '/ContractManager/RentContractSave', SetForm)
        } else {
            cust_contract_id = $(this)[0].parentNode.parentNode.children[17].innerText;
            if (cust_contract_id == "") {
                layer.msg("资源不存在请选中要操作的资源信息");
                return;
            }
            Open('编辑', '/ContractManager/RentalContractSave', SetForm)
        }
    });
    //查看图片
    $("body").on("click", ".lookimg", function () {
        var m_cust_contract_id = 0;
        var type = $("#contract_type").val();
        if (type == "1") {
            m_cust_contract_id = $(this)[0].parentNode.parentNode.children[15].innerText;
            var num = $(this)[0].parentNode.cellIndex;
            if (num == 11) {
                //合同照片
                GetImgList(m_cust_contract_id, 1);

            } else if (num == 14) {
                //使用前照片
                GetImgList(m_cust_contract_id, 2);
            }
        } else {
            m_cust_contract_id = $(this)[0].parentNode.parentNode.children[17].innerText;
            var num = $(this)[0].parentNode.cellIndex;
            if (num == 13) {
                //合同照片
                GetImgList(m_cust_contract_id, 1);

            } else if (num == 16) {
                //使用前照片
                GetImgList(m_cust_contract_id, 2);
            }
        }


    })
    //获取合同图片信息
    function GetImgList(m_cust_contract_id, file_type) {

        var daoru = layer.load(0, {
            shade: [0.1, '#676a6c'] //0.1透明度的白色背景
        });
        var url = "/ContractManager/GetContractFilePath";
        $.ajax({
            url: url,
            data: { cust_contract_id: m_cust_contract_id, file_type: file_type },
            type: "post",
            success: function (e) {
                layer.close(daoru);
                var data = eval(e);
                $("#imglist").html("");
                for (var i = 0; i < data.length; i++) {
                    $("#imglist").append('<div style="display: inline-block; "   data-name="' + data[i].file_name + '"><img layer-src="' + data[i].file_path + '" title="' + data[i].file_name + '" name="file_path" width="100px" height="100px" src="' + data[i].file_path + '" /></div>');
                }

                layer.photos({
                    photos: '#imglist'
                    , anim: 5,
                    closeBtn: 1
                });
                layer.open({
                    type: 1,//0（信息框，默认）1（页面层）2（iframe层）3（加载层）4（tips层）
                    title: "图片",
                    closeBtn: 1, //不显示关闭按钮
                    shade: [0],
                    area: ['600px', '500px'],
                    offset: 'ct', //右下角弹出
                    maxmin: true, //开启最大化最小化按钮
                    //time: 2000, //2秒后自动关闭
                    anim: 2,
                    content: $('#imgview'), //iframe的url，no代表不显示滚动条
                });

            },
            error: function () {
                layer.close(daoru);
                layer.msg("系统异常请稍后再试");
            }
        });
    }
    //导出全部图片
    $("body").on("click", "#btnexportimg", function () {
        //获取所有需要导出的图片信息
        var imglist = $("#imglist img");
        imglist.each(function (index, item) {
            $("#downloadimg")[0].href = item.src;
            $("#downloadimg")[0].click();
        })
    });
    //编辑

    $("#btnupdate").on("click", function () {
        if (cust_contract_id == "") {
            layer.msg("资源不存在请选中要操作的资源信息");
            return;
        }
        type = $(this).attr("data-id");
        if (type == 1) {
            Open('编辑', '/ContractManager/RentContractSave', SetForm)
        } else {
            Open('编辑', '/ContractManager/RentalContractSave', SetForm)
        }

    })
});
//设置编辑信息
function SetForm(v) {
    //请求数据
    //发送请求
    var daoru = layer.load(0, {
        shade: [0.1, '#676a6c'] //0.1透明度的白色背景
    });
    var url = "/ContractManager/GetContractById";
    $.ajax({
        url: url,
        data: { id: cust_contract_id, type: type },
        type: "post",
        success: function (e) {
            layer.close(daoru);
            var data = eval(e);
            v.SetData(data);
            v.$("#pay_type").val(data.pay_type);
            v.form.render('select');
            v.SetFile(data.filelist);

        },
        error: function () {
            layer.close(daoru);
            var data = eval(e);
            v.SetData(data);
            v.SetFile(data.filelist);

            //layer.close(daoru);
            //layer.msg("系统异常请稍后再试");
        }
    });
}
//以窗体打开
var vindex = 0;
var vrel_id = 0;
var vcontract_no = "";
var vid;
var vname;
if (typeof (parent.layer) != "undefined") {
    vindex = parent.layer.getFrameIndex(window.name); //获取窗口索引

}
function vbtnsave() {
    if (vrel_id <= 0) {
        layer.msg("请选中合同信息！");
        return;
    }
    parent.$("#" + vid + "").val(vrel_id);
    parent.$("#" + vname + "").val(vcontract_no);
    parent.layer.close(vindex);
}
function vbtncancel() {
    parent.layer.close(vindex);
}

//单位界面操作
function cus(v) {
    v.$("#btnlist").show();
    v.$("#btnshow").hide();
    v.$("#btnplay").hide();
    v.cus_name = "cus_name";
    v.cus_id = "cus_id";
    v.cus_mobile = "cus_mobile";
}
//去除条件
$("#clearCus").on("click", function () {
    $("#cus_name").val("");
    $("#cus_id").val("");
})
//资源界面
function estate(v) {
    var contract_type = $("#contract_type").val();
    v.$("#btnplay").hide();
    v.$("#btnlist").show();
    v.$("#btnshow").hide();
    v.p_estate_name = "estate_name";
    v.p_estate_id = "estate_id";
    v.parent_from = 2;
    
    //v.p_surplus_estate_area = "surplus_estate_area";
    v.p_estate_address = "estate_address";
    v.rent_out = contract_type == 2 ? "租赁合同" : "租入合同";

    if (contract_type == 2) {
        v.status = 1;
        var param = { status: 1 };
        v.gid.load(param);
    }
    else {
        v.status = 2;
        var param = { status: 2 };
        v.gid.load(param);
    }
    
}
//去除条件
$("#clearEstate").on("click", function () {
    $("#estate_name").val("");
    $("#estate_id").val("");
})
