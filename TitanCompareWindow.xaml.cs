using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Core;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc
{
    /// <summary>
    /// Логика взаимодействия для TitanCompareWindow.xaml
    /// </summary>
    public partial class TitanCompareWindow
    {
        public TitanCompareWindow(ObservableCollection<TitanShowedData> titansData, TitanSourceDataHelper titanHelper)
        {
            InitializeComponent();
            DataContext = new TitanCompareWindowViewModel(titansData, titanHelper);
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
