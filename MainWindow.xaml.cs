using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Core;
using HWEnchCalc.Titan;
using MahApps.Metro.Controls.Dialogs;

namespace HWEnchCalc
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Global<ProgressDialogController>
                .SetAwaitDialog(this.ShowProgressAsync("Пожалуйста подождите", "Идет выгрузка данных..."));


            Title = $"{Title} v{Assembly.GetExecutingAssembly().GetName().Version}";

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