<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="datatest.aspx.cs" Inherits="Xiucai.BPM.Admin.demo.datatest" %>
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


    <table id="list"></table>

 
    <script src="js/datatest.js"></script>
    

</asp:Content>
