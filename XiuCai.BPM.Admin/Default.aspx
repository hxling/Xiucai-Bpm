<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Xiucai.BPM.Admin.Default" %>
<%@ Register Assembly="Combres" Namespace="Combres" TagPrefix="cb" %>
<%@ Import Namespace="Xiucai.Common" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>疯狂秀才基本权限框架2013</title>
	<script type="text/javascript" src="Scripts/jquery-1.8.3.min.js"></script>
   
    <!--[if lte IE 8]><script src="/scripts/json2.js"></script><![endif]-->

	<%=WebExtensions.CombresLink(base.ThemeName) %>
	<cb:Include ID="siteCssCtrl" runat="server" SetName="siteCss"/>
	
	<script type="text/javascript" src="sys/ashx/ConfigHandler.ashx?action=js"></script>
	


	<script type="text/javascript">
		var theme = '<%=base.ThemeName %>';
	    var urlRoot = "<%= ConfigHelper.GetValue("sitepath") %>"; // 设置后台程序所在的目录 如： /admin
	</script>
</head>

<body onselectstart="return false;" class="easyui-layout" style="overflow-y: hidden; "  fit="true"   scroll="no">
	
	<div id="loading" style="position: fixed;top: -50%;left: -50%;width: 200%;height: 200%;background: #fff;z-index: 100;overflow: hidden;">
		<img src="images/ajax-loader.gif" style="position: absolute;top: 0;left: 0;right: 0;bottom: 0;margin: auto;"/>
	</div>
	

<noscript>
<div style=" position:absolute; z-index:100000; height:2046px;top:0px;left:0px; width:100%; background:white; text-align:center;">
	<img src="images/noscript.gif" alt='抱歉，请开启脚本支持！' />
</div></noscript>


		<%=NavContent %>
	

	<div  region="south" split="false" style="height: 30px; border-top:none; ">
		<div class="footer">By 疯狂秀才 Email:1055818239@qq.com</div>
	</div>
	<div id="mainPanle" region="center" style="background: #eee; overflow-y:hidden" border="false">
		<div id="tabs" class="easyui-tabs"  fit="true"  >
			<%--<div title="欢迎使用" style="padding:20px;overflow:hidden;" id="home">
				
			</div>--%>
		</div>
	</div>

	<div id="closeMenu" class="easyui-menu" style="width:150px;">
		<div id="refresh" iconCls="icon-arrow_refresh" >刷新</div>
		<div class="menu-sep"></div>
		<div id="close">关闭</div>
		<div id="closeall">全部关闭</div>
		<div id="closeother">除此之外全部关闭</div>
		<div class="menu-sep"></div>
		<div id="closeright">关闭右侧标签</div>
		<div id="closeleft">关闭左侧标签</div>
		<div class="menu-sep"></div>
		<div id="exit">退出</div>
	</div>
	
	<div id="notity"></div>
	
	
	
	
	<!-- 加入隐藏的帧，用于检查用户的登录状态是否已过期 -->
	<iframe height="0" width="0" src="CheckUserState.aspx"></iframe>
	
	
	<%=WebExtensions.CombresLink("siteJs") %>

	<script src="Scripts/validate/jquery.validate.min.js" type="text/javascript"></script>
	<script src="Scripts/validate/jQuery.Validate.message_cn.js" type="text/javascript"></script>

	<script type="text/javascript" src="Editor/xhEditor/xheditor-1.2.1.min.js"></script>
	<script src="Editor/xhEditor/xheditor_lang/zh-cn.js"></script>

	<script type="text/javascript" src="scripts/knockout/knockout-2.1.0.js"></script>
	<script type="text/javascript" src="scripts/knockout/knockout.mapping-latest.js"></script>
	
	<script type="text/javascript" src='ashx/MenuData.ashx'> </script>
	
	<%=WebExtensions.CombresLink("layout") %>
	
</body>
</html>
