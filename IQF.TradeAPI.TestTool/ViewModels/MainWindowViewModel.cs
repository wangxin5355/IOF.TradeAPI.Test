
using IQF.Framework;
using IQF.Framework.Serialization;
using IQF.Trade.ClientApi;
using IQF.TradeAPI.TestTool.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;

namespace IQF.TradeAPI.TestTool.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private ObservableCollection<InterfaceInfo> interfaceInfos=new ObservableCollection<InterfaceInfo>();
        public ObservableCollection<InterfaceInfo> InterfaceInfos
        {
            get { return interfaceInfos; }
            set { this.SetProperty(ref interfaceInfos, value); }
        }

        private ObservableCollection<string> responesInfo = new ObservableCollection<string>();
        public ObservableCollection<string> ResponesInfo
        {
            get { return responesInfo; }
            set { this.SetProperty(ref responesInfo, value); }
        }

        private InterfaceInfo selectedInterfaceInfo;
        public InterfaceInfo SelectedInterfaceInfo
        {
            get { return selectedInterfaceInfo; }
            set { SetProperty(ref selectedInterfaceInfo, value); }
        }

        private string curentBrokerType { get; set; }

        private string title = "期货交易接口测试工具";

        /// <summary>
        /// 重置接口参数
        /// </summary>
        public DelegateCommand<InterfaceInfo> ReSetParameterCommand { get; private set; }

        /// <summary>
        /// 选择期货公司
        /// </summary>
        public DelegateCommand<string> SelectBrokerTypeCommand { get; private set; }

        public DelegateCommand LogInCommand { get; private set; }

        public DelegateCommand LogOutCommand { get; private set; }

        public DelegateCommand<InterfaceInfo> ReqestOneCommand { get; private set; }

        public DelegateCommand ReqestAllCommand { get; private set; }


        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        
        private LohinStatus loginStatus = LohinStatus.LogOut;
        /// <summary>
        /// 登陆状态：已登陆，未登陆
        /// </summary>
        public LohinStatus LoginStatus
        {
            get { return loginStatus; }
            set { SetProperty(ref loginStatus, value); }
        }

        
        private string apiAddr="127.0.0.1";

        /// <summary>
        /// 接口基地址
        /// </summary>

        public string ApiAddr
        {
            get { return apiAddr; }
            set { SetProperty(ref apiAddr, value); }
        }


        private int apiPort=5000;

        /// <summary>
        /// 接口端口
        /// </summary>

        public int ApiPort
        {
            get { return apiPort; }
            set { SetProperty(ref apiPort, value); }
        }



        public MainWindowViewModel()
        {
         
            ReSetParameterCommand = new DelegateCommand<InterfaceInfo>(DoReSetParameterCommand);
            SelectBrokerTypeCommand = new DelegateCommand<string>(DoSelectBrokerType);
            LogInCommand = new DelegateCommand(DoLogin);
            LogOutCommand = new DelegateCommand(DoLogOut);
            ReqestOneCommand = new DelegateCommand<InterfaceInfo>(DoReqestOne);
            ReqestAllCommand = new DelegateCommand(DoReqestAll);
        }

        /// <summary>
        /// 设置当前期货公司
        /// </summary>
        /// <param name="selectBrokerType"></param>
        private void DoSelectBrokerType(string selectBrokerType)
        {
            this.curentBrokerType = selectBrokerType;

        }

        /// <summary>
        /// 执行登陆命令
        /// </summary>
        private void DoLogin()
        {
            //判断是否有选择期货公司
            if (string.IsNullOrEmpty(this.curentBrokerType))
            {
                MessageBox.Show("请先选择期货公司");
            }
            else
            {
                //初始化接口信息
                var dic = LoadRequestApiInfo();
                InitInterfaceInfo(this.curentBrokerType, dic);
                //执行登陆

                var loginInterface = interfaceInfos.FirstOrDefault(x => x.Index == (int)RequestIndex.LoginReq);
                string url = "http://" + this.apiAddr + ":" + this.apiPort + loginInterface.MethodUrl;
                string resp = HttpPost(url, loginInterface.Parameter);
                var jsonString = new JsonString(resp);
                var result = new ResultInfo<string>();
                result.Error_no = jsonString.GetInt("error_no");
                result.Error_info = jsonString.Get("error_info").SafeToString();
                result.Data = jsonString.Get("data").SafeToString();
                if(result.Error_no != 0)
                {
                    ResponesInfo.Add($"请求{loginInterface.ParameterFileName}失败，错误码{ result.Error_no}，错误信息{ result.Error_info}");
                }
                else
                {
                    ResponesInfo.Add($"请求{loginInterface.ParameterFileName}成功，返回数据{result.Data}");
                }
            }
        }

        /// <summary>
        /// 执行登陆出命令
        /// </summary>
        private void DoLogOut()
        {

        }

        private void DoReqestOne(InterfaceInfo interfaceInfo)
        {

        }

        private void DoReqestAll()
        { 
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

            foreach (var item in RequestApiInfo)
            {

                InterfaceInfo info = new InterfaceInfo();
                info.IsCalled = false;
                info.MethodUrl = item.Value;
                var name = item.Key.Split('.');
                info.ParameterFileName = name[name.Length - 1];
                info.Index = GetRequestIndex(info.ParameterFileName);
                //TODO：从json文件加载参数
                string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string parameterPath = Path.Combine(filePath, "Parameters", brokerType, info.ParameterFileName + ".json");
                if (!File.Exists(parameterPath))
                {
                    MessageBox.Show("请求参数文件丢失！" + parameterPath);
                    continue;
                }
                StreamReader file = File.OpenText(parameterPath);
                JsonTextReader reader = new JsonTextReader(file);
                JObject jsonObject = (JObject)JToken.ReadFrom(reader);
                string para = jsonObject.ToString();
                info.Parameter = para;
                this.InterfaceInfos.Add(info);
                file.Close();

            }
            this.InterfaceInfos = new ObservableCollection<InterfaceInfo>(this.InterfaceInfos.OrderBy(x => x.Index));


        }

        /// <summary>
        /// 重置选中的接口参数
        /// </summary>
        /// <param name="info"></param>
        private void DoReSetParameterCommand(InterfaceInfo info)
        {

            ParameterEditor patameterEditor = new ParameterEditor(info.ParameterFileName, this.curentBrokerType);
            bool? dil=patameterEditor.ShowDialog();
            if(dil.HasValue&& dil.Value)
            {
                //重新加载配置

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

 

        public int GetRequestIndex(string requestName)
        {
            if (Enum.TryParse<RequestIndex>(requestName, out RequestIndex request))
            {
               return (int)request;
            }
            else
            {
                return 0;
            }
        }


    }
}
