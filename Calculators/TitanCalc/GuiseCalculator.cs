using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Guise;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class GuiseCalculator : NotifyPropertyChangedBase
    {
       
        public double Result { get; private set; }
        public double FirstGuiseLvlUpEffectivePrec => Math.Round(_firstGuiseLvlUpEffective - 1, 5) * 100;
        public double SecondGuiseLvlUpEffectivePrec => Math.Round(_secondGuiseLvlUpEffective - 1, 5) * 100;

        public string FirstGuiseDesc { get; private set; }
        public string SecondGuiseDesc { get; private set; }

        private double _firstGuiseLvlUpEffective = 1;
        private double _secondGuiseLvlUpEffective = 1;

        public string ResultDesc { get; private set; } = "Результат:";

        public void Calculate(TitanInfo titanInfo)
        {
            if (titanInfo.Guises.Count < 2)
            {
                return;
            }

            var firstGuise = titanInfo.Guises[0];
            var firstGuiseShowName = TitanGuiseHelper.GetShowNameCore(firstGuise.GuiseType);
            FirstGuiseDesc = GetGuiseLvlUpDesc(firstGuise);
            _firstGuiseLvlUpEffective = GetGuiseEffectives(firstGuise, titanInfo);
            var firstGuiseEffectivesPerLvlUpCost = _firstGuiseLvlUpEffective / firstGuise.LvlUpCost;

            var secondGuise = titanInfo.Guises[1];
            var secondGuiseShowName = TitanGuiseHelper.GetShowNameCore(secondGuise.GuiseType);
            SecondGuiseDesc = GetGuiseLvlUpDesc(secondGuise);
            _secondGuiseLvlUpEffective = GetGuiseEffectives(secondGuise, titanInfo);
            var secondGuiseEffectivesPerLvlUpCost = _secondGuiseLvlUpEffective / secondGuise.LvlUpCost;

            var resulRaito = firstGuiseEffectivesPerLvlUpCost / secondGuiseEffectivesPerLvlUpCost;

            if (resulRaito > 1)
            {
                ResultDesc = $"Прокачка облика {firstGuiseShowName} эффективней";
                Result = (resulRaito - 1) * 100;
            }
            else
            {
                ResultDesc = $"Прокачка облика {secondGuiseShowName} эффективней";
                Result = (1 / resulRaito - 1) * 100;
            }
            PropertyChangedByName(nameof(Result));
            PropertyChangedByName(nameof(ResultDesc));
            PropertyChangedByName(nameof(FirstGuiseDesc));
            PropertyChangedByName(nameof(SecondGuiseDesc));
            PropertyChangedByName(nameof(FirstGuiseLvlUpEffectivePrec));
            PropertyChangedByName(nameof(SecondGuiseLvlUpEffectivePrec));
        }

        private string GetGuiseLvlUpDesc( GuiseInfo guise)
        {
            switch (guise.GuiseType)
            {
                case GuiseType.Hp:
                    return "Здоровье: выживаемость+";
                case GuiseType.Offence:
                    return "Атака: урон+";
                case GuiseType.ElementalOffence:
                    return "Стих урон: урон+";
                case GuiseType.ElementalDefence:
                    return "Стих защита: выживаемость+";
                default: return string.Empty;
            }
        }

        private double GetGuiseEffectives(GuiseInfo guise, TitanInfo titanInfo)
        {
            switch (guise.GuiseType)
            {
                case GuiseType.Hp:
                    return GetHpEffectives(guise, titanInfo);
                case GuiseType.Offence:
                    return GetAttackEffectives(guise, titanInfo);
                case GuiseType.ElementalOffence:
                    return GetElementalOffenceEffectives(guise, titanInfo);
                case GuiseType.ElementalDefence:
                    return GetElementalDefenceEffectives(guise, titanInfo);
                default: return 0;
            }
        }

        private double GetElementalOffenceEffectives(GuiseInfo guise, TitanInfo titanInfo)
        {
            var attackStart = titanInfo.TotalAttack + titanInfo.ElementalOffenceArt.StatValue;
            var attackFinish = attackStart + guise.IncreaseStatValue;

            var attackIncreaseEffective = attackFinish / attackStart;

            return attackIncreaseEffective;
        }

        private double GetAttackEffectives(GuiseInfo guise, TitanInfo titanInfo)
        {
            var attackStart = titanInfo.TotalAttack + titanInfo.ElementalOffenceArt.StatValue;
            var attackFinish = attackStart + guise.IncreaseStatValue;

            var attackIncreaseEffective = attackFinish / attackStart;

            return attackIncreaseEffective;
        }

        private double GetHpEffectives(GuiseInfo guise, TitanInfo titanInfo)
        {
            //Мы не учитываем бонус от элемент защиты поскольку она даёт процентную прибавку, а не 
            //абсолютную как в случае со стихийной атакой, где мы берем полную формулу
            var hpStart = titanInfo.TotalHp;
            var hpFinish = hpStart + guise.IncreaseStatValue;

            var hpIncreaseEffective = hpFinish / hpStart;

            return hpIncreaseEffective;
        }
        private double GetElementalDefenceEffectives(GuiseInfo guise, TitanInfo titan)
        {
            var effectiveHpStart = titan.TotalHp * (1 + titan.ElementalDefenceArt.StatValue / 300000);

            var effectiveHpFinish = titan.TotalHp * (1 + (titan.ElementalDefenceArt.StatValue
                                                     + titan.ElementalDefenceArt.IncreaseStatValue) / 300000);

            var hpIncreaseEffective = effectiveHpFinish / effectiveHpStart;

            return hpIncreaseEffective;
        }
    }
}