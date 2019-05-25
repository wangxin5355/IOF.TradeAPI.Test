using Dapper;
using IQF.Framework.Encrypt;
using System;
using System.Linq;

namespace IQF.BizCommon.User
{
	public static class UserInfoMgr
    {
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password)
        {
            var value = "IFQ" + password;
            return Cryptogram.GetMD5(value);
        }

        /// <summary>
        /// 加密手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string EncryptMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return string.Empty;
            }
            return Cryptogram.EncryptPassword(mobile);
        }

        /// <summary>
        /// 解密手机号
        /// </summary>
        /// <param name="enMobile">加密的手机号</param>
        /// <returns></returns>
        public static string DecryptMobile(string enMobile)
        {
            if (string.IsNullOrWhiteSpace(enMobile))
            {
                return string.Empty;
            }
            return Cryptogram.DecryptPassword(enMobile);
        }

		/// <summary>
		/// 通过手机号获取用户信息
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static UserModel GetByMobile(string mobile)
		{
			var userID = GetUserID(mobile);
			if (userID <= 0)
			{
				return null;
			}
			return GetBy(userID);
		}

		/// <summary>
		/// 是否存在这个手机号
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static bool Exist(string mobile)
		{
			var userID = GetUserID(mobile);
			return userID > 0;
		}

		/// <summary>
		/// 通过手机号获取用户编号
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static long GetUserID(string mobile)
		{
			if (string.IsNullOrWhiteSpace(mobile))
			{
				return -1;
			}
			var key = GetMobileKey(mobile);
			var userID = RedisManager.UserRedis.Get<long>(key);
			if (userID > 0)
			{
				return userID;
			}
			var enMobile = UserInfoMgr.EncryptMobile(mobile);
			var entity = GetUserByMobile(enMobile);
			if (entity == null)
			{
				return -1;
			}
			RedisManager.UserRedis.Set(key, entity.UserID, DateTime.Now.AddHours(24 * 7));
			return entity.UserID;
		}


		/// <summary>
		/// 通过手机号获取用户信息
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static UserModel GetBy(long userID)
		{
			if (userID <= 0)
			{
				return null;
			}
			var key = GetRedisKey(userID);
			var cache = RedisManager.UserRedis.Get<UserModel>(key);
			if (cache != null)
			{
				return cache;
			}
			var model = GetByID(userID);
			if (model == null)
			{
				return null;
			}
			RedisManager.UserRedis.Set(key, model, DateTime.Now.AddHours(4));
			return model;
		}

		/// <summary>
		/// 清除缓存
		/// </summary>
		/// <param name="userID"></param>
		public static void ClearCache(long userID)
		{
			if (userID <= 0)
			{
				return;
			}
			var key = GetRedisKey(userID);
			RedisManager.UserRedis.Remove(key);
		}

		/// <summary>
		/// 从数据库获取
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		private static UserModel GetByID(long userID)
		{
			var sql = @"select * from [DB_IQFUser].[dbo].[User] t where t.UserID = @userID;";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFUser))
			{
				var model = conn.Query<UserModel>(sql, new { userID = userID }).FirstOrDefault();
				if (model == null)
				{
					return null;
				}
				model.Mobile = DecryptMobile(model.Mobile);
				return model;
			}
		}

		private static UserModel GetUserByMobile(string mobile)
		{
			var sql = @"select * from [DB_IQFUser].[dbo].[User] t where t.Mobile = @mobile";
			using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFUser))
			{
				var all = conn.Query<UserModel>(sql, new { mobile = mobile }).FirstOrDefault();
				return all;
			}
		}
		/// <summary>
		/// 获取REIDS KEY
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		private static string GetRedisKey(long userID)
		{
			return "IQFUserInfo:" + userID;
		}

		private static string GetMobileKey(string mobile)
		{
			return "IQFUser:Mobile2Id" + mobile;
		}

		internal static bool CheckPwd(string mobile, string pwd)
        {
            var sql = @"select count(*) from [DB_IQFUser].[dbo].[User] t where t.Mobile = @mobile and t.Password = @pwd;";
            using (var conn = ConnectionString.Create(Framework.Dao.DatabaseName.DB_IQFUser))
            {
                var count = conn.Query<int>(sql, new { mobile = mobile, pwd = pwd }).FirstOrDefault();
                return count > 0;
            }
        }
    }

    /// <summary>
    /// 用户实体类
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 解密后的手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 包类型
        /// </summary>
        public int Packtype { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 图像链接
        /// </summary>
        public string HeadPicUrl { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
    }
}
