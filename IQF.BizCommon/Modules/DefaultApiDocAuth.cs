using IQF.BizCommon.User;
using IQF.Framework.IModules;

namespace IQF.BizCommon.Modules
{
	public class DefaultApiDocAuth : IApiDocAuth
	{
		public bool HasViewAuth(string userName, string password, string url)
		{
			if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
			{
				return false;
			}
			if (!InnerEmployeeMgr.IsInnerEmployee(userName))
			{
				return false;
			}
			var enMobile = UserInfoMgr.EncryptMobile(userName);
			var enPwd = UserInfoMgr.EncryptPassword(password);
			var ret = UserInfoMgr.CheckPwd(enMobile, enPwd);
			return ret;
		}
	}
}
