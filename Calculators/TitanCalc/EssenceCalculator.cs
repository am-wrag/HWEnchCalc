using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class EssenceCalculator : NotifyPropertyChangedBase
    {
        public double AtackIncreaseEffectivePrec => Math.Round(_attackIncreaseEffective - 1, 5) * 100;
        public double HpIncreaseEffectivePrec => Math.Round(_hpIncreaseEffective - 1, 5) * 100;
        public double Result { get; private set; }
        public string ResultDesc { get; private set; } = "Результат:";

        private const string AttackBetterThenDefMessage = "Результат: атака лучше защиты";
        private const string DefBetterThenAttackMessage = "Результат: защита лучше атаки";

        private double _hpIncreaseEffective = 1;
        private double _attackIncreaseEffective = 1;

        public void Calculate(TitanInfo titan)
        {
            if (!TitanCalculatorCore.CheckAllTitanStatsIsNoZero(titan)) return;

            var effectiveHpStart = titan.TotalHp * (1 + titan.ElementalDefenceArt.StatValue / 300000);
            var effectiveHpFinish = titan.TotalHp * (1 + (titan.ElementalDefenceArt.StatValue 
                                                     + titan.ElementalDefenceArt.IncreaseStatValue) / 300000);
            _hpIncreaseEffective = effectiveHpFinish / effectiveHpStart;
            var hpIncreaseEffectivePerEssence = (_hpIncreaseEffective - 1) / titan.ElementalDefenceArt.LevelUpCost;

            var effectiveAtackStart = titan.TotalAttack + titan.ElementalOffenceArt.StatValue;
            var effectiveAtackFinish = titan.TotalAttack + titan.ElementalOffenceArt.StatValue 
                                                    + titan.ElementalOffenceArt.IncreaseStatValue;
            _attackIncreaseEffective = effectiveAtackFinish / effectiveAtackStart;
            var attackIncreaseEffectivePerEssence = (_attackIncreaseEffective - 1) / titan.ElementalOffenceArt.LevelUpCost;

            var totalRait = hpIncreaseEffectivePerEssence / attackIncreaseEffectivePerEssence;

            if (totalRait > 1)
            {
                ResultDesc = DefBetterThenAttackMessage;
                Result = (totalRait - 1) * 100;
            }
            else
            {
                ResultDesc = AttackBetterThenDefMessage;
                Result = (1 / totalRait - 1) * 100;
            }

            Result = Math.Round(Result, 2);

            PropertyChangedByName(nameof(Result));
            PropertyChangedByName(nameof(ResultDesc));
            PropertyChangedByName(nameof(AtackIncreaseEffectivePrec));
            PropertyChangedByName(nameof(HpIncreaseEffectivePrec));
        }
    }
}