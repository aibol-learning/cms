<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="toHtml.aspx.cs" Inherits="SiteServer.API.SiteServer.toHtmlDemo.toHtml" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    转html测试
 
        <p>
            <asp:FileUpload ID="FileUpload1" runat="server" OnDataBinding="FileUpload1_DataBinding" />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </p>
    </form>
    <div id="htmlContent" runat="server">
    </div>
</body>
</html>
