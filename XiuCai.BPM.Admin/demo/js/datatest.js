var actionURL = "ashx/DataTestHandler.ashx";

$(function () {

    autoResize({ dataGrid: '#list', gridType: 'datagrid', callback: grid.bind, height: 0 });

    $('#a_add').click(CRUD.add);
    $('#a_edit').click(CRUD.edit);
    $('#a_delete').click(CRUD.del);
});

var grid = {
    bind: function (winSize) {
        $('#list').datagrid({
            url: actionURL,
            toolbar: '#toolbar',
            title: "会员列表",
            iconCls: 'icon icon-list',
            width: winSize.width,
            height: winSize.height,
            nowrap: false, //折行
            rownumbers: true, //行号
            striped: true, //隔行变色
            idField: 'KeyId',//主键
            singleSelect: true, //单选
            frozenColumns: [[]],
            columns: [[
                { title: 'ID', field: 'KeyId', width: 60 },
                {
                    title: 'Name', field: 'Name', width: 140
                },
                { title: 'Company', field: 'Company', width: 100 },
                { title: 'Ownner', field: 'Ownner', width: 80 },
                { title: 'DepID', field: 'DepID', width: 80 }
               
            ]],
            pagination: true,
            pageSize: PAGESIZE,
            pageList: [20, 40, 50]
        });
    },
    getSelectedRow: function () {
        return $('#list').datagrid('getSelected');
    },
    reload: function () {
        $('#list').datagrid('clearSelections').datagrid('reload', { filter: '' });
    }
};

function createParam(action, keyid) {
    var o = {};
    var query = top.$('#uiform').serializeArray();
    query = convertArray(query);
    o.jsonEntity = JSON.stringify(query);
    o.action = action;
    o.keyid = keyid;
    return "json=" + JSON.stringify(o);
}


var CRUD = {
    add: function () {
        var hDialog = top.$.hDialog({
            title: '添加会员', width: 350, height: 300, href:'/demo/html/form.html', iconCls: 'icon-add', submit: function () {
                if (top.$('#uiform').validate().form()) {
                    var query = createParam('add', '0');
                    $.ajaxjson(actionURL, query, function (d) {
                        if (parseInt(d.Data) > 0) {
                            msg.ok('添加成功！');
                            hDialog.dialog('close');
                            grid.reload();
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
                return false;
            }
        });
       
        top.$('#txtName').val('');
        top.$('#txtCompany').val('');
        
        top.$('#uiform').validate();
    },
    edit: function () {
        var row = grid.getSelectedRow();
        if (row) {
            var hDialog = top.$.hDialog({
                title: '编辑会员', width: 350, height: 300, href: '/demo/html/form.html', iconCls: 'icon-save',
                onLoad:function() {
                    top.$('#uiform').validate();
                    top.$('#txtName').val(row.Name);
                    top.$('#txtCompany').val(row.Company);
                },
                submit: function () {
                    if (top.$('#uiform').validate().form()) {
                        var query = createParam('edit', row.KeyId);;
                        $.ajaxjson(actionURL, query, function (d) {
                            if (parseInt(d.Data) > 0) {
                                msg.ok('修改成功！');
                                hDialog.dialog('close');
                                grid.reload();
                            } else {
                                MessageOrRedirect(d);
                            }
                        });
                    }
                    return false;
                }
            });
           
        } else {
            msg.warning('请选择要修改的行。');
        }
    },
    del: function () {
        var row = grid.getSelectedRow();
        if (row) {
            if (confirm('确认要执行删除操作吗？')) {
                var rid = row.KeyId;
                $.ajaxjson(actionURL, createParam('delete', rid), function (d) {
                    if (parseInt(d.Data) > 0) {
                        msg.ok('删除成功！');
                        grid.reload();
                    } else {
                        MessageOrRedirect(d);
                    }
                });
            }
        } else {
            msg.warning('请选择要删除的行。');
        }
    }
};

