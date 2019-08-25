var global = {
    apikey: "38bfc679-ec19-488d-9877-c66016bbe2d5", //apikey
    newsSiteId: 36, //新闻中心网站id
    loginInfo: null,  //当前登录信息
    backstageAPIUrl: {//工作区接口访问url
        loginInfo: 'http://backstage.aibol.com.cn/api/backstage/logininfo', //当前登录信息接口url
        tours: 'http://backstage.aibol.com.cn/api/backstage/tours', //安全生产管理巡视接口url
        tasks: 'http://backstage.aibol.com.cn/api/backstage/tasks', //待办任务接口url
        loginInfo: 'http://backstage.aibol.com.cn/api/backstage/logininfo', //工作区当前登录信息接口Url
        screenData: ' http://screen.aibol.com.cn/home/data' //大屏幕数据接口Url
    },
    toursTaskUrl: 'http://backstage.aibol.com.cn/tour/',  //安全生产管理巡视任务点击跳转的页面url
        // 正式服：http://10.138.20.219/tour/{id}
        // 内网测试服：http://10.138.20.219:82/tour/{id}
        // 外网测试服务：http://backstage.aibol.com.cn/tour/{id}
    backstageTaskUrl: 'http://backstage.aibol.com.cn/',  //待办任务点击跳转的页面url
        // 正式服：http://10.138.20.219/{redirectUrl}
        // 内网测试服：http://10.138.20.219:82/{redirectUrl}
        // 外网测试服务：http://backstage.aibol.com.cn/{redirectUrl}
} 


//获取当前登录用户信息
$.ajax({
    url: '/api/v1/users/current',
    type: 'post',
    success: function (response) {
        console.log(response);
    }
});