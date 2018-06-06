using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Xiucai.ValidateCode
{
    /// <summary>
    /// 自定义字符对象
    /// </summary>
    internal sealed class MyChar2G
    {
        // 字符
        private char m_charObj = '\0';
        // 字体
        private Font m_font;
        // 旧字体大小
        private Size m_oldSize;
        // 前景色
        private Brush m_foreColor;
        // 背景色
        private Brush m_backColor;
        // 旋转角度
        private float m_angle = 0.00f;

        #region 类构造器
        /// <summary>
        /// 类默认构造器
        /// </summary>
        public MyChar2G()
            : this('\0', new Font("system", 9.0f), Brushes.Black, Brushes.White)
        {
        }

        /// <summary>
        /// 类参数构造器
        /// </summary>
        /// <param name="c"></param>
        public MyChar2G(char c)
            : this(c, new Font("system", 9.0f), Brushes.Black, Brushes.White)
        {
        }

        /// <summary>
        /// 类参数构造器
        /// </summary>
        /// <param name="c"></param>
        /// <param name="fontFormat">字体格式</param>
        /// <param name="foreColor">前景色</param>
        /// <param name="backColor">背景色</param>
        public MyChar2G(char c, Font fontFormat, Brush foreColor, Brush backColor)
        {
            this.CharObj = c;
            this.Font = fontFormat;
            this.ForeColor = foreColor;
            this.BackColor = backColor;
        }
        #endregion

        /// <summary>
        /// 获取或设置字符
        /// </summary>
        public char CharObj
        {
            get
            {
                return this.m_charObj;
            }

            set
            {
                this.m_charObj = value;
            }
        }

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public Font Font
        {
            get
            {
                return this.m_font;
            }

            set
            {
                this.m_font = value;
            }
        }

        /// <summary>
        /// 获取或设置旧字体大小
        /// </summary>
        public Size OldSize
        {
            get
            {
                return this.m_oldSize;
            }

            set
            {
                this.m_oldSize = value;
            }
        }

        /// <summary>
        /// 获取或设置前景色
        /// </summary>
        public Brush ForeColor
        {
            get
            {
                return this.m_foreColor;
            }

            set
            {
                this.m_foreColor = value;
            }
        }

        /// <summary>
        /// 获取或设置背景色
        /// </summary>
        public Brush BackColor
        {
            get
            {
                return this.m_backColor;
            }

            set
            {
                this.m_backColor = value;
            }
        }

        /// <summary>
        /// 旋转角度
        /// </summary>
        public float Angle
        {
            set
            {
                this.m_angle = value;
            }

            get
            {
                return this.m_angle;
            }
        }

        /// <summary>
        /// 计算新字体大小
        /// </summary>
        /// <returns></returns>
        public Size ComputeNewSize()
        {
            // 将角度转换为 PI 角
            float pi_angle = Convert.ToSingle(this.Angle * Math.PI / 180.00f);

            // 声明新宽度, 新高度
            int newWidth = 0, newHeight = 0;

            // 计算新宽度, 新宽度的计算公式是 oldH * sin(r) + oldW * cos(r)
            newWidth += Convert.ToInt32(Math.Abs(this.OldSize.Height * Math.Sin(pi_angle)));
            newWidth += Convert.ToInt32(Math.Abs(this.OldSize.Width * Math.Cos(pi_angle)));

            // 计算新高度, 新高度的计算公式是 oldH * cos(r) + oldW * sin(r)
            newHeight += Convert.ToInt32(Math.Abs(this.OldSize.Height * Math.Cos(pi_angle)));
            newHeight += Convert.ToInt32(Math.Abs(this.OldSize.Width * Math.Sin(pi_angle)));

            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// 计算字符旋转后 x 轴左移量
        /// </summary>
        /// <returns></returns>
        public int ComputeLdx()
        {
            if (this.Angle <= 0)
                return 0;

            // 将角度转换为 PI 角
            float pi_angle = Convert.ToSingle(this.Angle * Math.PI / 180.00f);

            // 返回 x 轴左移量
            return Convert.ToInt32(Math.Abs(this.OldSize.Height * Math.Sin(pi_angle)));
        }

        /// <summary>
        /// 计算字符旋转后 y 轴上移量
        /// </summary>
        /// <returns></returns>
        public int ComputeTdy()
        {
            if (this.Angle >= 0)
                return 0;

            // 将角度转换为 PI 角
            float pi_angle = Convert.ToSingle(this.Angle * Math.PI / 180.00f);

            // 返回 x 轴左移量
            return Convert.ToInt32(Math.Abs(this.OldSize.Width * Math.Sin(pi_angle)));
        }
    }
}
