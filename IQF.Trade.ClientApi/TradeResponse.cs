using Newtonsoft.Json;
using System;

namespace IQF.Trade.ClientApi
{
    [Serializable()]
	public class TradeResponse
	{
		public TradeResponse()
			: this(0, string.Empty)
		{
		}

		public TradeResponse(int errorNo, string errorMsg)
		{
			this.ErrorNo = errorNo;
			this.ErrorMsg = errorMsg;
		}

		/// <summary>
		/// 错误码
		/// 对应 ErrCode
		/// </summary>
        [JsonProperty("error_no")]
		public int ErrorNo { get; set; }

        /// <summary>
        /// 错误信息
        /// 对应 ErrMsg
        /// </summary>
        [JsonProperty("error_info")]
        public string ErrorMsg { get; set; }

		/// <summary>
		/// 响应结果是否错误
		/// </summary>
		public bool IsError()
		{
			return this == null || ErrorNo != 0;
		}
	}
}