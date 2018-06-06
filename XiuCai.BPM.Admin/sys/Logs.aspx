<%@ Page Title="系统日志" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.Logs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="toolbar" >
        <div style="float: left;padding-top:2px;">
            <div style="float: left;width:100%;">
            操作人：<input type="text" name="opuser" id="txtOpuser" class="txt03" style="width: 120px;"/>&nbsp;
            操作日期：<input type="text" id="dtOpstart"/> 至 <input type="text" id="dtOpend"/>
                </div>
            <div style="float: left; margin-top:3px;">
            操作类型：<select name="optype" id="selOptype">
                     <option value="0">请选择</option>
                    <option value="1">添加</option>
                    <option value="2">编辑</option>
                    <option value="3">删除</option>
                    <option value="5,6">登录</option>
                    <option value="7">其他</option>
                </select>&nbsp;&nbsp;
        操作表：<input type="text" id="txtTablename" name="opname" class="txt03" style="width: 100px;"/>
        业务：<input type="text" id="txtBusingessName" name="busingessName" class="txt03" style="width: 100px;"/>
            </div>
            <%=base.BuildToolbar() %>

        </div>
        
    </div>
    <table id="logGrid"></table>

    <script type="text/javascript" src="js/log.js?v=6"></script>

</asp:Content>
