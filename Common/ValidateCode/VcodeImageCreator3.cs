using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Xiucai.ValidateCode
{
    internal class VcodeImageCreator3
    {
        //数学运算验证码
        #region 输出数学运算验证码表达式到浏览器


        /// <summary>
        /// 输出数学运算验证码表达式到浏览器
        /// </summary>
        /// <param name="context">httpcontext</param>
        /// <param name="sessionKey">保存运算值的SESSION的KEY</param>
        public void OutputImage(System.Web.HttpContext context, string sessionKey)
        {
            int mathResult = 0;
            string expression = null;

            Random rnd = new Random();

            ////生成3个10以内的整数，用来运算
            int operator1 = rnd.Next(0, 10);
            int operator2 = rnd.Next(0, 10);
            int operator3 = rnd.Next(0, 10);

            ////随机组合运算顺序，只做 + 和 * 运算
            switch (rnd.Next(0, 3))
            {
                case 0:
                    mathResult = operator1 + operator2 * operator3;
                    expression = string.Format("{0} + {1} × {2} = ?", operator1, operator2, operator3);
                    break;
                case 1:
                    mathResult = operator1 * operator2 + operator3;
                    expression = string.Format("{0} × {1} + {2} = ?", operator1, operator2, operator3);
                    break;
                default:
                    mathResult = operator2 + operator1 * operator3;
                    expression = string.Format("{0} + {1} × {2} = ?", operator2, operator1, operator3);
                    break;
            }

            using (Bitmap bmp = new Bitmap(150, 25))
            {
                using (Graphics graph = Graphics.FromImage(bmp))
                {
                    graph.Clear(Color.FromArgb(232, 238, 247)); ////背景色，可自行设置

                    ////画噪点
                    for (int i = 0; i <= 128; i++)
                    {
                        graph.DrawRectangle(
                            new Pen(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255))),
                            rnd.Next(2, 128),
                            rnd.Next(2, 38),
                            0.5f,
                            0.5f);
                    }

                    ////输出表达式
                    for (int i = 0; i < expression.Length; i++)
                    {
                        graph.DrawString(expression.Substring(i, 1),
                            new Font("Verdana", 12, FontStyle.Bold),
                            new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(128), rnd.Next(255))),
                            5 + i * 10,
                            rnd.Next(1, 5));
                    }

                    ////画边框，不需要可以注释掉
                    graph.DrawRectangle(new Pen(Color.Firebrick), 0, 0, 150 - 1, 25 - 1);
                }

                context.Session[sessionKey] = mathResult; ////将运算结果存入session

                ////禁用缓存
                DisableHttpCache(context);

                ////输出图片到浏览器，我采用的是 gif 格式，可自行设置其他格式
                context.Response.ContentType = "image/png";
                bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                context.Response.End();
            }
        }

        #endregion

        
        /// <summary>
        /// 禁用缓存
        /// </summary>
        /// <param name="context">httpcontext</param>
        private static void DisableHttpCache(System.Web.HttpContext context)
        {
            ////清除http缓存
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            ////禁用http缓存
            //// http 1.1
            context.Response.AddHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT");
            context.Response.AddHeader("Cache-Control", "no-store, no-cache, max-age=0, must-revalidate");
            //// http 1.0
            context.Response.AddHeader("Pragma", "no-cache");
        }

    }
}
