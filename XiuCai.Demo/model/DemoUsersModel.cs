using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xiucai.Common;
using Xiucai.Common.Data;

namespace Xiucai.Demo.Model
{
	[TableName("Demo_Users")]
	[Description("演示数据")]
	public class DemoUsersModel
	{
		/// <summary>
		/// KeyId
		/// </summary>
		[Description("KeyId")]
		public int KeyId { get; set; }
      
		/// <summary>
		/// 姓名
		/// </summary>
		[Description("姓名")]
		public string Name { get; set; }
      
		/// <summary>
		/// 企业
		/// </summary>
		[Description("企业")]
		public string Company { get; set; }
      
		/// <summary>
		/// 所有者
		/// </summary>
		[Description("所有者")]
		public int Ownner { get; set; }
      
		/// <summary>
		/// 部门ID
		/// </summary>
		[Description("部门ID")]
		public int DepID { get; set; }
      
				
		public override string ToString()
		{
			return JSONhelper.ToJson(this);
		}
	}
}