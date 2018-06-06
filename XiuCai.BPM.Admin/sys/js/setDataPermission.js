/**
* 设置数据权限
**/


function DataPermission(model,url) {
    this.obj = model;
    this.actionUrl = url;
}

DataPermission.prototype = {
    show: function () {
        if (this.obj) {
            var that = this;
            var ad = top.$.hDialog({
                max: false,
                width: 400,
                height: 500,
                title: '设置数据访问权限',
                iconCls: 'icon-lightning',
                content: '<ul id="depTree"></ul>',
                toolbar: [
                    { text: '全选', iconCls: 'icon-checkbox_yes', handler: function () { top.$('#depTree').tree('checkedAll'); } },
                    { text: '取消全选', iconCls: 'icon-checkbox_no', handler: function () { top.$('#depTree').tree('uncheckedAll'); } },
                    '-',
                    { text: '恢复', iconCls: 'icon-arrow_undo', handler: function () { top.$('#depTree').tree('uncheckedAll'); that.bindData(); } }
                ],
                submit: function () {
                    var roleid = that.obj.KeyId;
                    var deps = that.getDeps();
                    $.ajaxtext(that.actionUrl, 'action=setdep&keyid=' + roleid + '&deps=' + deps, function (d) {
                        if (d > 0) {
                            msg.ok('数据权限分配成功。');
                            //reload data
                            mygrid.reload();
                            ad.dialog('close');
                        } else {
                            alert('设置失败！');
                        }
                    });
                }
            });
            top.$(ad).hLoading();
            this.initTree();

        } else {
            msg.warning('请选择权限设置对象。');
        }

    },
    initTree: function () {
        var that = this;
        //初始化tree
        top.$('#depTree').tree({
            cascadeCheck: false, //是否联动选中节点
            lines: true, //显示连接线
            checkbox: true, //显示筛选框
            url: '/sys/ashx/rolehandler.ashx?action=deps',
            onLoadSuccess: function (node, data) {
                that.bindData();
                top.$.hLoading.hide(); //加载完毕后隐藏loading
            },
            onDblClick: function (node) { //双击节点选中节点及期子节点
                var t = top.$('#depTree');
                t.tree('check', node.target);

                function checkNodes(n) {
                    var childNodes = t.tree('getChildren', n.target);
                    $.each(childNodes, function () {
                        t.tree('check', this.target);
                        t.tree('checkedAll', this.target);
                        checkNodes(this);
                    });
                }

                checkNodes(node);
            }
        });
    },
    bindData: function () {
        var deps = this.obj.Departments;
        if (deps != '') {
            var depTree = top.$('#depTree');
            var arr = deps.split(',');
            for (var i = 0; i < arr.length; i++) {
                var node = depTree.tree('find', arr[i]);

                if (node) //&& depTree.tree('isLeaf', node.target)
                    depTree.tree("check", node.target);
            }
        }
    },
    getDeps: function () {
        var nodes = top.$('#depTree').tree('getChecked');

        if (nodes.length > 0) {
            var dwg = [];
            for (var i = 0; i < nodes.length; i++) {
                dwg.push(nodes[i].id);
            }

            //alert(dwg.join(','));
            return dwg.join(',');

        } else {
            return "";
        }
    }
}