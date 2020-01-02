<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.Cms.PageContentSearch" %>

<%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <!--#include file="../inc/head.html"-->
    <script type="text/javascript">
        $(document).ready(function () {
            loopRows(document.getElementById('contents'), function (cur) {
                cur.onclick = chkSelect;
            });
            //$("#DdlState").html("");

            //$("#DdlState").append('<option value="-200">全部</option>');
            //$("#DdlState").append('<option value="-99">新建</option>');
            //$("#DdlState").append('<option value="0">支部书记审批</option>');
            //$("#DdlState").append('<option value="-1">支部书记审批退稿</option>');
            //$("#DdlState").append('<option value="1">公司领导审批</option>');
            //$("#DdlState").append('<option value="-2">公司领导审批退稿</option>');
            //$("#DdlState").append('<option value="2">政工部</option>');
            //$("#DdlState").append('<option value="-3">政工部退稿</option>');
        });
    </script>
</head>

<body>
    <form class="m-l-15 m-r-15" runat="server">
        <ctrl:Alerts runat="server" />

        <div class="card-box">

            <div class="m-t-10">
                <div class="form-inline">
                    <div class="form-group">
                        <label class="col-form-label m-r-10">栏目</label>
                        <asp:DropDownList ID="DdlChannelId" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="form-control" runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group m-l-10">
                        <label class="col-form-label m-r-10">状态</label>
                        <asp:DropDownList ID="DdlState" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-inline m-t-10">
                    <div class="form-group">
                        <label class="col-form-label m-r-10">时间：从</label>
                        <ctrl:DateTimeTextBox ID="TbDateFrom" class="form-control" Columns="12" runat="server" />
                    </div>

                    <div class="form-group m-l-10">
                        <label class="col-form-label m-r-10">到</label>
                        <ctrl:DateTimeTextBox ID="TbDateTo" class="form-control" Columns="12" runat="server" />
                    </div>

                    <div class="form-group m-l-10">
                        <label class="col-form-label m-r-10">目标</label>
                        <asp:DropDownList ID="DdlSearchType" class="form-control" runat="server"></asp:DropDownList>
                    </div>

                    <div class="form-group m-l-10">
                        <label class="col-form-label m-r-10">关键字</label>
                        <asp:TextBox ID="TbKeyword" class="form-control" runat="server" />
                    </div>

                    <asp:Button class="btn btn-success m-l-10 btn-md" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                </div>
            </div>

            <div class="panel panel-default m-t-20">
                <div class="panel-body p-0">
                    <div class="table-responsive">
                        <table id="contents" class="table tablesaw table-hover m-0">
                            <thead>
                                <tr class="thead">
                                    <th>内容标题(点击查看) </th>
                                    <th class="text-nowrap">栏目 </th>
                                    <asp:Literal ID="LtlColumnsHead" runat="server"></asp:Literal>
                                    <th class="text-center text-nowrap" width="100">状态</th>
                                    <th width="20" class="text-center text-nowrap">
                                        <input type="checkbox" onclick="selectRows(document.getElementById('contents'), this.checked);">
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="RptContents" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-nowrap">
                                                <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
                                            </td>
                                            <asp:Literal ID="ltlColumns" runat="server"></asp:Literal>
                                            <td class="text-center text-nowrap">
                                                <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
                                            </td>
                                            <td class="text-center text-nowrap">
                                                <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>

                    </div>
                </div>
            </div>

            <ctrl:Pager ID="PgContents" runat="server" />

            <hr />

            <asp:Button class="btn m-r-5" ID="BtnSelect" Text="选择显示项" runat="server" />
            <asp:Button class="btn m-r-5" ID="BtnAddToGroup" Text="添加到内容组" runat="server" />
            <asp:Button class="btn m-r-5" ID="BtnTranslate" Text="转 移" runat="server" />
            <asp:PlaceHolder ID="PhCheck" runat="server" Visible="False">
                <asp:Button class="btn m-r-5" ID="BtnCheck" Text="审 核" runat="server" />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="PhTrash" runat="server" Visible="false">
                <asp:Button class="btn m-r-5 btn-success" ID="BtnRestore" Text="还 原" runat="server" />
                <asp:Button class="btn m-r-5" ID="BtnRestoreAll" Text="全部还原" runat="server" />
                <asp:Button class="btn m-r-5" ID="BtnDeleteAll" Text="清空回收站" runat="server" />
            </asp:PlaceHolder>
            <asp:Button class="btn m-r-5" ID="BtnDelete" Text="删 除" runat="server" />

        </div>

    </form>
</body>

</html>
<!--#include file="../inc/foot.html"-->
