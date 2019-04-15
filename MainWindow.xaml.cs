using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HWEnchCalc.Config;
using HWEnchCalc.Core;
using HWEnchCalc.Titan;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace HWEnchCalc
{

    public partial class MainWindow
    {
       public MainWindow()
        {
            InitializeComponent();

            var config = ConfigurationHelper.GetConfig();
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