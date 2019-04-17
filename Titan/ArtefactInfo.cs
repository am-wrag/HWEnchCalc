using System.ComponentModel.DataAnnotations;
using HWEnchCalc.Common;

namespace HWEnchCalc.Titan
{
    public class ArtefactInfo : NotifyPropertyChangedBase
    {
        [Key] public int Id { get; set; } = new  int();
        public string LevelUpInfo
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
        public double EssenceValue
        {
            get => _essenceValue;
            set
            {
                _essenceValue = value;
                PropertyChangedByMember();
            }
        }

        public ArtefactType ArtefactType { get; set; }

        private double _baseStatValue;
        private double _increaseStatValue;
        private double _essenceValue;
        private string _levelInfo;
        private int _starCount = 1;
        private double _starRaito = 1;

        public ArtefactInfo()
        {
        }

        private void SetArtefactParametersByLevelTable()
        {
            var artLvlUp = SourceArtLevelUpInfo.GetArtefactLevelUpInfo(_levelInfo, ArtefactType);
            StatValue = artLvlUp.StatValue;
            EssenceValue = artLvlUp.EssenceValue;
            IncreaseStatValue = artLvlUp.IncreaseStatValue;
            PropertyChangedByName(nameof(StatValue));
            PropertyChangedByName(nameof(EssenceValue));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }

        private void UpdateStatsByStarCount()
        {
            _starRaito = TitanHelper.GetArtefactStarRaito(ArtefactType, StarCount);
            PropertyChangedByName(nameof(StatValue));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }

        public void Update(ArtefactInfo artInfo)
        {
            LevelUpInfo = artInfo.LevelUpInfo;
            StarCount = artInfo.StarCount;
            StatValue = artInfo.StatValue;
            IncreaseStatValue = artInfo.IncreaseStatValue;
            EssenceValue = artInfo.EssenceValue;
            ArtefactType = artInfo.ArtefactType;
        }
    }
}