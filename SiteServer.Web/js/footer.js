$(function () {
    //Footer - վ������������ռ�����ȡ
    vm = new Vue({
        el: "#vm_siteVisitData",
        data: {
            siteVisitData: {    //վ�����������
                todayPV: null,  //վ���շ�����
                avgPV: null,    //վ��ƽ��������
                totalPV: null   //վ���ܷ�����
            },
            site: 'major'       //վ�������ͳ�Ƶ�վ���ʶ
        },
        methods: {
            //��ȡվ���շ���������
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

            //��ȡվ��ƽ������������
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

            //��ȡվ���ܷ���������
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

            //���վ�����������
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

            //վ���������������
            this.setPV();
        }
    })

    //IE9������placeholder
    if (navigator.appName == "Microsoft Internet Explorer" && parseInt(navigator.appVersion.split(";")[1].replace(/[ ]/g, "").replace("MSIE", "")) <= 9) {
        $('input').placeholder();
    }
});