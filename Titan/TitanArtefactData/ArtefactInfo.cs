using System.Collections.Generic;
using HWEnchCalc.Common;
using HWEnchCalc.DB;

namespace HWEnchCalc.Titan.TitanArtefactData
{
    public class ArtefactInfo : NotifyPropertyChangedBase
    {
        public List<string> LevelVariants { get; }

        public ArtefactType ArtefactType { get; private set; }

        public string LevelInfo
        {
            get => _levelInfo;
            set
            {
                _levelInfo = value;
                SetArtefactParametersByLevelTable();
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
            get => _baseStatValue * _starRaito;
            set
            {
                _baseStatValue = value / _starRaito;
                PropertyChangedByMember();
            }
        }
        
        /// <summary>
        /// На сколько увеличится стат при изменение уровня на +1
        /// </summary>
        public double IncreaseStatValue
        {
            get => _increaseStatValue * _starRaito;
            set
            {
                _increaseStatValue = value / _starRaito;
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

        private readonly TitanSourceDataHelper _titanHelper;
        private double _baseStatValue;
        private double _increaseStatValue;
        private double _essenceValue;
        private string _levelInfo;
        private int _starCount = 1;
        private double _starRaito = 1;
       
        public ArtefactInfo(ArtefactType artefactType, TitanSourceDataHelper titanHelper)
        {
            ArtefactType = artefactType;
            _titanHelper = titanHelper;
            LevelVariants = titanHelper.GetArtLvlUpVariants(ArtefactType.ElementalOffence);
        }

        private void SetArtefactParametersByLevelTable()
        {
            var artLvlUp = _titanHelper.GetTitanArtLevelUpInfo(LevelInfo, ArtefactType);
            StatValue = artLvlUp.StatValue;
            LevelUpCost = artLvlUp.LvlUpCostValue;
            IncreaseStatValue = artLvlUp.IncreaseStatValue;
            PropertyChangedByName(nameof(StatValue));
            PropertyChangedByName(nameof(LevelUpCost));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }

        private void UpdateStatsByStarCount()
        {
            _starRaito = _titanHelper.GetArtefactStarRaito(ArtefactType, StarCount);
            PropertyChangedByName(nameof(StatValue));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }

        public void Update(TitatnArtefactInfoDbo artInfo)
        {
            ArtefactType = artInfo.ArtefactType;
            LevelInfo = artInfo.LevelInfo;
            StarCount = artInfo.StarCount;
            StatValue = artInfo.StatValue;
            IncreaseStatValue = artInfo.IncreaseStatValue;
            LevelUpCost = artInfo.LevelUpCost;
        }
    }
}