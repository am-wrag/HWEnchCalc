using HWEnchCalc.Calculators;
using HWEnchCalc.Common;
using HWEnchCalc.Config;

namespace HWEnchCalc.Core
{
    public class HwEnchCalcViewModel : NotifyPropertyChangedBase
    {
        public TitanCalculator TitanCalc { get; set; }
        public CalculatorManager CalcManager { get; set; } = new CalculatorManager();

        public HwEnchCalcViewModel(Configuration config)
        {
            TitanCalc = new TitanCalculator(config);
        }
        
    }
}