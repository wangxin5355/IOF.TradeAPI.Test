using IQF.Framework.Dao;
using System.ComponentModel;

namespace IQF.TradeAccess.Entity
{
    public class BrokerCompanyEntity : IEntity
    {
        public long CompanyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BrokerType BrokerType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrokerID { get; set; }
        /// <summary>
        /// 期货公司介绍
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 期货公司图标
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FcCode { get; set; }
    }

    public enum BrokerType
    {
        /// <summary>
        /// 华创期货
        /// </summary>
        [Description("华创期货")]
        HuaChuang = 1,
        /// <summary>
        /// 创元期货
        /// </summary>
        [Description("创元期货")]
        ChuangYuan = 2,
        /// <summary>
        /// 国投安信
        /// </summary>
        [Description("国投安信")]
        GuoTouAnXin = 3,
        /// <summary>
        /// 海通期货
        /// </summary>
        [Description("海通期货")]
        HaiTong = 4,
        /// <summary>
        /// 广州期货
        /// </summary>
        [Description("广州期货")]
        GuangZhou = 5,
        /// <summary>
        /// 新纪元期货
        /// </summary>
        [Description("新纪元期货")]
        XinJiYuan = 6,
        /// <summary>
        /// 长安期货
        /// </summary>
        [Description("长安期货")]
        ChangAn = 7,
        /// <summary>
        /// 中财期货
        /// </summary>
        [Description("中财期货")]
        ZhongCai = 8,
        /// <summary>
        /// 国海良时
        /// </summary>
        [Description("国海良时")]
        GuoHaiLiangShi = 9,

