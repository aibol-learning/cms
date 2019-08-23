<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadExcel.aspx.cs" Inherits="SiteServer.API.SiteServer.aibol.UploadExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <!--#include file="../inc/head.html"-->
</head>
<body>
    <div id="app" class="card-box m-l-15 m-r-15" style="padding: 10px; margin-bottom: 10px;">
        <a href="信息员录入模板.xlsx">下载信息员录入模板</a>
        <br/>
        <br/>
        <a href="支部录入模板.xlsx">下载支部录入模板</a>
        <hr />
        <button class="btn btn-primary btn-lg" style="width: 180px" v-on:click="selectAuthor()">上传并导入信息员</button><br/><br/>
        <button class="btn btn-primary btn-lg" style="width: 180px" v-on:click="selectDepartment()">上传并导入支部</button>
        <form id="form01" action="/app/aibolfile/upload" method="post" enctype="multipart/form-data" style="display: none;">
            <input type="file" id="fileInput" multiple="multiple" name="files" v-on:change="sub()" /><br />
            <input type="hidden" name="type" v-model="type" /><br />
            <button type="button" v-on:click="subAuthor">submitAuthor</button>
            <button type="button" v-on:click="subDepartment">submitDepartment</button>
        </form>
        <%--<hr />
        <button  type="button" v-on:click="getAuthors">getAuthors</button>
        <button  type="button" v-on:click="getDepartments">getDepartments</button>
        <div>
            {{res}}
        </div>--%>
        

    </div>
    <script src="../assets/jquery/jquery-1.9.1.min.js"></script>
    <script src="../assets/vue/vue.min.js"></script>
    <script>
        $(function () {
            var vm = new Vue({
                el: "#app",
                data: {
                    type: "author",
                    res:""
                },
                methods: {
                    selectAuthor: function() {
                        this.type = "author";
                        $("#fileInput").click();
                    },
                    selectDepartment: function () {
                        this.type = "department";
                        $("#fileInput").click();
                    },

                    getAuthors: function() {
                        $.get("/api/aibol/GetAuthors", {}, function(re) {
                            vm.res = JSON.stringify(re);
                        });
                    },
                    getDepartments: function () {
                        $.get("/api/aibol/getDepartments", {}, function (re) {
                            vm.res = JSON.stringify(re);
                        });
                    },
                    subAuthor: function () {
                        this.type = "author";
                        setTimeout(function () {
                            vm.sub();
                        },100);
                    },
                    subDepartment: function () {
                        this.type = "department";
                        setTimeout(function() {
                            vm.sub();
                        }, 100);
                    },
                    sub: function () {
                        var form = document.getElementById("form01");
                        var data = new FormData(form);
                        $.ajax({
                            type: "POST",
                            url: "/api/aibol/Upload",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function (data) {
                                alert(data.msg);
                            },
                            error: function (err) {
                                alert(JSON.stringify(err));
                            }
                        });
                    }
                },
                created: function () {
                }
            });
        })
    </script>
</body>
</html>
