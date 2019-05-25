using System;

namespace IQF.Trade.Core.AccountArg
{
    public class AccountInfo
	{
		/// <summary>
		/// 期货帐号
		/// </summary>
		public string BrokerAccount { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 证件类型
		/// </summary>
		public IDType IDType { get; set; }

		/// <summary>
		/// 证件号码
		/// </summary>
		public string IDNumber { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { get; set; }

		/// <summary>
		/// 开户日期
		/// </summary>
		public DateTime OpenDate { get; set; }
	}

	public enum IDType
	{
		/// <summary>
		/// 身份证
		/// </summary>
		IDCard = 1,
		/// <summary>
		/// 军官证
		/// </summary>
		OfficerIDCard = 2,
		/// <summary>
		/// 警官证
		/// </summary>
		PoliceIDCard = 3,
		/// <summary>
		/// 士兵证
		/// </summary>
		SoldierIDCard = 4
	}
}
