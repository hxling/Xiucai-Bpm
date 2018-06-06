<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.Departments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
     <div id="toolbar">
        <%=base.BuildToolbar() %>
    </div>


    <table id="depGrid"></table>

 
    <script type="text/javascript" src="js/department.js?v=5"></script>
    

</asp:Content>