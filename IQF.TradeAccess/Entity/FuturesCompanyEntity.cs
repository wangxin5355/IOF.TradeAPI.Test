using IQF.Framework.Dao;
using System;

namespace IQF.TradeAccess.Entity
{
    public partial class FuturesCompany : IEntity
    {
        /// <summary>
        /// 期货公司代码
        /// </summary>
        public string fcCode { get; set; }
        /// <summary>
        /// 期货公司名称
        /// </summary>
        public string fcName { get; set; }
        /// <summary>
        /// 居间人代码
        /// </summary>
        public string jujianCode { get; set; }
        /// <summary>
        /// 经理代码
        /// </summary>
        public string jingliCode { get; set; }
        /// <summary>
        /// 开户Url
        /// </summary>
        public string kaihuUrl { get; set; }
        /// <summary>
        /// 期货公司地址
        /// </summary>
        public string fcCompanyUrl { get; set; }
        /// <summary>
        /// 期货公司logo地址
        /// </summary>
        public string logoImgUrl { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime addTime { get; set; }
        /// <summary>
        /// 期货公司介绍
        /// </summary>
        public string fcIntroduce { get; set; }
        /// <summary>
        /// 期货公司属性
        /// </summary>
        public string fcAttribute { get; set; }
        /// <summary>
        /// 软件下载地址
        /// </summary>
        public string softDownUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sortNum { get; set; }
    } //end of class

    public partial class VM_FutureCompanyPackType
    {
        public string fcCode { get; set; }
        public string fcName { get; set; }
        public string fcIntroduce { get; set; }
        public string packType { get; set; }
        public int isShow { get; set; }
        public int sortNum { get; set; }

        public string fcAttribute { get; set; }
        public string jingliCode { get; set; }
        public string jujianCode { get; set; }
        public string kaihuUrl { get; set; }
        public string fcCompanyUrl { get; set; }
        public string softDownUrl { get; set; }
        public string logoImgUrl { get; set; }

    }

} //end of namespace
