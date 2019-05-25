namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/modifytradepwd")]
    public class ModifyTradePwdReq : TradeRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
