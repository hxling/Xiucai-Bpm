var navgrid;
var formUrl = urlRoot+"/sys/html/navform.html?n=" + Math.random();
var actionURL ='ashx/NavigationHandler.ashx';

$(function () {
    autoResize({ dataGrid: '#navGrid', gridType: 'treegrid', callback: grid.databind, height: 5 });
    $('#a_add').click(crud.add);
    $('#a_setbtn').click(crud.setButtons);
    $('#a_edit').click(crud.edit);
    $('#a_delete').click(crud.del);
});

var grid = {
    databind: function (winsize) {
        navgrid = $('#navGrid').treegrid({
            toolbar: '#toolbar',
            width: winsize.width,
            height: winsize.height,
            url: actionURL,
            idField: 'KeyId',
            treeField: 'NavTitle',
            title: '导航菜单',
            iconCls: 'icon-nav',
            nowrap: false,
            rownumbers: true,
            animate: true,
            collapsible: false,
            frozenColumns: [[
                { title: 'ID', field: 'KeyId', width: 50 },
                { title: '菜单名称', field: 'NavTitle', width: 200 }
            ]],
            columns: [[
                { title: '图标', field: 'iconCls', width: 180 },
                { title: '标记', field: 'NavTag', width: 100 },
                { title: '链接地址', field: 'Linkurl', width: 360 },
                {
                    title: '是否显示', field: 'IsVisible', width: 80, align: 'center', formatter: function (v, d, i) {
                        return '<img src="'+urlRoot+'/images/' + (v ? "checkmark.gif" : "checknomark.gif") + '" />';
                    }
                },
                {
                    title: '弹出窗口', field: 'IsNewWindow', width: 80, align: 'center', formatter: function (v, d, i) {
                        return '<img src="' + urlRoot + '/images/' + (v ? "checkmark.gif" : "checknomark.gif") + '" />';
                    }
                },
                { title: '排序', field: 'Sortnum', width: 80, align: 'center' }
            ]]
        });
    },
    reload: function () {
        navgrid.treegrid('reload');
    },
    selected: function () {
        return navgrid.treegrid('getSelected');
    }
};
var showIcon = function () {
    top.$('#selecticon').click(function () {
        var iconDialog = top.$.hDialog({
            iconCls: 'icon-application_view_icons',
            href: urlRoot+'/css/iconlist.htm?v=' + Math.random(),
            title: '选取图标', width: 800, height: 600,showBtns:false,
            onLoad: function () {
                top.$('#iconlist li').attr('style', 'float:left;border:1px solid #fff;margin:2px;width:16px;cursor:pointer').click(function () {
                    var iconCls = top.$(this).find('span').attr('class').replace('icon ', '');
                    top.$('#txt_iconcls').val(iconCls);
                    top.$('#txt_iconurl').val(urlRoot+top.$(this).attr('title'));
                    top.$('#smallIcon').attr('class', "icon "+iconCls);

                    iconDialog.dialog('close');
                }).hover(function () {
                    top.$(this).css({ 'border': '1px solid red' });
                }, function () {
                    top.$(this).css({ 'border': '1px solid #fff' });
                });
                
             
                //top.$('#btnicon').click(function() {
                //    $.get(actionURL, 'action=buildIcon', function(d) {
                //        top.$.hDialog({
                //            content: '<textarea style="width:100%;height:100%;">' + d + '</textarea>',
                //            max: true
                //        });
                //    });
                    
                //});
            }
        });
    });

    top.$('#selectBigIcon').click(function() {
        var iconDialog = top.$.hDialog({
            iconCls: 'icon-application_view_icons',
            href: '/css/icon32list.html?v=' + Math.random(),
            title: '选取图标',
            width: 800,
            height: 600,
            showBtns: false,
            onLoad: function() {
                top.$('#icon32list li').css({ 'float': 'left', 'width': '32px', 'margin': '2px', 'border': '1px solid #fff' }).hover(function() {
                    top.$(this).css({ 'border': '1px solid red' });
                }, function() {
                    top.$(this).css({ 'border': '1px solid #fff' });
                }).click(function() {
                    top.$('#txt_bigimgurl').val($(this).attr('title'));
                    top.$('#imgBig').attr('src', $(this).attr('title'));
                    iconDialog.dialog('close');
                });
            }
        });
    });
};

function createParam(action, keyid,ids) {
    var o = {};
    var query = top.$('#uiform').serializeArray();
    query = convertArray(query);
    o.jsonEntity = JSON.stringify(query);
    o.action = action;
    o.keyid = keyid;
    if (ids)
        o.keyids = ids;
    else {
        o.keyids = '';
    }
    return "json=" + JSON.stringify(o);
}

function getChildNodes(treeNodeId, result) {
   
    var childrenNodes = navgrid.treegrid('getChildren', treeNodeId);
    
    if (childrenNodes) {
        for (var i = 0; i < childrenNodes.length; i++) {
            result.push(childrenNodes[i].KeyId);
            result = getChildNodes(childrenNodes[i].KeyId, result);
        }
    }

    return result;
}

