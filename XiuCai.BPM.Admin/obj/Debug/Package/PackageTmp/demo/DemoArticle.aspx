<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DemoArticle.aspx.cs" Inherits="Xiucai.BPM.Admin.demo.DemoArticle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- 也可以在页面中直接加入按钮
    <div class="toolbar">
        <a id="a_add" href="#" plain="true" class="easyui-linkbutton" icon="icon-add1" title="添加">添加</a>
        <a id="a_edit" href="#" plain="true" class="easyui-linkbutton" icon="icon-edit1" title="修改">修改</a>
        <a id="a_delete" href="#" plain="true" class="easyui-linkbutton" icon="icon-delete16" title="删除">删除</a>
        <a id="a_search" href="#" plain="true" class="easyui-linkbutton" icon="icon-search" title="搜索">搜索</a>
        <a id="a_reload" href="#" plain="true" class="easyui-linkbutton" icon="icon-reload" title="刷新">刷新</a>
    </div>
    -->



<!-- 工具栏按钮 -->
    <div id="toolbar"><%= base.BuildToolbar()%></div>

    <!-- datagrid 列表 -->
    <table id="list" ></table>  

    <!-- 引入多功能查询js -->
    <script src="../../scripts/Business/Search.js"></script>

    <!-- 引入js文件 -->
      <script src="js/DemoArticle.js"></script>
</asp:Content>



