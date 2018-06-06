
var actionUrl = 'ashx/userhandler.ashx';
var formUrl = urlRoot + '/sys/html/userform.html';
var edit_formUrl = urlRoot + '/sys/html/edituserform.html';
var editpass_formUrl = urlRoot + '/sys/html/editpass.html';

$(function() {
    var size = { width: $(window).width(), height: $(window).height() };
    mylayout.init(size);
    

    deptree.init();

    autoResize({ dataGrid: '#userGrid', gridType: 'datagrid', callback: mygrid.databind, height: 36, width: 230 });

    $('#a_add').click(crud.add);
    $('#a_edit').click(crud.update);
    $('#a_delete').click(crud.del);
    $('#a_editPass').click(crud.editPass);
    $('#a_role').click(SetRoles);
    $('#a_authorize').click(authorize.run);
    $('#a_dataset').click(authorize.setDeptments);

    $('#a_search').click(function () {
        search.go('userGrid');
    });
    $(window).resize(function () {
        size = { width: $(window).width(), height: $(window).height() };
        mylayout.resize(size);
    });

});

var mylayout = {
    init: function(size) {
        $('#layout').width(size.width - 4).height(size.height - 4).layout();
        var center = $('#layout').layout('panel', 'center');
        center.panel({
            onResize: function(w, h) {
                $('#userGrid').datagrid('resize', { width: w - 6, height: h - 36 });
            }
        });
    },
    resize: function(size) {
        mylayout.init(size);
        $('#layout').layout('resize');
    }
};

function createParam(action,keyid)
{
    var o = {};
    var form = top.$('#xiucaiForm');
    var query = '';
    if (form) {
        query = top.$('#xiucaiForm').serializeArray();
        query = convertArray(query);
        o.jsonEntity = JSON.stringify(query);
    }
    o.action = action;
    o.keyid = keyid;
    return "json=" + JSON.stringify(o);
}


var deptree = {
    init: function() {
        $('#deptree').tree({
            lines: true,
            url: actionUrl + '?' + createParam('deps'),
            animate: true,
            onLoadSuccess:function(node,data) {
                $('body').data('depData', data);
            },onClick: function(node) {
                var depId = node.id;
                var children = $('#deptree').tree('getChildren', node.target);
               // alert(children.length);
                var arr = [];
                arr.push(depId);
                for (var i = 0; i < children.length; i++) {
                    arr.push(children[i].id);
                }

                var strDepIds = arr.join(',');
                var filterObj = { "groupOp": "AND", "rules": [{ "field": "DepartmentId", "op": "in", "data": strDepIds }] };
                $('#userGrid').datagrid('load', { filter: JSON.stringify(filterObj) });
            }
        });
    },
    data: function(){
        var d = JSON.stringify($('body').data('depData'));
        d = '[{"id":0,"text":"请选择部门"},' + d.substr(1);
        return JSON.parse(d);
    }
    
};

var mygrid = {
    databind: function(size) {
        $('#userGrid').datagrid({
            url: actionUrl,
            width: size.width,
            height: size.height,
            idField: 'KeyId',
            singleSelect: true,
            striped: true,
            columns: [[
                { title: 'ID', field: 'KeyId', width: 80, sortable: true },
                { title: '用户名', field: 'UserName', width: 140, sortable: true },
                { title: '真实姓名', field: 'TrueName', width: 120, sortable: true },
                { title: '部门名称', field: 'depname', width: 160 },
                { title: '邮箱', field: 'Email', width: 200, sortable: true },
                {
                    title: '超管',
                    field: 'IsAdmin',
                    width: 80,
                    align: 'center',
                    formatter: function (v, d, i) {
                        if (d.UserName == "admin")
                            return '';
                        return '<img style="cursor:pointer" title="设置超管" onclick="javascript:setUserAttr(\'isadmin\','+d.KeyId+','+v+')" src="/css/icon/16/bullet_' + (v ? "tick.png" : "minus.png") + '" />';
                    }
                },
                {
                    title: '状态',
                    field: 'IsDisabled',
                    width: 80,
                    align: 'center',
                    formatter: function (v, d, i) {
                        if (d.UserName == "admin")
                            return '';
                        return '<img style="cursor:pointer" title="激活禁用帐号" onclick="javascript:setUserAttr(\'isdisabled\',' + d.KeyId + ',' + v + ')" src="/css/icon/16/bullet_' + (v == false ? "tick.png" : "minus.png") + '" />';
                    }
                },{title:'描述',field:'Remark',width:200}
            ]],
            pagination: true,
            pageSize:PAGESIZE,
            rowStyler: function (index, row, css) {
                if (row.UserName=="admin") {
                    return 'font-weight:bold;';
                }
            }
        });
    },
    reload: function() {
        $('#userGrid').datagrid('clearSelections').datagrid('reload');
    },
    selectRow: function() {
        return $('#userGrid').datagrid('getSelected');
    }
};

