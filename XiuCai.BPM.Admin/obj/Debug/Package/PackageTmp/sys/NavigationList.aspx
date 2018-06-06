<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="NavigationList.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.NavigationList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="toolbar">
        <%=base.BuildToolbar() %>
        <div class="datagrid-btn-separator"></div>
        <a id="a_setbtn" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-cog" title="分配按钮">分配按钮</a>
    </div>


    <table id="navGrid"></table>
    
     <script type="text/javascript" src="../scripts/Linqjs/linq.min.js"></script>
    <script type="text/javascript" src="../scripts/Linqjs/linq.jquery.js"></script>
   
    

    <script type="text/javascript" src="js/Navigation.js?v=16"></script>
</asp:Content>
