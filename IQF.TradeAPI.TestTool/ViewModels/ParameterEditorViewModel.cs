using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace IQF.TradeAPI.TestTool.ViewModels
{
	public class ParameterEditorViewModel : BindableBase
	{

        private ObservableCollection<ParameterInfo> parameters = new ObservableCollection<ParameterInfo>();
        public ObservableCollection<ParameterInfo> Parameters
        {
            get { return parameters; }
            set { this.SetProperty(ref parameters, value); }
        }

        private string title = "参数编辑器";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string ParameterFilePath { get; set; }

        public DelegateCommand<Window> SaveParameterCommand { get; private set; }
        /// <summary>
        ///
        /// </summary>
        /// <param name="requestName">请求名称</param>
        /// <param name="brokerType">期货公司类型</param>
        public ParameterEditorViewModel(string requestName,string brokerType)
        {
            this.SaveParameterCommand = new DelegateCommand<Window>(SaveParameters);
            this.title = title +"-"+ brokerType + "-" + requestName;
            //加载json
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ParameterFilePath = Path.Combine(filePath, "Parameters", brokerType, requestName+".json");
            StreamReader file = File.OpenText(ParameterFilePath);
            JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);
            foreach(var item in jsonObject.Children())
            {
                var key = item.Path;
                var value = item.First.ToString();
                ParameterInfo parameterInfo = new ParameterInfo();
                parameterInfo.Key = key;
                parameterInfo.Parameter = value;
                Parameters.Add(parameterInfo);
            }
            file.Close();


        }

        /// <summary>
        /// 保存配置文件关闭窗口
        /// </summary>
        private void SaveParameters(Window window)
        {
            dynamic objmatch = new ExpandoObject();
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            foreach (var item in parameters)
            {
                keyValuePairs.Add(item.Key, item.Parameter);
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(keyValuePairs, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ParameterFilePath, output);
            if(window!=null)
            {
                window.DialogResult = true;
                window.Close();
            }

            

        }
    }

    

    public class ParameterInfo: BindableBase
    {
        private string key;

        private string parameter;


        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                SetProperty(ref key, value);
            }
        }
        public string Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                SetProperty(ref parameter, value);
            }
        }
    }
}
