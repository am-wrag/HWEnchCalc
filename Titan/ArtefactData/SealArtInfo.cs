using System.Collections.Generic;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;
using HWEnchCalc.DB;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Titan.ArtefactData
{
    public class SealArtInfo : NotifyPropertyChangedBase
    {
        public ArtefactType ArtefactType { get; private set; }
        public List<int> LevelVariants => _artefactHelper.GetLevelVariants(ArtefactType);

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

        public double Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                PropertyChangedByMember();
            }
        }

        public double Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                PropertyChangedByMember();
            }
        }

        public int StarCount
        {
            get => _starCount;
            set
            {
                _starCount = value;
                UpdateStarRaito();
                UpdateStats();
                PropertyChangedByMember();
            }
        }

        public int LevelUpCost { get; set; }
        public BitmapImage Image { get; private set; }

        private int _level;
        private readonly SealArtefactHelper _artefactHelper;
        private double _attack;
        private double _hp;
        private int _starCount = 1;
        private double _starRaito = 1;

        public SealArtInfo(ArtefactType artefactType, TitanSourceDataHelper titanHelper)
        {
            ArtefactType = artefactType;
            Image = titanHelper.SealArtefactHelper.GetImage(artefactType);
            _artefactHelper = titanHelper.SealArtefactHelper;
        }

        public void UpdateFromDbo(SealArtInfoDbo artInfo)
        {
            ArtefactType = artInfo.ArtefactType;
            Level = artInfo.Level;
            StarCount = artInfo.StarCount;
            Attack = artInfo.Attack;
            Hp = artInfo.Hp;
            LevelUpCost = artInfo.LevelUpCost;
            PropertyChangedByName(nameof(LevelVariants));
        }

        public void Update(TitanSourceInfo titanSourceInfo)
        {
            Image = _artefactHelper.GetImage(titanSourceInfo.SealArtefact);
            ArtefactType = titanSourceInfo.SealArtefact;
            UpdateStats();
            PropertyChangedByName(nameof(LevelVariants));
            PropertyChangedByName(nameof(ArtefactType));
            PropertyChangedByName(nameof(Image));
        }

        private void UpdateStats()
        {
            var levelInfo = _artefactHelper.GetLevelInfo(Level, ArtefactType);
            Attack = levelInfo.Attack * _starRaito;
            Hp = levelInfo.Hp * _starRaito;
            LevelUpCost = levelInfo.UpToThisLevelCost;
        }

        private void UpdateStarRaito()
        {
            _starRaito = _artefactHelper.GetSealArtRaito(StarCount);
        }
    }
}