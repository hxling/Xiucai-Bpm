using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Xiucai.ValidateCode
{
    internal class VcodeImageCreator
    {
        // 随机对象
		private static readonly Random g_random = new Random();

        //前景字体颜色集
        private static readonly Color[] _COLOR_FACE = { Color.Black, Color.Red, Color.DarkBlue,Color.Fuchsia, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        //背景颜色集
        private static readonly Color[] _COLOR_BACKGROUND = { Color.FromArgb(247, 254, 236), 
                                                                Color.FromArgb(234, 248, 255),
                                                                Color.FromArgb(244, 250, 246), 
                                                                Color.FromArgb(248, 248, 248),
                                                                Color.FromArgb(245,248,252),
                                                                Color.FromArgb(240,251,235),
                                                                Color.FromArgb(247,247,255),
                                                                Color.FromArgb(255,245,250),
                                                            Color.FromArgb(255,249,237),
                                                            Color.FromArgb(240,251,235)};

		// 最小字符尺寸
		private int m_charMinSize = -1;
		// 最大字符尺寸
		private int m_charMaxSize = -1;

		#region 类构造器
		/// <summary>
		/// 类默认构造器
		/// </summary>
		public VcodeImageCreator()
		{
		}
		#endregion

		/// <summary>
		/// 设置或获取最小字符尺寸
		/// </summary>
		public int CharMinSize
		{
			set
			{
				this.m_charMinSize = value;
			}

			get
			{
				return this.m_charMinSize;
			}
		}

		/// <summary>
		/// 设置或获取最大字符尺寸
		/// </summary>
		public int CharMaxSize
		{
			set
			{
				this.m_charMaxSize = value;
			}

			get
			{
				return this.m_charMaxSize;
			}
		}

		/// <summary>
        /// 创建验证码图片
        /// </summary>
		/// <param name="width">图片宽度</param>
		/// <param name="height">图片高度</param>
		/// <param name="validationCode">验证码</param>
        public Image CreateImage(int width, int height, string validationCode)
        {
			// 生成 BITMAP 图像
			Bitmap bitmapImg = new Bitmap(width, height);

			// 获取绘制器对象
			Graphics g = Graphics.FromImage(bitmapImg);

			// 清除图片
		    //g.Clear(Color.White);
            g.Clear(_COLOR_BACKGROUND[new Random().Next(9)]); ////背景色，可自行设置
			// 获得字符数组
			char[] charList = validationCode.ToCharArray();
			// 字符索引
			int i = 0;

			// 坐标位置
			int startX = 0, startY = 0;
			// 上一字符大小
			Size lastCharSize = new Size(0, 0);

			// 贝塞尔曲线范围
			Rectangle beziersRect = new Rectangle();



			while (i < charList.Length)
			{
				// 创建自定义字符对象
				MyChar2G charObj = new MyChar2G(charList[i]);
				// 字体
				Font charFont = this.NextFont();
				// 字符大小
				SizeF charSize = g.MeasureString(Convert.ToString(charList[i]), charFont);

				// 旋转方向
				int rotateDirection = (g_random.Next(0, 256) >= 128) ? +1 : -1;
				// 旋转角度, 最大为 30 度
				float angle = Convert.ToSingle(g_random.NextDouble() * 60.00f) * rotateDirection;

				// 设置字体
				charObj.Font = charFont;
				// 设置字符大小
				charObj.OldSize = new Size((int)charSize.Width, (int)charSize.Height);
				// 设置角度
				charObj.Angle = angle;
				// 设置前景色
                charObj.ForeColor = new SolidBrush(_COLOR_FACE[i]);
				// 设置背景色
				charObj.BackColor = Brushes.White;

                

				if (i == 0)
				{
					// 坐标起始位置
					startX = g_random.Next(0, width / (charList.Length * 4));
					startY = g_random.Next(0, height / 4);

					beziersRect.X = startX;
					beziersRect.Y = startY;
				}
				else
				{
					startX += lastCharSize.Width * 4 / 7;
				}

				// 保存当前状态
				GraphicsState gs = g.Save();

				// 高质量
				g.SmoothingMode = SmoothingMode.HighQuality;

				// 平移绘制器
				g.TranslateTransform(
					startX + charObj.ComputeLdx(), 
					startY + charObj.ComputeTdy());

				// 旋转角度
				g.RotateTransform(angle);



				// 绘制字符
				g.DrawString(Convert.ToString(charObj.CharObj), charObj.Font, charObj.ForeColor, 0, 0);

				// 还原绘制工具
				g.Restore(gs);

				// 计算新字符大小
				lastCharSize = charObj.ComputeNewSize();

				i++;
			}

			// 设置贝塞尔曲线范围的宽度, 高度
			beziersRect.Width = startX + lastCharSize.Width;
			beziersRect.Height = lastCharSize.Height;

			// 绘制贝塞尔曲线
			this.DrawBeziers(g, beziersRect);

			return bitmapImg;
        }

		/// <summary>
		/// 绘制贝塞尔曲线
		/// </summary>
		/// <param name="g"></param>
		/// <param name="rect">绘制范围</param>
		private void DrawBeziers(Graphics g, Rectangle rect)
		{
			// 贝塞尔曲线坐标
			List<Point> bezierPointList = new List<Point>();

			// x 轴变化量
			int dx = rect.Width / 4;

			for (int i = 0; i < 4; i++)
			{
				Point newP = new Point();

				// x 位置
				newP.X = rect.Left + dx * (i + 1);
				// y 位置
				newP.Y = g_random.Next(0, rect.Height);

				// 添加新坐标到集合
				bezierPointList.Add(newP);
			}

            //g.Clear(Color.FromArgb(232, 238, 247)); ////背景色，可自行设置

            ////画噪点
            for (int i = 0; i <= rect.Width; i++)
            {
                g.DrawRectangle(
                    new Pen(Color.FromArgb(g_random.Next(0, 255), g_random.Next(0, 255), g_random.Next(0, 255))),
                    g_random.Next(2, rect.Width),
                    g_random.Next(2, rect.Height),
                    0.5f,
                    0.5f);
            }


			// 绘制贝塞尔曲线
			g.DrawBeziers(new Pen(Brushes.Black, 2), bezierPointList.ToArray());
		}

		/// <summary>
		/// 随机生成字体
		/// </summary>
		/// <returns></returns>
		public Font NextFont()
		{
			return new Font("Courier New", g_random.Next(this.CharMinSize, this.CharMaxSize), FontStyle.Bold);
        }

    }
}
