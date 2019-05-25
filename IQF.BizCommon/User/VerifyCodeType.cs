namespace IQF.BizCommon.User
{
	/// <summary>
	/// 验证码类型
	/// </summary>
	public enum VerifyCodeType
	{
		/// <summary>
		/// 登录发送验证码
		/// </summary>
		Login = 1,

		/// <summary>
		/// 重设密码发送验证码
		/// </summary>
		ResetPwd = 2,

		/// <summary>
		/// 补签开户协议 
		/// </summary>
		PatchOpenAcctAgreement = 3,

		/// <summary>
		/// 注册
		/// </summary>
		Register = 4
	}
}
