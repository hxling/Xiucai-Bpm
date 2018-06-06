using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xiucai.Common;
using Xiucai.Common.Data;

namespace Xiucai.Model
{
	[TableName("Demo_RuKuDan")]
	[Description("入库单")]
	public class DemoRuKuDanModel
	{
		/// <summary>
		/// KeyId
		/// </summary>
		[Description("KeyId")]
		public int KeyId { get; set; }
      
		/// <summary>
		/// 入库单编码
		/// </summary>
		[Description("入库单编码")]
		public string rkdCode { get; set; }
      
		/// <summary>
		/// 入库日期
		/// </summary>
		[Description("入库日期")]
		public DateTime rkDate { get; set; }
      
		/// <summary>
		/// 保管员
		/// </summary>
		[Description("保管员")]
		public string baoGuanYuan { get; set; }
      
		/// <summary>
		/// 录入人
		/// </summary>
		[Description("录入人")]
		public string ruluYuan { get; set; }
      
		/// <summary>
		/// 供货商名称
		/// </summary>
		[Description("供货商名称")]
		public string gonghuoShang { get; set; }
      
		/// <summary>
		/// 录入日期
		/// </summary>
		[Description("录入日期")]
		public DateTime ruluDate { get; set; }
      
		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string remark { get; set; }

        [DbField(false)]
        public List<DemoRuKuDanMingXiModel> products { get; set; }


	    public override string ToString()
		{
			return JSONhelper.ToJson(this);
		}
	}
}