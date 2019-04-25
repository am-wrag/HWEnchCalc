using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace HWEnchCalc.Calculators
{
    public class TitanCalculator : NotifyPropertyChangedBase
    {
        public double AtackIncreaseEffective { get; private set; }
        public double HpIncreaseEffective { get; private set; }
        public double Result { get; private set; }
        public string ResultDesc { get; private set; } = "Результат:";
        public TitanCalculationDataManager CalcManager { get; private set; }
       
        private const string AtackBetterThenDefMessage = "Результат: атака лучше защиты";
        private const string DefBetterThenAtackMessage = "Результат: защита лучше атаки";

        public TitanCalculator(Configuration config)
        {
            ParseConfigAsync(config);
        }

        private async void ParseConfigAsync(Configuration config)
        {
            var awaitDialogController = await Global<ProgressDialogController>.GetAwaitDialog();
            awaitDialogController.SetIndeterminate();
            //awaitDialogController.SetProgress(0.9);

            var titanHelper = await ParseTitanData(config);

            CalcManager = new TitanCalculationDataManager(titanHelper, config);
            CalcManager.PropertyChanged += Calculate;

            PropertyChangedByName(nameof(CalcManager));

            await awaitDialogController.CloseAsync();
        }

        public async Task<TitanSourceDataHelper> ParseTitanData(Configuration config)
        {
            var titanHelper = new TitanSourceDataHelper();
            return await Task.Factory.StartNew(() => titanHelper.Fill(config));
        }

        private void Calculate(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName != CalculatorManager.PropertyWhenModifyNeedCalculate) return;

            var titan = CalcManager.TitanInfo;

            if (!CheckAllTitanStatsIsNoZero(titan)) return;

            var effectiveHpStart = titan.Hp * (1 + titan.SecondArt.StatValue / 300000);
            var effectiveHpFinish = titan.Hp * (1 + (titan.SecondArt.StatValue + titan.SecondArt.IncreaseStatValue) / 300000);
            HpIncreaseEffective = effectiveHpFinish / effectiveHpStart;
            var effectiveHpIncreasePerEssence = (HpIncreaseEffective - 1) / titan.SecondArt.LevelUpCost;

            var effectiveAtackStart = titan.Attack + titan.FirstArt.StatValue;
            var effectiveAtackFinish = titan.Attack + titan.FirstArt.StatValue + titan.FirstArt.IncreaseStatValue;
            AtackIncreaseEffective = effectiveAtackFinish / effectiveAtackStart;
            var effectiveAtackIncreasePerEssence = (AtackIncreaseEffective - 1) / titan.FirstArt.LevelUpCost;

            var totalRait = effectiveHpIncreasePerEssence / effectiveAtackIncreasePerEssence;

            if (totalRait > 1)
            {
                ResultDesc = DefBetterThenAtackMessage;
                Result = (totalRait - 1) * 100;
            }
            else
            {
                ResultDesc = AtackBetterThenDefMessage;
                Result = (effectiveAtackIncreasePerEssence / effectiveHpIncreasePerEssence - 1) * 100;
            }

            Result = Math.Round(Result, 2);

            PropertyChangedByName(nameof(Result));
            PropertyChangedByName(nameof(ResultDesc));
            PropertyChangedByName(nameof(HpIncreaseEffective));
            PropertyChangedByName(nameof(AtackIncreaseEffective));
            
        }

        public static bool CheckAllTitanStatsIsNoZero(TitanInfo titanInfo)
        {
            if (titanInfo == null) return false;

            return titanInfo.Hp > 0
                   && titanInfo.Attack > 0
                   && titanInfo.SecondArt?.StatValue > 0
                   && titanInfo.SecondArt?.IncreaseStatValue > 0
                   && titanInfo.SecondArt?.LevelUpCost > 0
                   && titanInfo.FirstArt?.StatValue > 0
                   && titanInfo.FirstArt?.IncreaseStatValue > 0
                   && titanInfo.FirstArt?.LevelUpCost > 0;
        }
    }
}