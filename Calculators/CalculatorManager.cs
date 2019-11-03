using System.Windows;
using HWEnchCalc.Common;

namespace HWEnchCalc.Calculators
{
    public class CalculatorManager : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Метка того что изменение свойства затрагивают основной калькулятор в подписках
        /// </summary>
        public static readonly string PropertyWhenModifyNeedCalculate = "PropertyWhenModifyNeedCalculate";
        private const string EssenceCalcDesc = "Вычисление эффективности прокачки 1го или 2го артефакта титанов";
        private const string GoldCalcDest = "Вычисление эффективности прокачки титана по отношению к прокачке 3го артефакта!";
        private const int MaxCalculatorIndex = 1;

        public string CalcBannerText => _currentCalculatorIndex == 0 ? EssenceCalcDesc: GoldCalcDest;

        public int CurrentCalculatorIndex
        {
            get => _currentCalculatorIndex;
            set
            {
                _currentCalculatorIndex = value;
                PropertyChangedByMember();
                PropertyChangedByName(nameof(CalcBannerText));
            }
        }

        public Visibility NextBtnVisibility =>
            _currentCalculatorIndex == MaxCalculatorIndex ? Visibility.Hidden : Visibility.Visible;
        public Visibility PrevBtnVisibility => 
            _currentCalculatorIndex == 0 ? Visibility.Hidden : Visibility.Visible;

        public WpfCommand NextCalcCommand { get; }
        public WpfCommand PreviousCalcCommand { get; }

        private int _currentCalculatorIndex;

        public CalculatorManager()
        {
            NextCalcCommand = new WpfCommand(SetNextCalculator);
            PreviousCalcCommand = new WpfCommand(SetPreviousCalculator);
        }

        private void SetNextCalculator()
        {
            if (CurrentCalculatorIndex + 1 > MaxCalculatorIndex) return;

            CurrentCalculatorIndex++;

            UpdateVisibility();
        }

        private void SetPreviousCalculator()
        {
            if (CurrentCalculatorIndex - 1 < 0) return;

            CurrentCalculatorIndex--;

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            PropertyChangedByName(nameof(NextBtnVisibility));
            PropertyChangedByName(nameof(PrevBtnVisibility));
        }
    }
}