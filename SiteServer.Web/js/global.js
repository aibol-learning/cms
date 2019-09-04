var global = {
    apikey: "38bfc679-ec19-488d-9877-c66016bbe2d5", //apikey
    newsSiteId: 36, //新闻中心网站id
    loginInfo: null, //当前登录信息
    urls: {
        backstage: 'http://backstage.aibol.com.cn/',
        backstageTasks: 'http://backstage.aibol.com.cn/tasks'  //待办任务查看更多点击跳转的页面url
    }
};

// 获取当前登录用户信息
$.ajax({
    url: '/api/aibol/userinfo',
    type: 'post',
    success: function (response) {
        if (response.displayName || response.userName) {
            global.loginInfo = response;
        }
    }
});