
var pform = '<form  id="uiform" ><table cellpadding=5 cellspacing=0 width=100% align="center" class="grid2" border=0><tr><td align="right">';
pform += '权限名称：</td><td><input name="ButtonText" id="txtPname" type="text" class="txt03 required" ></td></tr><tr><td align="right">';
pform += '图标：</td><td><input name="iconcls" id="txticoncls" required="true" type="text" class="txt03" >&nbsp;<a id="selecticon" href="javascript:;" plain="true" class="easyui-linkbutton" icon="icon-search" title="选择图标"></a></td></tr><tr><td align="right">';
pform += '权限代码：</td><td><input name="ButtonTag" id="txtPcode" type="text" class="txt03 required" ></td></tr><tr><td align="right">';
pform += '排序：</td><td><input name="Sortnum" value="1" id="txtorderid" required="true" type="text"></td></tr><tr><td align="right">';
pform += '权限说明：</td><td><textarea name="remark" rows="3" name=remark style="width:200px;height:50px;" class="txt03" id="txtremark" /></td></tr></table></form>';

$(function () {

    autoResize({ dataGrid: '#btnGrid', gridType: 'datagrid', callback: grid.bind, height: 0 });

    $('#a_edit').click(CRUD.edit);
    $('#a_delete').click(CRUD.del);
    $('#a_add').click(CRUD.add);
    $('#a_search').click(function () {
        search.go('btnGrid');
    });

    /*导出EXCEL*/
    $('#a_export').click(function() {
        var ee = new ExportExcel('btnGrid', "Sys_Buttons");
        ee.go();
    });

    //初始化搜索框
    simpleSearch();
});

function simpleSearch()
{
    var columns = $('#btnGrid').datagrid('options').columns[0];
    $('#searchMenu').empty();
    $.each(columns, function (i, n) {
        $('#searchMenu').append('<div data-options="name:\''+n.field+'\'">' + n.title + '</div>');
    });

    $('#ss').searchbox({
        menu: '#searchMenu',
        searcher: function (value, name) {
            if (value != '') {
                var filter = { groupOp: 'And', rules: [{ field: name, op: 'eq', data: value }] };
                $('#btnGrid').datagrid('reload', { filter: JSON.stringify(filter) });
            }
            else {
                msg.warning('请输入关键字进行查询');
            }
        }
    });
}

var showICON = function () {
    top.$('#selecticon').click(function () {
        var iWin = top.$.hWindow({
            iconCls:'icon-application_view_icons',
            href: urlRoot + '/css/iconlist.htm?v=' + Math.random(), title: '图标选取', width: 800, height: 600, submit: function () {
                top.$('#i').window('close');
            }, onLoad: function () {
                top.$('#iconlist li').attr('style', 'float:left;border:1px solid #fff; line-height:20px; margin-right:4px;width:16px;cursor:pointer')
                .click(function () {
                    top.$('#txticoncls').val(top.$(this).find('span').attr('class').split(" ")[1]);
                    iWin.window('close');
                }).hover(function () {
                    top.$(this).css({ 'border': '1px solid red' });
                }, function () {
                    top.$(this).css({ 'border': '1px solid #fff' });
                });
            }
        });
    });
};
var actionURL = "ashx/ButtonHandler.ashx";
var grid = {
    bind: function (winSize) {
        $('#btnGrid').datagrid({
            url: actionURL ,
            toolbar:'#toolbar',
            title: "系统按扭",
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
                { title:'ID',field:'KeyId',width:60,sortable:true},
                {
                    title: '图标', field: 'iconCls', width: 40, formatter: function (v, d, i) {
                        return '<span class="icon ' + v + '">&nbsp;</span>';
                    }, align: 'center'
                },
                { title: '按钮名称', field: 'ButtonText', width: 100, sortable: true },
                { title: '权限标识', field: 'ButtonTag', width: 80, sortable: true },
                { title: '排序', field: 'Sortnum', width: 80, sortable: true },
                { title: '说明', field: 'Remark', width: 300 }
            ]],
            pagination: true,
            pageSize: PAGESIZE,
            pageList:[20,40,50]
        });
    },
    getSelectedRow: function () {
        return $('#btnGrid').datagrid('getSelected');
    },
    reload:function(){
        $('#btnGrid').datagrid('clearSelections').datagrid('reload', { filter: '' });
    }
};

function createParam(action,keyid)
{
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
            title: '添加按钮', width: 350, height: 300, content: pform, iconCls: 'icon-add', submit: function () {
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
        top.$('#txtPname,#txtPcode').validatebox();
        top.$('#txtPname').val('');
        top.$('#txtPcode').val('');
        top.$('#txtorderid').numberspinner();
        showICON();
        top.$('#uiform').validate();
    },
    edit: function () {
        var row = grid.getSelectedRow();
        if (row) {
            var hDialog = top.$.hDialog({
                title: '编辑按钮', width: 350, height: 300, content: pform, iconCls: 'icon-save', submit: function () {
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
            top.$('#uiform').validate();
            showICON();
            top.$('#txtPname').val(row.ButtonText);
            top.$('#txtPcode').val(row.ButtonTag);
            top.$('#txticoncls').val(row.iconCls);
            top.$('#txtremark').val(row.Remark);
            top.$('#txtorderid').val(row.Sortnum).numberspinner();
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

