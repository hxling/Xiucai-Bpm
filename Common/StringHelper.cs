using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Web;
using System.Collections;

namespace Xiucai.Common
{
    public class StringHelper
    {
        #region 删除HTML标记
        /// <summary>
        /// 删除HTML标记
        /// </summary>
        /// <param name="htmlString">带有样式的字符串</param>
        /// <returns></returns>
        public static string RemoveHtmlFormat(string htmlString)
        {
            return Regex.Replace(htmlString, "<[^>]+>", "");
        }
        #endregion

        #region 截断字符串
        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="str">要截断的字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            int i = 0, j = 0;
            foreach (char chr in str)
            {
                i += 2;
                if (i > length)
                {
                    str = str.Substring(0, j - 1) + "...";
                    break;
                }
                j++;
            }
            return str;
        }
        #endregion

        #region 生成唯一ID 由数字组成

        /// <summary>
        /// 生成唯一ID
        /// </summary>
        /// <returns></returns>
        public static string CreateIDCode()
        {
            DateTime Time1 = DateTime.Now.ToUniversalTime();
            DateTime Time2 = Convert.ToDateTime("1970-01-01");
            TimeSpan span = Time1 - Time2;   //span就是两个日期之间的差额   
            string t = span.TotalMilliseconds.ToString("0");

            return t;
        }
        #endregion

        #region 压缩与解压字符串

