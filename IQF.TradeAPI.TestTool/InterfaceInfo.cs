using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQF.TradeAPI.TestTool
{
    public class InterfaceInfo: BindableBase
    {
        private string methodUrl;

        private string parameter;

        private string parameterFileName;

        private bool isCalled;

        private int index;


        public string MethodUrl
        {
            get
            {
                return methodUrl;
            }
            set
            {
                SetProperty(ref methodUrl, value);
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

        public string ParameterFileName
        {
            get
            {
                return parameterFileName;
            }
            set
            {
                SetProperty(ref parameterFileName, value);
            }
        }

        public bool IsCalled
        {
            get
            {
                return isCalled;
            }
            set
            {
                SetProperty(ref isCalled, value);
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                SetProperty(ref index, value);
            }
        }
    }
}
