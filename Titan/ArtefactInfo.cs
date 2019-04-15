using System.ComponentModel.DataAnnotations;
using HWEnchCalc.Common;

namespace HWEnchCalc.Titan
{
    public class ArtefactInfo : NotifyPropertyChangedBase
    {
        [Key] public int Id { get; set; }
        public string LevelInfo
        {
            get => _levelInfo;
            set
            {
                _levelInfo = value;
                SetArtefactParametersByLevelTable();
            }
        }
        public ArtefactType ArtefactType { get; set; }

        public double BaseStatValue
        {
            get => _baseStatValue * _starRaito;
            set
            {
                _baseStatValue = value / _starRaito;
                PropertyChangedByMember();
            }
        }

        public double IncreaseStatValue
        {
            get => _increaseStatValue * _starRaito;
            set
            {
                _increaseStatValue = value / _starRaito;
                PropertyChangedByMember();
            }
        }

        public double EssenceValue
        {
            get => _essenceValue;
            set
            {
                _essenceValue = value;
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
            }
        }


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
            BaseStatValue = artLvlUp.StatValue;
            EssenceValue = artLvlUp.EssenceValue;
            IncreaseStatValue = artLvlUp.IncreaseStatValue;
            PropertyChangedByName(nameof(BaseStatValue));
            PropertyChangedByName(nameof(EssenceValue));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }

        private void UpdateStatsByStarCount()
        {
            _starRaito = TitanHelper.GetArtefactStarRaito(ArtefactType, StarCount);
            PropertyChangedByName(nameof(BaseStatValue));
            PropertyChangedByName(nameof(IncreaseStatValue));
        }
    }
}