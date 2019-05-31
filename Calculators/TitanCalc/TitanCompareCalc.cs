using System;
using HWEnchCalc.Common;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanCompareCalc : NotifyPropertyChangedBase
    {
        public double DamagePrecResult { get; private set; }
        public double HpPrecResult { get; private set; }
        public double EssencePrecResult { get; private set; }
        public double GuiseStonePrecResult { get; private set; }

        public string DamageResultText { get; private set; }

        public string HpResultText { get; private set; }
        public string EssenceResultText { get; private set; }
        public string GuiseStoneResultText { get; private set; }

        private const string ResultEqualsTextBase = "Значения равны";
        private const string LeftMoreResultText = "> Слева больше";
        private const string RightMoreResultText = "Справа больше <";


        public void Caclulate(TitanCompareData leftTitanData, TitanCompareData rightTitanData)
        {
            CalculateDamage(leftTitanData, rightTitanData);
            CalculateHp(leftTitanData, rightTitanData);
            CalculateEssence(leftTitanData, rightTitanData);
            CalculateGuise(leftTitanData, rightTitanData);
        }

        private void CalculateGuise(TitanCompareData leftTitanData, TitanCompareData rightTitanData)
        {
            var diff = leftTitanData.TotalGuiseStone - rightTitanData.TotalGuiseStone;

            if (diff < 0)
            {
                GuiseStonePrecResult = rightTitanData.TotalGuiseStone - leftTitanData.TotalGuiseStone;
                GuiseStoneResultText = RightMoreResultText;
            }
            if (diff > 0)
            {
                GuiseStonePrecResult = diff;
                GuiseStoneResultText = LeftMoreResultText;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (diff == 0)
            {
                GuiseStonePrecResult = 0;
                GuiseStoneResultText = ResultEqualsTextBase;
            }

            PropertyChangedByName(nameof(GuiseStonePrecResult));
            PropertyChangedByName(nameof(GuiseStoneResultText));
        }

        private void CalculateEssence(TitanCompareData leftTitanData, TitanCompareData rightTitanData)
        {
            var diff = leftTitanData.TotalEsscence - rightTitanData.TotalEsscence;

            if (diff < 0)
            {
                EssencePrecResult = rightTitanData.TotalEsscence - leftTitanData.TotalEsscence;
                EssenceResultText = RightMoreResultText;
            }
            if (diff > 0)
            {
                EssencePrecResult = diff;
                EssenceResultText = LeftMoreResultText;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (diff == 0)
            {
                EssencePrecResult = 0;
                EssenceResultText = ResultEqualsTextBase;
            }

            PropertyChangedByName(nameof(EssencePrecResult));
            PropertyChangedByName(nameof(EssenceResultText));
        }

        private void CalculateHp(TitanCompareData leftTitanData, TitanCompareData rightTitanData)
        {
            var rait = leftTitanData.EffectiveHp / rightTitanData.EffectiveHp;

            if (rait < 1)
            {
                HpPrecResult = Math.Round((1 / rait - 1) * 100, 2);
                HpResultText = RightMoreResultText;
            }
            if (rait > 1)
            {
                HpPrecResult = Math.Round((rait - 1) * 100, 2);
                HpResultText = LeftMoreResultText;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (rait == 1)
            {
                HpPrecResult = 0;
                HpResultText = ResultEqualsTextBase;
            }

            PropertyChangedByName(nameof(HpPrecResult));
            PropertyChangedByName(nameof(HpResultText));
        }

        private void CalculateDamage(TitanCompareData leftTitanData, TitanCompareData rightTitanData)
        {
            var rait = leftTitanData.EffectiveDamage / rightTitanData.EffectiveDamage;

            if (rait < 1)
            {
                DamageResultText = RightMoreResultText;
                DamagePrecResult = Math.Round((1 / rait - 1) * 100, 2);
            }
            if (rait > 1)
            {
                DamagePrecResult = Math.Round((rait - 1) * 100, 2);
                DamageResultText = LeftMoreResultText;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (rait == 1)
            {
                DamagePrecResult = 0;
                DamageResultText = ResultEqualsTextBase;
            }

            PropertyChangedByName(nameof(DamageResultText));
            PropertyChangedByName(nameof(DamagePrecResult));
        }
     
    }
}