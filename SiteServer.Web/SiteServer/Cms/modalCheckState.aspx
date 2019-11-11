<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.Cms.ModalCheckState" Trace="false" %>

<%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
<!DOCTYPE html>
<html class="modalPage">

<head>
    <meta charset="utf-8">
    <!--#include file="../inc/head.html"-->
    <script src="../../js/bootstrap.min.js"></script>
    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../js/jsPlumb-1.7.9-min.js"></script>
    <link href="../../css/jsPlumbToolkit-defaults.css" rel="stylesheet" />
    <style>
        #diagramContainer {
            position: relative;
            font-size: smaller;
            padding: 20px 50px;
            height: 288px;
            border: 1px solid gray;
            background: #fff;
            background-image: linear-gradient(#bab5b54d 1px, transparent 0), linear-gradient(90deg, #bab5b54d 1px, transparent 0), linear-gradient(#bab5b54d 1px, transparent 0), linear-gradient(90deg, #bab5b54d 1px, transparent 0);
            background-size: 15px 15px, 15px 15px, 75px 75px, 75px 75px;
        }

        .rectangle-size {
            position: absolute;
            text-align: center;
            line-height: 40px;
            height: 40px;
            width: 100px;
            border: 2px solid #000;
            border-radius: 5px;
        }

        .circle-size {
            position: absolute;
            text-align: center;
            line-height: 100px;
            height: 100px;
            width: 100px;
            border: 2px solid #000;
            border-radius: 50px;
        }

        .cst-label {
            background-color: white;
            padding: 5px;
        }

        .dim-color {
            color: #a2a1a1;
            border-color: #a2a1a1;
        }

        .start, .end {
            width: 50px;
            height: 36px;
            border-radius: 18px;
            position: absolute;
            border: 1px solid gray;
            color: white;
            text-align: center;
            line-height: 36px;
        }

        .start {
            background: green;
        }

        .end {
            background: red;
        }
    </style>
</head>

