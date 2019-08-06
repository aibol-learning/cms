<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadExcel.aspx.cs" Inherits="SiteServer.API.SiteServer.aibol.UploadExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
<div id="app">
    {{test}}
    <button v-on:click="getAdmins">getAdmins</button>
</div>
    <script src="../assets/jquery/jquery-1.9.1.min.js"></script>
    <script src="../assets/vue/vue.min.js"></script>
<script>
    $(function() {
        var vm = new Vue({
            el: "#app",
            data: {
                test:"111"
            },
            methods: {
                getAdmins: function() {
                    $.get("/api/aibol/getadmins", {}, function(re) {
                        vm.test = JSON.stringify(re);
                    });
                }
            },
            created: function() {
                alert("ok");
            }
        });

    })
</script>
</body>
</html>
