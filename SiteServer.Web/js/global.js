var global = {
    apikey: "38bfc679-ec19-488d-9877-c66016bbe2d5", //apikey
    newsSiteId: 36, //新闻中心网站id
    loginInfo: null, //当前登录信息
    urls: {
        backstage: 'http://backstage.aibol.com.cn/',
        sso: 'https://pass.aibol.com/backstage/identity/',
        backstageTasks: 'http://backstage.aibol.com.cn/tasks',  //待办任务查看更多点击跳转的页面url
        backstageTask: 'http://backstage.aibol.com.cn/task/'         //待办任务点击跳转的页面url
    }, 
    // 单点登录配置
    clients: {
        // 新闻门户
        portal: {
            clientId: 'pw_portal_local',
            redirectUri: escape('http://localhost:51687/api/aibol/idlogon')
        },
        // 新闻中心
        news: {
            clientId: 'pw_news',
            redirectUri: escape('http://localhost:51687/newssite/')
        },
        getLoginUrl: function (clientId) {
            var clientConfig = global.clients[clientId];

            return global.urls.sso +
                'connect/authorize?' +
                'client_id=' + clientConfig.clientId +
                '&redirect_uri=' + clientConfig.redirectUri +
                '&scope=openid%20profile' +
                '&response_mode=form_post' +
                '&response_type=id_token%20token' +
                '&state=' + new Date().getTime() +
                '&nonce=' + clientId;
        },
        getLogoutUrl: function () {
            return "/api/aibol/logout";
        }
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