var crud = {
    initData: function(depid) {
        top.$('#txt_isdisabled,#txt_isadmin').combobox({ panelHeight: 'auto' });
        var _depid = depid || 0;
        top.$('#txt_department').combotree({
            data: deptree.data(),
            valueField: 'id',
            textField: 'text',
            value: _depid
        });
        top.$('#txt_role').combobox({ multiple: true,valueField:'KeyId',textField:'RoleName'});

        $.getJSON(actionUrl + '?' + createParam('roles'), function(d) {
            top.$('#txt_role').combobox({ data: d });
            (function (roles) {
                $.each(roles, function(i, n) {
                    if (n.IsDefault == 1)
                        top.$('#txt_role').combobox('setValue', n.KeyId);
                });
            }(d));
        });

        top.$('#userTab').tabs({
            onSelect: function() {
                top.$('.validatebox-tip').remove();
            }
        });

        top.$('#txt_passSalt').val(randomString());
    },
    add: function() {
        var addDialog = top.$.hDialog({
            href: formUrl + '?v=' + Math.random(),
            width: 500,
            height: 400,
            title: '新建帐号',
            iconCls: 'icon-user_add',
            onLoad: function () {
                var dep = $('#deptree').tree('getSelected');
                var depID = 0;
                if(dep)
                    depID = dep.id || 0;
                
                //如果左侧有选中部门，则添加的时候，部门默认选中
                crud.initData(depID);
            },
            closed: false,
            submit: function () {
                var tab = top.$('#userTab').tabs('getSelected');
                var index = top.$('#userTab').tabs('getTabIndex', tab);
                if (top.$('#xiucaiForm').form('validate')) {
                    var query = createParam('add', 0) + '&roles=' + top.$('#txt_role').combo('getValues');
                    $.ajaxjson(actionUrl, query, function(d) {
                        if (d.Success) {
                            msg.ok('添加成功');
                            mygrid.reload();
                            addDialog.dialog('close');
                        } else {
                            if(d.Data==-2) {
                                msg.error('用户名已存在，请更改用户名。');
                                if (index > 0)
                                    top.$('#userTab').tabs('select', 0);
                                top.$('#txt_username').select();
                            } else {
                                MessageOrRedirect(d);
                            }
                        }
                    });
                } else {
                    if (index > 0)
                        top.$('#userTab').tabs('select', 0);
                }
            }
        });
        
        
        
    },
    update: function() {
        var row = mygrid.selectRow();
        if (row) {
            var editDialog = top.$.hDialog({
                href: edit_formUrl + '?v=' + Math.random(),
                width: 500,
                height: 350,
                title: '修改帐号',
                iconCls: 'icon-user_add',
                onLoad: function() {
                    var viewModel = top.ko.mapping.fromJS(row);
                    top.ko.applyBindings(viewModel);
                    crud.initData();
                    top.$('#txt_department').combotree('setValue', row.DepartmentId);
                    top.$('#txt_isadmin').combobox('setValue', row.IsAdmin.toString());
                    top.$('#txt_isdisabled').combobox('setValue', (row.IsDisabled).toString());
                },
                submit: function() {
                    if (top.$('#xiucaiForm').form('validate')) {
                        var query = createParam('update', row.KeyId);
                        $.ajaxjson(actionUrl, query, function(d) {
                            if (d.Success) {
                                msg.ok('修改成功');
                                mygrid.reload();
                                editDialog.dialog('close');
                            } else {
                                if (d.Data == -2) {
                                    msg.error('用户名已存在，请更改用户名。');
                                    if (index > 0)
                                        top.$('#userTab').tabs('select', 0);
                                    top.$('#txt_username').select();
                                } else {
                                    MessageOrRedirect(d);
                                }
                            }
                        });
                    } else {
                        var tab = top.$('#userTab').tabs('getSelected');
                        var index = top.$('#userTab').tabs('getTabIndex', tab);
                        if (index > 0)
                            top.$('#userTab').tabs('select', 0);
                    }
                }
            });
        } else {
            msg.warning('请选择要修改的用户。');
        }
    },
    del: function() {
        var row = mygrid.selectRow();
        if (row) {
            if(row.UserName =="admin") {
                msg.warning('admin为系统帐号，不能删除！');
                return false;
            }
            var query = createParam('delete', row.KeyId);
            top.$.messager.confirm('删除帐号', '确认要删除选中的用户吗?', function(r) {
                if (r) {
                    $.ajaxjson(actionUrl, query, function(d) {
                        if (d.Success) {
                            msg.ok('删除成功');
                            mygrid.reload();
                        } else {
                            if(d.Data == -2)
                                msg.error('admin为系统帐号，不能删除。');
                            else {
                                MessageOrRedirect(d);
                            }
                        }
                    });
                }
            });
        } else {
            msg.warning('请选择要删除的用户。');
        }
    },
    editPass: function() {
        var row = mygrid.selectRow();
        if(row) {
            var pDialog = top.$.hDialog({
                href: editpass_formUrl,
                title: '修改密码',
                width: 300,
                height: 200,iconCls:'icon-key',
                onLoad: function() {
                   
                },
                submit: function () {
                    if (top.$('#xiucaiForm').form('validate')) {
                        var query = createParam('editpass', row.KeyId) + '&password=' + top.$('#txt_newpass').val();
                        $.ajaxtext(actionUrl, query, function (d) {
                            if (d > 0) {
                                msg.ok('亲，密码修改成功。');
                                pDialog.dialog('close');
                            } else {
                                msg.error('亲，密码修改失败啦~~');
                            }
                        });
                    }
                }
            });
        }else {
            msg.warning('请选择帐号。');
        }
    }
};

