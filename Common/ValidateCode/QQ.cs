using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Web;

namespace Xiucai.ValidateCode
{
    public class QqValidateCode
    {
        public QqValidateCode(){}
        public QqValidateCode(int codetype)
        {
            this.CodeType = codetype;
        }

        string codeSerial = "";
        int _codetype = 3;

        protected static string VCODE_SESSION = "__validatecodeimage";

        protected static int VCODE_LENGTH = 4;

        //是否加密验证码
        protected static bool VCODE_IsEncrypt = false;

        protected static bool VCODE_IsIgnore = false;

        //产生图片 宽度：_WIDTH, 高度：_HEIGHT
        private static readonly int _WIDTH = 130, _HEIGHT = 53;
        //字体集
        //private static readonly string[] _FONT_FAMIly = { "Arial", "Arial Black", "Arial Italic", "Courier New", "Courier New Bold Italic", "Courier New Italic", "Courier New Italic", "Courier New Bold Italic" };
        private static readonly string[] _FONT_FAMIly = { "Arial", "Arial Black", "Arial Italic", "Tahoma", "Verdana", "Franklin Gothic Medium", "Impact", "Latha" };
        //字体大小集
        private static readonly int[] _FONT_SIZE = { 20, 25, 30 };
        //前景字体颜色集
        private static readonly Color[] _COLOR_FACE = { Color.Black, Color.Red, Color.DarkBlue,Color.Fuchsia, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple , 
                                                          Color.FromArgb(0, 255, 0),Color.FromArgb(0,0,255), Color.FromArgb(31, 111, 0), Color.FromArgb(0, 255, 255), Color.FromArgb(113, 153, 67),
                                                          Color.FromArgb(30, 99, 140), Color.FromArgb(206, 60, 19), Color.FromArgb(255, 0, 255) };
        
        //背景颜色集
        private static readonly Color[] _COLOR_BACKGROUND = { Color.FromArgb(247, 254, 236), Color.FromArgb(234, 248, 255), Color.FromArgb(244, 250, 246), Color.FromArgb(248, 248, 248) };

        //文本布局信息
        private static StringFormat _DL_FORMAT = new StringFormat(StringFormatFlags.NoClip);
        //左右旋转角度
        private static readonly int _ANGLE = 60;

        public string CodeSerial
        {
            get
            {
                switch (_codetype)
                {
                    case 1:
                        codeSerial = "0,1,2,3,4,5,6,7,8,9";
                        break;
                    case 2:
                        codeSerial = "a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                        break;
                    case 3:
                        codeSerial = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
                        break;
                    default:
                        codeSerial = "0,1,2,3,4,5,6,7,8,9";
                        break;
                }
                return codeSerial;
            }
        }

        public int CodeType
        {
            get { return _codetype; }
            set { _codetype = value; }
        }

        private string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = VCODE_LENGTH;
            }

            string[] arr = CodeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        private string GetCheckCode(HttpContext context)
        {
            string _checkCode = string.Empty;

            _checkCode = CreateVerifyCode(VCODE_LENGTH);

            if (VCODE_IsEncrypt)
                context.Session[VCODE_SESSION] = Common.StringHelper.MD5string(_checkCode, VCODE_IsIgnore);
            else
                context.Session[VCODE_SESSION] = VCODE_IsIgnore ? _checkCode : _checkCode.ToLower();

            return _checkCode;
        }

        public void CreateImage(HttpContext context)
        {
            string code = GetCheckCode(context);
            _DL_FORMAT.Alignment = StringAlignment.Center;
            _DL_FORMAT.LineAlignment = StringAlignment.Center;

            long tick = DateTime.Now.Ticks;
            Random Rnd = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            using (Bitmap _img = new Bitmap(_WIDTH, _HEIGHT))
            {
                using (Graphics g = Graphics.FromImage(_img))
                {
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    Point dot = new Point(20, 20);

                    // 定义一个无干扰线区间和一个起始位置
                    int nor = Rnd.Next(53), rsta = Rnd.Next(130);
                    // 绘制干扰正弦曲线 M:曲线平折度, D:Y轴常量 V:X轴焦距
                    int M = Rnd.Next(15) + 5, D = Rnd.Next(20) + 15, V = Rnd.Next(5) + 1;

                    int ColorIndex = Rnd.Next(15);

                    float Px_x = 0.0F;
                    float Px_y = Convert.ToSingle(M * Math.Sin(V * Px_x * Math.PI / 180) + D);
                    float Py_x, Py_y;

                    //填充背景
                    g.Clear(_COLOR_BACKGROUND[Rnd.Next(4)]);

                    //前景刷子 //背景刷子
                    using (Brush _BrushFace = new SolidBrush(_COLOR_FACE[ColorIndex]))
                    {
                        #region 绘制正弦线
                        for (int i = 0; i < 131; i++)
                        {

                            //初始化y点
                            Py_x = Px_x + 1;
                            Py_y = Convert.ToSingle(M * Math.Sin(V * Py_x * Math.PI / 180) + D);

                            //确定线条颜色
                            if (rsta >= i || i > (rsta + nor))
                                //初始化画笔
                                using (Pen _pen = new Pen(_BrushFace,  2.0F))
                                {
                                    //绘制线条
                                    g.DrawLine(_pen, Px_x, Px_y, Py_x, Py_y);
                                }

                            //交替x,y坐标点
                            Px_x = Py_x;
                            Px_y = Py_y;
                        }
                        #endregion

                        //初始化光标的开始位置
                        g.TranslateTransform(18, 4);

                        #region 绘制校验码字符串
                        for (int i = 0; i < code.Length; i++)
                        {
                            //随机旋转 角度
                            int angle = Rnd.Next(-_ANGLE, _ANGLE);
                            //移动光标到指定位置
                            g.TranslateTransform(dot.X, dot.Y);
                            //旋转
                            g.RotateTransform(angle);

                            //初始化字体
                            using (Font _font = new Font(_FONT_FAMIly[Rnd.Next(0,8)], _FONT_SIZE[Rnd.Next(0, 3)]))
                            {

                                using (Brush fontBrushFace = new SolidBrush(_COLOR_FACE[Rnd.Next(15)]))
                                //绘制
                                    g.DrawString(code[i].ToString(), _font, fontBrushFace, 1, 1, _DL_FORMAT);
                            }
                            //反转
                            g.RotateTransform(-angle);
                            //重新定位光标位置
                            g.TranslateTransform(2, -dot.Y);
                        }
                        #endregion

                    }
                }

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    context.Response.ContentType = "Image/PNG";
                    context.Response.Clear();
                    context.Response.BufferOutput = true;
                    _img.Save(ms, ImageFormat.Png);
                    ms.Flush();
                    context.Response.BinaryWrite(ms.GetBuffer());
                    context.Response.End();
                }
            }

        }
    }
}
