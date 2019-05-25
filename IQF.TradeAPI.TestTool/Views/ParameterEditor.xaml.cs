using IQF.TradeAPI.TestTool.ViewModels;
using System.Windows;

namespace IQF.TradeAPI.TestTool.Views
{
    /// <summary>
    /// Interaction logic for ParameterEditor.xaml
    /// </summary>
    public partial class ParameterEditor : Window
    {
        public ParameterEditor(string requestName, string brokerType)
        {
            ParameterEditorViewModel viewModel = new ParameterEditorViewModel(requestName, brokerType);
            this.DataContext = viewModel;
            InitializeComponent();
        
        }
    }
}
