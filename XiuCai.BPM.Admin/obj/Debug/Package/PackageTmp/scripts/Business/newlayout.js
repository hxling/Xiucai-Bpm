
var onlyOpenTitle = "欢迎使用";
var opentabs = 5; //允许打开的TAB数量

$(function () {
    if (menus) {
        initNav();
        tabClose();
        addTab('欢迎使用', 'welcome.html', 'icon-house_star'); //添加默认TAB
    } else {
        $.messager.alert("系统提示", "<font color=red><b>您没有任何权限！请联系管理员。</b></font>", "warning", function () {
            location.href = '/ajax/loginout.ashx';
        });
    }
    $('#editpass').click(function () {
        editMyPass();
    });

    $('body').layout({
        onExpand: function () {
            $('body').layout('resize');
        }
    });

    //Tab右键菜单
    $('#closeMenu').menu({
        onClick: function (item) {
            closeTab(item.id);
        }
    });

});

function initNav() {
    if (sys_config.showType) {
        switch (sys_config.showType) {
            case "menubutton":
                MenuButton.init();
                break;
            case "Accordion":
                Accordion.Init(menus, 0);
                break;
            case "Accordion2":
                Accordion.Init(menus, 1); //手风琴大图标
                break;
            case "menuAccordion":
                MenuAccordion.init(0);//菜单+手风琴小图标
                break;
            case "menuAccordion2":
                MenuAccordion.init(1); //菜单+手风琴大图标
                break;
            case "AccordionTree": //手风琴+tree
                AccordionTree.init();
                break;
            case "tree":
                treeNav.init();
                break;
        }
    }
    else
        MenuButton.init();

    $('#tabs').tabs({
        tools: [{
            title: '首页',
            iconCls: 'icon-house_star',
            handler: function () {
                addTab('Welcome', 'welcome.html', 'icon-house_star');
                return false;
            }
        }, {
            iconCls: 'icon-arrow_refresh',
            handler: function () {
                var tab = $('#tabs').tabs('getSelected');
                if (tab.panel('options').title != onlyOpenTitle)
                    closeTab('refresh');
                return false;
            }
        }, {
            iconCls: 'panel-tool-close',
            handler: function () {
                if (confirm('亲，确认要关闭所有窗口吗？')) {
                    closeTab("closeall");
                }
            }
        }],
        onContextMenu: function (e, title) {
            if (title != onlyOpenTitle) {
                e.preventDefault();
                $('#closeMenu').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
                $('#tabs').tabs('select', title);
            } else {
                return false;
            }
        }
    });
}

