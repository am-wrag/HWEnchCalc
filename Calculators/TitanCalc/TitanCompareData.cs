using System;
using System.Collections.ObjectModel;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanCompareData : NotifyPropertyChangedBase
    {
        public ObservableCollection<TitanShowedData> TitanList { get; }
        public TitanInfo SelectedTitan { get; private set; }

        public double EffectiveDamage => SelectedTitan.TotalAttack + SelectedTitan.ElementalOffenceArt.StatValue;

        public double EffectiveHp => GetEffectiveHp();
        public int TotalEsscence => GetTotalEssence();
        public int TotalGuiseStone => GetTotalGuiseStone();

        public int SelectedIndex
        {
            get => _selectedIndex;

            set
            {
                _selectedIndex = value;
                UpdateSelectedTitan();
            }
        }

        private int _selectedIndex;
        private readonly TitanSourceDataHelper _titanHelper;

        public TitanCompareData(ObservableCollection<TitanShowedData> titansData, TitanSourceDataHelper titanHelper)
        {
            _titanHelper = titanHelper;
            TitanList = titansData;
            SelectedTitan = new TitanInfo(titanHelper);
        }

        private double GetEffectiveHp()
        {
            var effectiveHp = SelectedTitan.TotalHp * (1 + SelectedTitan.ElementalDefenceArt.StatValue / 300000);

            return Math.Round(effectiveHp, 1);
        }

        private int GetTotalEssence()
        {
            var offenceArt = SelectedTitan.ElementalOffenceArt;
            var defenceArt = SelectedTitan.ElementalDefenceArt;

            var result = _titanHelper.ElementArtefactHelper.GetTotalEssenceCount(offenceArt.ArtefactType, offenceArt.Level) +
                         _titanHelper.ElementArtefactHelper.GetTotalEssenceCount(defenceArt.ArtefactType, defenceArt.Level);

            return result;
        }

        private int GetTotalGuiseStone()
        {
            var result = 0;

            foreach (var guise in SelectedTitan.Guises)
            {
                result += _titanHelper.GuiseHelper.GetTotalGuiseStoneCount(guise.GuiseType, guise.Level);
            }

            return result;
        }

        private void UpdateSelectedTitan()
        {
            SelectedTitan = TitanList[SelectedIndex].TitanInfo;
            PropertyChangedByName(nameof(SelectedTitan));
            PropertyChangedByName(nameof(EffectiveDamage));
            PropertyChangedByName(nameof(EffectiveHp));
            PropertyChangedByName(nameof(TotalEsscence));
            PropertyChangedByName(nameof(TotalGuiseStone));
        }
    }
}