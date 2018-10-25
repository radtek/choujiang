function initDatePicker(startSelector, endSelector) {
    var startElem = $(startSelector);
    if (!startElem) {
        console.error("不能找到元素");
    }
    var start = {
        max: '2099-06-16 23:59:59'
        , istoday: false
        , choose: function (datas) {
            if (endElem) {
                end.min = datas; //开始日选好后，重置结束日的最小日期
                end.start = datas //将结束日的初始值设定为开始日
            }
        }
    };
    startElem.on("click", function () {
        start.elem = this;
        laydate(start);
    })

    var endElem = $(endSelector);
    if (endElem) {
        var end = {
            max: '2099-06-16 23:59:59'
            , istoday: false
            , choose: function (datas) {
                start.max = datas; //结束日选好后，重置开始日的最大日期
            }
        };

        endElem.on("click", function () {
            end.elem = this;
            laydate(end);
        })
    }
}