using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.TradeAPI.TestTool
{
    public enum RequestIndex
    {
        LoginReq = 1,//登陆

        QryAccountInfoReq = 2,//查下账户信息

        QryAssetReq = 3,//查询资产

        QryBalanceReq = 4,//查询余额

        QryMarginRateReq=5,//查询保证金率

        QrySettlement=6,//查询Settlement

        BankToBrokerReq=7,//银行转期货公司

        BrokerToBankReq=8,//期货公司转银行

        ContractBankReq=9,//获取绑定银行信息

        ContractBankListReq = 10,//获取绑定银行列表

        QryTransferReq = 11,//查询转账记录

        SendOrderReq = 12,//下单

        CancelOrderReq = 13,//撤单

        QryHisOrderReq = 14,//查询历史委托

        QryHisTradeReq = 15,//查询历史成交

        QryOrderReq=16,//查询委托

        QryTradeReq=17,//查询成交

        QryPositionReq=18,//查询持仓


        ModifyFundPwdReq=19,//修改资金密码

        ModifyTradePwdReq=20,//修改交易密码

        LogoutReq=21,//登出

    }
    public enum LohinStatus
    {
        Login = 1,
        LogOut = 2,
    }

}