//手风琴效果导航
var Accordion = {
    addNav: function (data, iconType) {
        $.each(data, function (i, sm) {
            var menulist = "";
            if (iconType == 0) { //小图标
                menulist += '<ul class="smallicon menuItem" id="menu'+sm.id+'">';
                $.each(sm.children, function (j, o) {
                    menulist += '<li  data-options=\'' + JSON.stringify(o.attributes) + '\'><div><a ref="' + o.id + '" href="#" rel="'
                        + o.attributes.url + '?navid=' + o.id + '" ><span iconCls="' + o.iconCls + '" class="icon ' + o.iconCls
                        + '" >&nbsp;</span><span class="nav">' + o.text
                        + '</span></a></div></li> ';
                });
            } else {
                menulist += '<ul class="bigicon menuItem" id="menu' + sm.id + '">';
                $.each(sm.children, function (j, o) {
                    menulist += '<li data-options=\'' + JSON.stringify(o.attributes) + '\' ><div><a ref="' + o.id + '" href="#" rel="'
                        + o.attributes.url + '?navid=' + o.id + '" ><span class="img" iconCls="' + o.iconCls + '">'
                        + '<img src="' + urlRoot + o.attributes.BigImageUrl + '"/></span><span class="nav">' + o.text
                        + '</span></a></div></li> ';
                    
                });
            }
            menulist += '</ul>';

            $('#wnav').accordion('add', {
                title: sm.text,
                content: menulist,
                iconCls: sm.iconCls,
                border: false,
                selected: (i == 0)
            });
            
            $('#menu' + sm.id).data("menus", sm);
        });
    },
    Init: function (menuData, iconType) {
        $("#wnav").accordion({
            fit: true,
            border: false,
            onSelect: function (t, i) {
                //alert(t);
            }
        });

        Accordion.addNav(menuData, iconType);

        $('.menuItem li').live({
            click: function () {
                var a = $(this).children('div').children('a');
                var tabTitle = $(a).children('.nav').text();

                var url = $(a).attr("rel");
                var menuid = $(a).attr("ref");
                var icon = $(a).children('span').attr('iconCls');

               
                var mattr = $(this).data('options');
                if (mattr.IsNewWindow) {
                    $.hDialog({
                        title: tabTitle,
                        href: url,
                        width: mattr.WinWidth,
                        height: mattr.WinHeight
                    });
                } else {
                    addTab(tabTitle, url, icon);
                }

                
                $('.accordion li div').removeClass("selected");
                $(this).children('div').addClass("selected");
                return false;
            }
        });
    }
};
//横向导航
var MenuButton = {
    initMenuItem: function (n, str) {
        $.each(n.children, function (j, o) {
            //递归
            if (o.children.length > 0) {
                str += '<div iconCls="' + o.iconCls + '" id="' + o.attributes.url + '?navid=' + o.id + '">';
                str += '<span iconCls="' + o.iconCls + '">' + o.text + '</span><div style="width:120px;">';
                str = MenuButton.initMenuItem(o, str);
                str += '</div></div>';
            } else
                str += '<div iconCls="' + o.iconCls + '" id="' + o.attributes.url + '?navid=' + o.id + '">' + o.text + '</div>';
        });
        return str;
    },
    addNav: function (data) {
        var menulist = "";
        var childMenu = '';
        $.each(data, function (i, n) {
            menulist += String.format('<a href="javascript:void(0)" id="mb{0}" class="easyui-menubutton" menu="#mm{0}" iconCls="{1}">{2}</a>',
                (i + 1), n.iconCls, n.text);

            if (n.children.length > 0) {
                childMenu += '<div id="mm' + (i + 1) + '" style="width:120px;">';
                childMenu = MenuButton.initMenuItem(n, childMenu);

                childMenu += '</div>';
            }
        });

        $('#wnav').append(menulist).append(childMenu);

    },
    init: function () {
        MenuButton.addNav(menus);
        $('#wnav').css({ 'float': 'left', 'width': '100%', 'height': '30px', 'padding': '3px 0px 0px 20px', 'background': '#6ABEFA url(/images/datagrid_title_bg.png)' });

        if (theme == "gray") {
            $('#wnav').css('background', 'url(Scripts/easyui/themes/gray/images/tabs_enabled.gif)');
        }

        var northPanel = $('body').layout('panel', 'north');
        northPanel.panel('resize', { height: 103 });

        $('body').layout('resize');

        var mb = $('#wnav .easyui-menubutton').menubutton();
        $.each(mb, function (i, n) {
            $($(n).menubutton('options').menu).menu({
                onClick: function (item) {
                    var tabTitle = item.text;
                    var url = item.id;
                    var icon = item.iconCls;
                    addTab(tabTitle, url, icon);
                    return false;
                }
            });
        });
    }
};
//左侧树形菜单
var treeNav = {
    init: function () {
        $("#wnav").tree({
            animate: true,
            lines: true,
            data: menus,
            onClick: function (node) {
                if (node.attributes.url != "#" && node.attributes.url != '') {
                    
                    addTab(node.text, node.attributes.url + '?navid=' + node.id, node.iconCls);
                } else {
                    $('#wnav').tree('toggle', node.target);
                }
            }
        });
    }
};

var findMenu = function (id) {
    var m;
    $.each(menus, function (i, n) {
        if (id == n.id)
            m = n;
    });
    return m;
}

