using IQF.Framework;
using IQF.Trade.Core.AccountArg;
using IQF.Trade.Core.BankArg;
using IQF.Trade.Core.OrderArg;
using IQF.Trade.Core.PositionArg;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IQF.Trade.Core
{
    public interface ITradeExAdapter : IDisposable
    {
        AdapterAccountCfg AdapterAccountCfg { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        ResultInfo Init();

        /// <summary>
        /// 错误事件
        /// </summary>
        event Action<int, string> OnError;

        /// <summary>
        /// 查询当日委托
        /// </summary>
        /// <returns></returns>
        ResultInfo<List<OrderEx>> GetOrders();

        /// <summary>
        /// 查询当前持仓
        /// </summary>
        /// <returns></returns>
        ResultInfo<List<PositionEx>> GetPositions();

        /// <summary>
        /// 查询资产信息
        /// </summary>
        /// <returns></returns>
        ResultInfo<AssetInfoEx> GetAssetInfo();

        /// <summary>
        /// 下单
        /// 下单成功后返回ordeID
        /// </summary>
        /// <returns></returns>
        ResultInfo<string> SendOrder(SendOrderArg arg);

        /// <summary>
        /// 撤单
        /// 订单编号映射不启用时，仅仅orderID是可用的
        /// </summary>
        /// <returns></returns>
        ResultInfo CancelOrder(CancelOrderArg arg);

        /// <summary>
        /// 订单状态改变事件
        /// </summary>
        event Action<OrderEx, ITradeExAdapter> OnOrderChanged;

        /// <summary>
        /// 订单成交事件
        /// </summary>
        event Action<TradeInfo, ITradeExAdapter> OnTradeChanged;

        /// <summary>
        /// 查询当日成交
        /// </summary>
        /// <returns></returns>
        ResultInfo<List<TradeInfo>> GetTrade();

        /// <summary>
        /// 查询转账流水
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<List<TransferSerial>> GetTransferSerial(GetTransferSerialArg arg);

        /// <summary>
        /// 查询绑定的银行
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<ContractBank> GetContractBank();

        /// <summary>
        /// 银行转券商
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<TransferInfo> ReqBankToBroker(BankBrokerTransferArg arg);

        /// <summary>
        /// 券商转银行
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<TransferInfo> ReqBrokerToBank(BankBrokerTransferArg arg);

        /// <summary>
        /// 查询银行余额
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<BankBalance> GetBankBalance(GetBankBalanceArg arg);

        /// <summary>
        /// 修改用户交易密码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo ModifyUserPassword(ModifyPasswordArg arg);

        /// <summary>
        /// 修改资金密码
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo ModifyFundAccountPassword(ModifyPasswordArg arg);

        /// <summary>
        /// 查询合约保证金率
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<List<InstrumentMarginRate>> GetInstrumentMarginRate(GetMarginRateArg arg);

        /// <summary>
        /// 查询历史账单
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<string> GetSettlementInfo(GetSettlementInfoArg arg);

        /// <summary>
        /// 获取帐号信息
        /// </summary>
        /// <returns></returns>
        ResultInfo<AccountInfo> GetAccountInfo();

        /// <summary>
        /// 查询历史委托
        /// </summary>
        /// <returns></returns>
        ResultInfo<List<OrderEx>> GetHisOrder(GetHisOrderArg arg);

        /// <summary>
        /// 查询历史成交
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<List<TradeInfo>> GetHisTrade(GetHisTradeArg arg);

        /// <summary>
        /// 查询绑定的银行卡列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        ResultInfo<List<ContractBank>> GetContractBankList();
    }

}
