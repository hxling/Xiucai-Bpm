<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="DataBackup.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.DataBackup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="toolbar" ><%=base.BuildToolbar() %></div>
    <table id="datagrid"></table>
    <script src="js/databackup.js"></script>
</asp:Content>