var crud = {
    bindCtrl: function (navId) {
        
        var treeData = navgrid.treegrid('getData');
        treeData = JSON.stringify(treeData).replace(/KeyId/g, 'id').replace(/NavTitle/g, 'text');
        treeData = '[{"id":0,"selected":true,"text":"请选择父级菜单"},' + treeData.substr(1, treeData.length - 1);
       
        top.$('#txt_parentid').combotree({
            data: JSON.parse(treeData),
            valueField: 'id',
            textField: 'text',
            panelWidth: '180',
            editable: false,
            lines: true,
            onSelect: function (item) {
                var nodeId = top.$('#txt_parentid').combotree('getValue');
                if (item.id == navId) {
                    top.$('#txt_parentid').combotree('setValue',nodeId);
                    alert('上级菜单不能与当前菜单相同!');
                }
            }
        }).combotree('setValue', 0);
        showIcon(); //选取图标
        top.$('#txt_orderid').numberspinner();
        top.$('#uiform').validate({
            //此处加入验证
        });

        top.$('#chkIsNewWindow').click(function() {
            var flag = $(this).is(':checked');
            if (flag) {
                top.$('#dialogSize').show();
            } else {
                top.$('#dialogSize').hide();
            }
        });
    },
    add: function () {
        var addDialog =top.$.hDialog({
            href: formUrl, title: '添加菜单', iconCls: 'icon-add', width: 500, height: 400,
            onLoad:function () {
                crud.bindCtrl();
                var row = grid.selected();
                if (row){
                    top.$('#txt_parentid').combotree('setValue', row.KeyId);
                }

            },
            submit: function () {
                if (top.$('#uiform').validate().form()) {
                    var param = createParam('add', '0');
                    $.ajaxjson(actionURL, param, function (d) {
                        if (d.Success) {
                            msg.ok(d.Message);
                            addDialog.dialog('close');
                            grid.reload();
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });

       
    },
    edit: function () {
        var row = grid.selected();
        if (row) {
            var editDailog = top.$.hDialog({
                href: formUrl, title: '编辑菜单', iconCls: 'icon-add', width: 500, height: 400,
                onLoad:function () {
                    crud.bindCtrl(row.KeyId);
                    top.$('#txt_ptag').val(row.NavTag);
                    top.$('#txt_title').val(row.NavTitle);
                    top.$('#txt_url').val(row.Linkurl);
                    top.$('#txt_iconcls').val(row.iconCls);
                    top.$('#smallIcon').attr('class', "icon " + row.iconCls);
                    top.$('#txt_parentid').combotree('setValue', row.ParentID);
                    top.$('#chkvisible').attr('checked', row.IsVisible);
                    top.$('#txt_orderid').numberspinner('setValue', row.Sortnum);
                    top.$('#txt_iconurl').val(row.iconUrl);
                    top.$('#txt_bigimgurl').val( row.BigImageUrl);
                    top.$('#imgBig').attr('src', urlRoot + row.BigImageUrl);
                },
                submit: function () {
                    if (top.$('#uiform').validate().form()) {
                        var query = createParam('edit', row.KeyId);
                        $.ajaxjson(actionURL, query, function (d) {
                            if (d.Success) {
                                msg.ok(d.Message);
                                editDailog.dialog('close');
                                grid.reload();
                            } else {
                                MessageOrRedirect(d);
                            }
                        });
                    }
                }
            });

            
        } else {
            msg.warning('请选择要修改菜单!');
            return false;
        }
        return false;
    },
    del: function () {
        var row = grid.selected();
        if (row != null) {
            var nodes = [row.KeyId];
            getChildNodes(row.KeyId, nodes);
            var query = createParam("delete", row.KeyId, nodes.join(','));

            if (confirm('确认要删除选中的菜单吗？\r\n注意：将连同子菜单一同删除。')) {
                $.ajaxjson(actionURL, query, function (d) {
                    if (d.Success) {
                        msg.ok(d.Message);
                        grid.reload();
                    } else {
                        MessageOrRedirect(d);
                    }
                });
            } else
                return false;
        }
        else {
            msg.warning('请选择要删除的菜单!');
            return false;
        }
        return false;
    },
    setButtons: function () { //给菜单分配按钮
        var row = grid.selected();
        var btngrid;
        if (row) {
            var setDialog = top.$.hDialog({
                title: '菜单名称：' + row.NavTitle,
                width: 440, height: 400, iconCls: 'icon-cog', cache: false,
                href: '/sys/html/setnavbuttons.html?n=' + Math.random(),
                onLoad:function() {
                    btngrid = top.$('#left_btns').datagrid({
                        title:'所有按钮',
                        url: '/sys/' + actionURL + '?' + createParam('btns', 0),
                        nowrap: false, //折行
                        fit: true,
                        rownumbers: true, //行号
                        striped: true, //隔行变色
                        idField: 'KeyId',//主键
                        singleSelect: true, //单选
                        frozenColumns: [[]],
                        columns: [[
                            {
                                title: '图标', field: 'iconCls', width: 38, formatter: function (v, d, i) {
                                    return '<span class="icon ' + v + '">&nbsp;</span>';
                                }, align: 'center'
                            },
                            { title: '按钮名称', field: 'ButtonText', width: 80, align: 'center' },
                            { title: '备注', field: 'Remark', width: 180, hidden: true }
                        ]],
                        onDblClickRow: function (rowIndex, rowData) {
                            var currRows = top.$('#right_btns').datagrid('getRows');
                            var hasBtns = Enumerable.from(currRows).where("x=>x.KeyId==" + rowData.KeyId).select("$").toArray();
                            if (hasBtns.length > 0)
                                return false;
                            else {
                                top.$('#right_btns').datagrid('appendRow', rowData);
                            }
                        },
                        onLoadSuccess: function (data) {
                            var arr = Enumerable.from(row.Buttons).select("$.KeyId").toArray();
                            top.$('#right_btns').datagrid({
                                title:'已选按钮',
                                nowrap: false, //折行
                                fit: true,
                                rownumbers: true, //行号
                                striped: true, //隔行变色
                                idField: 'KeyId',//主键
                                singleSelect: true, //单选
                                frozenColumns: [[]],
                                columns: [[
                                    {
                                        title: '图标', field: 'iconCls', width: 38, formatter: function (v, d, i) {
                                            return '<span class="icon ' + v + '">&nbsp;</span>';
                                        }, align: 'center'
                                    },
                                    { title: '按钮名称', field: 'ButtonText', width: 80, align: 'center' },
                                    { title: '备注', field: 'Remark', width: 180, hidden: true }
                                ]],
                                onDblClickRow: function (rowIndex, rowData) {
                                    top.$('#right_btns').datagrid('deleteRow', rowIndex);
                                }
                            });

                            $.each(data.rows, function (i, n) {
                                if ($.inArray(n.KeyId, arr) > -1)
                                    top.$('#right_btns').datagrid('appendRow', n);
                            });
                            
                            //绑定移动按钮事件
                            top.$('#btnUp').click(function () { moveGridRow.Up(top.$('#right_btns')); });
                            top.$('#btnDown').click(function () { moveGridRow.Down(top.$('#right_btns')); });
                            top.$('#btnRight').click(function () { moveGridRow.Insert(top.$('#left_btns'), top.$('#right_btns')); });
                            top.$('#btnLeft').click(function () { moveGridRow.Remove(top.$('#right_btns')); });
                        }
                    });

                    
                },
                submit: function() {
                    var btns = top.$('#right_btns').datagrid('getRows');
                    
                    if(btns.length >0) {
                        var permissionids = Enumerable.from(btns).select("$.KeyId").toArray().join(',');
                        $.ajaxtext(actionURL, createParam('setbtns', row.KeyId, permissionids ), function(d) {
                            if (d > 0){
                                msg.ok('菜单按钮设置成功。');
                                setDialog.dialog('close');
                                grid.reload();
                            }
                            else {
                                alert(d);
                            }
                        });
                    } else {
                        alert('请选择按钮啊，亲！');
                    }
                }
            });
            
            
        }
        else
            msg.warning('请选择导航菜单');
    }
};


var moveGridRow = {
    Up: function (jq) {
        var rowindex = jq.datagrid('getSelectedIndex');
        if (rowindex > -1) {
            var rows = jq.datagrid('getRows');
            var newRowIndex = rowindex - 1;
            if (newRowIndex < 0)
                newRowIndex = 0;

            var targetRow = rows[newRowIndex];
            var currentRow = rows[rowindex];

            rows[newRowIndex] = currentRow;
            rows[rowindex] = targetRow;

            jq.datagrid('loadData', rows);
            jq.datagrid('selectRow', newRowIndex);

        } else
            alert('亲，都到顶啦，在点就可以见到天宫1号啦！');
    },
    Down:function(jq) {
        var rowindex = jq.datagrid('getSelectedIndex');
        var rows = jq.datagrid('getRows');
        if (rowindex < rows.length - 1) {
            var newRowIndex = rowindex + 1;

            var targetRow = rows[newRowIndex];
            var currentRow = rows[rowindex];

            rows[newRowIndex] = currentRow;
            rows[rowindex] = targetRow;

            jq.datagrid('loadData', rows);
            jq.datagrid('selectRow', newRowIndex);

        } else
            alert('亲，到底啦，在点就罢工啦！');
    },
    Insert: function(ljq,rjq) {
        var rows = ljq.datagrid('getSelected');
        if(rows) {
            var currRows = rjq.datagrid('getRows');
            var hasBtns = Enumerable.from(currRows).where("x=>x.KeyId==" + rows.KeyId).select("$").toArray();
            if (hasBtns.length > 0)
                return false;
            else {
                rjq.datagrid('appendRow', rows);
            }
        } else {
            alert('请选择按钮。');
            return false;
        }
    },
    Remove: function (jq) {
        var rowindex = jq.datagrid('getSelectedIndex');
        if(rowindex >-1)
            jq.datagrid('deleteRow', rowindex);
        return false;
    }
}