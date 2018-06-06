using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Web;
using System.Data;
using System.IO;

namespace Xiucai.Common
{
    public class BadWordsFilter
    {
        //保存脏字的字典
        private List<string> KeyWordDictionary = new List<string>();
        private byte[] fastCheck = new byte[char.MaxValue];
        private BitArray charCheck = new BitArray(char.MaxValue);
        private int maxWordLength = 0;
        private int minWordLength = int.MaxValue;
        private string _replaceString = "*";
        private string _fileText = "";

        /// <summary>
        /// 返回脏字文本(只读)
        /// </summary>
        public string KeyWordText
        {
            get { return _fileText; }
        }

        /// <summary>
        /// 读取指定文件地址的文本文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public string BadWordReadText(string filePath)
        {
            _fileText = string.Empty;
            if (File.Exists(filePath))
            {
                //Encoding.GetEncoding("gb2312")
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                _fileText = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            return _fileText;
        }

        /// <summary>
        /// 将指定内容写入到指定文件路径(true成功,false不成功)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="text">要写入的文本内容</param>
        /// <returns></returns>
        public bool BadWordWriteText(string filePath, string text)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath, false, Encoding.Default);
                sw.Write(text);
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        static string badwordfilepath = HttpContext.Current.Server.MapPath("~/app_data/badword.txt");

        public BadWordsFilter()
            : this(badwordfilepath)
        {

        }

        /// <summary>
        /// 存放XML的绝对位置
        /// </summary>
        /// <param name="filePath"></param>
        public BadWordsFilter(string filePath)
        {            
            string srList = BadWordReadText(filePath);
            string[] badwords = srList.Split('|');
            foreach (string word in badwords)
            {
                maxWordLength = Math.Max(maxWordLength, word.Length);
                minWordLength = Math.Min(minWordLength, word.Length);
                for (int i = 0; i < 7 && i < word.Length; i++)
                {
                    fastCheck[word[i]] |= (byte)(1 << i);
                }

                for (int i = 7; i < word.Length; i++)
                {
                    fastCheck[word[i]] |= 0x80;
                }

                if (word.Length == 1)
                {
                    charCheck[word[0]] = true;
                }
                else
                {
                    KeyWordDictionary.Add(word);
                }
            }
        }

        /// <summary>
        /// 判断指定的文本中是否存在不允许的字符
        /// </summary>
        /// <param name="text">要判断的文本</param>
        /// <returns></returns>
        public bool HasBadWord(string text)
        {
            int index = 0;

            while (index < text.Length)
            {


                if ((fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                //单字节检测
                if (minWordLength == 1 && charCheck[text[index]])
                {
                    return true;
                }


                //多字节检测
                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {
                    //快速排除
                    if ((fastCheck[text[index + j]] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        string sub = text.Substring(index, j + 1);

                        if (KeyWordDictionary.Contains(sub))
                        {
                            return true;
                        }
                    }
                }
                index++;
            }
            return false;
        }


        public string ReplaceBadWord(string text)
        {
            int index = 0;

            for (index = 0; index < text.Length; index++)
            {
                if ((fastCheck[text[index]] & 1) == 0)
                {
                    while (index < text.Length - 1 && (fastCheck[text[++index]] & 1) == 0) ;
                }

                //单字节检测
                if (minWordLength == 1 && charCheck[text[index]])
                {
                    //return true;
                    text = text.Replace(text[index], _replaceString[0]);
                    continue;
                }
                //多字节检测
                for (int j = 1; j <= Math.Min(maxWordLength, text.Length - index - 1); j++)
                {

                    //快速排除
                    if ((fastCheck[text[index + j]] & (1 << Math.Min(j, 7))) == 0)
                    {
                        break;
                    }

                    if (j + 1 >= minWordLength)
                    {
                        string sub = text.Substring(index, j + 1);

                        if (KeyWordDictionary.Contains(sub))
                        {

                            //替换字符操作
                            char cc = _replaceString[0];
                            string rp = _replaceString.PadRight((j + 1), cc);
                            text = text.Replace(sub, rp);
                            //记录新位置
                            index += j;
                            break;
                        }
                    }
                }
            }

            return text;
        }

    }
}
