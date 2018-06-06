
var themes = [{ "id": "default", "text": "默认皮肤", "selected": true }, { "id": "gray", "text": "流行灰" }]
var savecookdays = [{ "id": 7, "text": "保存7天", "selected": true }, { "id": 30, "text": "保存30天" }, { "id": 365, "text": "永久保存" }, { "id": 1, "text": "不保存" }]

$(function () {

    var h = 360;
  
    if (!showValidateCode) {
        h = 280;
        
    }


    $.hDialog({
        title:'用户登录',boxPadding:'2px',
        width: 478,closable:false,
        height: h, iconCls: 'icon-user',
        modal:false,draggable:false,
        href: 'common/html/loginForm.html',
        buttons:[{
            text: '登录',
            iconCls: 'icon-user',
            handler: login
        }, {
            text: 'About Me',
            iconCls: 'icon-comments',
            handler:aboutMe
        }],align:'center',
        onLoad: function () {
            
            if (!showValidateCode) {
                $('#vCodebox').hide();
                $('#vCodebox').next().hide();
                $('#txt_validatecode').val('1055818239');
                $('.login_top').height(120);
            }

            $('#txt_save').combobox({
                data: savecookdays, width: 120, valueField: 'id', textField: 'text', editable: false, panelHeight: 'auto'
            });
            $('#imgValidateCode').click(function () {
                $(this).attr('src', "validateCode.ashx?t=4&n=" + Math.random());
            });
        }
    });
    
    //响应键盘的回车事件
    $(this).keydown(function (event) {
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;
            return login();
        }
    });

    
});



function login() {
    
    $('#loginForm').form('submit', {
        url: 'ashx/loginhandler.ashx',
        onSubmit: function () {
            var isValid = $('#loginForm').form('validate');
            if(isValid) {
                $.hLoading.show({ msg: '正在登录中...' });
            }
            return isValid;
        },
        success: function (data) {
            $.hLoading.hide();
            var d = eval('(' + data + ')');
            if (d.success)
                location.href = "/";
            else {
                //更新验证码
                $('#imgValidateCode').click();
                alert(d.message);
            }
        }
    });
}

function aboutMe(){
    $.hDialog({
        title: '关于疯狂秀才',
        width: 400,
        height: 300,
        showBtns: false,
        content:''
    });
}

function getsize() {
    var windowHeight = 0;
    var widowWidth = 0;
    if (typeof (window.innerHeight) == 'number') {
        windowHeight = window.innerHeight;
        widowWidth = window.innerWidth;
    }
    else {
        if (document.documentElement && document.documentElement.clientHeight) {
            windowHeight = document.documentElement.clientHeight;
            widowWidth = document.documentElement.clientWidth;
        }
        else {
            if (document.body && document.body.clientHeight) {
                windowHeight = document.body.clientHeight;
                widowWidth = document.body.clientWidth;
            }
        }
    }

    return { width: widowWidth, height: windowHeight };
}