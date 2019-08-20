<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.BackgroundPages.BasePageCms" %>

<%@ Register TagPrefix="ctrl" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
<!DOCTYPE html>
<html style="background-color: #eeeeee;">

<head>
    <script src="../assets/jquery/jquery-1.9.1.min.js"></script>
    <script src="../assets/vue/vue.min.js"></script>
    <link href="../assets/datePicker/skin/WdatePicker.css" rel="stylesheet" />
    <script src="../assets/datePicker/WdatePicker.js"></script>
    <meta charset="utf-8">
    <!--#include file="../inc/head.html"-->
    <script type="text/javascript">
        var siteId;
        $(document).ready(function () {
            $('body').height($(window).height());
            $('body').addClass('scroll');
            function getQueryVariable(variable) {
                var query = window.location.search.substring(1);
                var vars = query.split("&");
                for (var i = 0; i < vars.length; i++) {
                    var pair = vars[i].split("=");
                    if (pair[0] == variable) { return pair[1]; }
                }
                return (false);
            }

            siteId = getQueryVariable("siteId");
            var vm = new Vue({
                el: "#app",
                data: {
                },
                methods: {
                    exportData: function () {
                        window.open('/api/aibol/GetExcel?siteId=' + siteId + "&startTime=" + $("#startTime").val() + "&endTime=" + $("#endTime").val());
                    }
                }
            });
        });
    </script>
</head>

<body style="margin: 0; padding: 0; background-color: #eeeeee;">
    <form class="m-0" runat="server">
        <div class="list-group mail-list">
            <div onclick="location.reload(true);" style="cursor: pointer; background-color: #dddddd;" class="list-group-item b-0">
                栏目列表
            </div>
        </div>
        <table class="table table-sm table-hover table-tree">
            <tbody>
                <ctrl:ChannelTree runat="server"></ctrl:ChannelTree>
            </tbody>
        </table>
    </form>
    <div id="app">

        <input id="startTime" class="Wdate" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'});" />
        <br/>
        <br/>
        <input id="endTime"  class="Wdate" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'});" />

        <button type="button" class="btn btn-primary btn-sm" v-on:click="exportData">导出全部</button>

    </div>


</body>

</html>
<!--#include file="../inc/foot.html"-->
