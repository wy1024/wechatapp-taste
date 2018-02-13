Page({
    data:{
        resultType:"",
        resultContent:""
    },
    onLoad:function(options){
        var resultType=options.resultType;
        if(resultType=="success"){
            this.setData({
                resultType:"success",
                resultContent:"订单已送厨房",
                url:'../../order/list/list?status=tosend'
            });
        }
    }
});