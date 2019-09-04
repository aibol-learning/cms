<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.Cms.ModalContentCheck" Trace="false" %>

<%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
<!DOCTYPE html>
<html class="modalPage">

<head>
    <meta charset="utf-8">
    <!--#include file="../inc/head.html"-->
    <script src="../assets/jquery/jquery-1.9.1.min.js"></script>
    <script src="../assets/vue/vue.min.js"></script>
    <link href="../assets/select2/dist/css/select2.css" rel="stylesheet" />
    <script src="../assets/select2/dist/js/select2.js"></script>
    <script src="../assets/select2/dist/js/i18n/zh-CN.js"></script>
    <script>
        $(function () {

            var siteId = '<%=SiteId %>';
            var channelId = '<%=_channelId %>';
            var contentId = '<%=_contentId %>';

            var getChecker = function () {
                var checkerSelect2 = $("#CheckerSelect2");

                var data = {
                    lv: $("#DdlCheckType").val(),
                    siteId: siteId,
                    channelId: channelId,
                    contentId: contentId
                }

                $.get("/api/aibol/GetCheckerByContent", data, function (res) {
                    if (res.code == 200) {
                        checkerSelect2.select2({
                            language: "zh-CN",
                            ajax: {
                                url: '/api/aibol/GetCheckers',
                                dataType: 'json',
                                delay: 250,
                                data: function (params) {
                                    var query = {
                                        lv: $("#DdlCheckType").val(),
                                        search: params.term,
                                        page: params.page || 1
                                    }
                                    return query;
                                }
                            }
                        });
                        //checkerSelect2.val(res.data).trigger("change");
                        if (res.data.id) {
                            if (checkerSelect2.find("option[value='" + res.data.id + "']").length) {
                                checkerSelect2.val(res.data.id).trigger('change');
                            } else {
                                var newOption = new Option(res.data.text, res.data.id, true, true);
                                checkerSelect2.append(newOption).trigger('change');
                            }
                        }

                        checkerSelect2.val(res.data.id).trigger("change");

                    }
                });

                checkerSelect2.on('change',
                    function (e) {
                        $("#Checker").val(e.target.value);
                    });
            }

            var DdlCheckTypeChange = function () {
                if ($("#DdlCheckType").val() == 0 || $("#DdlCheckType").val() == 1) {
                    $("#checkDiv").show();
                    if ($("#DdlCheckType").val() == 1) {
                        $("#confirmText").show();
                        $("#checkDiv").css("padding-top"," 40px");
                    } else {
                        $("#confirmText").hide();
                        $("#checkDiv").css("padding-top", "0px");
                    }

                    getChecker();
                } else {
                    $("#checkDiv").hide();
                }
            }

            DdlCheckTypeChange();

            $("#DdlCheckType").on("change", function () {
                DdlCheckTypeChange();
            });



        })
    </script>
</head>

<body>
    <form runat="server">
        <ctrl:Alerts runat="server" />

        <div class="form-group form-row">
            <label class="col-3 col-form-label text-right">内容标题</label>
            <div class="col-8">
                <h6>
                    <asp:Literal ID="LtlTitles" runat="server"></asp:Literal></h6>
            </div>
            <div class="col-1"></div>
        </div>

        <div class="form-group form-row">
            <label class="col-3 col-form-label text-right">审核状态</label>
            <div class="col-8">
                <asp:DropDownList ID="DdlCheckType" class="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-1"></div>
        </div>

        <div class="form-group form-row" id="checkDiv" style="position: relative; padding-top: 40px;">
            <div id="confirmText" style="color: red; position: absolute;top:0px; text-align: center; left: 143px;">
                是否确认需要公司领导审批?
            </div>
            <label class="col-3 col-form-label text-right">指定审核人</label>
            <div class="col-8">
                <select id="CheckerSelect2" class="form-control"></select>
                <input id="Checker" type="text" style="display:none" runat="server"/></div>
            <div class="col-1"></div>
        </div>

        <div class="form-group form-row">
            <label class="col-3 col-form-label text-right">转移到栏目</label>
            <div class="col-8">
                <asp:DropDownList ID="DdlTranslateChannelId" class="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-1"></div>
        </div>

        <div class="form-group form-row">
            <label class="col-3 col-form-label text-right">审核原因</label>
            <div class="col-8">
                <asp:TextBox ID="TbCheckReasons" class="form-control" TextMode="MultiLine" Rows="3" runat="server" />
            </div>
            <div class="col-1"></div>
        </div>

        <hr />

        <div class="text-right mr-1">
            <asp:Button class="btn btn-primary m-l-5" Text="确 定" OnClick="Submit_OnClick" runat="server" />
            <button type="button" class="btn btn-default m-l-5" onclick="window.parent.layer.closeAll()">取 消</button>
        </div>

    </form>
</body>

</html>
<!--#include file="../inc/foot.html"-->
