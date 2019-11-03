using System.Collections.ObjectModel;
using System.ComponentModel;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Core
{
    public class TitanCompareWindowViewModel : NotifyPropertyChangedBase
    {
        public TitanCompareData LeftTitanData { get; }
        public TitanCompareData RightTitanData { get; }
        public TitanCompareCalc TitanCompareCalc { get; }

        public TitanCompareWindowViewModel(ObservableCollection<TitanShowedData> titansData, TitanSourceDataHelper titanHelper)
        {
            LeftTitanData = new TitanCompareData(titansData, titanHelper);
            RightTitanData = new TitanCompareData(titansData, titanHelper);
            TitanCompareCalc = new TitanCompareCalc();
            LeftTitanData.PropertyChanged += Calculate;
            RightTitanData.PropertyChanged += Calculate;
        }

        private void Calculate(object sender, PropertyChangedEventArgs e)
        {
            TitanCompareCalc.Caclulate(LeftTitanData, RightTitanData);
            PropertyChangedByName(nameof(TitanCompareCalc));
        }
    }
}