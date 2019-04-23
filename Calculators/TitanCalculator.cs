using System;
using System.ComponentModel;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators
{
    public class TitanCalculator : NotifyPropertyChangedBase
    {
        public double AtackIncreaseEffective { get; set; }
        public double HpIncreaseEffective { get; set; }
        public double Result { get; private set; }
        public string ResultDesc { get; private set; } = "Результат";
        public TitanCalculationDataManager CalcManager { get; set; }
       
        private const string AtackBetterThenDefMessage = "Атака лучше защиты, результат в %";
        private const string DefBetterThenAtackMessage = "Защита лучше атаки, результат в %";

        public TitanCalculator(Configuration config)
        {
            CalcManager = new TitanCalculationDataManager(config);
            CalcManager.PropertyChanged += Calculate;
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