//设置用户是否为超管及激活禁用帐号
function setUserAttr(action,uid,val) {
    var query = createParam(action, uid) + '&val=' + val;
    $.ajaxjson(actionUrl, query, function(d) {
        if (d.Success) {
            mygrid.reload();
        } else {
            MessageOrRedirect(d);
        }
    });
}

function SetRoles() { //设置角色
    var row = mygrid.selectRow();
    if(row) {
        var rDialog = top.$.hDialog({
            href: '/sys/html/setroles.html', width: 600, height: 500, title: '设置帐号角色',iconCls:'icon-group',
            onLoad: function () {
               top.$('#rlayout').layout();
               top.$('#uname').text(row.UserName);
               top.$('#allRoles,#selectedRoles').datagrid({
                   width:400,
                   height: 350,
                   iconCls: 'icon-group',
                   nowrap: false, //折行
                   rownumbers: true, //行号
                   striped: true, //隔行变色
                   idField: 'KeyId',//主键
                   singleSelect: true, //单选
                   columns: [[
                       { title: '角色名称', field: 'RoleName', width: 140 },
                       { title: '备注', field: 'Remark', width: 210 }
                   ]],
                   pagination: false,
                   pageSize: 20,
                   pageList: [20, 40, 50]
               });

               top.$('#allRoles').datagrid({
                   url: '/sys/ashx/rolehandler.ashx',
                   onDblClickRow: function (rowIndex, rowData) {
                       top.$('#a_select_role').click();
                   }
               });

               top.$('#selectedRoles').datagrid({
                   url: '/sys/ashx/userhandler.ashx?'+createParam('getroles',row.KeyId),
                   onDblClickRow: function(rowIndex, rowData) {
                       top.$('#selectedRoles').datagrid('deleteRow', rowIndex);
                   }
               });
               top.$('#a_select_role').click(function() {
                   var _row = top.$('#allRoles').datagrid('getSelected');
                   if (_row) {
                       var hasRoleName = false;
                       var roles =top.$('#selectedRoles').datagrid('getRows');
                       $.each(roles,function (i, n) {
                           if (n.RoleName == _row.RoleName) {
                               hasRoleName = true;
                           }
                       });
                       if(!hasRoleName)
                           top.$('#selectedRoles').datagrid('appendRow', _row);
                       else {
                           alert('角色已存在，请不要重复添加。');
                           return false;
                       }
                   } else {
                       alert('请选择角色');
                   }
               });
               
               top.$('#a_delete_role').click(function () {
                   var trow = top.$('#selectedRoles').datagrid('getSelected');
                   if (trow) {
                       var rIndex = top.$('#selectedRoles').datagrid('getRowIndex', trow);
                       top.$('#selectedRoles').datagrid('deleteRow', rIndex).datagrid('unselectAll');
                   } else {
                       alert('请选择角色');
                   }
               });
           },
           submit: function() {
               var selectedRoles = top.$('#selectedRoles').datagrid('getRows');
               var roleIdArr = [];
               $.each(selectedRoles, function(i, n) {
                   roleIdArr.push(n.KeyId);
               });
               var query = createParam("setroles", row.KeyId) + '&roles=' + roleIdArr.join(',');
               $.ajaxtext(actionUrl, query, function (d) {
                   if (d > 0) {
                       msg.ok('亲,角色设置成功啦!');
                       rDialog.dialog('close');
                   } else {
                       alert(':( 设置失败啦。');
                   }
               });
           }
        });
    } else {
        msg.warning('亲，请选择一个帐号哦！');
    }
}


