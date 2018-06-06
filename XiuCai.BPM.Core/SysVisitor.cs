using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Xiucai.BPM.Core.Dal;
using Xiucai.BPM.Core.Model;
using Xiucai.Common.Provider;
using Xiucai.Common;
using Xiucai.BPM.Core.Bll;
namespace Xiucai.BPM.Core
{
    public class SysVisitor
    {

        public static SysVisitor Instance
        {
            get { return SingletonProvider<SysVisitor>.Instance; }
        }



        #region Session Key

        public const string SessionUserIdKey = "HXLING-BPM-ADMIN-USERID";
        public const string SessionUserNameKey = "HXLING-BPM-ADMIN-USERNAME";
        public const string SessionIsAdminKey = "HXLING-BPM-ADMIN-ISADMIN";
        /// <summary>
        /// 用户可访问的部门列表
        /// </summary>
        public const string SessionDepartmentsKey = "HXLING-BPM-USER-DEPARTMENTS";
        #endregion

        #region CookieName Key

        public const string CookieNameKey = "HXLING-BPM-COOKIE-NAME";
        public const string CookieUserNameKey = "HXLING-BPM-COOKIE-USERNAME";
        public const string CookiePasswordKey = "HXLING-BPM-COOKIE-PASSWORD";
        public const string CookieDepartmentsKey = "HXLING-BPM-COOKIE-USER-DEPARTMENTS";
        #endregion

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get { return PublicMethod.GetInt(HttpContext.Current.Session[SessionUserIdKey]); }
            set { HttpContext.Current.Session[SessionUserIdKey] = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get { return PublicMethod.GetString(HttpContext.Current.Session[SessionUserNameKey]); }
            set { HttpContext.Current.Session[SessionUserNameKey] = value; }
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdmin
        {
            get { return PublicMethod.GetBool(HttpContext.Current.Session[SessionIsAdminKey]); }
            set { HttpContext.Current.Session[SessionIsAdminKey] = value; }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser
        {
            get { return (User) HttpContext.Current.Session["HXLING-BPM-ADMIN"]; }
            set { HttpContext.Current.Session["HXLING-BPM-ADMIN"] = value; }
        }

        /// <summary>
        /// 当前用户可以访问的部门数据
        /// </summary>
        public string Departments
        {
            get { return HttpContext.Current.Session[SessionDepartmentsKey] as string; }
            set { HttpContext.Current.Session[SessionDepartmentsKey] = value; }
        }



        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string ThemeName
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentUser.ConfigJson))
                    return "default";
                return JSONhelper.ConvertToObject<ConfigModel>(CurrentUser.ConfigJson).Theme.Name;
            }
        }

        public int GridRows
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentUser.ConfigJson))
                    return 20;
                return JSONhelper.ConvertToObject<ConfigModel>(CurrentUser.ConfigJson).GridRows;
            }
        }

        public bool IsGuest
        {
            get
            {
                if (string.IsNullOrEmpty(UserName))
                    return !UserBll.Instance.UserLogin();
                return false;
            }
        }

        public void LoginOut()
        {

            //写入退出日志
            LogModel log = new LogModel();
            log.BusinessName = "用户退出";
            log.OperationIp = PublicMethod.GetClientIP();
            log.OperationTime = DateTime.Now;
            log.PrimaryKey = "";
            log.UserId = UserId;
            log.TableName = "";
            log.OperationType = (int)OperationType.LoginOut;
            LogDal.Instance.Insert(log);


            CookieHelper.ClearUserCookie("", SysVisitor.CookieNameKey);
            UserName = null;
            UserId = 0;

        }

    }
}
