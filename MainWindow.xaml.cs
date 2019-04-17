using System.ComponentModel;
using System.Windows.Controls;
using HWEnchCalc.Config;
using HWEnchCalc.Core;
using HWEnchCalc.Titan;

namespace HWEnchCalc
{

    public partial class MainWindow
    {
       public MainWindow()
        {
            InitializeComponent();

            var config = ConfigurationManager.GetConfig();
            DataContext = new HwEnchCalcViewModel(config);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor desc 
                && desc.Attributes[typeof(ColumnAttribute)] is ColumnAttribute att)
            {
                e.Column.Header = att.Name;
                e.Cancel = att.IsHide;
            }
        }
    }
}