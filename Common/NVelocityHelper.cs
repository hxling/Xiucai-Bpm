using System.Text;
using NVelocity;
using Commons.Collections;
using NVelocity.App;
using NVelocity.Runtime;
using System.Web;
using NVelocity.Context;
using System.IO;

namespace Xiucai.Common
{
    public class NVelocityHelper
    {
        private VelocityEngine velocity;
        private IContext context = null;
        private string _stringTemplate;

        private string _templatepath;

        public NVelocityHelper():this(null) { }

        public NVelocityHelper(string templatePath) 
        {
            this.Init(templatePath);
            _templatepath = templatePath;
        }

        public string TemplatePath
        {
            get {
                return _templatepath;
            }
        }


        /// <summary>
        /// 模板字符串
        /// </summary>
        public string StringTemplate
        {
            get { return _stringTemplate; }
            set { _stringTemplate = value; }
        }

        public override string  ToString()
        {
            if (this.StringTemplate == "")
                return "";
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    velocity.Evaluate(context, sw, null, this.StringTemplate);
                    return sw.GetStringBuilder().ToString();
                }
            }
        }


        private void Init(string templatePath) 
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            if (!string.IsNullOrEmpty(templatePath))
            {
                props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatePath);
                
            }
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }

        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            context.Put(key, value);
        }

        /// <summary>
        /// 生成字符
        /// </summary>
        /// <param name="templatFileName">模板文件名</param>
        public string FileToString(string templatFileName)
        {
            if (templatFileName == "")
                return "";

            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            using (StringWriter writer = new StringWriter())
            {
                template.Merge(context, writer);
                
                return writer.ToString();
            }
        }

        /// <summary>
        /// 输出模板文件内容
        /// </summary>
        /// <param name="templateFileName">模板文件名</param>
        public void Display(string templateFileName)
        {
            string s = this.FileToString(templateFileName);
            HttpContext context = HttpContext.Current;
            HttpContext.Current.Response.Clear();
            context.Response.Write(s);
            context.Response.Flush();
            context.Response.End();
        }

        /// <summary>
        /// 保存根据模板生成的文件
        /// </summary>
        /// <param name="templateFileName">模板文件名称</param>
        /// <param name="saveFileName">新文件完全限定名称</param>
        public void SaveFile(string templateFileName, string saveFileName)
        {
            
            string filecontent = this.FileToString(templateFileName);

            string savepath = saveFileName.Substring(0, saveFileName.LastIndexOf('\\'));

            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);


            using (StreamWriter sw = new StreamWriter(saveFileName,false,Encoding.UTF8))
            {
                sw.Write(filecontent);
                sw.Flush();
                sw.Close();
            }
        }

        public void SaveFile(string saveFileName)
        {
            string filecontent =this.ToString();

            string savepath = saveFileName.Substring(0, saveFileName.LastIndexOf('\\'));

            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);

            using (StreamWriter sw = new StreamWriter(saveFileName, false, Encoding.UTF8))
            {
                sw.Write(filecontent);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
