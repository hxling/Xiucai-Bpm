
var _roleGrid;
var actionUrl = 'ashx/rolehandler.ashx';
var formHTML = '<form id=xiucaiForm><table cellpadding=5 cellspacing=0 width=100% align="center" class="grid" border=0><tr><td align="right">\
                角色名称：</td><td align="left"><input name=rolename type="text"  required="true" style="width:150px" class="txt03" id="txtrolename" /></td></tr><tr><td align="right">\
                排序：</td><td align="left"><input name=sortnum type="text" value=0 style="width:100px"  id="txtsortnum" /></td></tr><tr><td align="right">\
                 </td><td align="left"><input name="isdefault" type="checkbox" value=1  id="txtIsDefault" style="vertical-align: middle"/><label for="txtIsDefault" style="vertical-align: middle">默认角色</label></td></tr><tr><td align="right">\
                备注</td><td align="left"><textarea rows="3" name=remark style="width:250px;height:50px;" class="txt03" id="txtremark" /></td></tr>\
                </table></form>';
$(function () {
    
    autoResize({ dataGrid: '#roleGrid', gridType: 'datagrid', callback: mygrid.databind, height: 0 });

    $('#a_set').linkbutton({ text: '分配权限' }).click(authorize.run);
    $('#a_add').click(crud.add);
    $('#a_edit').click(crud.edit);
    $('#a_delete').click(crud.del);
    $('#a_dataset').click(authorize.setDeptments);
});

var mygrid = {
    databind: function(size) {
        _roleGrid = $('#roleGrid').datagrid({
            url: actionUrl,
            width: size.width,
            height: size.height,
            title: '角色列表',
            toolbar: '#toolbar',
            iconCls: 'icon icon-list',
            nowrap: false, //折行
            rownumbers: true, //行号
            striped: true, //隔行变色
            idField: 'KeyId',//主键
            singleSelect: true, //单选
            columns: [[
                { title: 'ID', field: 'KeyId', width: 100, sortable: true },
                { title: '角色名称', field: 'RoleName', width: 160, sortable: true },
                { title: '排序', field: 'Sortnum', width: 80, sortable: true },
                { title: '默认', field: 'IsDefault', width: 80 },
                { title: '备注', field: 'Remark', width: 280 },
                { title: '部门列表', field: 'Departments',hidden:true }
            ]],
            pagination: true,
            pageSize: PAGESIZE,
            pageList: [20, 40, 50]
        });
    },
    selected: function() {
        return _roleGrid.datagrid('getSelected');
    },
    reload: function () {
        _roleGrid.datagrid('clearSelections').datagrid('reload', { filter: '' });
    }
};

function createParam(action, keyid) {
    var o = {};
    var query = top.$('#xiucaiForm').serializeArray();
    query = convertArray(query);
    o.jsonEntity = JSON.stringify(query);
    o.action = action;
    o.keyid = keyid;
    return "json=" + JSON.stringify(o);
}