        #region 压缩字符串
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="unCompressedString">要压缩的字符串</param>
        /// <returns></returns>
        public static string ZipString(string unCompressedString)
        {

            byte[] bytData = System.Text.Encoding.UTF8.GetBytes(unCompressedString);
            MemoryStream ms = new MemoryStream();
            Stream s = new GZipStream(ms, CompressionMode.Compress);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] compressedData = (byte[])ms.ToArray();
            return System.Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }

        #endregion

        #region 解压字符串
        /// <summary>
        ///  解压字符串
        /// </summary>
        /// <param name="unCompressedString">要解压的字符串</param>
        /// <returns></returns>
        public static string UnzipString(string unCompressedString)
        {
            System.Text.StringBuilder uncompressedString = new System.Text.StringBuilder();
            byte[] writeData = new byte[4096];

            byte[] bytData = System.Convert.FromBase64String(unCompressedString);
            int totalLength = 0;
            int size = 0;

            Stream s = new GZipStream(new MemoryStream(bytData), CompressionMode.Decompress);
            while (true)
            {
                size = s.Read(writeData, 0, writeData.Length);
                if (size > 0)
                {
                    totalLength += size;
                    uncompressedString.Append(System.Text.Encoding.UTF8.GetString(writeData, 0, size));
                }
                else
                {
                    break;
                }
            }
            s.Close();
            return uncompressedString.ToString();
        }

        #endregion

        #endregion

        #region 转全角的函数(SBC case)
        /// 
        /// 转全角的函数(SBC case)
        /// 
        /// 任意字符串
        /// 全角字符串
        ///
        ///全角空格为12288，半角空格为32///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///        
        public string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        #endregion

        #region 转半角的函数(DBC case)
        /// 
        /// 转半角的函数(DBC case)
        /// 
        /// 任意字符串
        /// 半角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        #region Html 编码

        /// <summary>
        /// 对文本框中的字符进行HTML编码
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }

        /// <summary>
        /// 对字符串进行HTML解码,解析为可为页面识别的代码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <returns></returns>
        public static string HtmlDecode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        #endregion

        #region  用户名过滤
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Filter(string userName)
        {
            if (IsExist(userName,"!")) return false;
            if (IsExist(userName, "！")) return false;
            if (IsExist(userName, "#")) return false;
            if (IsExist(userName, "&")) return false;
            if (IsExist(userName, "$")) return false;
            if (IsExist(userName, "*")) return false;
            if (IsExist(userName, ".")) return false;
            if (IsExist(userName, ",")) return false;
            if (IsExist(userName, ";")) return false;
            if (IsExist(userName, "'")) return false;
            if (IsExist(userName, "<")) return false;
            if (IsExist(userName, ">")) return false;
            return true;
        }

        public static bool IsExist(string userName, string filterStr)
        {
            if (userName.IndexOf(filterStr) > -1)
                return true;
            return false;
        }
        #endregion

        #region SQL注入过滤
        /// <summary>
        ///SQL注入过滤
        /// </summary>
        /// <param name="InText">要进行过滤的字符串</param>
        /// <returns>如果参数存在不安全字符,则返回true</returns>
        public static bool SqlFilter2(string InText)
        {
            string word = "exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将指定的str串执行sql关键字过滤并返回
        /// </summary>
        /// <param name="str">要过滤的字符串</param>
        /// <returns></returns>
        public static string SqlFilter(string str)
        {
            return str.Replace("'", "").Replace("&#39;", "").Replace("--", "").Replace("&","").Replace("/*","").Replace(";","").Replace("%","");
        }

        /// <summary>
        /// 将指定的串列表执行sql关键字过滤并以[,]号分隔返回
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string SqlFilters(params string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strs)
            {
                sb.Append(SqlFilter(str) + ",");
            }
            if (sb.Length > 0)
                return sb.ToString().TrimEnd(',');
            return "";
        }

        public static bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = false;
            try
            {
                if (Str != "")
                {
                    string SqlStr = "&#39;|insert|select*|and'|or'|insertinto|deletefrom|altertable|update|createtable|createview|dropview|createindex|dropindex|createprocedure|dropprocedure|createtrigger|droptrigger|createschema|dropschema|createdomain|alterdomain|dropdomain|);|select@|declare@|print@|char(|select";
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = true;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

       
        #endregion

        #region 获取CheckBoxList控件中选中的项
        /// <summary>
        /// 获取CheckBoxList控件中选中的项的value，字符串由,分隔
        /// </summary>
        /// <param name="chk">CheckBoxList 控件ID</param>
        /// <returns></returns>
        public static string GetCheckedItemValue(CheckBoxList chk)
        {
            string s = "";
            foreach (ListItem li in chk.Items)
            {
                if (li.Selected)
                    s += li.Value + ",";
            }
            return s.TrimEnd(',');
        }
        /// <summary>
        /// 获取CheckBoxList控件中选中的项的Text，字符串由,分隔
        /// </summary>
        /// <param name="chk">CheckBoxList 控件ID</param>
        /// <returns></returns>
        public static string GetCheckedItemText(CheckBoxList chk)
        {
            string s = "";
            foreach (ListItem li in chk.Items)
            {
                if (li.Selected)
                    s += li.Text + ",";
            }
            return s.TrimEnd(',');
        }
        #endregion

        #region 根据提供的Id字符串，将列表中的项选中
        /// <summary>
        /// 根据提供的Id字符串，将列表中的项选中
        /// </summary>
        /// <param name="itemid">Id字符串，由,分隔</param>
        /// <param name="checkboxlist">CheckBoxList控件</param>
        public static void CheckItem(string itemid, CheckBoxList checkboxlist)
        {
            foreach (ListItem li in checkboxlist.Items)
            {
                if (itemid.IndexOf(li.Value) != -1)
                    li.Selected = true;
            }
        }

        #endregion

        #region 加密字符串 MD5
        #region 利用 MD5 加密算法加密字符串
        /// <summary>
        /// 利用 MD5 加密算法加密字符串
        /// </summary>
        /// <param name="src">字符串源串</param>
        /// <returns>返加MD5 加密后的字符串</returns>
        public static string ComputeMD5(string src)
        {
            //将密码字符串转化成字节数组
            byte[] byteArray = GetByteArray(src);

            //计算 MD5 密码
            byteArray = (new MD5CryptoServiceProvider().ComputeHash(byteArray));

            //将字节码转化成字符串并返回
            return BitConverter.ToString(byteArray);
        }

        /// <summary>
        /// 将指定串加密为不包含中杠的MD5值
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="isupper">返回值的大小写(true大写,false小写)</param>
        /// <returns></returns>
        public static string ComputeMD5(string str, bool isupper)
        {
            string md5str = ComputeMD5(str);
            if (isupper)
                return md5str.ToUpper();
            return md5str.ToLower();
        }
        #endregion

        #region 将字符串翻译成字节数组
        /// <summary>
        /// 将字符串翻译成字节数组
        /// </summary>
        /// <param name="src">字符串源串</param>
        /// <returns>字节数组</returns>
        private static byte[] GetByteArray(string src)
        {
            byte[] byteArray = new byte[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                byteArray[i] = Convert.ToByte(src[i]);
            }

            return byteArray;
        }
        #endregion

        #region MD5string

        public static string MD5string(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        public static string MD5string(string str,bool isupper)
        {
            string md5string = MD5string(str);
            if (isupper)
                return md5string.ToUpper();
            else
                return md5string.ToLower();
        }

        #endregion
        #endregion

        #region SHA1 加密
        public string SHA1(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        #endregion

        #region DES加密字符串
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString,string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        #endregion

        #region DES解密字符串
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="key">解密密钥，要求8位</param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString,string key)
        {
            try
            {
                //默认密钥向量
                byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        #endregion

        #region AES 加密 解密
       
       

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">要加密字符串</param>
        /// <returns>返回加密后字符串</returns>
        public static String EncryptAES(String str,string aeskey)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(aeskey);
            Byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(str);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }



        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">要解密字符串</param>
        /// <returns>返回解密后字符串</returns>
        public static String DecryptAES(String str, string aeskey)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(aeskey);
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return System.Text.UTF8Encoding.UTF8.GetString(resultArray);
        }

 #endregion

        #region  base64 字符串编码
        /// <summary>
        /// base64 字符串编码
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        /// <returns></returns>
        public static string ToBase64(string str)
        {
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
            return Convert.ToBase64String(data); 
        }

        /// <summary>
        /// base 64 字符串解码
        /// </summary>
        /// <param name="base64str">要解码的字符串</param>
        /// <returns></returns>
        public static string UnBase64(string base64str)
        {
            byte[] data = Convert.FromBase64String(base64str);
            return System.Text.ASCIIEncoding.ASCII.GetString(data); 
        }


        #endregion

        #region 转换为中文星期
        /// <summary>
        /// 转换为中文星期
        /// </summary>
        /// <param name="dayfweek">英文星期</param>
        /// <returns></returns>
        public static string ConvertWeekDayToCn(DayOfWeek dayfweek)
        {
            switch (dayfweek)
            {
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                default:
                    return "";
            }
        }

        #endregion

        #region 执行CMD 命令
        /// <summary>
        /// 执行CMD 命令
        /// </summary>
        /// <param name="strCommand">命令串</param>
        /// <returns></returns>
        public static string RunCommand(string strCommand)
        {
            Process process = new Process();
            process.StartInfo.FileName = "CMD.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(strCommand);
            process.StandardInput.WriteLine("exit");
            string str = process.StandardOutput.ReadToEnd();
            process.Close();
            return str;
        }
        #endregion

       
        public static string Escape(string s)
        {
            StringBuilder builder = new StringBuilder();
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            for (int i = 0; i < bytes.Length; i += 2)
            {
                builder.Append("%u");
                builder.Append(bytes[i + 1].ToString("X2"));
                builder.Append(bytes[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public static string ReplaceHtml(string str)
        {
            if (str == null || str.Length==0)
                return "";
            str = str.Replace("<", "&lt");
            str = str.Replace(">", "&gt");
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            return str; 
        }

        public static string ReplaceEnter(string str)
        {
            if (str == null || str.Length == 0)
                return "";
            
            str = str.Replace("\n", "");
            str = str.Replace("\r", "");
            return str;
        }

        /// <summary>
        /// 替换指定符串中的首个指定字符为新的字符
        /// </summary>
        /// <param name="sourcestr">要修改的字符串</param>
        /// <param name="oldstr">被替换的字符串</param>
        /// <param name="newstr">替换字符串 </param>
        /// <returns></returns>
        public static string ReplaceFirst(string sourcestr,string oldstr, string newstr)
        {
            Regex reg = new Regex(oldstr);
            if (reg.IsMatch(sourcestr))
            {
                sourcestr = reg.Replace(sourcestr, newstr, 1);
            }
            return sourcestr;
        }

        #region 生成随机字符串，格式：1q2w3e4r
        /// <summary>
        /// 生成随机字符串，格式：1q2w3e4r
        /// </summary>
        /// <returns></returns>
        public static string BuildPassword()
        {
            Random random = new Random();
            List<int> ints = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                ints.Add(random.Next(9));
            }

            List<string> strs = new List<string>();

            //string CodeSerial = "a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            string CodeSerial = "a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z";

            string[] arr = CodeSerial.Split(',');

            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < 4; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                strs.Add(arr[randValue]);
            }

            string passwd = "";

            for (int k = 0; k < 4; k++)
            {
                passwd += ints[k].ToString() + strs[k];
            }

            return passwd;
        }
        #endregion

        public static object GetRequestObject(string key)
        {
            return HttpContext.Current.Request[key];
        }
    }
}
