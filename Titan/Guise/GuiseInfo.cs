using System.Collections.Generic;
using HWEnchCalc.Common;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Titan.Guise
{
    public class GuiseInfo : NotifyPropertyChangedBase
    {
        public List<int> LevelVariants => _titanHelper.GuiseHelper.GetLevelVariants(GuiseType);
        public GuiseType GuiseType { get; }
        public string ShowName { get; }
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                UpdateStats();
            }
        }

        public double StatValue
        {
            get => _statValue;
            set
            {
                _statValue = value;
                PropertyChangedByMember();
            }
        }

        public double IncreaseStatValue
        {
            get => _increaseStatValue;
            set
            {
                _increaseStatValue = value;
                PropertyChangedByMember();
            }
        }

        public int LvlUpCost
        {
            get => _lvlUpCost;
            set
            {
                _lvlUpCost = value;
                PropertyChangedByMember();
            }
        }

        private readonly TitanSourceDataHelper _titanHelper;
        private int _level;
        private double _statValue;
        private double _increaseStatValue;
        private int _lvlUpCost;

        public GuiseInfo(GuiseType guiseType, TitanSourceDataHelper titanHelper)
        {
            GuiseType = guiseType;
            ShowName = TitanGuiseHelper.GetShowName(GuiseType);
            _titanHelper = titanHelper;
            UpdateStats();
        }

        private void UpdateStats()
        {
            var sourceInfo = _titanHelper.GuiseHelper.GetGuiseLevelInfo(Level, GuiseType);

            StatValue = sourceInfo.StatValue;
            IncreaseStatValue = sourceInfo.IncreaseStatValue;
            LvlUpCost = sourceInfo.LvlUpCost;
        }
    }
}