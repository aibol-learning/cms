/** 主站首页js **/

$(function () {
    //顶部轮播图
    var ary = location.href.split("&");
    jQuery(".slideBox").slide({
        mainCell: ".bd ul",
        effect: ary[1],
        autoPlay: true,
        trigger: ary[3],
        easing: ary[4],
        delayTime: ary[10],
        mouseOverStop: ary[6],
        pnLoop: ary[7]
    });

    //生产数据
    var charts = new Vue({
        el: "#charts",
        data: {
            chart2: {
                daily: [],
                weekly: [],
                monthly: [],
                annual: []
            },
            chart3: {
                daily: [],
                weekly: [],
                monthly: [],
                annual: []
            },
            chart4: {
                So2: [],
                Nox: [],
                Dust: []
            },
            chart5: {
                Yesterday: {},
                Weekly: {},
                Monthly: {},
                Annual: {},
            },
            chart6: {
                Yesterday: {},
                Weekly: {},
                Monthly: {},
                Annual: {},
            },
            chart7: {
                Yesterday: {},
                Weekly: {},
                Monthly: {},
                Annual: {},
            },
            chart8: [],
            chart9: []
        },
        methods: {
            setData: function (data) {
                this.set1(data);
                this.set2(data);
                this.set3(data);
                this.set4(data);
                this.chart5 = data.Solars[0];
                this.chart6 = data.Solars[1];
                this.chart7 = data.Solars[2];
                this.set8(data);
                this.set9(data);
                    
                setTimeout(function () {
                    $("#charts").css("visibility", "visible");
                },100)
                
            },
            set1: function (res) {
                var colorList = [
                    //'#ff7f50', '#87cefa', '#da70d6', '#32cd32', '#6495ed', '#ff69b4', '#ba55d3'
                    '#ff3300', '#ff3300', '#ff3300', '#ff3300', '#ff3300', '#ff3300', '#ff3300'
                ];

                //大屏幕1
                var myChart1 = echarts.init(document.getElementById('chart1'));

                var itemStyle = {
                    normal: {
                        color: function (params) {
                            if (params.dataIndex < 0) {
                                // for legend
                                return colorList[colorList.length - 1];
                            }
                            else {
                                // for bar
                                return colorList[params.dataIndex];
                            }
                        },
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}',
                            distance: 0,
                            color: 'red',
                            fontSize: '16'
                        }
                    }
                };

                var powerData = function () {
                    var data = [];
                    for (var i = 0; i < 7; i++) {
                        data.push(parseInt(res.GeneratorSets[i].Powers[res.GeneratorSets[i].Powers.length - 1]));
                    }
                    return data;
                }

                var powerData2 = function () {
                    var total = [630, 630, 700, 700, 1000, 1000, 0];
                    var data = [];
                    for (var i = 0; i < 7; i++) {
                        data.push((total[i] - parseInt(res.GeneratorSets[i].Powers[res.GeneratorSets[i].Powers.length - 1])));
                    }
                    return data;
                }

                // 指定图表的配置项和数据
                var optionChart1 = {
                    color: ['#3398DB'],
                    tooltip: {},
                    //legend: {
                    //    data: ['发电量', '剩余']
                    //},
                    xAxis: {
                        data: ["一号机组", "二号机组", "三号机组", "四号机组", "五号机组", "六号机组", "光伏"]
                    },
                    yAxis: {
                        axisLabel: {
                            formatter: '{value}'
                        }
                    },
                    series: [{
                        name: '发电量',
                        type: 'bar',
                        stack: '使用情况',
                        barCategoryGap: '50%',
                        itemStyle: itemStyle,
                        data: powerData()
                    },
                    {
                        name: '剩余',
                        type: 'bar',
                        stack: '使用情况',
                        barCategoryGap: '50%',
                        itemStyle: {
                            normal: {
                                color: 'rgba(0,0,255, 0.06)'
                            }
                        },
                        data: powerData2()
                    }
                    ]
                };

                // 使用刚指定的配置项和数据显示图表。
                myChart1.setOption(optionChart1);
            },
            set2: function (data) {
                //大屏幕2
                for (var i = 0; i < 6; i++) {
                    this.chart2.daily.push(data.GeneratorSets[i].PowerReport.Daily[data.GeneratorSets[i].PowerReport.Daily.length - 1]);
                    this.chart2.weekly.push(data.GeneratorSets[i].PowerReport.Weekly[data.GeneratorSets[i].PowerReport.Weekly.length - 1]);
                    this.chart2.monthly.push(data.GeneratorSets[i].PowerReport.Monthly[data.GeneratorSets[i].PowerReport.Monthly.length - 1]);
                    this.chart2.annual.push(data.GeneratorSets[i].PowerReport.Annual[data.GeneratorSets[i].PowerReport.Annual.length - 1]);
                }
            },
            set3: function (data) {
                //大屏幕3
                var totalDaily = 0, totalWeekly = 0, totalMonthly = 0, totalAnnual = 0;
                var g1Daily = 0, g1Weekly = 0, g1Monthly = 0, g1Annual = 0;
                var g2Daily = 0, g2Weekly = 0, g2Monthly = 0, g2Annual = 0;
                var g3Daily = 0, g3Weekly = 0, g3Monthly = 0, g3Annual = 0;
                for (var i = 0; i < 6; i++) {
                    var d = this.chart2.daily[i];
                    var w = this.chart2.weekly[i];
                    var m = this.chart2.monthly[i];
                    var a = this.chart2.annual[i];
                    totalDaily += d;
                    totalWeekly += w;
                    totalMonthly += m;
                    totalAnnual += a;
                    if (i < 2) {
                        g1Daily += d;
                        g1Weekly += w;
                        g1Monthly += m;
                        g1Annual += a;
                    }
                    else if (i < 4) {
                        g2Daily += d;
                        g2Weekly += w;
                        g2Monthly += m;
                        g2Annual += a;
                    }
                    else if (i < 6) {
                        g3Daily += d;
                        g3Weekly += w;
                        g3Monthly += m;
                        g3Annual += a;
                    }
                }
                this.chart3.daily.push(totalDaily);
                this.chart3.daily.push(g1Daily);
                this.chart3.daily.push(g2Daily);
                this.chart3.daily.push(g3Daily);

                this.chart3.weekly.push(totalWeekly);
                this.chart3.weekly.push(g1Weekly);
                this.chart3.weekly.push(g2Weekly);
                this.chart3.weekly.push(g3Weekly);

                this.chart3.monthly.push(totalMonthly);
                this.chart3.monthly.push(g1Monthly);
                this.chart3.monthly.push(g2Monthly);
                this.chart3.monthly.push(g3Monthly);

                this.chart3.annual.push(totalAnnual);
                this.chart3.annual.push(g1Annual);
                this.chart3.annual.push(g2Annual);
                this.chart3.annual.push(g3Annual);
            },
            set4: function (data) {
                for (var i = 0; i < 6; i++) {
                    this.chart4.So2.push(data.GeneratorSets[i].So2);
                    this.chart4.Nox.push(data.GeneratorSets[i].Nox);
                    this.chart4.Dust.push(data.GeneratorSets[i].Dust);
                }
            },
            set8: function (data) {
                var self = this;
                var groupName = ['一号机组', '二号机组', '三号机组', '四号机组', '五号机组', '六号机组'];
                data.GeneratorSets.forEach(function (node, index) {
                    if (index < 6) {
                        var percent = Math.round(node.RunningDays / node.LongestDays * 100);
                        self.chart8.push({
                            groupName: groupName[index],
                            RunningDays: node.RunningDays,
                            LongestDays: node.LongestDays,
                            percentBarHtml: '<div class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="' + percent + '" style="width: ' + percent + '%"></div>'
                        })
                    }
                });
            },
            set9: function (data) {
                var self = this;
                var groupName = ['一号机组', '二号机组', '三号机组', '四号机组', '五号机组', '六号机组'];
                data.Stopdays.StopRunningDays.forEach(function (node, index) {
                    var percent = Math.round(node / data.Stopdays.StopRunningHistory[index] * 100);
                        
                    self.chart9.push({
                        groupName: groupName[index],
                        StopRunningDays: node,
                        StopRunningHistory: data.Stopdays.StopRunningHistory[index],
                        percentBarHtml: '<div class="progress-bar progress-bar-warning progress-bar-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="' + percent +'" style="width: '+percent+'%"></div>'
                    })
                });
            }
        },
        created: function () {
            var self = this;
            $.get('/api/aibol/screenData', {}, function (res) {
                self.setData(res)
            });
        }
    });

    //重要新闻, 次要新闻 
    vm = new Vue({
        el: "#vm",
        data: {
            primaryNews: [],            //重要新闻 
            imageNews: {                //图片新闻
                active: true,
                channelName: '图片新闻',
                navigationUrl: '',
                contents: []
            },    
            secondaryNews: [],          //次要新闻 
        },
        methods: {
            //获取新闻中心配置为“门户主要新闻”的栏目
            getPrimaryNewsChannelsByGroup: function () {
                var self = this;
                $.ajax({
                    url: encodeURI('/api/v1/stl/channels?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&groupChannel=门户主要新闻'),
                    type: 'get',
                    success: function (response) {
                        for (var i = 0; i < response.value.length; i++) {
                            var tempObj = {
                                id: response.value[i].id,
                                channelName: response.value[i].channelName,
                                navigationUrl: response.value[i].navigationUrl,
                                contents: [],
                                active: true
                            }
                            self.primaryNews.push(tempObj);
                            self.getPrimaryNewsContents(tempObj.id);
                        }
                    }
                })
            },

            //获取新闻中心配置为“门户主要新闻”的栏目内容 前8条
            getPrimaryNewsContents: function (channelId) {
                var self = this;
                var summaryLen = screen.width >= 1280 ? 55 : 43;
                $.ajax({
                    url: '/api/v1/stl/contents?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&channelId=' + channelId + "&totalNum=8&order=addDate",
                    type: 'get',
                    success: function (response) {
                        for (var i = 0; i < self.primaryNews.length; i++) {
                            if (self.primaryNews[i].id == channelId) {
                                self.primaryNews[i].contents = response.value;
                                self.primaryNews[i].contents.forEach(function (node, index) {
                                    if (node.title.length > 20) {
                                        node.titleShort = node.title.substring(0, 20) + "…";
                                    }
                                    else {

                                        node.titleShort = node.title;
                                    }
                                    if (node.summary.length > summaryLen) {
                                        node.summary = node.summary.substring(0, summaryLen) + "…";
                                    }
                                    node.addDate = node.addDate.substring(0, 10);
                                })
                            }
                        }
                    }
                })
            },

            //获取新闻中心"头条新闻"栏目的内容 推荐+前6条
            getHeadlineContents: function (channelId) {
                var self = this;
                $.ajax({
                    url: '/api/v1/stl/contents?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&channelIndex=头条新闻&totalNum=6&isRecommend=true&order=addDate',
                    type: 'get',
                    success: function (response) {
                        var data = response.value.slice(0, 6);
                        data.forEach(function (node, index) {
                            if (node.title.length > 19) {
                                node.titleShort = node.title.substring(0, 19) + "…";
                            }
                            else {
                                node.titleShort = node.title;
                            }
                        });

                        self.imageNews.contents = data;
                        window.setTimeout(function () {
                            $("#slider_news").slick({
                                infinite: true,
                                dots: true,
                                autoplay: true,
                                autoplaySpeed: 5000
                            });
                        }, 100)
                    }
                })
            },

            //获取新闻中心"头条新闻"栏目详情
            getHeadlineChannelInfo: function (channelId) {
                var self = this;
                $.ajax({
                    url: '/api/v1/stl/channel?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&channelIndex=头条新闻',
                    type: 'get',
                    success: function (response) {
                        self.imageNews.navigationUrl = response.value.navigationUrl;
                    }
                })
            },

            //获取新闻中心配置为“门户次要新闻”的栏目前8个
            getSecondaryNewsChannelsByGroup: function () {
                var self = this;
                $.ajax({
                    url: encodeURI('/api/v1/stl/channels?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&groupChannel=门户次要新闻&totalNum=8'),
                    type: 'get',
                    success: function (response) {
                        var grounpCount = Math.ceil(response.value.length / 4);
                        var tempArray = [];
                        for (var i = 0; i < grounpCount; i++) {
                            var tempArray = [];
                            for (var j = 0; j < 4; j++) {
                                var c = i * 4 + j;
                                if (c < response.value.length) {
                                    var tempObj = {
                                        id: response.value[c].id,
                                        channelName: response.value[c].channelName,
                                        navigationUrl: response.value[c].navigationUrl,
                                        contents: []
                                    }
                                    if (j == 0) {
                                        tempObj.active = true;
                                    }
                                    else {
                                        tempObj.active = false;
                                    }
                                    tempArray.push(tempObj);

                                    self.getSecondaryNewsContents(tempObj.id);
                                }
                            }
                            self.secondaryNews.push(tempArray);
                        }
                    }
                })
            },

            //获取新闻中心配置为“门户次要新闻”的栏目内容 推荐+前5条
            getSecondaryNewsContents: function (channelId) {
                var self = this;
                $.ajax({
                    url: '/api/v1/stl/contents?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey + '&channelId=' + channelId + "&totalNum=5&isRecommend=true&order=addDate",
                    type: 'get',
                    success: function (response) {
                        for (var i = 0; i < self.secondaryNews.length; i++) {
                            for (var j = 0; j < self.secondaryNews[i].length; j++) {
                                if (self.secondaryNews[i][j].id == channelId) {
                                    self.secondaryNews[i][j].contents = response.value;
                                    self.secondaryNews[i][j].contents.forEach(function (node, index) {
                                        node.addDate = node.addDate.substring(0, 10);
                                        if (node.title.length > 25) {
                                            node.titleShort = node.title.substring(0, 25) + "…";
                                        }
                                        else {
                                            node.titleShort = node.title;
                                        }
                                    })
                                }
                            }
                        }
                    }
                })
            },

            //tab切换
            changeTab4SecondaryNews: function (bundleIndex, tabIndex) {
                this.secondaryNews[bundleIndex].forEach(function (node, index) {
                    node.active = false;
                });
                this.secondaryNews[bundleIndex][tabIndex].active = true;
            }
        },
        created: function () {
            //获取新闻中心配置为“门户主要新闻”的栏目及内容
            this.getPrimaryNewsChannelsByGroup();
            //获取新闻中心配置为“门户次要新闻”的栏目及内容
            this.getSecondaryNewsChannelsByGroup();
            //获取新闻中心"头条新闻"栏目详情
            this.getHeadlineChannelInfo();
            //获取新闻中心“头条新闻”的栏目内容
            this.getHeadlineContents();
        }
    })

    //报刊阅览 - 图片左右滚动
    var sliderSize2 = 4;
    if ($(window).width() <= 800) {
        sliderSize2 = 2;
    }
    if ($(window).width() > 800 & $(window).width() <= 1024) {
        sliderSize2 = 3;
    }

    var $slider2 = $('.slider2 ul');
    var $slider_child_l2 = $('.slider2 ul li').length;
    var $slider_width2 = $('.slider2 ul li').width() + 12;
    $slider2.width($slider_child_l2 * $slider_width2);

    var slider_count2 = 0;

    if ($slider_child_l2 < sliderSize2) {
        $('#btn-right2').css({
            cursor: 'auto'
        });
        $('#btn-right2').removeClass("dasabled");
    }

    $('#btn-right2').click(function () {
        if ($slider_child_l2 < sliderSize2 || slider_count2 >= $slider_child_l2 - sliderSize2) {
            return false;
        }

        slider_count2++;
        $slider2.animate({
            left: '-=' + $slider_width2 + 'px'
        }, 'fast');
        slider_pic2();
    });

    $('#btn-left2').click(function () {
        if (slider_count2 <= 0) {
            return false;
        }
        slider_count2--;
        $slider2.animate({
            left: '+=' + $slider_width2 + 'px'
        }, 'fast');
        slider_pic2();
    });

    function slider_pic2() {
        if (slider_count2 >= $slider_child_l2 - sliderSize2) {
            $('#btn-right2').css({
                cursor: 'auto'
            });
            $('#btn-right2').addClass("dasabled");
        } else if (slider_count2 > 0 && slider_count2 <= $slider_child_l2 - sliderSize2) {
            $('#btn-left2').css({
                cursor: 'pointer'
            });
            $('#btn-left2').removeClass("dasabled");
            $('#btn-right2').css({
                cursor: 'pointer'
            });
            $('#btn-right2').removeClass("dasabled");
        } else if (slider_count2 <= 0) {
            $('#btn-left2').css({
                cursor: 'auto'
            });
            $('#btn-left2').addClass("dasabled");
        }
    }

    //判断是否为当日新添加的文章, 打上new标记
    var _today = new Date();
    var yyyy = _today.getFullYear();
    var MM = _today.getMonth() + 1;
    var dd = _today.getDate();
    MM = MM < 10 ? ('0' + MM) : MM;
    dd = dd < 10 ? ('0' + dd) : dd;
    var today = yyyy + "-" + MM + '-' + dd;
    $(".js_addDate").each(function () {
        if ($(this).text() == today) {
            $(this).parents("li").eq(0).find(".label_new").show();
        }
    })
})