var lastIndex = 0;
var authorize = {
    run: function () {
        var user = mygrid.selectRow();
        if (!user) {
            msg.warning('亲，请选择帐号哦。');
            return false;
        }
        var ad = top.$.hDialog({
            max: true, title: '分配权限-用户名：'+user.UserName,
            content: '<div style="padding:2px;overflow:hidden;"><table id="nb"></table></div>',
            toolbar: [
                { text: '全选', iconCls: 'icon-checkbox_yes', handler: function () { authorize.btnchecked(true); } },
                { text: '取消全选', iconCls: 'icon-checkbox_no', handler: function () { authorize.btnchecked(false); } },
                '-',
                { text: '编辑全部', iconCls: 'icon-pencil', handler: function () { authorize.apply('beginEdit'); } },
                { text: '取消编辑', iconCls: 'icon-pencil_delete', handler: function () { authorize.apply('cancelEdit'); } },
                '-',
                { text: '应用', iconCls: 'icon-disk_multiple', handler: function () { authorize.apply('endEdit'); } }
            ],
            submit: function () {
                authorize.apply('endEdit');
                var data = authorize.getChanges(user);
                if (data) {
                    $.ajaxtext(actionUrl, createParam('authorize') + '&data=' + data, function (d) {
                        if (d > 0) {
                            msg.ok('权限分配成功。');
                            ad.dialog('close');
                        } else {
                            alert('分配权限失败啦！');
                        }
                    });
                } else {
                    alert('您没有分配任何权限！');
                }
            }
        });

        var nb = top.$('#nb').treegrid({
            title: '导航菜单',
            url: '/sys/ashx/userHandler.ashx?' + createParam('menus', user.KeyId),
            height: ad.dialog('options').height - 115,
            idField: 'KeyId',
            treeField: 'NavTitle',
            iconCls: 'icon-nav',
            nowrap: false,
            rownumbers: true,
            animate: true,
            collapsible: false,
            frozenColumns: [[{ title: '菜单名称', field: 'NavTitle', width: 200 }]],
            columns: [authorize.allBtns()],
            onClickRow: function (row) {
                if (lastIndex != row.KeyId) {
                    nb.treegrid('endEdit', lastIndex);
                }
                authorize.apply('beginEdit', row.KeyId);
                lastIndex = row.KeyId;
            },
            onContextMenu: function (e, row) {
                authorize.rowCmenu(e, row);
            }
        });
        return false;
    },
    rowCmenu: function (e, row) { //row 右键菜单
        var createRowMenu = function () {
            var rmenu = top.$('<div id="rmenu" style="width:100px;"></div>').appendTo('body');
            var menus = [{ title: '编辑并全选', iconCls: '' }, { title: '编辑', iconCls: 'icon-edit' }, '-',
                { title: '全选', iconCls: '' }, { title: '取消全选', iconCls: '' }, '-',
               { title: '取消编辑', iconCls: '' }, { title: '应用', iconCls: 'icon-ok' }];
            for (var i = 0; i < menus.length; i++) {
                if (menus[i].title)
                    top.$('<div iconCls="' + menus[i].iconCls + '"/>').html(menus[i].title).appendTo(rmenu);
                else {
                    top.$('<div class="menu-sep"></div>').appendTo(rmenu);
                }
            }
        };

        e.preventDefault();
        if (top.$('#rmenu').length == 0) { createRowMenu(); }

        top.$('#nb').treegrid('select', row.KeyId);
        if (lastIndex != row.KeyId) { nb.treegrid('endEdit', lastIndex); }
        lastIndex = row.KeyId;

        top.$('#rmenu').menu({
            onClick: function (item) {
                switch (item.text) {
                    case '全选': authorize.btnchecked(true); break;
                    case '取消全选': authorize.btnchecked(false); break;
                    case '编辑': authorize.apply('beginEdit', row.KeyId); break;
                    case '编辑并全选':
                        authorize.apply('beginEdit', row.KeyId);
                        authorize.btnchecked(true);
                        break;
                    case '取消编辑': authorize.apply('cancelEdit', row.KeyId); break;
                    case '应用': authorize.apply('endEdit', row.KeyId); break;
                    default:
                        break;
                }
            }
        }).menu('show', { left: e.pageX, top: e.pageY });
    },
    allBtns: function () {
        Enumerable.from(btns).forEach("o=>o.formatter=function(v,d,i){return authorize.formatter(v,d,i,o.field);}");
        return btns;
    },
    formatter: function (v, d, i, field) {//按钮初始化
        if (v) {
            if (v == '√')
                return '<font color=\"#39CB00\"><b>' + v + '</b></font>';
            else return v;
        } else {
            //return d.hasbtns.length;
            return Enumerable.from(d.hasbtns).any("n=>n=='" + field + "'") ? "<font color=\"#39CB00\"><b>√</b></font>" : "x";
        }
    },
    findCtrl: function (g, fieldname, keyid) {
        return g.treegrid('getEditor', { id: keyid, field: fieldname }).target;
    },
    btnchecked: function (flag) {
        var rows = top.$('#nb').treegrid('getSelections');
        if (rows) {
            $.each(rows, function (i, n) {
                var editors = top.$('#nb').treegrid('getEditors', n.KeyId);
                $.each(editors, function () {
                    if (!$(this.target).is(":hidden"))
                        $(this.target).attr('checked', flag);

                });
            });
        } else {
            msg.warning('请选择菜单。');
        }
    },
    apply: function (action, keyid) {
        if (!keyid)
            top.$('#nb').treegrid('selectAll');

        var rows = top.$('#nb').treegrid('getSelections');
        $.each(rows, function (i, n) {
            top.$('#nb').treegrid(action, this.KeyId);
            if (action == 'beginEdit') {
                var editors = top.$('#nb').treegrid('getEditors', n.KeyId);
                Enumerable.from(btns).forEach(function (x, z) {
                    var hasbtn = Enumerable.from(n.Buttons).any('$=="' + x.field + '"');
                    Enumerable.from(editors).forEach(function (b) {
                        if (!hasbtn && b.field == x.field)
                            $(b.target).remove();
                    });
                });
            }
        });

        if (action != "beginEdit")
            top.$('#nb').treegrid('clearSelections');
    },
    getChanges: function (user) {
        var rows = top.$('#nb').treegrid('getChildren');

        var o = { userId: user.KeyId, menus: [] };

        Enumerable.from(rows).forEach(function (x) {
            var btns = Enumerable.from(x).where('t=>t.value=="√"').select('$.key').toArray();
            if (btns.length > 0) {
                var n = { navid: x.KeyId, buttons: [] };
                n.buttons = btns;
                o.menus.push(n);
            }
        });
        if(o.menus.length >0)
            return JSON.stringify(o);
        return "";
    },
    setDeptments: function () { //设置人员数据访问权限
        var u = mygrid.selectRow();
        var dp = new DataPermission(u, actionUrl);
        dp.show();
    }
};