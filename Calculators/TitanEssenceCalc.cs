using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using HWEnchCalc.Calculators.EssenceCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Core;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators
{
    public class TitanEssenceCalc : NotifyPropertyChangedBase
    {
        public List<string> FirstArtLevelUpVariants => SourceArtLevelUpInfo.FirstArtLevels.Select(a => a.LevelInfo).ToList();
        public List<string> SecondArtLevelUpVariants => SourceArtLevelUpInfo.SecondArtLevels.Select(a => a.LevelInfo).ToList();
        public double AtackIncreaseEffective { get; set; }
        public double HpIncreaseEffective { get; set; }
        public double Result { get; private set; }
        public string ResultDesc { get; private set; } = "Результат";
        public EssCalcResultManager ResultManager { get; set; }

        private const string AtackBetterThenDef = "Атака лучше защиты, результат в %";
        private const string DefBetterThenAtack = "Защита лучше атаки, результат в %";

        public TitanEssenceCalc(TitanStats titanBaseStats, Configuration config)
        {
            ResultManager = new EssCalcResultManager(titanBaseStats, config);
            ResultManager.PropertyChanged += Calculate;
        }

        private void Calculate(object sender, PropertyChangedEventArgs args)
        {
            if(args.PropertyName != CalculatorManager.PropertyWhenModifyNeedCalculate) return;

            var calcResult = ResultManager.Result;

            if (!CheckAllTitanStatsIsNoZero(calcResult)) return;

            var titanBaseStats = calcResult.TitanBaseStats;

            var effectiveHpStart = titanBaseStats.Hp * (1 + calcResult.DefArt.StatValue / 300000);
            var effectiveHpFinish = titanBaseStats.Hp *
                                    (1 + (calcResult.DefArt.StatValue +
                                          calcResult.DefArt.IncreaseStatValue) / 300000);
            HpIncreaseEffective = effectiveHpFinish / effectiveHpStart;
            var effectiveHpIncreasePerEssence = (HpIncreaseEffective - 1) / calcResult.DefArt.EssenceValue;

            var effectiveAtackStart = titanBaseStats.Attack + calcResult.AttackArt.StatValue;
            var effectiveAtackFinish = titanBaseStats.Attack + calcResult.AttackArt.StatValue +
                                       calcResult.AttackArt.IncreaseStatValue;
            AtackIncreaseEffective = effectiveAtackFinish / effectiveAtackStart;
            var effectiveAtackIncreasePerEssence = (AtackIncreaseEffective - 1) / calcResult.AttackArt.EssenceValue;

            var totalRait = effectiveHpIncreasePerEssence / effectiveAtackIncreasePerEssence;

            if (totalRait > 1)
            {
                ResultDesc = DefBetterThenAtack;
                Result = (totalRait - 1) * 100;
            }
            else
            {
                ResultDesc = AtackBetterThenDef;
                Result = (effectiveAtackIncreasePerEssence / effectiveHpIncreasePerEssence - 1) * 100;
            }

            Result = Math.Round(Result, 2);

            PropertyChangedByName(nameof(Result));
            PropertyChangedByName(nameof(ResultDesc));
            PropertyChangedByName(nameof(HpIncreaseEffective));
            PropertyChangedByName(nameof(AtackIncreaseEffective));
            
        }

        public static bool CheckAllTitanStatsIsNoZero(EssenceCalcResult currentCalcInfo)
        {
            if (currentCalcInfo == null) return false;

            return currentCalcInfo.TitanBaseStats.Hp != 0
                   && currentCalcInfo.TitanBaseStats.Attack != 0
                   && currentCalcInfo.DefArt.StatValue != 0
                   && currentCalcInfo.DefArt.IncreaseStatValue != 0
                   && currentCalcInfo.DefArt.EssenceValue != 0
                   && currentCalcInfo.AttackArt.StatValue != 0
                   && currentCalcInfo.AttackArt.IncreaseStatValue != 0
                   && currentCalcInfo.AttackArt.EssenceValue != 0;
        }
    }
}