//菜单 + 手风琴 大、小图标
var MenuAccordion = {
    init: function (iconType) {
        var menusHtml = '';
        //添加横向菜单
        $.each(menus, function (i, n) {
            menusHtml += '<a id="' + n.id + '" style="float:left" href="javascript:;" rel="' + n.attributes.url + '" class="easyui-linkbutton" data-options="iconCls:\'' + n.iconCls + '\',plain:true">' + n.text + '</a> ';
        });

        var northPanel = $('body').layout('panel', 'north');
        northPanel.panel('resize', { height: 103 });

        $('#tnav').css({ 'float': 'left', 'width': '100%', 'height': '30px', 'padding': '3px 0px 0px 20px', 'background': '#6ABEFA url(/images/datagrid_title_bg.png)' });

        if (theme == "gray") {
            $('#tnav').css('background', 'url(Scripts/easyui/themes/gray/images/tabs_enabled.gif)');
        }

        $('#tnav').empty().html(menusHtml);
        $('#tnav a').linkbutton().click(function () {

            var menuId = $(this).attr('id');

            var tabTitle = $(this).text();
            var url = $(this).attr("rel");
            var icon = $(this).linkbutton('options').iconCls;

            if (!url && url != '' && url != '#')
                addTab(tabTitle, url, icon);

            var currentMenuObj = findMenu(menuId);
            if (currentMenuObj) {
                if (currentMenuObj.children.length > 0) {
                    var west = $('body').layout('panel', 'west');
                    if (west.length == 0) {
                        $('body').layout('add', {
                            title: tabTitle,
                            region: 'west',
                            width: 180,
                            split: true,
                            content: '<ul id="wnav"></ul>',
                            iconCls: icon
                        });
                    } else {
                        west.panel({ title: tabTitle, content: '<ul id="wnav"></ul>', iconCls: icon });
                    }

                    Accordion.Init(currentMenuObj.children, iconType);
                }
            }

        });

        $('body').layout('remove', 'west');
    }
};

//手风琴 +tree
var AccordionTree = {
    init: function () {



        $.each(menus, function (i, n) {
            //$('#wnav').accordion('add', {
            //    title: n.text,
            //    content: '<div style="padding:0px;"><ul id="nt' + i + '"></ul></div>',
            //    border: false,
            //    iconCls: n.iconCls,
            //    selected:(i==0)
            //});

            $('#wnav').append('<div style="padding:0px;" title="' + n.text + '" data-options="border:false,iconCls:\'' + n.iconCls + '\'"><ul id="nt' + i + '"></ul></div>');

        });

        $("#wnav").accordion({
            fit: true,
            border: false,
            onSelect: function (t, i) {
                $('#nt' + i).tree({
                    lines: false,
                    animate: true,
                    data: menus[i].children,
                    onClick: function (node) {
                        if (node.attributes.url != "" && node.attributes.url != '#') {
                            addTab(node.text, node.attributes.url + '?navid=' + node.id, node.iconCls);
                        } else {
                            $('#nt' + index).tree('toggle', node.target);
                        }
                    }
                });
            }
        });


    }
};



function addTab(subtitle, url, icon) {
    if (url == "" || url == "#")
        return false;
    var tabCount = $('#tabs').tabs('tabs').length;
    var hasTab = $('#tabs').tabs('exists', subtitle);
    var add = function () {
        if (!hasTab) {
            $('#tabs').tabs('add', {
                title: subtitle,
                content: createFrame(url),
                closable: (subtitle != onlyOpenTitle),
                icon: icon
            });
        } else {
            $('#tabs').tabs('select', subtitle);
            //closeTab('refresh'); //选择TAB时刷新页面
        }
    };
    if (tabCount > opentabs && !hasTab) {
        var msg = '<b>您当前打开了太多的页面，如果继续打开，会造成程序运行缓慢，无法流畅操作！</b>';
        $.messager.confirm("系统提示", msg, function (b) {
            if (b) add();
            else return false;
        });
    } else {
        add();
    }
}

function createFrame(url) {
    var s = '<iframe scrolling="auto" frameborder="0"  style="width:100%;height:100%;" src="' + url + '" ></iframe>';
    return s;
}

function tabClose() {
    /*双击关闭TAB选项卡*/
    $(".tabs-inner").live('dblclick', function () {
        var subtitle = $(this).children(".tabs-closable").text();
        if (subtitle != onlyOpenTitle && subtitle != "")
            $('#tabs').tabs('close', subtitle);
    });
}

