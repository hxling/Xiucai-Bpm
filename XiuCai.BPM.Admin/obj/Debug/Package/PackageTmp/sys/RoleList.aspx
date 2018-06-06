<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src='ashx/RoleHandler.ashx?json={"action":"btnColumns"}'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="toolbar">
        <%=base.BuildToolbar() %>
    </div>


    <table id="roleGrid" ></table>
    
    <script type="text/javascript" src="../scripts/Linqjs/linq.min.js"></script>
    <script type="text/javascript" src="../scripts/Linqjs/linq.jquery.js"></script>
    <script type="text/javascript" src="js/setDataPermission.js"></script>
    <script type="text/javascript" src="js/role.js?v=8"></script>

</asp:Content>
