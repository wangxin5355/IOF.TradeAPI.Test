using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.BizCommon.Data.Entity
{
	/// <summary>
	/// 现货合约数据实体类
	/// </summary>
	public class SpotContractEntity
	{
		public long SpotID { get; set; }

		public string SpotCode { get; set; }

		public string SpotName { get; set; }

		public int VarietyID { get; set; }

		public DateTime AddTime { get; set; }
	}
}
