<%@ Page Title="" Language="C#" MasterPageFile="../Site1.Master" AutoEventWireup="true" CodeBehind="SysConfig.aspx.cs" Inherits="Xiucai.BPM.Admin.sys.SysConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
<div id="sysconfig" style="margin:10px;">

    <h1>基本设置</h1>
    <div class="c">
        <ul>
            <li><div>皮肤：</div><input type="text" id="txt_theme" name="theme" /></li>
            
        </ul>
    </div>
    <h1>菜单设置</h1>
    <div class="c">
        <ul>
            <li><div>表现方式：</div><input type="text" id="txt_nav_showtype" name="navshowtype"/></li>
            <li><div>&nbsp;</div> <img id="imgPreview" title="点击看大图" src="/images/menuStyles/Accordion.png" style="width: 200px; margin-top: 3px;padding: 2px; border: 1px solid #ccc;" alt=""/></li>
        </ul>
    </div>
    <h1>数据表格设置</h1>
    <div class="c">
        <ul>
            <li><div>每页记录数：</div><input type="text" id="txt_grid_rows" name="gridrows" /></li>
        </ul>
    </div>
    
</div>

<div style="margin:140px;width:160px; margin-top:40px; font-family:'Microsoft YaHei'">

    <a id="btnok" href="javascript:;" 
        class="alertify-button alertify-button-ok">保存设置</a>
    
</div>
    <script src="ashx/ConfigHandler.ashx?action=js" type="text/javascript"></script>
    <script src="js/config.js?nguid=7" type="text/javascript"></script>

</asp:Content>
