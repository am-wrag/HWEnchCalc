using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;
using MahApps.Metro.Controls.Dialogs;

namespace HWEnchCalc.Calculators
{
    public class TitanCalculatorCore : NotifyPropertyChangedBase
    {
        public TitanCalculationDataManager CalcManager { get; private set; }
        public EssenceCalculator EssenceCalc { get; set; } = new EssenceCalculator();
        public GuiseCalculator GuiseCalc { get; set; } = new GuiseCalculator();

        public int CalculatorIndex
        {
            get => _calculatorIndex;
            set
            {
                _calculatorIndex = value;
                PropertyChangedByName(nameof(CalculatorDesc));
            }
        }

        public string CalculatorDesc => GetCalculatorDesc();

        private int _calculatorIndex;

        public TitanCalculatorCore(Configuration config)
        {
            InitializeAsync(config);
        }

        private async void InitializeAsync(Configuration config)
        {
            var awaitDialogController = await Global<ProgressDialogController>.ShowAwaitDialog();
            awaitDialogController.SetIndeterminate();
            //awaitDialogController.SetProgress(0.9);

            var titanHelper = await ParseTitanData(config);
            awaitDialogController.SetMessage("Выгрузка данных завершена!");

            var titanBuilder = new TitanInfoMapper(titanHelper);
            var dbOperator = new DbOperator(config.ConnectionInfo.DefaultConnection);

            CalcManager = new TitanCalculationDataManager(titanBuilder, titanHelper, dbOperator);
            CalcManager.PropertyChanged += Calculate;

            PropertyChangedByName(nameof(CalcManager));

            await awaitDialogController.CloseAsync();
        }

        private async Task<TitanSourceDataHelper> ParseTitanData(Configuration config)
        {
            return await Task.Factory.StartNew(() =>
            {
                var titanHelper = new TitanSourceDataHelper();
                return titanHelper.Fill(config);
            });
        }

        private void Calculate(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != CalculatorManager.PropertyWhenModifyNeedCalculate) return;

            EssenceCalc.Calculate(CalcManager.TitanInfo);
            GuiseCalc.Calculate(CalcManager.TitanInfo);
        }

        private string GetCalculatorDesc()
        {
            switch (CalculatorIndex)
            {
                case 0:
                    return "Вычисление эффективности прокачки стихийных атрефактов";
                case 1:
                    return "Вычисление эффективности прокачки обликов";

                default: return string.Empty;
            }
        }

        public static bool CheckAllTitanStatsIsNoZero(TitanInfo titanInfo)
        {
            if (titanInfo == null) return false;

            return titanInfo.TotalHp > 0
                   && titanInfo.TotalAttack > 0
                   && titanInfo.ElementalOffenceArt?.StatValue > 0
                   && titanInfo.ElementalOffenceArt?.IncreaseStatValue > 0
                   && titanInfo.ElementalOffenceArt?.LevelUpCost > 0
                   && titanInfo.ElementalOffenceArt?.StatValue > 0
                   && titanInfo.ElementalOffenceArt?.IncreaseStatValue > 0
                   && titanInfo.ElementalOffenceArt?.LevelUpCost > 0;
        }
    }
}