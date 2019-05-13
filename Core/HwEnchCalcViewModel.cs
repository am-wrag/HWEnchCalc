using HWEnchCalc.Calculators;
using HWEnchCalc.Common;
using HWEnchCalc.Config;

namespace HWEnchCalc.Core
{
    public class HwEnchCalcViewModel : NotifyPropertyChangedBase
    {
        private bool _isAllTextboxReadOnly = true;

        public bool IsAllTextboxReadOnly
        {
            get => _isAllTextboxReadOnly;
            set
            {
                _isAllTextboxReadOnly = value;
                PropertyChangedByMember();
            }
        }

        public TitanCalculatorCore TitanCalculator { get; set; }
        public CalculatorManager CalcManager { get; set; } = new CalculatorManager();

        public HwEnchCalcViewModel(Configuration config)
        {
            TitanCalculator = new TitanCalculatorCore(config);
        }
    }
}