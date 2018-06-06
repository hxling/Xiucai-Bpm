
$(function() {
    autoResize({ dataGrid: '#logGrid', gridType: 'datagrid', callback: mygrid.databind, height: 5 });

    $('#dtOpstart,#dtOpend').datebox({ width: 100 });
    $('#selOptype').combobox({ panelHeight: 'auto', width: 110 });
    $('#a_search').click(search);
    $('#a_delete').click(DeleteLog);
    
    $('#toolbar').css({
        height: '60px'
    });
});

var mygrid = {
    databind: function (size) {
        $('#logGrid').datagrid({
            url:'ashx/loghandler.ashx',
            toolbar: '#toolbar',
            width: size.width,
            height: size.height,
            idField: 'KeyId',
            singleSelect: true,
            striped: true,
            pagination: true,
            pageSize: PAGESIZE,
            pageList:[20,10,30,50],
            columns: [[
                { title: 'ID', field: 'KeyId', width: 60 },
                { title: '操作类型', field: 'OperationType', width: 80 ,formatter: function(v,d,i) {
                    return mygrid.formatterOt(v);
                }, align: 'center'
                },
                { title: '操作表', field: 'TableName', width: 120, align: 'center' },
                { title: '业务名称', field: 'BusinessName', width: 180, align: 'center' },
                { title: '主键值', field: 'PrimaryKey', width: 100, align: 'center' },
                { title: '操作人', field: 'UserName', width: 120, align: 'center' },
                { title: '操作时间', field: 'OperationTime', width: 150,align:'center' },
                { title: '操作IP', field: 'OperationIp', width: 200, align: 'center' }
            ]],
            onDblClickRow: function (rowIndex, rowData) {
                var logId = rowData.KeyId;
                if (parseInt(rowData.OperationType) <= 6) {
                    
                    top.$.hDialog({
                        content: '<table id="logDetailGrid"></table>',
                        width: 680,
                        height: 500,
                        showBtns: false,
                        title: '日志详细信息',
                        iconCls: 'icon-folder_page'
                    });

                    top.$('#logDetailGrid').datagrid({
                        url: '/sys/ashx/loghandler.ashx?action=logdetail&keyid=' + logId,
                        title: "操作人：<font color=red>" + rowData.UserName + "</font> 操作类型：<font color=red>" + mygrid.formatterOt(rowData.OperationType) + "</font> 操作表：<font color=red>" + rowData.BusinessName + "(" + rowData.TableName + ")</font> 操作时间：<font color=red>" + rowData.OperationTime + '</font>',
                        idField: 'KeyId',
                        fit: true,
                        singleSelect: true,
                        fitColumns: true,
                        striped: true,
                        columns: [[
                            { title: '字段', field: 'FieldName', width: 80, align: 'center' },
                            { title: '名称', field: 'FieldText', width: 80, align: 'center' },
                            { title: '旧值', field: 'OldValue', width: 80, align: 'center' },
                            { title: '新值', field: 'NewValue', width: 80, align: 'center' }
                        ]]
                    });
                } else {
                    $.getJSON('/sys/ashx/loghandler.ashx?action=logdetail&keyid=' + logId, function (data) {
                        var dd = top.$.hDialog({
                            width: 680,
                            height: 500,
                            showBtns: false,
                            title: '日志详细信息',
                            iconCls: 'icon-folder_page',
                            content: '<font size=18>'+data[0].Remark+'</font>'
                        });
                    });
                }
            }
        });
    },
    formatterOt: function(v) {
        switch (v) {
            case 1:
                return "添加";
            case 2:
                return "编辑";
            case 3:
                return "删除";
            case 4:
                return "查询";
            case 5:
                return "登录";
            case 6:
                return "退出";
            default:
                return "其他";
        }
    },
    reload: function() {
        $('#logGrid').datagrid('reload',{});
    }
}

function  search() {
    var opUser = $('#txtOpuser').val();
    var table = $('#txtTablename').val();
    var dtOpstart = $('#dtOpstart').datebox('getValue');
    var dtOpend = $('#dtOpend').datebox('getValue');
    var opType = $('#selOptype').combobox('getValue');
    var busingessName = $('#txtBusingessName').val();
    var ruleArr = [];
    if (opUser != '')
        ruleArr.push({ "field": "UserName", "op": "eq", "data": opUser });
    if (table != '')
        ruleArr.push({ "field": "tableName", "op": "eq", "data": table });
    if (dtOpstart != '')
        ruleArr.push({ "field": "OperationTime", "op": "ge", "data": dtOpstart });
    if (dtOpend != '')
        ruleArr.push({ "field": "OperationTime", "op": "le", "data": dtOpend });
    if(opType != '0')
        ruleArr.push({ "field": "OperationType", "op": "in", "data": opType });
    if (busingessName != "")
        ruleArr.push({ "field": "BusinessName", "op": "eq", "data": escape(busingessName) });
    
    if (ruleArr.length > 0) {
        var filterObj = { groupOp: 'AND', rules: ruleArr };
        $('#logGrid').datagrid('load', { filter: JSON.stringify(filterObj) });
    } else {
        mygrid.reload();
    }

}

function DeleteLog() {
    var dd = top.$.hDialog({
        title: '删除日志',
        iconCls: 'icon-delete',
        width: 300,
        height: 150,
        content: '<p style="text-align:center;">日志保留时间：<select id="txtLogDate"><option value=7>保留近一周</option><option value=30>保留近一个月</option><option value=90>保留近三个月</option><option value=0>不保留，全部删除</option></select></p>',
        submit:function () {
            var days = top.$('#txtLogDate').combobox('getValue');
            $.ajaxjson('/sys/ashx/loghandler.ashx?action=clearlog&days=' + days, null, function(d) {
                if (d.Success) {
                    msg.ok(d.Message);
                    mygrid.reload();
                    dd.dialog('close');
                } else {
                    MessageOrRedirect(d);
                }
            });
        }
    });

    top.$('#txtLogDate').combobox({panelHeight:'auto',editable:false});

}