$(function () {
    autoResize({ dataGrid: '#datagrid', gridType: 'datagrid', callback: mygrid.databind, height:5});
    $('#a_add').linkbutton({ text: '备份数据库' }).click(mygrid.add);
    $('#a_delete').click(mygrid.del);
    $('#a_download').click(mygrid.download);
});

var mygrid = {
    databind:function(size) {
        $('#datagrid').datagrid({
            toolbar: '#toolbar',
            url:'ashx/databasehandler.ashx',
            width: size.widht, height: size.height,
            nowrap: false, //折行
            rownumbers: true, //行号
            striped: true, //隔行变色
            idField: 'FileName',//主键
            singleSelect: true, //单选
            columns: [[
                { title: '文件名称', field: 'FileName', width: 200 },
                { title: '文件大小', field: 'FileSize', width: 120 },
                {title:'备份日期',field:'CreateDate',width:160}
            ]]
        });
    },
    reload: function() {
        $('#datagrid').datagrid('reload');
    },
    getSelected:function () {
        return $('#datagrid').datagrid('getSelected');
    },
    add: function () { //备份文件
        if (confirm("您确认要备份数据库吗？")) {
            $.ajaxjson("ashx/databasehandler.ashx", "action=backup", function(d) {
                if (d.Success) {
                    msg.ok(d.Message);
                    mygrid.reload();
                } else {
                    MessageOrRedirect(d);
                }
            });
        }
    },
    del: function() {
        var row = mygrid.getSelected();
        if (row) {
            if (confirm("您确认要删除此备份文件吗？")) {
                var fileName = row.FileName;
                $.ajaxjson("ashx/databasehandler.ashx", "action=del&n=" + fileName, function(d) {
                    if (d.Success) {
                        msg.ok(d.Message);
                        mygrid.reload();
                    } else {
                        MessageOrRedirect(d);
                    }
                });
            }
            return false;
        } else {
            msg.warning("请选择要删除的数据。");
        }
    },
    download: function() {
        var row = mygrid.getSelected();
        if (row) {
            window.open("ashx/databasehandler.ashx?action=down&n=" + row.FileName);
        } else {
            msg.warning("请选择要下载的备份文件。");
        }
    }
}