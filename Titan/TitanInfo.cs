using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;
using HWEnchCalc.DB;
using HWEnchCalc.Titan.ArtefactData;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Titan
{
    public class TitanInfo : NotifyPropertyChangedBase
    {
        public List<int> LevelVariants { get; set; } = new List<int>();
        public List<int> TotemLevelVariants { get; set; } = new List<int>();
        public BitmapImage FaceImage { get; set; }
        public BitmapImage BorderImage { get; set; }
        public ElementalArtInfo FirstArt { get; set; }
        public ElementalArtInfo SecondArt { get; set; }
        public SealArtInfo SealArt { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateTitanImage();
                UpdateSealArtefact();
                UpdateTitanStats();
                PropertyChangedByMember();
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                UpdateTitanStats();
                PropertyChangedByMember();
            }
        }

        public int StarCount
        {
            get => _starCount;
            set
            {
                _starCount = value;
                UpdateTitanStats();
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
        public int TotemLevel
        {
            get => _totemLevel;
            set
            {
                _totemLevel = value;
                UpdateTitanStats();
                PropertyChangedByMember();
            }
        }
        public int TotemStars
        {
            get => _totemStars;
            set
            {
                _totemStars = value;
                UpdateTitanStats();
                PropertyChangedByMember();
            }
        }

        private TitanSourceInfo TitanSourceInfo => _titanHelper.GetTitanSourseInfo(_name);
        private readonly TitanSourceDataHelper _titanHelper;
        private double _attack;
        private double _hp;
        private string _name;
        private int _totemLevel;
        private int _totemStars = 1;
        private int _starCount = 1;
        private int _level = 1;

        public TitanInfo(TitanSourceDataHelper titanHelper, int maxLevel)
        {
            _titanHelper = titanHelper;

            FirstArt = new ElementalArtInfo(ArtefactType.ElementalOffence, titanHelper);
            SecondArt = new ElementalArtInfo(ArtefactType.ElementalDefence, _titanHelper);
            SealArt = new SealArtInfo(ArtefactType.None, titanHelper);

            FirstArt.PropertyChanged += NotifyCalculator;
            SecondArt.PropertyChanged += NotifyCalculator;
            SealArt.PropertyChanged += UpdateStats;

            TotemLevelVariants.Add(0);//нулевой уровень = отсутствие тотема
            for (var i = maxLevel; i > 0; i--)
            {
                LevelVariants.Add(i);
                TotemLevelVariants.Add(i);
            }
        }
        
        public void UpdateTitanImage()
        {
            if (TitanSourceInfo == null)
            {
                FaceImage = new BitmapImage();
                BorderImage = new BitmapImage();
            }
            else
            {
                FaceImage = TitanSourceInfo.GetFaceImage();
                BorderImage = TitanSourceInfo.GetBorderImage();
            }

            PropertyChangedByName(nameof(FaceImage));
            PropertyChangedByName(nameof(BorderImage));
        }

        public void Update(TitanInfoDbo titanInfoDbo)
        {
            Name = titanInfoDbo.Name;
            Level = titanInfoDbo.Level;
            StarCount = titanInfoDbo.StarCount;
            Attack = titanInfoDbo.Attack;
            Hp = titanInfoDbo.Hp;
            TotemLevel = titanInfoDbo.TotemLevel;
            TotemStars = titanInfoDbo.TotemStars;
            FirstArt.UpdateFromDbo(titanInfoDbo.FirstArtefact);
            SecondArt.UpdateFromDbo(titanInfoDbo.SecondArtefact);
            SealArt.UpdateFromDbo(titanInfoDbo.SealArtefact);
        }

        private void UpdateStats(object sender, PropertyChangedEventArgs e)
        {
            UpdateTitanStats();
        }

        private void UpdateSealArtefact()
        {
            SealArt.Update(TitanSourceInfo);
            PropertyChangedByName(nameof(SealArt));
        }

        private void UpdateTitanStats()
        {
            if (TitanSourceInfo == null)
            {
                Hp = 0;
                Attack = 0;
                return;
            }

            if (StarCount < TitanSourceInfo.MinStarLevel || StarCount > TitanSourceInfo.MaxStarLevel)
            {
                StarCount = TitanSourceInfo.MinStarLevel;
            }

            var (hp, attack) = _titanHelper.GetHpAndAttack(TitanSourceInfo, Level, StarCount);
            var totemBonus = _titanHelper.TotemHelper.GetTotemHpAndAttackBonus(TitanSourceInfo.TotemType, TotemLevel, TotemStars);

            Hp = hp + totemBonus.hp + SealArt.Hp;
            Attack = attack + totemBonus.attack + SealArt.Attack;
        }

        private void NotifyCalculator(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedByMember();
        }
    }
}