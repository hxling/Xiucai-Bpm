var formcategoryUrl = urlRoot + '/sys/html/diccategoryform.html?n=' + Math.random();
var dicUrl = urlRoot + '/sys/html/dicform.html?n=' + Math.random();
var actionUrl = 'ashx/dichandler.ashx';

$(function () {
    var size = { width: $(window).width(), height: $(window).height() };
    mylayout.init(size);
    $(window).resize(function () {
        size = { width: $(window).width(), height: $(window).height() };
        mylayout.resize(size);
    });

    DicCategory.bindTree();
    autoResize({ dataGrid: '#userGrid', gridType: 'datagrid', callback: mygrid.databind, height: 4, width: 204 });

    $('#a_addCategory').click(DicCategory.addCategory);
    $('#a_delCategory').click(DicCategory.delCategory);
    $('#a_editCategory').click(DicCategory.editCategory);
    
    //字典数据
    $('#a_add').click(mygrid.add);
    $('#a_edit').click(mygrid.edit);
    $('#a_delete').click(mygrid.del);
    $('#a_refresh').click(function() {
        var node = DicCategory.getSelected();
        if (node)
            mygrid.reload(node.id);
        else {
            msg.warning('请选择字典类别。');
        }
    });
});

var mylayout = {
    init: function (size) {
        $('#layout').width(size.width - 4).height(size.height - 4).layout();
        var center = $('#layout').layout('panel', 'center');
        center.panel({
            onResize: function (w, h) {
                $('#dicGrid').datagrid('resize', { width: w, height: h });
            }
        });
    },
    resize: function (size) {
        mylayout.init(size);
        $('#layout').layout('resize');
    }
};

