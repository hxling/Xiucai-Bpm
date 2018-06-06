using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xiucai.Common;
using Xiucai.Common.Data;

namespace Xiucai.Model
{
	[TableName("Demo_RuKuDanMingXi")]
	[Description("入库单明细")]
	public class DemoRuKuDanMingXiModel
	{
		/// <summary>
		/// KeyID
		/// </summary>
		[Description("KeyId")]
		public int KeyId { get; set; }
      
		/// <summary>
		/// productName
		/// </summary>
		[Description("productName")]
		public string productName { get; set; }
      
		/// <summary>
		/// price
		/// </summary>
		[Description("price")]
		public int price { get; set; }
      
		/// <summary>
		/// state
		/// </summary>
		[Description("state")]
		public bool state { get; set; }
      
		/// <summary>
		/// category
		/// </summary>
		[Description("category")]
		public int category { get; set; }
      
		/// <summary>
		/// inventory
		/// </summary>
		[Description("inventory")]
		public int inventory { get; set; }
      
		/// <summary>
		/// rkdId
		/// </summary>
		[Description("rkdId")]
		public int rkdId { get; set; }
      
				
		public override string ToString()
		{
			return JSONhelper.ToJson(this);
		}
	}
}