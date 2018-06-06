using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Xiucai.BPM.Core;
using Xiucai.BPM.Core.Bll;
using Xiucai.BPM.Core.Model;
using Xiucai.Common;
using ICSharpCode.SharpZipLib.Zip;

namespace Xiucai.BPM.Admin.sys.ashx
{
    /// <summary>
    /// DataBaseHandler 的摘要说明
    /// </summary>
    public class DataBaseHandler : IHttpHandler,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            UserBll.Instance.CheckUserOnlingState();

            var rpm = new RequestParamModel<object> {CurrentContext = context, Action = context.Request["action"]};

            switch (rpm.Action)
            {
                case "backup":
                    BackupDb(context);
                    break;
                case "down":
                    DownloadFile(context);
                    break;
                case "del":
                    DeleteFile(context);
                    break;
                default:
                    context.Response.Write(JSONhelper.ToJson(DbFiles()));
                    break;
            }

        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="context"></param>
        void BackupDb(HttpContext context)
        {
            string dbname = ConfigHelper.GetValue("dbname");
            string backupName = StringHelper.CreateIDCode();
            string savePath = context.Server.MapPath("~/dbase/");

            string backupSql = "DUMP TRANSACTION {0} WITH NO_LOG; BACKUP DATABASE {0} to DISK ='{1}' ";
            backupSql = string.Format(backupSql, dbname, savePath + backupName+".bak");
            try
            {
                //执行备份
                Xiucai.Common.Data.DbUtils.ExecuteNonQuery(backupSql, null);
                addZipEntry(backupName, savePath);

                //写入操作日志
                LogBll<object> log = new LogBll<object>();
                log.AddLog("备份数据库", "数据库备份成功，文件名：" + backupName + ".bak");

                context.Response.Write(new JsonMessage { Data = "1", Message = "数据库备份成功。", Success = true }.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write(new JsonMessage { Data = "1", Message = ex.StackTrace, Success = false }.ToString());
            }
        }


        //删除备份文件
        void DeleteFile(HttpContext context)
        {
            string basepath = context.Server.MapPath("~/dbase/");
            string filename = context.Request["n"].ToString();
            string downpath = basepath + filename;
            File.Delete(downpath);

            //写入操作日志
            LogBll<object> log = new LogBll<object>();
            log.AddLog("删除备份文件", "数据库备份文件删除成功，文件名：" + filename);

            context.Response.Write(new JsonMessage { Data = "1", Message = "删除成功。", Success = true }.ToString());
            context.Response.End();
        }

        //下载备份文件
        void DownloadFile(HttpContext context)
        {
            string basepath = context.Server.MapPath("~/dbase/");
            string filename = context.Request["n"];
            string downpath = basepath + filename;
            MemoryStream ms = null;
            context.Response.ContentType = "application/octet-stream";

            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            ms = new MemoryStream(File.ReadAllBytes(downpath));

            //写入操作日志
            LogBll<object> log = new LogBll<object>();
            log.AddLog("下载备份数据库", "数据库备份文件下载，文件名：" + filename);

            context.Response.Clear();
            context.Response.BinaryWrite(ms.ToArray());
            context.Response.End();
        }

        //获取备份文件列表
        IEnumerable DbFiles()
        {
            string path = HttpContext.Current.Server.MapPath("~/dbase/");
            DirectoryInfo di = new DirectoryInfo(path);
            return from n in di.GetFiles()
                   orderby n.CreationTime descending
                   select
                       new
                           {
                               FileName = n.Name,
                               FileSize = ((float)n.Length / 1024 /1024).ToString("N3")+" M",
                               CreateDate = n.CreationTime.ToString("yyyy-MM-dd HH:mm:ss")
                           };
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="newZipFileName">新文件名</param>
        /// <param name="savePath">保存路径</param>
        public void addZipEntry(string newZipFileName,string savePath)
        {
            var newZipFullpath = savePath + newZipFileName;
            
            FileStream zipfs = File.Create(newZipFullpath+".zip");
            ZipOutputStream zos = new ZipOutputStream(zipfs);
            FileInfo fi = new FileInfo(newZipFullpath+".bak" );
            FileStream fs = File.OpenRead(fi.FullName);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);

            ZipEntry entry = new ZipEntry(newZipFileName+".zip");
            zos.PutNextEntry(entry);
            zos.Write(buffer, 0, buffer.Length);
            fs.Close();
            zos.Finish();
            zos.Close();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}