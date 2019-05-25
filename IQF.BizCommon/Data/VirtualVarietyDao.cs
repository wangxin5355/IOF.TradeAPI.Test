namespace IQF.BizCommon.Data
{
	public class VirtualVarietyDao
    {
        public static bool IsVirtualVariety(long vartietyID)
        {
            return vartietyID > 10000;
        }
    }
}