var crud = {
    initValidate: function() {
        top.$('#xiucaiForm').validate();
    },
    add: function() {
        var addDialog = top.$.hDialog({
            content: formHTML,
            title: '添加角色',
            iconCls: 'icon-add',
            width: 450,
            height: 260,
            submit: function() {
                if(top.$('#xiucaiForm').validate().form()) {
                    var query = createParam('add', 0);
                    $.ajaxjson(actionUrl, query, function (d) {
                        if (parseInt(d.Data) > 0) {
                            msg.ok(d.Message);
                            addDialog.dialog('close');
                            mygrid.reload();
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });
        top.$('#txtsortnum').numberspinner({min:0,max:999});
    },
    edit: function () {
        var row = mygrid.selected();
        
        if (!row) {
            msg.warning('请选择修改的角色。');
            return false;
        }
        
        var editWin = top.$.hDialog({
            content: formHTML,
            title: '编辑角色',
            iconCls: 'icon-add',
            width: 450,
            height: 260,
            submit: function () {
                if (top.$('#xiucaiForm').validate().form()) {
                    var query = createParam('edit', row.KeyId);
                    $.ajaxjson(actionUrl, query, function (d) {
                        if (parseInt(d.Data) > 0) {
                            msg.ok(d.Message);
                            editWin.dialog('close');
                            mygrid.reload();
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });
        
        //Load Data
        top.$('#txtrolename').val(row.RoleName);
        top.$('#txtremark').val(row.Remark);
        top.$('#txtsortnum').numberspinner({ min: 0, max: 999 }).numberspinner('setValue', row.Sortnum);
        top.$('#txtIsDefault').attr('checked', row.IsDefault == 1);
        return false;
    },
    del: function() {
        var row = mygrid.selected();
        if(!row) {
            msg.warning('请选择要删除的角色。');
            return false;
        }

        if (confirm('确认要删除选中的数据吗?')) {
            var query = createParam('delete', row.KeyId);
            $.ajaxjson(actionUrl, query, function (d) {
                if (parseInt(d.Data) > 0) {
                    msg.ok(d.Message);
                    mygrid.reload();
                } else {
                    MessageOrRedirect(d);
                }
            });
        }
        return false;
    }
};

var lastIndex = 0;
var authorize = {
    run: function () {
        var role = mygrid.selected();
        if (!role) {
            msg.warning('请选择一个角色。');
            return false;
        }
        var ad = top.$.hDialog({
            max: true, title: '分配权限',
            content:'<div style="padding:2px;overflow:hidden;"><table id="nb"></table></div>',
            toolbar: [
                { text: '全选', iconCls: 'icon-checkbox_yes', handler: function () { authorize.btnchecked(true); } },
                { text: '取消全选', iconCls: 'icon-checkbox_no', handler: function () { authorize.btnchecked(false);} },
                '-',
                { text: '编辑全部', iconCls: 'icon-pencil', handler: function () { authorize.apply('beginEdit');} },
                { text: '取消编辑', iconCls: 'icon-pencil_delete',handler:function () { authorize.apply('cancelEdit');} },
                '-',
                { text: '应用', iconCls: 'icon-disk_multiple', handler: function () { authorize.apply('endEdit'); } }
            ],
            submit: function () {
                var data = authorize.getChanges(role);
                if(data) {
                    $.ajaxtext(actionUrl, createParam('authorize') + '&data=' + data, function(d) {
                        if (d > 0) {
                            msg.ok('权限分配成功。');
                            ad.dialog('close');
                        } else {
                            alert('您没有分配任何权限！');
                        }
                    });
                } 
            }
        });
        
        var nb = top.$('#nb').treegrid({
            title: '导航菜单',
            url: '/sys/ashx/roleHandler.ashx?'+createParam('menus',role.KeyId),
            height: ad.dialog('options').height-115,
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
                authorize.apply('beginEdit',row.KeyId);
                lastIndex = row.KeyId;
            },
            onContextMenu: function (e, row) {
                authorize.rowCmenu(e, row);
            }
        });
        return false;
    },
    rowCmenu: function(e,row) { //row 右键菜单
        var createRowMenu = function() {
            var rmenu = top.$('<div id="rmenu" style="width:100px;"></div>').appendTo('body');
            var menus = [{ title: '编辑并全选', iconCls: '' }, { title: '编辑', iconCls: 'icon-edit' },'-',
                { title: '全选', iconCls: '' }, { title: '取消全选', iconCls: '' }, '-',
               { title: '取消编辑', iconCls: '' }, { title: '应用', iconCls: 'icon-ok' }];
            for (var i = 0; i < menus.length; i++) {
                if(menus[i].title)
                    top.$('<div iconCls="' + menus[i].iconCls + '"/>').html(menus[i].title).appendTo(rmenu);
                else {
                    top.$('<div class="menu-sep"></div>').appendTo(rmenu);
                }
            }
        };

        e.preventDefault();
        if (top.$('#rmenu').length ==0) {createRowMenu();}

        top.$('#nb').treegrid('select', row.KeyId);
        if (lastIndex != row.KeyId) {nb.treegrid('endEdit', lastIndex);}
        lastIndex = row.KeyId;

        top.$('#rmenu').menu({
            onClick: function (item) {
                switch (item.text) {
                    case '全选': authorize.btnchecked(true); break;
                    case '取消全选':authorize.btnchecked(false);break;
                    case '编辑':authorize.apply('beginEdit', row.KeyId);break;
                    case '编辑并全选':
                        authorize.apply('beginEdit', row.KeyId);
                        authorize.btnchecked(true);
                        break;
                    case '取消编辑':authorize.apply('cancelEdit', row.KeyId);break;
                    case '应用':authorize.apply('endEdit', row.KeyId);break;
                    default:
                        break;
                }
            }
        }).menu('show', {left: e.pageX,top: e.pageY});
    },
    allBtns: function() {
        Enumerable.from(btns).forEach("o=>o.formatter=function(v,d,i){return authorize.formatter(v,d,i,o.field);}");
        return btns;
    },
    formatter: function (v, d, i,field) {//按钮初始化
        if (v) {
            if (v == '√')
                return '<font color=\"#39CB00\"><b>' + v + '</b></font>';
            else return v;
        } else {
            //return d.hasbtns.length;
            return Enumerable.from(d.hasbtns).any("n=>n=='" + field + "'") ? "<font color=\"#39CB00\"><b>√</b></font>" : "x";
        }
    },
    findCtrl: function(g, fieldname, keyid) {
        return g.treegrid('getEditor', { id: keyid, field: fieldname }).target;
    },
    btnchecked: function (flag) {
        var rows = top.$('#nb').treegrid('getSelections');
        if (rows) {
            $.each(rows, function(i,n) {
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
        if(!keyid)
            top.$('#nb').treegrid('selectAll');
       
        var rows = top.$('#nb').treegrid('getSelections');
        $.each(rows, function (i,n) {
            top.$('#nb').treegrid(action, this.KeyId);
            if (action == 'beginEdit') {
                var editors = top.$('#nb').treegrid('getEditors', n.KeyId);
                Enumerable.from(btns).forEach(function (x, z) {
                    var hasbtn = Enumerable.from(n.Buttons).any('$=="' + x.field + '"');
                    Enumerable.from(editors).forEach(function(b) {
                        if (!hasbtn && b.field == x.field)
                            $(b.target).remove();
                    });
                });
            }
        });
        
        if (action != "beginEdit")
            top.$('#nb').treegrid('clearSelections');
    },
    getChanges: function(role) {
        var rows = top.$('#nb').treegrid('getChildren');

        var o = { roleId: role.KeyId, menus: [] };
        
        Enumerable.from(rows).forEach(function(x) {
            var n = { navid: x.KeyId, buttons: [] };
            n.buttons = Enumerable.from(x).where('t=>t.value=="√"').select('$.key').toArray();
            o.menus.push(n);
        });
        return JSON.stringify(o);
    },
    setDeptments: function () { //设置部门权限
        var role = mygrid.selected();
        var dp = new DataPermission(role, actionUrl);
        dp.show();
    }
};

