//系统全局设置
var _data = {
    theme: [{ "title": "默认皮肤", "name": "default" },
        { "title": "流行灰", "name": "gray" },
        { "title": "黑色", "name": "black" },
        { "title": "Bootstrap", "name": "bootstrap" },
        { "title": "Metro", "name": "metro" },
        { "title": "Metro-Blue", "name": "metro-blue" },
        { "title": "Metro-Gray", "name": "metro-gray" },
        { "title": "Metro-Green", "name": "metro-green" },
        { "title": "Metro-Orange", "name": "metro-orange" },
        { "title": "Metro-Red", "name": "metro-red" }
    ],
    navType: [{ "id": "menubutton", "text": "横向菜单" }, { "id": "Accordion", "text": "手风琴(2级)", "selected": true }, { "id": "Accordion2", "text": "手风琴大图标(2级)" }, { "id": "tree", "text": "树形结构" },
    { "id": "menuAccordion", "text": "菜单+手风琴（小图标-3级）" }, { "id": "menuAccordion2", "text": "菜单+手风琴（大图标-3级）" },
    { "id": "AccordionTree", "text": "手风琴+树形目录(2级+)" }]
};

function initCtrl() {
    $('#txt_theme').combobox({
        data: _data.theme, panelHeight: 'auto', editable: false, valueField: 'name', textField: 'title'
    });

    $('#txt_nav_showtype').combobox({
        data: _data.navType, panelHeight: 'auto', editable: false, valueField: 'id', textField: 'text', width: 180,
        onSelect:function (item) {
            $('#imgPreview').attr('src', urlRoot + '/images/menustyles/' + item.id + '.png');
        }
    });

    $('#imgPreview').click(function() {
        var src = $(this).attr('src');
        top.$.hDialog({
            content: '<img src="' + urlRoot + src + '" />',
            width: 665,
            height: 655,
            title: '效果图预览',
            showBtns:false
        });
    });

    $('#txt_grid_rows').val(20).numberspinner({ min: 10, max: 500, increment: 10 });

    if (sys_config) {
        $('#txt_theme').combobox('setValue', sys_config.theme.name);
        $('#txt_nav_showtype').combobox('setValue', sys_config.showType);
        $('#txt_grid_rows').numberspinner('setValue', sys_config.gridRows);
        $('#imgPreview').attr('src', urlRoot + '/images/menustyles/' + sys_config.showType + '.png');
        $('#showValidateCode').attr('checked', sys_config.showValidateCode);
    }
}

$(function () {
    initCtrl();
    $('#btnok').click(saveConfig);

    $('body').css('overflow', 'auto');

    
    
});

function saveConfig() {
    var theme = $('#txt_theme').combobox('getValue');
    var navtype = $('#txt_nav_showtype').combobox('getValue');
    var gridrows = $('#txt_grid_rows').numberspinner('getValue');

    var findThemeObj = function () {
        var obj = null;
        $.each(_data.theme, function (i, n) {
            if (n.name == theme)
                obj = n;
        });
        return obj;
    };
    var configObj = { theme: findThemeObj(),showType: navtype, gridRows: gridrows,showValidateCode:$('#showValidateCode').is(':checked') };

    var str = JSON.stringify(configObj);
    
    $.ajaxtext('ashx/ConfigHandler.ashx', 'json=' + str, function (d) {
        if (d == 1)
            msg.ok('恭喜，全局设置保存成功,按F5看效果');
        else
            alert(d);
    });
}