using IQF.Trade.ClientApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOF.TradeAPI.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public List<InterfaceInfo> interfaceInfos = new List<InterfaceInfo>();

        /// <summary>
        /// 初始化接口和参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            var dic=  LoadRequestApiInfo();
            InitInterfaceInfo("chuangyuan", dic);
        }


        /// <summary>
        /// 加载交易服务类型
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> LoadRequestApiInfo()
        {
            var dict = new Dictionary<string, string>();
            var file = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "IQF.Trade.ClientApi.dll", SearchOption.AllDirectories)[0];
            var assembly = Assembly.LoadFrom(file);
            var types = assembly.GetTypes().Where(w => typeof(TradeRequest).IsAssignableFrom(w) && w != typeof(TradeRequest));
            foreach (var type in types)
            {
                var apiInfo = type.GetCustomAttribute<TradeApiInfoAttribute>();
                if (apiInfo == null)
                {
                    throw new Exception("未能加载接口信息");
                }
                else
                {
                    dict.Add(type.FullName, apiInfo.ApiUrl);

                }
            }
            return dict;
        }

        public void InitInterfaceInfo(string brokerType, Dictionary<string, string> RequestApiInfo)
        {
            /*
            * 1、登陆：api/account/login
            * 2、查询账户信息:api/account/qryaccountinfo
            * 3、查询资产信息:api/account/qryasset
            * 4、查询余额:api/account/qrybalance
            * 
            * 
            * 
            */

            foreach(var item in RequestApiInfo)
            {
                
            }


        }

        /// <summary>
        /// HTTP POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        private static string HttpPost(string url, string parameters, int timeOut = 30000)
        {
            var request = HttpWebRequest.CreateHttp(url);
            //如果是发送HTTPS请求  
            if (true == url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request.ProtocolVersion = HttpVersion.Version10;
            }
            request.Method = "POST";
            if (timeOut > 0)
                request.Timeout = timeOut;

            byte[] data = Encoding.UTF8.GetBytes(parameters);
            request.ContentType = "application/json;charset=utf-8";
            request.KeepAlive = true;
            request.Proxy = null;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var response = request.GetResponse())
            {
                using (var rs = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(rs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
