using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using Xiucai.Common;

namespace Xiucai.Upload
{
    public class Upload
    {

        #region 上传图片
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imgBuffer">字节数组</param>
        /// <param name="uploadpath">保存路径。绝对或虚拟路径</param>
        /// <param name="imgformat">图片保存格式</param>
        /// <returns>上传成功后返回的新的文件名</returns>
        public static string UploadImage(byte[] imgBuffer, string uploadpath, ImageFormat imgformat)
        {
            try
            {
                System.IO.MemoryStream m = new MemoryStream(imgBuffer);

                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadpath)))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadpath));

                string imgname = StringHelper.CreateIDCode()+"."+imgformat.ToString().ToLower();

                string _path = HttpContext.Current.Server.MapPath(uploadpath) + imgname;

                Image img = System.Drawing.Image.FromStream(m);
                img.Save(_path, imgformat);
                m.Close();

                return uploadpath + imgname;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="uploadpath">保存路径。绝对或虚拟路径</param>
        /// <param name="imgformat">图片保存格式</param>
        /// <returns>上传成功后返回的新的文件名</returns>
        public static string UploadImage(Stream stream, string uploadpath, ImageFormat imgformat)
        {
            try
            {
                Image img = Image.FromStream(stream);
                string filename = StringHelper.CreateIDCode() + "." + imgformat.ToString().ToLower();
                filename = HttpContext.Current.Server.MapPath(uploadpath) + filename;
                img.Save(filename, imgformat);
                return filename;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="postfile">客户端上传的文件</param>
        /// <param name="uploadpath">保存地址</param>
        /// <param name="imgformat">图片格式</param>
        /// <returns></returns>
        public static string UploadImage(HttpPostedFile postfile, string uploadpath, ImageFormat imgformat)
        {
            switch (imgformat.ToString().ToLower())
            {
                case "jpeg":
                    return UploadImageForJPEG(postfile, uploadpath);
                case "bmp":
                    return UploadImageForBMP(postfile, uploadpath);
                case "png":
                    return UploadImageForPNG(postfile, uploadpath);
                case "gif":
                    return UploadImageForGIF(postfile, uploadpath);
                default:
                    return UploadImageForJPEG(postfile, uploadpath);
            }
        }

        /// <summary>
        /// 上传图片，保存为JPEG格式
        /// </summary>
        /// <param name="postfile">HttpPostedFile</param>
        /// <param name="uploadpath">保存文件地址</param>
        /// <returns>返回上传后的路径</returns>
        public static string UploadImage(HttpPostedFile postfile, string uploadpath,bool autoImageName)
        {
            if(autoImageName){
                switch(Path.GetExtension(postfile.FileName).ToLower())
                {
                    case ".jpg":
                        return UploadImageForJPEG(postfile, uploadpath);
                    case ".gif":
                        return UploadImageForGIF(postfile, uploadpath);
                    case ".png":
                        return UploadImageForPNG(postfile, uploadpath);
                    default:
                        return UploadImageForJPEG(postfile, uploadpath);
                }
            }
            else
            {
                Image img = Image.FromStream(postfile.InputStream);
                ImageHelper.ZoomAuto(postfile, uploadpath, img.Width, img.Height, "", "", null);
                return uploadpath;
            }
        }

        /// <summary>
        /// 自动生成新的图片名称
        /// </summary>
        /// <param name="postfile"></param>
        /// <param name="uploadpath"></param>
        /// <returns></returns>
        public static string UploadImage(HttpPostedFile postfile, string uploadpath)
        {
            return UploadImage(postfile, uploadpath, true);
        }

        


        #region 水印

        #region 上传图片，不缩放，并添加文字水印
        /// <summary>
        /// 上传图片，不缩放，并添加文字水印
        /// </summary>
        /// <param name="postedfile">HTTPPOSTEDFILE</param>
        /// <param name="uploadpath">保存的全路径，包括文件名</param>
        /// <param name="text">水印文字</param>
        /// <param name="waterTextFont">文字水印字体</param>
        public static void UploadImageWithWaterText(HttpPostedFile postedfile, string uploadpath, string text,Font waterTextFont)
        {
            Image img = Image.FromStream(postedfile.InputStream);
            ImageHelper.ZoomAuto(postedfile, uploadpath, img.Width, img.Height, text, "", waterTextFont);
        }

        /// <summary>
        /// 上传图片，不缩放，并添加文字水印
        /// </summary>
        /// <param name="postedfile">HTTPPOSTEDFILE</param>
        /// <param name="uploadpath">保存的全路径，包括文件名</param>
        /// <param name="text">水印文字</param>
        public static void UploadImageWithWaterText(HttpPostedFile postedfile, string uploadpath, string text)
        {
            Image img = Image.FromStream(postedfile.InputStream);
            ImageHelper.ZoomAuto(postedfile, uploadpath, img.Width, img.Height, text, "", null);
        }

        #endregion

        #region 上传图片，不缩放，并添加图片水印
        /// <summary>
        /// 上传图片，不缩放，并添加图片水印
        /// </summary>
        /// <param name="postedfile">源图</param>
        /// <param name="uploadpath">保存的路径，包含上传后的文件名</param>
        /// <param name="waterimg">水印图片的虚拟路径</param>
        public static void UploadImageWithWaterImage(HttpPostedFile postedfile, string uploadpath, string waterimg)
        {
            Image img = Image.FromStream(postedfile.InputStream);
            waterimg = HttpContext.Current.Server.MapPath(waterimg);
            ImageHelper.ZoomAuto(postedfile, uploadpath, img.Width, img.Height, "", waterimg,null);
        }

        #endregion

        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="postfile">源图</param>
        /// <param name="uploadpath">保存路径及文件名</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public static void CutImageAutoZoom(HttpPostedFile postfile, string uploadpath, int width, int height)
        {
            ImageHelper.ZoomAuto(postfile, uploadpath, width, height, "", "",null);
        }


        #endregion


       


        private static byte[] GetPostFileByte(HttpPostedFile postfile)
        {
            int filelength = postfile.ContentLength;
            byte[] buffer = new byte[filelength];
            postfile.InputStream.Read(buffer, 0, filelength);
            return buffer;
        }

        private static string UploadImageForJPEG(HttpPostedFile postfile, string uploadpath)
        {
            byte[] buffer = GetPostFileByte(postfile);
            return UploadImage(buffer, uploadpath, ImageFormat.Jpeg);
        }

        private static string UploadImageForGIF(HttpPostedFile postfile, string uploadpath)
        {
            byte[] buffer = GetPostFileByte(postfile);
            return UploadImage(buffer, uploadpath, ImageFormat.Gif);
        }

        private static string UploadImageForPNG(HttpPostedFile postfile, string uploadpath)
        {
            byte[] buffer = GetPostFileByte(postfile);
            return UploadImage(buffer, uploadpath, ImageFormat.Png);
        }

        private static string UploadImageForBMP(HttpPostedFile postfile, string uploadpath)
        {
            byte[] buffer = GetPostFileByte(postfile);
            return UploadImage(buffer, uploadpath, ImageFormat.Bmp);
        }

        


        #endregion

        #region 上传任何文件

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="postfile">上传的原始文件</param>
        /// <param name="uploadpath">保存地址,如：'/upload/images/aaaa.jpg'</param>
        /// <returns>返回上传后的文件名</returns>
        public static string UploadFile(HttpPostedFile postfile, string uploadpath)
        {
            try
            {
                string savepath = HttpContext.Current.Server.MapPath(uploadpath);
                if (!Directory.Exists(uploadpath))
                    Directory.CreateDirectory(uploadpath);

                string ext = Path.GetExtension(postfile.FileName);
                string filename = StringHelper.CreateIDCode() + ext;
                if (uploadpath.IndexOf(ext) == -1) //判断
                {
                    savepath = savepath + filename;
                }
                postfile.SaveAs(savepath);
                return uploadpath+filename;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        

        #endregion

    }
}
