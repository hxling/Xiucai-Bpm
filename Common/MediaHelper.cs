using System;
using System.Collections.Generic;
using System.Text;

namespace Xiucai.Common
{
    public class MediaHelper
    {
        private MediaHelper() {}
        private static MediaHelper media = null;
        public static MediaHelper Current
        {
            get
            {
                if (media == null)
                    media = new MediaHelper();

                return media;
            }
        }
        public  string Play(string url, string width, string height)
        {
            string strTmp = url.ToLower();
            if (strTmp.EndsWith(".wmv") || strTmp.EndsWith(".asf") || strTmp.EndsWith(".avi"))
            {
                return wmv(url, width, height);
            }
            else if (strTmp.EndsWith(".wma"))
            {
                return wma(url, width, height);
            }
            else if (strTmp.EndsWith(".avi"))
            {
                return avi(url, width, height);
            }
            else if (strTmp.EndsWith(".mpg"))
            {
                return mpg(url, width, height);
            }
            else if (strTmp.EndsWith(".mp3"))
            {
                return mp3(url, width, height);
            }
            else if (strTmp.EndsWith(".swf"))
            {
                return swf(url, width, height);
            }
            else if (strTmp.EndsWith(".jpg") || strTmp.EndsWith(".gif"))
            {
                return img(url, width, height);
            }
            else if (strTmp.EndsWith(".rm"))
            {
                return rm(url, width, height);
            }

            else
            {
                return "数据错误";
            }
        }


        ///   <summary>    
        ///   wmv格式文件播放    
        ///   </summary>    
        ///   <param   name="url"></param>    
        ///   <returns></returns>    
        private  string wmv(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<object   id=\"WMPlay\"   style=\"WIDTH:   " + width + "px;height:" + height + "px\"   \n");
            sb.Append("classid=\"CLSID:6BF52A52-394A-11D3-B153-00C04F79FAA6\"   type=application/x-oleobject   standby=\"Loading   Windows   Media   Player   components...\"");
            sb.Append("codebase=\"downloads/mediaplayer10.0_cn.exe\"   VIEWASTEXT>\n");
            sb.Append("<param   name=\"URL\"   value='" + url + "'>\n");
            sb.Append("<param   name=\"controls\"   value=\"ControlPanel,StatusBa\">");
            //<PARAM   NAME="CONTROLS"   VALUE="ControlPanel,StatusBar">    
            sb.Append("<param   name=\"hidden\"   value=\"1\">");
            sb.Append("<param   name=\"ShowControls\"   VALUE=\"0\">");
            sb.Append("<param   name=\"rate\"   value=\"1\">\n");
            sb.Append("<param   name=\"balance\"   value=\"0\">\n");
            sb.Append("<param   name=\"currentPosition\"   value=\"-1\">\n");
            sb.Append("<param   name=\"defaultFrame\"   value=\"\">\n");
            sb.Append("<param   name=\"playCount\"   value=\"100\">\n");
            sb.Append("<param   name=\"autoStart\"   value=\"-1\">\n");
            sb.Append("<param   name=\"currentMarker\"   value=\"0\">\n");
            sb.Append("<param   name=\"invokeURLs\"   value=\"-1\">\n");
            sb.Append("<param   name=\"baseURL\"   value=\"\">\n");
            sb.Append("<param   name=\"volume\"   value=\"85\">\n");
            sb.Append("<param   name=\"mute\"   value=\"0\">\n");
            sb.Append("<param   name=\"uiMode\"   value=\"mini\">\n");
            sb.Append("<param   name=\"stretchToFit\"   value=\"0\">\n");
            sb.Append("<param   name=\"windowlessVideo\"   value=\"0\">\n");
            sb.Append("<param   name=\"enabled\"   value=\"-1\">\n");
            sb.Append("<param   name=\"enableContextMenu\"   value=\"false\">\n");
            sb.Append("<param   name=\"fullScreen\"   value=\"0\">\n");
            sb.Append("<param   name=\"SAMIStyle\"   value=\"\">\n");
            sb.Append("<param   name=\"SAMILang\"   value=\"\">\n");
            sb.Append("<param   name=\"SAMIFilename\"   value=\"\">\n");
            sb.Append("<param   name=\"captioningID\"   value=\"\">\n");


