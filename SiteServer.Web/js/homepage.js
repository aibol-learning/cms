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

    //大屏幕数据表
    var setCharts, charts;
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
                    
                jQuery(".slideBox1").slide({ mainCell: ".bd ul", effect: "left", autoPlay: true, delayTime: 1000 });
                setTimeout(function () {
                    $("#charts").css("visibility", "visible");
                },100)
                
            },
            set1: function (res) {
                var colorList = [
                    '#ff7f50', '#87cefa', '#da70d6', '#32cd32', '#6495ed', '#ff69b4', '#ba55d3'
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
                        //label : {show: true, position: 'top'}
                    }
                };

                var powerData = function () {
                    var data = [];
                    for (var i = 0; i < 7; i++) {
                        data.push((res.GeneratorSets[i].Powers[res.GeneratorSets[i].Powers.length - 1]).toFixed(2));
                    }
                    return data;
                }

                var powerData2 = function () {
                    var total = [630, 630, 700, 700, 1000, 1000, 500];
                    var data = [];
                    for (var i = 0; i < 7; i++) {
                        data.push((total[i] - res.GeneratorSets[i].Powers[res.GeneratorSets[i].Powers.length - 1]).toFixed(2));
                    }
                    return data;
                }

                // 指定图表的配置项和数据
                var optionChart1 = {
                    color: ['#3398DB'],
                    //title: {
                    //text: 'ECharts 入门示例'
                    //},
                    tooltip: {},
                    legend: {
                        data: ['发电量', '剩余']
                    },
                    xAxis: {
                        data: ["一号机组", "二号机组", "三号机组", "四号机组", "五号机组", "六号机组", "光伏"]
                    },
                    yAxis: {
                        axisLabel: {
                            formatter: '{value}(MW)'
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
                                color: 'rgba(0,0,255, 0.2)'
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
            set8: function (res) {
                //大屏幕8 - 柱状图
                var getData = function() {
                    var data = [];
                    for (var i = 0; i < 6; i++) {
                        data.push(res.GeneratorSets[i].RunningDays);
                    }
                    return data;
                }
                        
                //图表初始化
                var myChart8 = echarts.init(document.getElementById('chart8'));

                // 指定图表的配置项和数据
                var option8 = {
                    color: ['#3bafda'],
                    //title: {
                    //    text: ''
                    //},
                    tooltip: {},
                    //legend: {
                    //    data: ['连续运行时间']
                    //},
                    xAxis: {
                        data: ['一号机组', '二号机组', '三号机组', '四号机组', '五号机组', '六号机组'],
                        axisLabel: {
                            interval: 0,
                            //rotate: 45
                        }
                    },
                    yAxis: {
                    },
                    series: [{
                        name: '连续运行时间',
                        type: 'bar',
                        barCategoryGap: '50%',
                        data: getData()
                    }]
                };
                //使用配置项和数据显示图表。
                myChart8.setOption(option8);
            }
        }
    });
    
    $.get('/api/aibol/screenData', {}, function (res) {
        setCharts(res);
    });

    var setCharts = function (res) {
        //大屏幕
        charts.setData(res);
    }


    //头条新闻，头条聚集
    vm = new Vue({
        el: "#vm",
        data: {
            headlineNews_focus: [],     //头条聚集
            headlineNews: []            //头条新闻
        },
        methods: {
            //获取新闻中心网站所有一级栏目
            getChannels: function () {
                var def = $.Deferred();
                $.ajax({
                    url: '/api/v1/stl/channels?siteId=' + global.newsSiteId + '&apiKey=' + global.apikey,
                    type: 'get',
                    success: function (response) {
                        def.resolve(response.value);
                    },
                    error: function (response) {
                        def.reject(response);
                    }
                });
                return def.promise();
            },

            //头条新闻数据源请求：新闻中心-头条新闻，前4条
            getHeadlineNews: function (node) {
                var self = this;
                if (node.indexName == '头条新闻') {
                    $.ajax({
                        url: '/api/v1/stl/contents?siteId=' + global.newsSiteId + '&totalNum=4&apiKey=' + global.apikey + '&channelId=' + node.id,
                        type: 'get',
                        success: function (response) {
                            self.headlineNews = response.value;
                        }
                    })
                }
            },

            //头条聚焦数据源请求： 新闻中心-头条新闻，有“推荐”属性的内容，前8条
            getHeadlineNewsFocus: function (node) {
                var self = this;
                if (node.indexName == '头条新闻') {
                    $.ajax({
                        url: '/api/v1/stl/contents?siteId=' + global.newsSiteId + '&isRecommend=true&totalNum=8&apiKey=' + global.apikey + '&channelId=' + node.id,
                        type: 'get',
                        success: function (response) {
                            self.headlineNews_focus = response.value;
                        }
                    })
                }
            }
        },
        created: function () {
            var self = this;

            //头条新闻|头条聚焦数据请求(siteServer接口)
            this.getChannels().done(function (data) {
                data.forEach(function (node, index) {
                    self.getHeadlineNews(node);
                    self.getHeadlineNewsFocus(node);
                })
            }).fail(function (data) {
                console.log("reject");
            });
        }
    })

    //报刊阅览 - 图片左右滚动
    var sliderSize2 = 3;

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