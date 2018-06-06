using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Xiucai.Common
{
    /// <summary>
    /// FTP类
    /// </summary>
    public class FTP :IDisposable
    {
        private string _ftpServerIP;
        private string _ftpUserName;
        private string _ftpPassword;

        private Uri ftpUri;
        private string _path;

        #region 属性
        /// <summary>
        /// ftp 路径
        /// </summary>
        public string FtpPath
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// ftp Ip地址
        /// </summary>
        public string FtpServerIP
        {
            get { return _ftpServerIP; }
            set { _ftpServerIP = value; }
        }

        /// <summary>
        /// ftp 用户名
        /// </summary>
        public string FtpUserName
        {
            get { return _ftpUserName; }
            set { _ftpUserName = value; }
        }

        /// <summary>
        /// ftp 密码
        /// </summary>
        public string FtpPassword
        {
            get { return _ftpPassword; }
            set { _ftpPassword = value; }
        }

        #endregion

        #region 构造函数

        public FTP(string ftpServerIp, string username, string passwd)
        { 
            this.FtpServerIP = ftpServerIp;
            this.FtpUserName = username;
            this.FtpPassword = passwd;
            this.ftpUri = new Uri("ftp://" + ftpServerIp);
        }

        public FTP(string ftpServerIp, string username, string passwd,string ftp_path) :this(ftpServerIp, username, passwd)
        {
            this.FtpPath = ftp_path;
            this.ftpUri = new Uri("ftp://" + ftpServerIp+"/"+ftp_path);
        }


        #endregion

        #region 方法
        public string GetFiles()
        {
            FtpWebRequest listRequest = (FtpWebRequest) WebRequest.Create (ftpUri);
 
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
           
            listRequest.Credentials = new NetworkCredential(FtpUserName ,FtpPassword);
 
            FtpWebResponse listResponse = (FtpWebResponse) listRequest.GetResponse ();
            Stream responseStream = listResponse.GetResponseStream ();
            StreamReader readStream = new StreamReader (responseStream , System.Text.Encoding.Default);

            string result = "";

            if ( readStream != null)
            {
                result = readStream.ReadToEnd () ;
            }
 
            //string.Format ( "状态: {0},{1}" ,listResponse.StatusCode,  listResponse.StatusDescription );
 
            listResponse.Close ( );
            responseStream.Close ( );
            readStream.Close ( );

            return result;
        }

        /// <summary>
        /// 获取文件及文件夹列表
        /// </summary>
        /// <returns></returns>
        public List<FileStruct> GetList()
        {
            string dataString =GetFiles();
            DirectoryListParser parser = new DirectoryListParser(dataString);
            List<FileStruct> list = parser.DirectoryList;
            list.AddRange(parser.FileList);
            return list;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public bool MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create.
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUri + "/"  +dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 将文件上载到ftp服务器
        /// </summary>
        /// <param name="filename">文件名</param>
        public bool Upload(string filename)
        {
            bool result = false;
            FileInfo fileInf = null;
            fileInf = new FileInfo(filename);
            string uri = ftpUri+"/"+ fileInf.Name;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                result= true;
            }
            catch (Exception ex)
            {return false; }
            
            return result;
        }
        /// <summary>
        /// 将文件追加到现有的文件夹内
        /// </summary>
        /// <param name="filename">文件名</param>
        public void AppendFile(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = ftpUri + fileInf.Name;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.AppendFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 用于删除ftp服务器的文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public bool Delete(string fileName)
        {
            try
            {
                string uri = ftpUri+"/" + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 重命名目录
        /// </summary>
        /// <param name="currentFilename">原来名称</param>
        /// <param name="newFilename">新名称</param>
        public bool ReName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUri+ currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// </summary> 
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            string m = string.Empty;
            if (drectory != null&&drectory.Length > 0)
            {
                foreach (string str in drectory)
                {
                    if (str.Trim().Substring(0, 1).ToUpper() == "D")
                    {
                        m += str.Substring(54).Trim() + "\n";
                    }
                }
            }
            char[] n = new char[] { '\n' };
            return m.Split(n);
        }
        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(ftpUri);
                ftp.Credentials= new NetworkCredential(FtpUserName, FtpPassword);
　　　　　      ftp.Method= WebRequestMethods.Ftp.ListDirectoryDetails;
               // ListFilesAndDirectories
                //ftp.Method = WebRequestMethods.Ftp.ListDirectory();
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
　　　　　　　      line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folderName"></param>
        public bool RemoveDirectory(string folderName)
        {
            try
            {
                string uri = ftpUri +"/"+ folderName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获得当前文件夹下的所有目录（仅文件夹）
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFolder()
        {
            
            //返回值
            List<string> result = new List<string>(); ;
            //先得到此文件夹下的所有信息
            List<FileStruct> allList = this.GetList();
            //循环选出文件夹
            FileStruct fs = new FileStruct ();
            for (int i = 0; i < allList.Count;i++ ){
                fs = allList[i];
                if (fs.FileType=="文件夹"){
                    result.Add(fs.Name);
                }
            }
            return result;
        }
        /// <summary>
        /// 获得当前文件夹下的所有文件（仅文件）
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFiles()
        {
            //返回值
            List<string> result = new List<string>(); ;
            //先得到此文件夹下的所有信息
            List<FileStruct> allList = this.GetList();
            //循环选出文件夹
            FileStruct fs = new FileStruct();
            for (int i = 0; i < allList.Count; i++)
            {
                fs = allList[i];
                if (fs.FileType != "文件夹")
                {
                    result.Add(fs.Name);
                }
            }
            return result;
        }
        #endregion

        public void Dispose()
        {}
      
    }

    public struct FileStruct
    {
        /// <summary>
        /// 属性
        /// </summary>
        public string Flags; 
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner;
        /// <summary>
        /// 是否为目录
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// 更新时间
        /// </summary>
        public string CreateTime;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize;

        /// <summary>
        /// 类型
        /// </summary>
        public string FileType;
    }

    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }

    public class DirectoryListParser
    {
        private List<FileStruct> _myListArray;

        public List<FileStruct> FullListing
        {
            get
            {
                return _myListArray;                
            }
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileStruct> FileList
        {            
            get
            {
                List<FileStruct> _fileList = new List<FileStruct>();
                foreach(FileStruct thisstruct in _myListArray)
                {
                    if(!thisstruct.IsDirectory)
                    {
                        _fileList.Add(thisstruct);                        
                    }
                }
                return _fileList;
            }
        }

        /// <summary>
        /// 目录列表
        /// </summary>
        public List<FileStruct> DirectoryList
        {
            get
            {
                List<FileStruct> _dirList = new List<FileStruct>();
                foreach(FileStruct thisstruct in _myListArray)
                {
                    if(thisstruct.IsDirectory)
                    {
                        _dirList.Add(thisstruct);                        
                    }
                }
                return _dirList;
            }
        }

        public DirectoryListParser(string responseString)
        {
            _myListArray = GetList(responseString);            
        }
        
        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>(); 
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:                            
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (f.Name != "" && f.Name != "." && f.Name != "..")
                    {
                        myListArray.Add(f);     
                    }
                }
            }
            return myListArray;
        }
        
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            // Assuming the record style as 
            //文件夹 02-03-04  07:46PM       <DIR>          Append
            //文件 02-12-11  02:20AM                26599 xxd.asp
            FileStruct f = new FileStruct();
            //string processstr = Record.Trim();
            //string dateStr = processstr.Substring(0,8);      
            //processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            //string timeStr = processstr.Substring(0, 7);
            //processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            //f.CreateTime = dateStr + " " + timeStr;
            //if (processstr.Substring(0,5) == "<DIR>")
            //{
            //    f.IsDirectory = true;    
            //    processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            //}
            //else
            //{
            //    string[] strs = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //    processstr = strs[1];
            //    f.IsDirectory = false;
            //}
            //f.Name = processstr;  //Rest is name   

            string[] arr = Regex.Split(Record.Trim(), "\\s+");

            f.Name = arr[3];
            f.CreateTime = arr[0] + "  " + arr[1];

            if (arr[2] == "<DIR>")
            {
                f.FileSize = 0;
                f.IsDirectory = true;
                f.FileType = "文件夹";
            }
            else
            {
                f.FileSize = int.Parse(arr[2]);
                f.IsDirectory = false;
                if (arr[3].IndexOf(".")>-1)
                    f.FileType = arr[3].Split('.')[1];
                else
                    f.FileType = "未知";
            }

            return f;
        }
        
        public FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if(s.Length > 10 
                    && Regex.IsMatch(s.Substring(0,10),"(-|d)((-|r)(-|w)(-|x)){3}"))
                {
                    return FileListStyle.UnixStyle;
                }    
                else if (s.Length > 8 
                    && Regex.IsMatch(s.Substring(0, 8),  "[0-9]{2}-[0-9]{2}-[0-9]{2}"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }
        
        private FileStruct ParseFileStructFromUnixStyleRecord(string record)
        {
            ///Assuming record style as
            /// dr-xr-xr-x   1 owner    group               0 Nov 25  2002 bussys
            FileStruct f = new FileStruct();
            if (record[0] == '-' || record[0] == 'd')
            {// its a valid file record
                string processstr = record.Trim();
                f.Flags = processstr.Substring(0, 9);
                f.IsDirectory = (f.Flags[0] == 'd');
                processstr = (processstr.Substring(11)).Trim();
                _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //skip one part
                f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
                f.CreateTime = getCreateTimeString(record);
                int fileNameIndex = record.IndexOf(f.CreateTime)+f.CreateTime.Length;
                f.Name = record.Substring(fileNameIndex).Trim();   //Rest of the part is name            
    
            }
            else
            {
                f.Name = "";
            }
            return f;
        }

        private string getCreateTimeString(string record)
        {
            //Does just basic datetime string validation for demo, not an accurate check
            //on date and time fields
            string month = "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)";
            string space = @"(\040)+";
            string day = "([0-9]|[1-3][0-9])";
            string year = "[1-2][0-9]{3}";
            string time = "[0-9]{1,2}:[0-9]{2}";            
            Regex dateTimeRegex = new Regex(month+space+day+space+"("+year+"|"+time+")", RegexOptions.IgnoreCase);
            Match match = dateTimeRegex.Match(record);
            return match.Value;
        }    
    
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0,pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
       }


    }
}