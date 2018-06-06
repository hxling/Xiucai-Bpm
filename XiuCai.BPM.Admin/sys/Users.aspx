<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src='ashx/RoleHandler.ashx?json={"action":"btnColumns"}'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="layout">
        <div region="west" iconCls="icon-chart_organisation" split="true" title="部门树" style="width:220px;padding: 5px" collapsible="false">
            <ul id="deptree"></ul>
        </div>
        <div region="center" title="员工帐号" iconCls="icon-users" style="padding: 2px; overflow: hidden">
            <div id="toolbar">
               <%=base.BuildToolbar() %>
                <div class="datagrid-btn-separator"></div>
                <a id="a_role" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-group" title="角色">角色</a>
                <a id="a_authorize" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-group_gear" title="授权">授权</a>
                <div class="datagrid-btn-separator"></div>
                <a id="a_editPass" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-key" title="修改密码">修改密码</a>
            </div>
            <table id="userGrid" toolbar="#toolbar"></table>

        </div>
    </div>
    
    <script type="text/javascript" src="../scripts/Linqjs/linq.min.js"></script>
    <script type="text/javascript" src="../scripts/Linqjs/linq.jquery.js"></script>
    <script type="text/javascript" src="js/setDataPermission.js"></script>
    <script type="text/javascript" src="../scripts/Business/Search.js?v=3"></script>
    <script type="text/javascript" src="js/user.js?s=134"></script>
</asp:Content>
