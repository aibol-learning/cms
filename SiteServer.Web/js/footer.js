$(function () {
    //Footer - 站点访问量数据收集、获取
    vm = new Vue({
        el: "#vm_siteVisitData",
        data: {
            siteVisitData: {    //站点访问量数据
                todayPV: null,  //站点日访问量
                avgPV: null,    //站点平均访问量
                totalPV: null   //站点总访问量
            },
            site: 'major'       //站点访问量统计的站点标识
        },
        methods: {
            //获取站点日访问量数据
            getTodayPV: function () {
                var self = this;
                $.ajax({
                    url: '/api/aibol/GetPV?site=' + self.site,
                    type: 'get',
                    success: function (response) {
                        if (response.code == 200 && response.data) {
                            self.siteVisitData.todayPV = response.data;
                        }
                    }
                })
            },

            //获取站点平均访问量数据
            getAvgPV: function () {
                var self = this;
                $.ajax({
                    url: '/api/aibol/GetAvgPV?site=' + self.site,
                    type: 'get',
                    success: function (response) {
                        if (response.code == 200 && response.data) {
                            self.siteVisitData.avgPV = response.data;
                        }
                    }
                });
            },

            //获取站点总访问量数据
            getTotalPV: function () {
                var self = this;
                $.ajax({
                    url: '/api/aibol/GetTotalPV?site=' + self.site,
                    type: 'get',
                    success: function (response) {
                        if (response.code == 200 && response.data) {
                            self.siteVisitData.totalPV = response.data;
                        }
                    }
                });
            },

            //添加站点访问量数据
            setPV: function () {
                var self = this;
                $.ajax({
                    url: '/api/aibol/addpv?site=' + self.site,
                    type: 'get',
                    success: function (response) {
                        if (response.code == 200) {
                            self.getTodayPV();
                            self.getAvgPV();
                            self.getTotalPV();
                        }
                    }
                });
            }
        },
        created: function () {
            var self = this;

            //站点访问量数据设置
            this.setPV();
        }
    })

    //IE9及以下placeholder
    if (navigator.appName == "Microsoft Internet Explorer" && parseInt(navigator.appVersion.split(";")[1].replace(/[ ]/g, "").replace("MSIE", "")) <= 9) {
        $('input').placeholder();
    }
});