<body>
    <form runat="server">
        <ctrl:Alerts runat="server" />

        <div class="form-group form-row">
            <label class="col-2 col-form-label text-right">内容标题</label>
            <div class="col-10 form-control-plaintext">
                <asp:Literal ID="LtlTitle" runat="server"></asp:Literal>

            </div>
        </div>

        <div class="form-group form-row">
            <label class="col-2 col-form-label text-right">审核状态</label>
            <div class="col-4 form-control-plaintext">
                <asp:Literal ID="LtlState" runat="server"></asp:Literal>
            </div>
        </div>

        <asp:PlaceHolder ID="PhCheckReasons" runat="server" Visible="false">
            <table class="table tablesaw table-hover m-0">
                <thead>
                    <tr class="thead">
                        <th>审核人</th>
                        <th>审核时间</th>
                        <th>原因</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RptContents" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltlCheckDate" runat="server"></asp:Literal>
                                </td>
                                <td>
                                    <asp:Literal ID="ltlReasons" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </asp:PlaceHolder>

        <hr />

        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-body">

                        <div id="diagramContainer">

                            <div id="start" class="start" style="top: 22px;">开始</div>
                            <div id="step1" class="rectangle-size" style="top: 20px; left: 160px;">新建</div>
                            <div id="step2" class="rectangle-size" style="top: 20px; left: 300px;">支部书记审批</div>
                            <div id="step3" class="rectangle-size" style="top: 120px; left: 300px;">公司领导审批</div>
                            <div id="step4" class="rectangle-size" style="top: 220px; left: 300px;">政工部审批</div>
                            <div id="step5" class="rectangle-size" style="top: 220px; left: 160px;">审批通过</div>
                            <div id="end" class="end" style="top: 222px;">结束</div>
                        </div>
                        <script>
                            /* global jsPlumb */
                            jsPlumb.ready(function () {
                                var flag = 0;
                                $('#myModal').on('show.bs.modal', function (e) {
                                    if (flag != 0) {
                                        return;
                                    }
                                    flag = 1;
                                    setTimeout(function () {
                                        var currentLv = <%=ContentInfo.IsChecked?3:ContentInfo.CheckedLevel %>+2;
                                        var lv1Sub = '<%=ContentInfo.Lv1AdminSub %>';
                                        var lv2Sub = '<%=ContentInfo.Lv2AdminSub %>';
                                        var lv3Sub = '<%=ContentInfo.Lv3AdminSub %>';
                                        if (lv1Sub) {
                                            $("#lv1").html("加载中...");
                                            $.get("/api/aibol/GetAdminBySub", { sub: lv1Sub }, function(res) {
                                                $("#lv1").html(res.data.text);
                                            });
                                        }
                                        if (lv2Sub) {
                                            $("#lv2").html("加载中...");
                                            $.get("/api/aibol/GetAdminBySub", { sub: lv2Sub }, function (res) {
                                                $("#lv2").html(res.data.text);
                                            });
                                        }
                                        if (lv3Sub) {
                                            $("#lv3").html("加载中...");
                                            $.get("/api/aibol/GetAdminBySub", { sub: lv3Sub }, function (res) {
                                                $("#lv3").html(res.data.text);
                                            });
                                        }

                                        if (currentLv == -97) {
                                            currentLv = 1;
                                        }

                                        for (var i = 1; i <= 5; i++) {
                                            var bgcolor = currentLv == i ? '#184785' : i > currentLv ? '#eeeeef' : '#008C5E';
                                            var color = currentLv == i ? '#fff' : i > currentLv ? 'black' : '#fff';
                                            var border = currentLv == i ? 'black' : i > currentLv ? '#346789' : 'black';
                                            $("#step" + i).css({ background: bgcolor, color: color, 'border-color': border});
                                        }

                                              /* global jsPlumb */
                                              function makeStyle(checklv) {
                                                  var config = {};
                                                  config.connectorPaintStyle = {
                                                      lineWidth: 2,
                                                      strokeStyle: currentLv == checklv ? '#184785' : checklv > currentLv ? '#346789' : '#008C5E',
                                                      joinstyle: 'round',
                                                      outlineColor: '',
                                                      outlineWidth: ''
                                                  };

                                                  return {
                                                      // 端点形状
                                                      endpoint: ['Dot', {
                                                          radius: 6,
                                                          fill: currentLv == checklv ? '#184785' : checklv > currentLv ? '#346789' : '#008C5E'
                                                      }],
                                                      // 连接线的样式
                                                      connectorStyle: config.connectorPaintStyle,
                                                      // 端点的样式
                                                      paintStyle: {
                                                          fillStyle: currentLv == checklv ? '#184785' : checklv > currentLv ? '#346789' : '#008C5E',
                                                          radius: 4
                                                      },
                                                      isSource: true,
                                                      connector: ['Straight', {
                                                          gap: 0,
                                                          cornerRadius: 5,
                                                          alwaysRespectStubs: true
                                                      }],
                                                      isTarget: true,
                                                      // 设置连接点最多可以链接几条线
                                                      maxConnections: -1,
                                                      connectorOverlays: [
                                                          ['Arrow', {
                                                              width: 8,
                                                              length: 10,
                                                              location: 1
                                                          }]
                                                      ]
                                                  };
                                              }




                                          jsPlumb.setContainer('diagramContainer');


                                        jsPlumb.addEndpoint('start', { uuid: 'start', anchor: 'Right' }, makeStyle(1));
                                        jsPlumb.addEndpoint('step1', { uuid: 'step1', anchor: 'Left' }, makeStyle(1));
                                        jsPlumb.connect({ uuids: ['start', 'step1'] });
                                        

                                        jsPlumb.addEndpoint('step1', { uuid: 'step1', anchor: 'Right' }, makeStyle(1));
                                        var step2 = jsPlumb.addEndpoint('step2', { uuid: 'step2', anchor: 'Left' }, makeStyle(2));
                                        jsPlumb.connect({ uuids: ['step1', 'step2'] });
                                        if (currentLv>=2) {
                                            step2.addOverlay(['Custom', {
                                                create: function (component) {
                                                    return $('<p id="lv1" style="color: white; background-color: #00b19d; padding: 2px; border-radius: 5px; margin-left: 100px; margin-top: 18px; font-size: xx-small;">未指派</p>');
                                                },
                                                location: 0.5
                                            }]);
                                        }

                                        jsPlumb.addEndpoint('step2', { uuid: 'step2', anchor: 'Bottom' }, makeStyle(2));
                                        var step3 = jsPlumb.addEndpoint('step3', { uuid: 'step3', anchor: 'Top' }, makeStyle(3));
                                        jsPlumb.connect({ uuids: ['step2', 'step3'] });
                                        
                                        if (currentLv >= 3) {
                                            step3.addOverlay(['Custom', {
                                                create: function (component) {
                                                    return $('<p id="lv2" style="color: white; background-color: #00b19d; padding: 2px; border-radius: 5px; margin-left: 49px; margin-top: 40px; font-size: xx-small;">未指派</p>');
                                                },
                                                location: 0.5
                                            }]);
                                        }

                                        jsPlumb.addEndpoint('step3', { uuid: 'step3', anchor: 'Bottom' }, makeStyle(3));
                                        var step4 = jsPlumb.addEndpoint('step4', { uuid: 'step4', anchor: 'Top' }, makeStyle(4));
                                        jsPlumb.connect({ uuids: ['step3', 'step4'] });
                                        
                                        if (currentLv >= 4) {
                                            step4.addOverlay(['Custom', {
                                                create: function (component) {
                                                    return $('<p id="lv3" style="color: white; background-color: #00b19d; padding: 2px; border-radius: 5px;margin-left: 49px; margin-top: 40px; font-size: xx-small;">未指派</p>');
                                                },
                                                location: 0.5
                                            }]);
                                        }

                                        jsPlumb.addEndpoint('step4', { uuid: 'step4', anchor: 'Left' }, makeStyle(4));
                                        jsPlumb.addEndpoint('step5', { uuid: 'step5', anchor: 'Right' }, makeStyle(5));
                                          jsPlumb.connect({ uuids: ['step4', 'step5'] });

                                        jsPlumb.addEndpoint('step5', { uuid: 'step5', anchor: 'Left' }, makeStyle(5));
                                        jsPlumb.addEndpoint('end', { uuid: 'end', anchor: 'Right' }, makeStyle(5));
                                          jsPlumb.connect({ uuids: ['step5', 'end'] });


                                      }, 300);
                                  })

                              })
                        </script>
                        <%--ID:<%=ContentInfo.Id %><br/><br/>
                          当前状态:<%=ContentInfo.CheckedLevel %><br/><br/>
                          初审:<%=ContentInfo.Lv1AdminSub %>     <br/><br/>
                          复审:<%=ContentInfo.Lv2AdminSub %>     <br/><br/>
                          终审:<%=ContentInfo.Lv3AdminSub %>     <br/><br/>--%>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="text-right mr-1">

            <button class="btn btn-primary m-l-5" type="button" data-toggle="modal" data-target="#myModal">查看流程图</button>



            <asp:Button class="btn btn-primary m-l-5" ID="BtnCheck" Text="审 核" OnClick="Submit_OnClick" runat="server" />
            <button type="button" class="btn btn-default m-l-5" onclick="window.parent.layer.closeAll()">取 消</button>
        </div>

    </form>
</body>

</html>
<!--#include file="../inc/foot.html"-->
