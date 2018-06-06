<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="DataDic.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.DataDic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="layout">
        <div region="west" style="width:200px;border-right: 0px;" border="true">
            <div class="easyui-panel" title="字典类别" border="false" iconCls="icon-book_red" >
                <div class="toolbar" style="background:#efefef;border-bottom:1px solid #ccc">
                    <a id="a_addCategory" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-add" title="添加字典类别">添加</a>
                    <a id="a_editCategory" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-edit" title="修改字典类别">修改</a>
                    <a id="a_delCategory" style="float:left" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-delete" title="删除字典类别">删除</a>
                </div>
                <div style="padding:5px;">
                    <ul id="dataDicType"></ul>
                </div>
            </div>
            <div id="noCategoryInfo" style="font-family:微软雅黑; font-size: 18px; color:#BCBCBC; padding: 40px 5px;display:none;">
                　　亲，还没有字典类别哦，点击 添加 按钮创建新的类别。
            </div>
        </div>
        <div region="center" border="false" style="overflow: hidden;">
            <div id="toolbar"><%=base.BuildToolbar() %></div>
            <table id="dicGrid"></table>
        </div>
    </div>
    <script type="text/javascript" src="js/dataDic.js?v=90"></script>
</asp:Content>
