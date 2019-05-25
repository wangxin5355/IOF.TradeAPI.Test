namespace IQF.Trade.Core.Session
{
    public interface ISessionManager
    {
        ITradeExAdapter Get(string tradeToken);
        int GetCount();
        ITradeExAdapter GetOrCreate(string tradeToken);
        bool Remove(string tradeToken);
    }
}