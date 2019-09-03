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
                padding:20px 50px ;
                height: 288px;
                border: 1px solid gray;
                background: #fff;
                background-image: 
                                linear-gradient(#bab5b54d 1px, transparent 0), 
                                linear-gradient(90deg, #bab5b54d 1px, transparent 0), 
                                linear-gradient(#bab5b54d 1px, transparent 0), 
                                linear-gradient(90deg, #bab5b54d 1px, transparent 0);
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
        </style>
    </head>

    <body>
      <form runat="server">
        <ctrl:alerts runat="server" />

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
                              <div id="step1" class="rectangle-size" style="top: 20px;">草稿</div>
                              <div id="step2" class="rectangle-size"  style="top: 20px; right: 100px;">支部书记审批</div>
                              <div id="step3" class="rectangle-size"  style="top: 120px;right: 100px;">公司领导审批</div>
                              <div id="step4" class="rectangle-size"  style="top: 120px;">政工部审批</div>
                              <div id="step5" class="rectangle-size"  style="top: 220px;">审批通过</div>
                          </div>
                          <script>
                              /* global jsPlumb */
                              jsPlumb.ready(function () {
                                  var flag = 0;
                                  $('#myModal').on('show.bs.modal', function (e) {
                                      if (flag !=0 ) {
                                          return;
                                      }
                                      flag = 1;
                                      setTimeout(function () {


                                          /* global jsPlumb */
                                          function makeStyle(flag) {
                                              var config = {};
                                              config.connectorPaintStyle = {
                                                  lineWidth: 1,
                                                  strokeStyle: flag == 'dim' ? '#a2a1a1' : 'black',
                                                  joinstyle: 'round',
                                                  outlineColor: '',
                                                  outlineWidth: ''
                                              };

                                              return {
                                                  // 端点形状
                                                  endpoint: ['Dot', {
                                                      radius: 6,
                                                      fill: flag == 'dim' ? '#a2a1a1' : 'black'
                                                  }],
                                                  // 连接线的样式
                                                  connectorStyle: config.connectorPaintStyle,
                                                  // 端点的样式
                                                  paintStyle: {
                                                      fillStyle: flag == 'dim' ? '#a2a1a1' : 'black',
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

                                          var config = {
                                              baseStyle: makeStyle('base'),
                                              dimStyle: makeStyle('dim')
                                          };
                                          var pointStyle = config.baseStyle;

                                          jsPlumb.setContainer('diagramContainer');
                                          jsPlumb.addEndpoint('step1', { uuid: 'step1', anchor: 'Right' }, pointStyle);
                                          jsPlumb.addEndpoint('step2', { uuid: 'step2', anchor: 'Left' }, pointStyle);
                                          jsPlumb.connect({ uuids: ['step1', 'step2'] });

                                          jsPlumb.addEndpoint('step2', { uuid: 'step2', anchor: 'Bottom' }, pointStyle);
                                          jsPlumb.addEndpoint('step3', { uuid: 'step3', anchor: 'Top' }, pointStyle);
                                          jsPlumb.connect({ uuids: ['step2', 'step3'] });

                                          jsPlumb.addEndpoint('step3', { uuid: 'step3', anchor: 'Left' }, pointStyle);
                                          jsPlumb.addEndpoint('step4', { uuid: 'step4', anchor: 'Right' }, pointStyle);
                                          jsPlumb.connect({ uuids: ['step3', 'step4'] });

                                          jsPlumb.addEndpoint('step4', { uuid: 'step4', anchor: 'Bottom' }, pointStyle);
                                          jsPlumb.addEndpoint('step5', { uuid: 'step5', anchor: 'Top' }, pointStyle);
                                          jsPlumb.connect({ uuids: ['step4', 'step5'] });


                                      },300)
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