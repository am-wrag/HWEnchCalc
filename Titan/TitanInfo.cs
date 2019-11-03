using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;
using HWEnchCalc.Titan.ArtefactData;
using HWEnchCalc.Titan.Guise;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Titan
{
    public class TitanInfo : NotifyPropertyChangedBase
    {
        public List<int> LevelVariants { get; } = new List<int>();
        public List<int> TotemLevelVariants { get; } = new List<int>();
        public ObservableCollection<GuiseInfo> Guises { get; }
        public BitmapImage FaceImage { get; private set; }
        public BitmapImage BorderImage { get; private set; }
        public ElementalArtInfo ElementalOffenceArt { get; }
        public ElementalArtInfo ElementalDefenceArt { get; }
        public SealArtInfo SealArt { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateTitanImage();
                UpdateGuises();
                UpdateSealArtefact();
                UpdateTitanStats();
                PropertyChangedByMember();
                PropertyChangedByName(nameof(TotemName));
                PropertyChangedByName(nameof(TotemImage));
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

        public double TotalAttack
        {
            get => _totalAttack;
            set
            {
                _totalAttack = value;
                PropertyChangedByMember();
            }
        }

        public double TotalHp
        {
            get => _totalHp;
            set
            {
                _totalHp = value;
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

        public string TotemName => _titanHelper.TotemHelper.GetName(TitanSourceInfo);
        [NotMapped] public BitmapImage TotemImage => _titanHelper.TotemHelper.GetTotemImage(TitanSourceInfo);

        private TitanSourceInfo TitanSourceInfo => _titanHelper.GetTitanSourceInfo(_name);
        private readonly TitanSourceDataHelper _titanHelper;
        private double _totalAttack;
        private double _totalHp;
        private string _name;
        private int _totemLevel;
        private int _totemStars = 1;
        private int _starCount = 1;
        private int _level = 1;

        public TitanInfo(TitanSourceDataHelper titanHelper)
        {
            _titanHelper = titanHelper;

            var defaultGuise = new GuiseInfo(GuiseType.None, titanHelper);
            Guises = new ObservableCollection<GuiseInfo> {defaultGuise};

            ElementalOffenceArt = new ElementalArtInfo(ArtefactType.ElementalOffence, titanHelper);
            ElementalDefenceArt = new ElementalArtInfo(ArtefactType.ElementalDefence, titanHelper);
            SealArt = new SealArtInfo(ArtefactType.None, titanHelper);

            SetSubscriptions();
            SetTitanLevels();
        }

        public TitanInfo(
            string name,
            int level,
            double totalAttack,
            double totalHp,
            int starCount,
            int totemLevel,
            int totemStars,
            ElementalArtInfo elementalOffenceArt,
            ElementalArtInfo elementalDefenceArt,
            SealArtInfo sealArtInfo,
            ObservableCollection<GuiseInfo> guises,
            TitanSourceDataHelper titanHelper)
        {
            _titanHelper = titanHelper;

            _name = name;
            _level = level;
            _totalAttack = totalAttack;
            _totalHp = totalHp;
            _starCount = starCount;
            _totemLevel = totemLevel;
            _totemStars = totemStars;

            Guises = guises;
            ElementalOffenceArt = elementalOffenceArt;
            ElementalDefenceArt = elementalDefenceArt;
            SealArt = sealArtInfo;

            SetSubscriptions();
            SetTitanLevels();
            UpdateTitanImage();
        }

        private void SetTitanLevels()
        {
            TotemLevelVariants.Add(0); //нулевой уровень = отсутствие тотема
            for (var i = _titanHelper.TitanMaxLevel; i > 0; i--)
            {
                LevelVariants.Add(i);
                TotemLevelVariants.Add(i);
            }
        }

        private void SetSubscriptions()
        {
            ElementalOffenceArt.PropertyChanged += NotifyCalculator;
            ElementalDefenceArt.PropertyChanged += NotifyCalculator;
            SealArt.PropertyChanged += UpdateStats;
            foreach (var guise in Guises)
            {
                guise.PropertyChanged += UpdateStats;
            }
        }

        private void UpdateTitanImage()
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

        private void UpdateGuises()
        {
            if(Guises == null) return;

            Guises.Clear();

            foreach (var guiseType in TitanSourceInfo.Guises)
            {
                var guise = new GuiseInfo(guiseType, _titanHelper);
                guise.PropertyChanged += UpdateStats;
                Guises.Add(guise);
            }
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
                TotalHp = 0;
                TotalAttack = 0;
                return;
            }

            if (StarCount < TitanSourceInfo.MinStarLevel || StarCount > TitanSourceInfo.MaxStarLevel)
            {
                StarCount = TitanSourceInfo.MinStarLevel;
            }

            var HpAndAttackPerTitanLevel = _titanHelper.GetHpAndAttack(TitanSourceInfo, Level, StarCount);
            var totemBonus =
                _titanHelper.TotemHelper.GetTotemHpAndAttackBonus(TitanSourceInfo.TotemType, TotemLevel, TotemStars);
            var guiseBonus = _titanHelper.GuiseHelper.GetGuisesHpAndAttackBonus(Guises);

            TotalHp = HpAndAttackPerTitanLevel.Hp + totemBonus.hp + SealArt.Hp + guiseBonus.hp;
            TotalAttack = HpAndAttackPerTitanLevel.Attack + totemBonus.attack + SealArt.Attack + guiseBonus.attack;
        }

        private void NotifyCalculator(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedByMember();
        }
    }
}