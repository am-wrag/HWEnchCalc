using System.Collections.Generic;
using HWEnchCalc.Common;
using HWEnchCalc.DB;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Titan.ArtefactData
{
    public class ElementalArtInfo : NotifyPropertyChangedBase
    {
        public List<int> LevelVariants { get; }

        public ArtefactType ArtefactType { get; private set; }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                UpdateStats();
                PropertyChangedByMember();
            }
        }
        public int StarCount
        {
            get => _starCount;
            set
            {
                _starCount = value;
                UpdateStatsByStarCount();
                PropertyChangedByMember();
            }
        }
        /// <summary>
        /// Количество стата на данном уровне
        /// </summary>
        public double StatValue
        {
            get => _statValue;
            set
            {
                _statValue = value;
                PropertyChangedByMember();
            }
        }
        
        /// <summary>
        /// На сколько увеличится стат при изменение уровня на +1
        /// </summary>
        public double IncreaseStatValue
        {
            get => _increaseStatValue;
            set
            {
                _increaseStatValue = value;
                PropertyChangedByMember();
            }
        }
        /// <summary>
        /// Количество эссенций нужное для повышения уровня
        /// </summary>
        public double LevelUpCost
        {
            get => _essenceValue;
            set
            {
                _essenceValue = value;
                PropertyChangedByMember();
            }
        }

        private readonly ElementalArtefactHelper _artefactHelper;
        private double _statValue;
        private double _increaseStatValue;
        private double _essenceValue;
        private int _level;
        private int _starCount = 1;
        private double _starRaito = 1;
       
        public ElementalArtInfo(ArtefactType artefactType, TitanSourceDataHelper artefactHelper)
        {
            ArtefactType = artefactType;
            _artefactHelper = artefactHelper.ElementArtefactHelper;
            LevelVariants = _artefactHelper.GetArtLvlUpVariants(ArtefactType.ElementalOffence);
        }

        private void UpdateStats()
        {
            var artLvlUp = _artefactHelper.GetElementArtLevelUpInfo(Level, ArtefactType);
            LevelUpCost = artLvlUp.LvlUpCostValue;
            StatValue = artLvlUp.StatValue * _starRaito;
            IncreaseStatValue = artLvlUp.IncreaseStatValue * _starRaito;
        }

        private void UpdateStatsByStarCount()
        {
            _starRaito = _artefactHelper.GetElementalArtRaito(StarCount);
            UpdateStats();
        }

        public void UpdateFromDbo(ElementArtInfoDbo artInfo)
        {
            ArtefactType = artInfo.ArtefactType;
            Level = artInfo.Level;
            StarCount = artInfo.StarCount;
            StatValue = artInfo.StatValue;
            IncreaseStatValue = artInfo.IncreaseStatValue;
            LevelUpCost = artInfo.LevelUpCost;
        }
    }
}