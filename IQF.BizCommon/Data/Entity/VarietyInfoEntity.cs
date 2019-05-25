using System;

namespace IQF.BizCommon.Data.Entity
{
    public class VarietyInfoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long VarietyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string KeyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EditTime { get; set; }
    }
}