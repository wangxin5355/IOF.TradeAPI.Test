using System;

namespace IQF.Trade.Core.AccountArg
{
    /// <summary>
    /// 修改密码实体类
    /// </summary>
    [Serializable]
	public class ModifyPasswordArg
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }
	}
}