function closeTab(action) {
    var alltabs = $('#tabs').tabs('tabs');
    var currentTab = $('#tabs').tabs('getSelected');
    var allTabtitle = [];
    $.each(alltabs, function (i, n) {
        allTabtitle.push($(n).panel('options').title);
    });
    switch (action) {
        case "refresh":
            var iframe = $(currentTab.panel('options').content);
            var src = iframe.attr('src');
            $('#tabs').tabs('update', {
                tab: currentTab,
                options: {
                    content: createFrame(src)
                }
            });
            break;
        case "close":
            var currtab_title = currentTab.panel('options').title;
            $('#tabs').tabs('close', currtab_title);
            break;
        case "closeall":
            $.each(allTabtitle, function (i, n) {
                if (n != onlyOpenTitle) {
                    $('#tabs').tabs('close', n);
                }
            });
            break;
        case "closeother":
            var currtab_title = currentTab.panel('options').title;
            $.each(allTabtitle, function (i, n) {
                if (n != currtab_title && n != onlyOpenTitle) {
                    $('#tabs').tabs('close', n);
                }
            });
            break;
        case "closeright":
            var tabIndex = $('#tabs').tabs('getTabIndex', currentTab);

            if (tabIndex == alltabs.length - 1) {
                alert('亲，后边没有啦 ^@^!!');
                return false;
            }
            $.each(allTabtitle, function (i, n) {
                if (i > tabIndex) {
                    if (n != onlyOpenTitle) {
                        $('#tabs').tabs('close', n);
                    }
                }
            });

            break;
        case "closeleft":
            var tabIndex = $('#tabs').tabs('getTabIndex', currentTab);
            if (tabIndex == 1) {
                alert('亲，前边那个上头有人，咱惹不起哦。 ^@^!!');
                return false;
            }
            $.each(allTabtitle, function (i, n) {
                if (i < tabIndex) {
                    if (n != onlyOpenTitle) {
                        $('#tabs').tabs('close', n);
                    }
                }
            });

            break;
        case "exit":
            $('#closeMenu').menu('hide');
            break;
    }
}

var str_editpass = '<form id="xiuCai_Form"><table class="grid">';
str_editpass += '<tr><td>登录名：</td><td><span id="loginname"></span></td></tr>';
str_editpass += '<tr><td>旧密码：</td><td><input required="true" id="txtOldPassword" name="password" type="password" class="txt03 easyui-validatebox" /></td></tr>';
str_editpass += '<tr><td>新密码：</td><td><input validType="safepass"  required="true" id="txtNewPassword" name="password" type="password" class="txt03 easyui-validatebox" /></td></tr>';
str_editpass += '<tr><td>确认密码：</td><td><input data-options="required:true" validType="equals[\'#txtNewPassword\']" id="txtReNewPassword" type="password" class="txt03 easyui-validatebox" /></td></tr>';
str_editpass += '</table></form>';


var editMyPass = function () {
    top.$.hDialog({
        width: 330, height: 240, title: '修改密码', iconCls: 'icon-key', content: str_editpass, submit: function () {
            if ($('#xiuCai_Form').form('validate')) {
                $.ajaxjson('/sys/ashx/userhandler.ashx', "action=editpass2&old=" + $('#txtOldPassword').val() + "&new=" + $('#txtNewPassword').val(), function (d) {
                    if (d.Success) {
                        alert('密码修改成功！请重新登录。');
                        location.href = 'ashx/loginout.ashx';
                    } else
                        alert(d.Message);
                });
            }
        }
    });

    $('#loginname').text($('#curname').text());
};


$(function () {
    $('#loginOut').click(function () {
        $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {
            if (r) {
                $.ajaxtext("ashx/loginout.ashx", null, function (msg) {
                    if (msg == "ok") {
                        var dir = location.pathname;
                        //alert(dir);
                        if (dir == '/') {
                            location.href = '/login.html';
                        } else {
                            dir = dir.substr(0, dir.lastIndexOf('/') + '/login.html');
                            location.href = dir;
                        }
                    }
                });
            }
        });
    });


    $(window).load(function () {
        $('#loading').fadeOut();
    });

});
