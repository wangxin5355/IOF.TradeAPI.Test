namespace IQF.Trade.ClientApi.Account
{
    [TradeApiInfo("/api/account/modifyfundpwd")]
    public class ModifyFundPwdReq : TradeRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
