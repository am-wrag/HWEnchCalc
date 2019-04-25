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

        public string CalcBannerText => _currenCalculatorIndex == 0 ? EssenceCalcDesc: GoldCalcDest;

        public int CurrentCalculatorIndex
        {
            get => _currenCalculatorIndex;
            set
            {
                _currenCalculatorIndex = value;
                PropertyChangedByMember();
                PropertyChangedByName(nameof(CalcBannerText));
            }
        }

        public Visibility NextBtnVisibility =>
            _currenCalculatorIndex == MaxCalculatorIndex ? Visibility.Hidden : Visibility.Visible;
        public Visibility PrevBtnVisibility => 
            _currenCalculatorIndex == 0 ? Visibility.Hidden : Visibility.Visible;

        public WpfCommand NextCalcCommand { get; }
        public WpfCommand PreviosCalcCommand { get; }

        private int _currenCalculatorIndex;

        public CalculatorManager()
        {
            NextCalcCommand = new WpfCommand(SetNextCalulator);
            PreviosCalcCommand = new WpfCommand(SetPreviosCalulator);
        }

        private void SetNextCalulator()
        {
            if (CurrentCalculatorIndex + 1 > MaxCalculatorIndex) return;

            CurrentCalculatorIndex++;

            UpdateVisibility();
        }

        private void SetPreviosCalulator()
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