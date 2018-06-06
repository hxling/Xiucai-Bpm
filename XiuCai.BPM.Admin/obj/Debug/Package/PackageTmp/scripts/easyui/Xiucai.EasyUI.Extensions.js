(function ($) {
    
    function guidDialogId() {
        var  s4 = function () {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        };
        return "XiuCai-" + (s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4());
    }

    $.hDialog = function(options) {
        options = $.extend({}, $.hDialog.defaults, options || {});

        var dialogId = guidDialogId();
        if (options.id)
            dialogId = options.id;

        //if (!options.href && !options.content) {
        //    alert('缺少必要的参数 href or content');
        //    return false;
        //}
        
        var defaultBtn = [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: options.submit
            }, {
                text: '关闭',
                iconCls:'icon-cancel',
                handler: function () {
                    $("#" + dialogId).dialog("close");
                }
            }];
        
        if (!options.showBtns)
            defaultBtn = [];
        
        if(options.buttons.length ==0)
            options.buttons = defaultBtn;
        
        if (options.max) {
            //dialog.dialog('maximize');
            var winWidth = $(window).width();
            var winHeight = $(window).height();
            options.width = winWidth - 20;
            options.height = winHeight - 20;
        }


        var $dialog = $('<div/>').css('padding', options.boxPadding).appendTo($('body'));
        
        var dialog = $dialog.dialog($.extend(options, {
            onClose: function () {
                dialog.dialog('destroy');
            }
        })).attr('id', dialogId);
        //.dialog('refresh').dialog('open')
        
        $dialog.find('.dialog-button').css('text-align', options.align);
       
        return dialog;
    };

    $.hDialog.defaults = $.extend({}, $.fn.dialog.defaults, {
        boxPadding:'3px',
        align:'right', //按钮对齐方式
        href: '',
        id:'',
        content: '',
        height: 200,
        width: 400,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        closable: true,
        modal: true,
        shadow: false,
        mask: true,
        cache: false,
        closed: false,//默认是否关闭窗口 如果为true,需调用open方法打开
        showBtns: true,
        buttons:[],
        submit:function () {
            alert('写入可执行代码');
            return false;
        },
        onBeforeClose: function () {
            $(this).find(".combo-f").each(function () {
                var panel = $(this).data().combo.panel;
                panel.panel("destroy");
            });
            $(this).empty();
        },
        onMove: function(left,right) {
            $('.validatebox-tip').remove();
        }
        
    });

    ///////////////////////////////////////////////////////////////////////////////////////////////
    
    $.hWindow = function(options) {
        var windowId = guidDialogId();

        options = $.extend({}, $.hDialog.defaults, options || {});
        if (!options.href && !options.content) {
            alert('缺少必要的参数 href or content');
            return false;
        }
        
        var $dialog = $('<div/>').attr('id', windowId).appendTo($('body'));

        if (options.max) {
            //dialog.dialog('maximize');
            var winWidth = $(window).width();
            var winHeight = $(window).height();
            options.width = winWidth - 20;
            options.height = winHeight - 20;
        }

        var win = $dialog.window($.extend(options, {
            onClose: function () {
                win.window('destroy');
            }
        })).window('refresh').attr('id', windowId);

        
        return win;
    };

    $.hWindow.defaults = $.extend({ }, $.fn.window.defaults, {
        href: '',
        content: '',
        height: 300,
        width: 400,
        collapsible: false, 	//折叠
        closable: true,         //显示右上角关闭按钮
        minimizable: false, 	//最小化
        maximizable: false, 	//最大化
        resizable: false, 	    //是否允许改变窗口大小
        title: '窗口标题', 	    //窗口标题
        modal: true, 		    //模态	
        draggable: true,        //允许拖动
        max: false,
        onBeforeClose:function() {
            $(this).find(".combo-f").each(function () {
                var panel = $(this).data().combo.panel;
                alert(panel.html());
                panel.panel("destroy");
            });
            $(this).empty();
        }
    });


    ///////////////////////////////////////////////////////////////////////////////////////////////
    
    //扩展datagrid 方法 getSelectedIndex
    $.extend($.fn.datagrid.methods, {
        getSelectedIndex: function (jq) {
            var row = $(jq).datagrid('getSelected');
            if (row)
                return $(jq).datagrid('getRowIndex', row);
            else
                return -1;
        },
        checkRows: function (jq, idValues) {
            if (idValues && idValues.length > 0) {
                var rows = $(jq).datagrid('getRows');
                var keyFild = $(jq).datagrid('options').idField;
                $.each(rows, function (i, n) {
                    if ($.inArray(n[keyFild], idValues)) {
                        $(jq).datagrid('checkRow', row);
                    }
                })
            }
            return jq;
        }
    });
    //扩展 combobox 方法 selectedIndex
    $.extend($.fn.combobox.methods, {
        selectedIndex: function (jq, index) {
            if (!index)
                index = 0;
            var data = $(jq).combobox('options').data;
            var vf = $(jq).combobox('options').valueField;
            $(jq).combobox('setValue', eval('data[index].' + vf));
        }
    });

    //释放IFRAME内存
    $.fn.panel.defaults = $.extend({}, $.fn.panel.defaults, {
        onBeforeDestroy: function () {
            var frame = $('iframe', this);
            if (frame.length > 0) {
                frame[0].contentWindow.document.write('');
                frame[0].contentWindow.close();
                frame.remove();
                if ($.browser.msie) {
                    CollectGarbage();
                }
            }
        }
    });

    //tree 方法扩展 全选、取消全选
    $.extend($.fn.tree.methods, {
        checkedAll: function (jq, target) {
            var data = $(jq).tree('getChildren');
            if(target)
                data = $(jq).tree('getChildren', target);
            
            $.each(data, function(i,n) {
                $(jq).tree('check', n.target);
            });
        }
    });

    $.extend($.fn.tree.methods, {
        uncheckedAll: function (jq) {
            var data = $(jq).tree('getChildren');
            $.each(data, function (i, n) {
                $(jq).tree('uncheck', n.target);
            });
        }
    });

})(jQuery)