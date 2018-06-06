<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="ButtonList.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.ButtonList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div id="toolbar">
       
        <%=base.BuildToolbar() %>

        <div style="float:right;margin-right:10px; margin-top:3px;">
            <input id="ss" class="easyui-searchbox" style="width:240px" />
            <div id="searchMenu" style="width:120px;">
                
            </div>
        </div>


    </div>


    <table id="btnGrid"></table>

 
    <script type="text/javascript" src="../scripts/Business/Search.js?v=3"></script>
    
    <script src="../scripts/Business/Export.js"></script>
    <script type="text/javascript" src="js/Buttons.js?v=4"></script>

</asp:Content>