            sb.Append("</object><br>\n");

            return sb.ToString();
        }

        /// <summary>
        /// 播放wma格式文件
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        private  string wma(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<object   id=\"WMPlay\"   classid=\"clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95\"   style=\"Z-INDEX:   101;   LEFT:   40px;   WIDTH:   240px;   POSITION:   absolute;   TOP:   32px;   HEIGHT:   248px\"   >");
            sb.Append("<param   name=\"Filename\"   value=\"" + url + "\">");
            sb.Append("<param   name=\"PlayCount\"   value=\"1\">");
            sb.Append("<param   name=\"AutoStart\"   value=\"0\">");
            sb.Append("<param   name=\"ClickToPlay\"   value=\"1\">");
            //sb.Append("<param   name=\"DisplaySize\"   value=\"0\">");    
            sb.Append("<param   name=\"EnableFullScreen   Controls\"   value=\"1\">");
            sb.Append("<param   name=\"ShowAudio   Controls\"   value=\"1\">");
            sb.Append("<param   name=\"EnableContext   Menu\"   value=\"1\">");
            sb.Append("<param   name=\"ShowDisplay\"   value=\"1\">");

            sb.Append("</object>");

            return sb.ToString();

        }
        ///   <summary>    
        ///   avi格式文件播放    
        ///   </summary>    
        ///   <param   name="url"></param>    
        ///   <returns></returns>    
        private  string avi(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<object   id=\"WMPlay\"   width=\"400\"   height=\"200\"   border=\"0\"   classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\">");
            sb.Append("<param   name=\"ShowDisplay\"   value=\"0\">");
            sb.Append("<param   name=\"ShowControls\"   value=\"1\">");
            sb.Append("<param   name=\"AutoStart\"   value=\"1\">");
            sb.Append("<param   name=\"AutoRewind\"   value=\"0\">");
            sb.Append("<param   name=\"PlayCount\"   value=\"0\">");
            sb.Append("<param   name=\"Appearance   value=\"0   value=\"\"\">");
            sb.Append("<param   name=\"BorderStyle   value=\"0   value=\"\"\">");
            sb.Append("<param   name=\"MovieWindowHeight\"   value=\"240\">");
            sb.Append("<param   name=\"MovieWindowWidth\"   value=\"320\">");
            sb.Append("<param   name=\"FileName\"   value=\"" + url + "\">");
            sb.Append("</object>");

            return sb.ToString();
        }

        /// <summary>
        /// 播放mpg格式的文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private  string mpg(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<object   classid=\"clsid:05589FA1-C356-11CE-BF01-00AA0055595A\"   id=\"WMPlay\"   width=\"239\"   height=\"250\">");
            sb.Append("<param   name=\"Filename\"   value=\"" + url + "\"   valuetype=\"ref\">");
            sb.Append("<param   name=\"Appearance\"   value=\"0\">");
            sb.Append("<param   name=\"AutoStart\"   value=\"-1\">");
            sb.Append("<param   name=\"AllowChangeDisplayMode\"   value=\"-1\">");
            sb.Append("<param   name=\"AllowHideDisplay\"   value=\"0\">");
            sb.Append("<param   name=\"AllowHideControls\"   value=\"-1\">");
            sb.Append("<param   name=\"AutoRewind\"   value=\"-1\">");
            sb.Append("<param   name=\"Balance\"   value=\"0\">");
            sb.Append("<param   name=\"CurrentPosition\"   value=\"0\">");
            sb.Append("<param   name=\"DisplayBackColor\"   value=\"0\">");
            sb.Append("<param   name=\"DisplayForeColor\"   value=\"16777215\">");
            sb.Append("<param   name=\"DisplayMode\"   value=\"0\">");
            sb.Append("<param   name=\"Enabled\"   value=\"-1\">");
            sb.Append("<param   name=\"EnableContextMenu\"   value=\"-1\">");
            sb.Append("<param   name=\"EnablePositionControls\"   value=\"-1\">");
            sb.Append("<param   name=\"EnableSelectionControls\"   value=\"0\">");
            sb.Append("<param   name=\"EnableTracker\"   value=\"-1\">");

            sb.Append("<param   name=\"FullScreenMode\"   value=\"0\">");
            sb.Append("<param   name=\"MovieWindowSize\"   value=\"0\">");
            sb.Append("<param   name=\"PlayCount\"   value=\"1\">");
            sb.Append("<param   name=\"Rate\"   value=\"1\">");
            sb.Append("<param   name=\"SelectionStart\"   value=\"-1\">");
            sb.Append("<param   name=\"SelectionEnd\"   value=\"-1\">");
            sb.Append("<param   name=\"ShowControls\"   value=\"-1\">");
            sb.Append("<param   name=\"ShowDisplay\"   value=\"-1\">");
            sb.Append("<param   name=\"ShowPositionControls\"   value=\"0\">");
            sb.Append("<param   name=\"ShowTracker\"   value=\"-1\">");
            sb.Append("<param   name=\"Volume\"   value=\"-480\">");
            sb.Append("</object>");

            return sb.ToString();
        }
        /// <summary>
        /// 播放rm格式的文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private  string rm(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<OBJECT   ID=\"WMPlay\"   codebase=\"downloads/RealPlayer10-5GOLD_cn0302.exe\"   CLASSID=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\"   HEIGHT=" + height + "   WIDTH=" + width + ">");
            sb.Append("<param   name=\"SRC\"   value=\"" + url + "\">");
            //sb.Append("<param   name=\"_ExtentX\"   value=\"9313\">");    
            //sb.Append("<param   name=\"_ExtentY\"   value=\"7620\">");    
            sb.Append("<param   name=\"AUTOSTART\"   value=\"0\">");
            sb.Append("<param   name=\"SHUFFLE\"   value=\"0\">");
            sb.Append("<param   name=\"PREFETCH\"   value=\"0\">");
            sb.Append("<param   name=\"NOLABELS\"   value=\"0\">");
            sb.Append("<param   name=\"CONTROLS\"   value=\"ImageWindow,ControlPanel,statusbar\">");
            sb.Append("<param   name=\"CONSOLE\"   value=\"Clip1\">");
            sb.Append("<param   name=\"LOOP\"   value=\"0\">");
            sb.Append("<param   name=\"NUMLOOP\"   value=\"0\">");
            sb.Append("<param   name=\"CENTER\"   value=\"0\">");
            sb.Append("<param   name=\"MAINTAINASPECT\"   value=\"0\">");
            sb.Append("<param   name=\"BACKGROUNDCOLOR\"   value=\"#000000\">");
            //sb.Append("<embed   SRC   type=\"audio/x-pn-realaudio-plugin\"   CONSOLE=\"Clip1\"   CONTROLS=\"ImageWindow\"   HEIGHT=\"250\"   WIDTH=\"354\"   AUTOSTART=\"false\">");    
            sb.Append("</OBJECT>");

            return sb.ToString();
        }

        /// <summary>
        /// 播放swf格式的文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private  string swf(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (width == "")
                width = "100%";
            if (height == "")
                height = "300";

            sb.Append("<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" width=\""+width+"\" height=\""+height+"\" id=\"movie_name\" align=\"middle\">");
            sb.Append("    <param name=\"movie\" value=\""+url+"\"/>");
            sb.Append("    <!--[if !IE]>-->");
            sb.Append("    <object type=\"application/x-shockwave-flash\" data=\"" + url + "\"  width=\"" + width + "\" height=\"" + height + "\">");
            sb.Append("       <param name=\"movie\" value=\"movie_name.swf\"/>");
            sb.Append("        <!--<![endif]-->");
            sb.Append("        <a href=\"http://www.adobe.com/go/getflash\">");
            sb.Append("            <img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash player\"/>");
            sb.Append("        </a>");
            sb.Append("        <!--[if !IE]>-->");
            sb.Append("    </object>");
            sb.Append("    <!--<![endif]-->");
            sb.Append("</object>");
            return sb.ToString();
        }
        /// <summary>
        /// 播放mp3
        /// </summary>
        /// <param name="url"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private  string mp3(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<object   classid=\"WMPlay\"   codebase=\"http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,4,5,715\"   type=\"application/x-oleobject\"   width=\"   +   width   +   \"   height=\"   +   height   +   \"   hspace=\"0\"   standby=\"Loading   Microsoft   Windows   Media   Player   components...\"   id=\"NSPlay\">");

            sb.Append("<param   name=\"AutoRewind\"   value=\"0\">");
            sb.Append("<param   name=\"FileName\"   value=\"   +   url   +   \">");
            sb.Append("<param   name=\"ShowControls\"   value=\"1\">");
            sb.Append("<param   name=\"ShowPositionControls\"   value=\"0\">");
            sb.Append("<param   name=\"ShowAudioControls\"   value=\"1\">");
            sb.Append("<param   name=\"ShowTracker\"   value=\"0\">");
            sb.Append("<param   name=\"ShowDisplay\"   value=\"0\">");
            sb.Append("<param   name=\"ShowStatusBar\"   value=\"1\">");
            sb.Append("<param   name=\"ShowGotoBar\"   value=\"0\">");
            sb.Append("<param   name=\"ShowCaptioning\"   value=\"0\">");
            sb.Append("<param   name=\"AutoStart\"   value=\"1\">");
            sb.Append("<param   name=\"Volume\"   value=\"-2500\">");
            sb.Append("<param   name=\"AnimationAtStart\"   value=\"0\">");
            sb.Append("<param   name=\"TransparentAtStart\"   value=\"0\">");
            sb.Append("<param   name=\"AllowChangeDisplaySize\"   value=\"0\">");
            sb.Append("<param   name=\"AllowScan\"   value=\"0\">");
            sb.Append("<param   name=\"EnableContextMenu\"   value=\"0\">");
            sb.Append("<param   name=\"ClickToPlay\"   value=\"0\">");

            sb.Append("</object>");
            return sb.ToString();

        }

        private  string img(string url, string width, string height)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<img   src=\"" + url + "\"   height=\"" + height + "\"   width=\"" + width + "\"   border=\"0\">");
            return sb.ToString();
        }

        public  string FLV(string url, string flvPlayerpath, string width, string height)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"");
            sb.Append(" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0\"");
            sb.Append(" width=\"" + width + "\"");
            sb.Append(" height=\"" + height + "\">");
            sb.Append("\r\n\t<param name=\"movie\" value=\"" + flvPlayerpath + "\" />");
            sb.Append("\r\n\t<param name=\"quality\" value=\"high\" />");
            sb.Append("\r\n\t<param name=\"allowFullScreen\" value=\"true\" />");
            sb.Append("\r\n\t<param name=\"FlashVars\" value=\"vcastr_file=" + url + "\" />");
            sb.Append("\r\n\t<embed src=\"" + flvPlayerpath + "\" allowfullscreen=\"true\" flashvars=\"vcastr_file=" + url + "\"");
            sb.Append(" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"" + width + "\" height=\"" + height + "\"></embed>");
            sb.Append("</object>");
            return sb.ToString();
        }
    }

}