var DicCategory = {
    bindTree: function() {
        $('#dataDicType').tree({
            url: 'ashx/dichandler.ashx?action=category',
            onLoadSuccess: function(node, data) {
                if (data.length == 0) {
                    $('#noCategoryInfo').fadeIn();
                }

                $('body').data('categoryData', data);
            },
            onClick: function(node) {
                var cc = node.id;
                $('#dicGrid').treegrid({
                    url: actionUrl,
                    queryParams: { categoryId: cc }
                });
            }
        });
    },
    reload: function() {
        $('#dataDicType').tree('reload');
    },
    getSelected: function() {
        return $('#dataDicType').tree('getSelected');
    },
    addCategory: function() {
        var addDialog = top.$.hDialog({
            title: '添加字典类别',
            iconCls: 'icon-add',
            href: formcategoryUrl,
            width: 400,
            height: 300,
            onLoad: function() {
                top.$('#txt_sortnum').numberspinner({ min: 0, max: 999 });
            },
            submit: function() {
                var isValid = top.$('#dicCateForm').form("validate");
                if (isValid) {
                    $.ajaxjson(actionUrl, 'action=add_cate&' + top.$('#dicCateForm').serialize(), function(d) {
                        if (d.Data > 0) {
                            msg.ok('亲，字典类别添加成功。');
                            addDialog.dialog('close');
                            DicCategory.reload();
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });
    },
    editCategory: function() {
        var node = DicCategory.getSelected();
        if (node) {
            var editDialog = top.$.hDialog({
                title: '编辑字典类别',
                iconCls: 'icon-edit',
                href: formcategoryUrl,
                width: 400,
                height: 300,
                onLoad: function() {
                    top.$('#txt_sortnum').val(node.attributes.Sortnum).numberspinner({ min: 0, max: 999 });
                    top.$('#txt_remark').val(node.attributes.Remark);
                    top.$('#txt_title').val($.trim(node.text.substring(0, node.text.indexOf('['))));
                    top.$('#txt_code').val(node.attributes.Code);
                },
                submit: function() {
                    var isValid = top.$('#dicCateForm').form("validate");
                    if (isValid) {
                        $.ajaxjson(actionUrl, 'action=edit_cate&' + top.$('#dicCateForm').serialize() + '&keyid=' + node.id, function(d) {
                            if (d.Data > 0) {
                                msg.ok('亲，字典类别编辑成功啦。');
                                editDialog.dialog('close');
                                DicCategory.reload();
                            } else {
                                MessageOrRedirect(d);
                            }
                        });
                    }
                }
            });
        } else {
            msg.warning('亲,请选择字典类别。');
        }
    },
    delCategory: function() {
        var node = DicCategory.getSelected();
        if (node) {
            if (confirm('亲,确认要删除此类别吗?')) {
                $.ajaxjson(actionUrl, 'action=del_cate&cateId=' + node.id, function(d) {
                    if (d.Data > 0) {
                        msg.ok('亲，字典类别删除成功。');
                        DicCategory.reload();
                    } else {
                        MessageOrRedirect(d);
                    }
                });
            }
        } else {
            msg.warning('亲,请选择字典类别。');
        }
    }
};

function createParam(action, keyid) {
    var o = {};
    var query = top.$('#dicForm').serializeArray();
    query = convertArray(query);
    o.jsonEntity = JSON.stringify(query);
    o.action = action;
    o.keyid = keyid;
    return "json=" + JSON.stringify(o);
}

var dicDialog;
var mygrid = {
    databind: function (winSize) {
        $('#dicGrid').treegrid({
            toolbar:'#toolbar',
            title: "数据字典",
            iconCls: 'icon icon-list',
            width: winSize.width,
            height: winSize.height,
            nowrap: false, //折行
            rownumbers: true, //行号
            striped: true, //隔行变色
            idField: 'KeyId',//主键
            treeField:'Title',
            singleSelect: true, //单选
            frozenColumns: [[]],
            columns: [[
                {title:'ID',field:'KeyId',width:60},
                { title: '名称', field: 'Title', width: 140 },
                { title: '编码', field: 'Code', width: 140 },
                { title: '排序', field: 'Sortnum', width: 80 },
                { title:'状态',field:'Status',width:60,formatter:function(v,d,i) {
                    return '<img src="/images/' + (v == '1' ? 'checkmark.gif' : 'checknomark.gif') + '" />';
                },align:'center'
                },
                {
                    title: '默认值', field: 'IsDefault', width: 60, formatter: function (v, d, i) {
                        return '<img src="/images/' + (v == '1' ? 'checkmark.gif' : 'checknomark.gif') + '" />';
                    }, align: 'center'
                },
                { title: '说明', field: 'Remark', width: 300 }
            ]],
            pagination: false,
            pageSize: PAGESIZE,
            pageList:[20,40,50]
        });
    },
    reload: function (cc) {
        $('#dicGrid').treegrid({
            url: actionUrl,
            queryParams: { categoryId: cc }
        });
    },
    GetSelectedRow:function() {
        return $('#dicGrid').treegrid('getSelected');
    },
    initCtrl: function(dicId) {
        top.$('#txt_status').combobox({ panelHeight: 'auto', editable: false });
        top.$('#txt_sortnum').numberspinner({ min: 0, max: 999 });
        var cateData = $('body').data('categoryData');
        //alert(JSON.stringify(cateData));
        var comboCategory = top.$('#txt_category').combobox({ data: cateData, valueField: 'id', textField: 'text', editable: false, required: true, missingMessage: '请选择类别' ,disabled:true});
        var cnode = DicCategory.getSelected();
        if(cnode)
            comboCategory.combobox('setValue', cnode.id);

        var dicData = $("#dicGrid").treegrid('getData');
        if (dicData.length > 0) {
            dicData = JSON.stringify(dicData).replace(/KeyId/g, "id").replace(/Title/g, "text");
            dicData = '[{id:0,text:"== 请选择 =="},' + dicData.substr(1);
        }
        else
            dicData = '[{id:0,text:"== 请选择 =="}]';

        var parentTree = top.$('#txt_parentid').combotree({
            data: eval(dicData),
            valueField: 'id',
            textField: 'text',
            editable: false,
            onSelect: function (item) {
                var nodeId = parentTree.combotree('getValue');
                if (item.id == dicId) {
                    parentTree.combotree('setValue', nodeId);
                    alert('上级不能与当前字典相同！');
                    return false;
                }
            }
        });
        
        var crow = mygrid.GetSelectedRow();
        if (!dicId && crow) {
            top.$('#txt_parentid').combotree('setValue', crow.KeyId);
        }else
            top.$('#txt_parentid').combotree('setValue', 0);
    },
    add: function () {
        
        if(!DicCategory.getSelected()) {
            msg.warning('请选择字典类别！');
            return false;
        }
        
        dicDialog = top.$.hDialog({
            href: dicUrl,
            width: 400,
            height: 380,
            title: '新建字典',
            iconCls: 'icon-add',
            onLoad: function() {
                mygrid.initCtrl();
            },
            submit:function() {
                var query = createParam("add", 0);
                if(top.$('#dicForm').form('validate')) {
                    $.ajaxjson(actionUrl, query, function (d) {
                        if (d.Data > 0) {
                            msg.ok(d.Message);
                            mygrid.reload(top.$('#txt_category').combobox('getValue'));
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });
    },
    edit: function () {
        var row = mygrid.GetSelectedRow();
        if(row==null) {
            msg.warning("请选择字典数据。");
            return false;
        }
        dicDialog = top.$.hDialog({
            href: dicUrl,
            width: 400,
            height: 380,
            title: '编辑字典',
            iconCls: 'icon-edit',
            onLoad: function () {
                mygrid.initCtrl(row.KeyId);
                top.$('#txt_title').val(row.Title);
                top.$('#txt_code').val(row.Code);
                top.$('#txt_parentid').combotree('setValue', row.ParentId);
                top.$('#txt_sortnum').numberspinner('setValue', row.Sortnum);
                top.$('#txt_remark').val(row.Remark);
                top.$('#txt_status').combobox('setValue', row.Status);
                top.$('#chkDefault').attr('checked', row.isDefault == 1);
            },
            submit: function () {
                var query = createParam("edit", row.KeyId);
                if (top.$('#dicForm').form('validate')) {
                    $.ajaxjson(actionUrl, query, function (d) {
                        if (d.Data > 0) {
                            msg.ok(d.Message);
                            mygrid.reload(top.$('#txt_category').combobox('getValue'));
                            dicDialog.dialog('close');
                        } else {
                            MessageOrRedirect(d);
                        }
                    });
                }
            }
        });
    },
    del: function() {
        var row = mygrid.GetSelectedRow();
        if (row) {
            var childs = $('#dicGrid').treegrid('getChildren', row.KeyId);
            if (childs.length > 0) {
                msg.warning('当前字典有下级数据，不能删除。<br> 请先删除子节点数据。');
                return false;
            }
            
            if (confirm('确认要删除此条字典数据吗?')) {
                var query = createParam("del", row.KeyId);
                $.ajaxjson(actionUrl, query, function(d) {
                    if (d.Data == 1) {
                        msg.ok(d.Message);
                        var node = DicCategory.getSelected();
                        if (node)
                            mygrid.reload(node.id);
                    } else {
                        MessageOrRedirect(d);
                    }
                });
            }
        } else {
            msg.warning('请选中要删除的字典数据。');
        }
    }
}