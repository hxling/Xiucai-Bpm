
function ExportExcel(gridid, tableName, fields, ignoreFields) {
    this.grid = $('#' + gridid);
    this.fields = fields || this.grid.datagrid('options').columns[0];
    this.dataTable = tableName;

    this.ignoreFields = ignoreFields; // 不导出忽略字段，即查询中不包括忽略的字段
}

ExportExcel.prototype = {
    go: function () {
        var self = this;
        var hDialog = top.$.hDialog({
            content: '<p><b>请选择要导出的字段：</b>&nbsp;&nbsp;&nbsp;&nbsp;<input style="vertical-align:middle" type="checkbox" checked id="__check_all" /><label style="vertical-align:middle" for="__check_all">全选</label></p><ul class="checkbox" id="field_list"></ul>',
            width: 400, height: 300, title: '导出Excel数据',
            submit: function () {
                var selectedFields = '';
                top.$('#field_list :checked').each(function () {
                    var v = $(this).val();
                    var _field = v.split('|')[0];
                    
                    if (self.ignoreFields && $.inArray(_field, self.ignoreFields) > -1) {
                        return;
                    }
                    selectedFields += _field + " as '" + v.split('|')[1] + "',";
                });
                if (selectedFields != '')
                    selectedFields = selectedFields.substr(0, selectedFields.length - 1);
                else
                    selectedFields = " * ";

                var tableName = self.dataTable;

                var where = $('body').data('where');
                if (!where)
                    where = "";
                window.open('/ashx/exportexcel.aspx?tableName=' + tableName + '&fields=' + selectedFields + '&filters=' + where);
            }
        });

         //[{ 'title': 'keyid', 'field': 'keyid' }, { 'title': '按钮名称', 'field': 'ButtonText' }, { 'title': '权限标识', 'field': 'ButtonTag' }, { 'title': '排序', 'field': 'Sortnum' }];
        var lis = '';
        jQuery.each(this.fields, function (i, n) {
            lis += '<li><input type="checkbox" checked style="vertical-align:middle" value="' + n.field + '|' + n.title + '" id="' + n.title + '"  ><label style="vertical-align:middle" for="' + n.field + '">' + n.title + '</label></li>';
        });

        top.$('#field_list').empty().append(lis);
        top.$('#__check_all').click(function () {
            var flag = $(this).is(":checked");
            top.$('#field_list :checkbox').attr('checked', flag);
        });
    }
}