        /// <summary>
        /// 永安期货
        /// </summary>
        [Description("永安期货")]
        YongAn = 11,
        /// <summary>
        /// 华泰期货
        /// </summary>
        [Description("华泰期货")]
        HuaTai = 12,
        /// <summary>
        /// 中信期货
        /// </summary>
        [Description("中信期货")]
        ZhongXin = 13,
        /// <summary>
        /// 国泰君安
        /// </summary>
        [Description("国泰君安")]
        GuoTaiJunAn = 14,
        /// <summary>
        /// 银河期货
        /// </summary>
        [Description("银河期货")]
        YinHe = 15,
        /// <summary>
        /// 方正中期
        /// </summary>
        [Description("方正中期")]
        FangZheng = 16,
        /// <summary>
        /// 光大期货
        /// </summary>
        [Description("光大期货")]
        GuangDa = 17,
        /// <summary>
        /// 广发期货
        /// </summary>
        [Description("广发期货")]
        GuangFa = 18,
        /// <summary>
        /// 申银万国
        /// </summary>
        [Description("申银万国")]
        ShenYinWanGuo = 19,
        /// <summary>
        /// 上海东证
        /// </summary>
        [Description("上海东证")]
        ShangHaiDongZheng = 20,
        /// <summary>
        /// 浙商期货
        /// </summary>
        [Description("浙商期货")]
        ZheShang = 21,
        /// <summary>
        /// 国际期货
        /// </summary>
        [Description("国际期货")]
        GuoJi = 22,
        /// <summary>
        /// 南华期货
        /// </summary>
        [Description("南华期货")]
        NanHua = 23,
        /// <summary>
        /// 中信建投
        /// </summary>
        [Description("中信建投")]
        ZhongXinJianTou = 24,
        /// <summary>
        /// 瑞达期货
        /// </summary>
        [Description("瑞达期货")]
        RuiDa = 25,
        /// <summary>
        /// 招商期货
        /// </summary>
        [Description("招商期货")]
        ZhaoShang = 26,
        /// <summary>
        /// 国信期货
        /// </summary>
        [Description("国信期货")]
        GuoXin = 27,
        /// <summary>
        /// 中粮期货
        /// </summary>
        [Description("中粮期货")]
        ZhongLiang = 28,
        /// <summary>
        /// 华信期货
        /// </summary>
        [Description("华信期货")]
        HuaXin = 29,
        /// <summary>
        /// 长江期货
        /// </summary>
        [Description("长江期货")]
        ChangJiang = 30,
        /// <summary>
        /// 新湖期货
        /// </summary>
        [Description("新湖期货")]
        XinHu = 31,
        /// <summary>
        /// 弘业期货
        /// </summary>
        [Description("弘业期货")]
        HongYe = 32,
        /// <summary>
        /// 鲁证期货
        /// </summary>
        [Description("鲁证期货")]
        LuZheng = 33,
        /// <summary>
        /// 宏源期货
        /// </summary>
        [Description("宏源期货")]
        HongYuan = 34,
        /// <summary>
        /// 上海中期
        /// </summary>
        [Description("上海中期")]
        ShangHaiZhongQi = 35,
        /// <summary>
        /// 兴证期货
        /// </summary>
        [Description("兴证期货")]
        XingZheng = 36,
        /// <summary>
        /// 东航期货
        /// </summary>
        [Description("东航期货")]
        DongHang = 37,
        /// <summary>
        /// 五矿经易
        /// </summary>
        [Description("五矿经易")]
        WuKuangJingYi = 38,
        /// <summary>
        /// 金瑞期货
        /// </summary>
        [Description("金瑞期货")]
        JinRui = 39,
        /// <summary>
        /// 建信期货
        /// </summary>
        [Description("建信期货")]
        JianXin = 40,
        /// <summary>
        /// 信达期货
        /// </summary>
        [Description("信达期货")]
        XinDa = 41,
        /// <summary>
        /// 中金期货
        /// </summary>
        [Description("中金期货")]
        ZhongJin = 42,
        /// <summary>
        /// 格林大华
        /// </summary>
        [Description("格林大华")]
        GeLinDaHua = 43,
        /// <summary>
        /// 东海期货
        /// </summary>
        [Description("东海期货")]
        DongHai = 44,
        /// <summary>
        /// 一德期货
        /// </summary>
        [Description("一德期货")]
        YiDe = 45,
        /// <summary>
        /// 国贸期货
        /// </summary>
        [Description("国贸期货")]
        GuoMao = 46,
        /// <summary>
        /// 平安期货
        /// </summary>
        [Description("平安期货")]
        PingAn = 47,
        /// <summary>
        /// 中大期货
        /// </summary>
        [Description("中大期货")]
        ZhongDa = 48,
        /// <summary>
        /// 大有期货
        /// </summary>
        [Description("大有期货")]
        DaYou = 49,
        /// <summary>
        /// 大地期货
        /// </summary>
        [Description("大地期货")]
        DaDi = 50,
        /// <summary>
        /// 华龙期货
        /// </summary>
        [Description("华龙期货")]
        HuaLong = 51,
        /// <summary>
        /// 国元期货
        /// </summary>
        [Description("国元期货")]
        GuoYuan = 52,
        /// <summary>
        /// 中投期货
        /// </summary>
        [Description("中投期货")]
        ZhongTou = 53,
        /// <summary>
        /// 迈科期货
        /// </summary>
        [Description("迈科期货")]
        MaiKe = 54,
        /// <summary>
        /// 宝城期货
        /// </summary>
        [Description("宝城期货")]
        BaoCheng = 55,
        /// <summary>
        /// 国联期货
        /// </summary>
        [Description("国联期货")]
        GuoLian = 56,
        /// <summary>
        /// 民生期货
        /// </summary>
        [Description("民生期货")]
        MinSheng = 57,
        /// <summary>
        /// 英大期货
        /// </summary>
        [Description("英大期货")]
        YingDa = 58,
        /// <summary>
        /// 中原期货
        /// </summary>
        [Description("中原期货")]
        ZhongYuan = 59,
        /// <summary>
        /// 安粮期货
        /// </summary>
        [Description("安粮期货")]
        AnLiang = 60,
        /// <summary>
        /// 混沌天成
        /// </summary>
        [Description("混沌天成")]
        HunDunTianCheng = 61,
        /// <summary>
        /// 中融汇信
        /// </summary>
        [Description("中融汇信")]
        ZhongRongHuiXin = 62,
        /// <summary>
        /// 西部期货
        /// </summary>
        [Description("西部期货")]
        XiBu = 63,
        /// <summary>
        /// 徽商期货
        /// </summary>
        [Description("徽商期货")]
        WeiShang = 64,
        /// <summary>
        /// 华安期货
        /// </summary>
        [Description("华安期货")]
        HuaAn = 65,
        /// <summary>
        /// 江西瑞奇
        /// </summary>
        [Description("江西瑞奇")]
        JiangXiRuiQi = 66,
        /// <summary>
        /// 中衍期货
        /// </summary>
        [Description("中衍期货")]
        ZhongYan = 67,
        /// <summary>
        /// 铜冠金源
        /// </summary>
        [Description("铜冠金源")]
        TongGuanJinYuan = 68,
        /// <summary>
        /// 东吴期货
        /// </summary>
        [Description("东吴期货")]
        DongWu = 69,
        /// <summary>
        /// 云晨期货
        /// </summary>
        [Description("云晨期货")]
        YunChen = 70,
        /// <summary>
        /// 恒泰期货
        /// </summary>
        [Description("恒泰期货")]
        HengTai = 71,
        /// <summary>
        /// 山金期货
        /// </summary>
        [Description("山金期货")]
        ShanJin = 72,
        /// <summary>
        /// 广州金控
        /// </summary>
        [Description("广州金控")]
        GuangZhouJinKong = 73,
        /// <summary>
        /// 锦泰期货
        /// </summary>
        [Description("锦泰期货")]
        JinTai = 74,
        /// <summary>
        /// 海航期货
        /// </summary>
        [Description("海航期货")]
        HaiHang = 75,
        /// <summary>
        /// 天风期货
        /// </summary>
        [Description("天风期货")]
        TianFeng = 76,
        /// <summary>
        /// 华联期货
        /// </summary>
        [Description("华联期货")]
        HuaLian = 77,
        /// <summary>
        /// 倍特期货
        /// </summary>
        [Description("倍特期货")]
        BeiTe = 78,
        /// <summary>
        /// 兴业期货
        /// </summary>
        [Description("兴业期货")]
        XingYe = 79,
        /// <summary>
        /// 上海大陆
        /// </summary>
        [Description("上海大陆")]
        ShangHaiDaLu = 80,
        /// <summary>
        /// 美尔雅期货
        /// </summary>
        [Description("美尔雅期货")]
        MeiErYa = 81,
        /// <summary>
        /// 华融期货
        /// </summary>
        [Description("华融期货")]
        HuaRong = 82,
        /// <summary>
        /// 国金期货
        /// </summary>
        [Description("国金期货")]
        GuoJin = 83,
        /// <summary>
        /// 红塔期货
        /// </summary>
        [Description("红塔期货")]
        HongTa = 84,
        /// <summary>
        /// 金石期货
        /// </summary>
        [Description("金石期货")]
        JinShi = 85,
        /// <summary>
        /// 华金期货
        /// </summary>
        [Description("华金期货")]
        HuaJin = 86,
        /// <summary>
        /// 中辉期货
        /// </summary>
        [Description("中辉期货")]
        ZhongHui = 87,
        /// <summary>
        /// 冠通期货
        /// </summary>
        [Description("冠通期货")]
        GuanTong = 88,
        /// <summary>
        /// 神华期货
        /// </summary>
        [Description("神华期货")]
        ShenHua = 89,
        /// <summary>
        /// 福能期货
        /// </summary>
        [Description("福能期货")]
        FuNeng = 90,
        /// <summary>
        /// 南证期货
        /// </summary>
        [Description("南证期货")]
        NanZheng = 91,
        /// <summary>
        /// 中钢期货
        /// </summary>
        [Description("中钢期货")]
        ZhongGang = 92,
        /// <summary>
        /// 道通期货
        /// </summary>
        [Description("道通期货")]
        DaoTong = 93,
        /// <summary>
        /// 西南期货
        /// </summary>
        [Description("西南期货")]
        XiNan = 94,
        /// <summary>
        /// 华鑫期货
        /// </summary>
        [Description("华鑫期货")]
        HuaXinQiHuo = 95,
        /// <summary>
        /// 九州期货
        /// </summary>
        [Description("九州期货")]
        JiuZhou = 96,
        /// <summary>
        /// 华西期货
        /// </summary>
        [Description("华西期货")]
        HuaXi = 97,
        /// <summary>
        /// 北京首创
        /// </summary>
        [Description("北京首创")]
        BeiJingShouChuang = 98,
        /// <summary>
        /// 浙江新世纪
        /// </summary>
        [Description("浙江新世纪")]
        XinShiJi = 99,
        /// <summary>
        /// 中电投先融
        /// </summary>
        [Description("中电投先融")]
        XianRong = 100,
        /// <summary>
        /// 同信久恒
        /// </summary>
        [Description("同信久恒")]
        TongXinJiuHeng = 101,
        /// <summary>
        /// 金汇期货
        /// </summary>
        [Description("金汇期货")]
        JinHui = 102,
        /// <summary>
        /// 上海浙石
        /// </summary>
        [Description("上海浙石")]
        ShangHaiZheShi = 103,
        /// <summary>
        /// 上海东方
        /// </summary>
        [Description("上海东方")]
        ShangHaiDongFang = 104,
        /// <summary>
        /// 东兴期货
        /// </summary>
        [Description("东兴期货")]
        DongXing = 105,
        /// <summary>
        /// 国富期货
        /// </summary>
        [Description("国富期货")]
        GuoFu = 106,
        /// <summary>
        /// 渤海期货
        /// </summary>
        [Description("渤海期货")]
        BoHai = 107,
        /// <summary>
        /// 大越期货
        /// </summary>
        [Description("大越期货")]
        DaYue = 108,
        /// <summary>
        /// 中银国际
        /// </summary>
        [Description("中银国际")]
        ZhongYinGuoJi = 109,
        /// <summary>
        /// 乾坤期货
        /// </summary>
        [Description("乾坤期货")]
        QianKun = 110,
        /// <summary>
        /// 盛达期货
        /// </summary>
        [Description("盛达期货")]
        ShengDa = 111,
        /// <summary>
        /// 金元期货
        /// </summary>
        [Description("金元期货")]
        JinYuan = 112,
        /// <summary>
        /// 第一创业
        /// </summary>
        [Description("第一创业")]
        DiYiChuangYe = 113,
        /// <summary>
        /// 中航期货
        /// </summary>
        [Description("中航期货")]
        ZhongHang = 114,
        [Description("国都期货")]
        GuoDu = 115,
        /// <summary>
        /// 中天期货
        /// </summary>
        [Description("中天期货")]
        ZhongTian = 116,
        /// <summary>
        /// 通惠期货
        /// </summary>
        [Description("通惠期货")]
        TongHui = 117,
        /// <summary>
        /// 财达期货	
        /// </summary>
        [Description("财达期货")]
        CaiDa = 118,
        /// <summary>
        /// 金信期货
        /// </summary>
        [Description("金信期货")]
        JinXin = 119,
        /// <summary>
        /// 大通期货
        /// </summary>
        [Description("大通期货")]
        DaTong = 120,
        /// <summary>
        /// 摩根大通
        /// </summary>
        [Description("摩根大通")]
        MoGenDaTong = 121,
        /// <summary>
        /// 鑫鼎盛期货
        /// </summary>
        [Description("鑫鼎盛期货")]
        XinDingSheng = 122,
        /// <summary>
        /// 新疆天利
        /// </summary>
        [Description("新疆天利")]
        XinJiangTianLi = 123,
        /// <summary>
        /// 新晟期货
        /// </summary>
        [Description("新晟期货")]
        XinSheng = 124,
        /// <summary>
        /// 黑龙江时代
        /// </summary>
        [Description("黑龙江时代")]
        HeiLongJiangShiDai = 125,
        /// <summary>
        /// 河北恒银
        /// </summary>
        [Description("河北恒银")]
        HeBeiHengYin = 126,
        /// <summary>
        /// 金鹏期货
        /// </summary>
        [Description("金鹏期货")]
        JinPeng = 127,
        /// <summary>
        /// 德盛期货
        /// </summary>
        [Description("德盛期货")]
        DeSheng = 128,
        /// <summary>
        /// 招金期货
        /// </summary>
        [Description("招金期货")]
        ZhaoJin = 129,
        /// <summary>
        /// 深圳瑞龙
        /// </summary>
        [Description("深圳瑞龙")]
        ShenZhenRuiLong = 130,
        /// <summary>
        /// 瑞银期货
        /// </summary>
        [Description("瑞银期货")]
        RuiYin = 131,
        /// <summary>
        /// 上海东亚
        /// </summary>
        [Description("上海东亚")]
        ShangHaiDongYa = 132,
        /// <summary>
        /// 江海汇鑫
        /// </summary>
        [Description("江海汇鑫")]
        JiangHaiHuiXin = 133,
        /// <summary>
        /// 津投期货
        /// </summary>
        [Description("津投期货")]
        JinTou = 134,
        /// <summary>
        /// 文峰期货
        /// </summary>
        [Description("文峰期货")]
        WenFeng = 135,
        /// <summary>
        /// 江苏东华
        /// </summary>
        [Description("江苏东华")]
        JiangSuDongHua = 136,
        /// <summary>
        /// 和融期货
        /// </summary>
        [Description("和融期货")]
        HeRong = 137,
        /// <summary>
        /// 集成期货
        /// </summary>
        [Description("集成期货")]
        JiCheng = 138,
        /// <summary>
        /// 江信国盛
        /// </summary>
        [Description("江信国盛")]
        JiangXinGuoSheng = 139,
        /// <summary>
        /// 首创京都
        /// </summary>
        [Description("首创京都")]
        ShouChuangJinDu = 140,
        /// <summary>
        /// 晟鑫期货
        /// </summary>
        [Description("晟鑫期货")]
        ShengXin = 141,
        /// <summary>
        /// 大连良运
        /// </summary>
        [Description("大连良运")]
        DaLianLiangYun = 142,
        /// <summary>
        /// 华闻期货
        /// </summary>
        [Description("华闻期货")]
        HuaWen = 143,
        /// <summary>
        /// 海证期货
        /// </summary>
        [Description("海证期货")]
        HaiZheng = 144,
        /// <summary>
        /// 天富期货
        /// </summary>
        [Description("天富期货")]
        TianFu = 145,
        /// <summary>
        /// 天鸿期货
        /// </summary>
        [Description("天鸿期货")]
        TianHong = 146,
        /// <summary>
        /// 和合期货
        /// </summary>
        [Description("和合期货")]
        HeHe = 147,
        /// <summary>
        /// 山西三立
        /// </summary>
        [Description("山西三立")]
        ShanXiSanLi = 148,
        /// <summary>
        /// 东方汇金
        /// </summary>
        [Description("东方汇金")]
        DongFangHuiJin = 149,
        /// <summary>
        /// 中州期货
        /// </summary>
        [Description("中州期货")]
        ZhongZhou = 150,
        /// <summary>
        /// 长安期货-飞马
        /// </summary>
        [Description("长安期货-飞马")]
        ChangAnFm = 151,
        /// <summary>
        /// 期货公司模拟
        /// </summary>
        [Description("模拟")]
        MoNi = 200
    }
}
