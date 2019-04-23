using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;
using HWEnchCalc.DB;
using HWEnchCalc.Titan.TitanArtefactData;

namespace HWEnchCalc.Titan
{
    public class TitanInfo : NotifyPropertyChangedBase
    {
        public List<int> LevelVariants { get; set; } = new List<int>();
        public BitmapImage TitanFaceImg { get; set; }
        public BitmapImage TitanBorderImg { get; set; }
        public ArtefactInfo FirstArt { get; set; }
        public ArtefactInfo SecondArt { get; set; }
        public ArtefactInfo SealArt { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateTitanImage();
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

        private TitanSourceInfo TitanSourceInfo => _titanHelper.GetTitanSourseInfo(_name);
        private readonly TitanSourceDataHelper _titanHelper;
        private double _attack;
        private double _hp;
        private string _name;
        private int _starCount = 1;
        private int _level = 1;

        public TitanInfo(TitanSourceDataHelper titanHelper, int maxLevel)
        {
            _titanHelper = titanHelper;

            FirstArt = new ArtefactInfo(ArtefactType.ElementalOffence, titanHelper);
            SecondArt = new ArtefactInfo(ArtefactType.ElementalDefence, _titanHelper);
            SealArt = new ArtefactInfo(ArtefactType.None, titanHelper);

            FirstArt.PropertyChanged += NotifyCalculator;
            SecondArt.PropertyChanged += NotifyCalculator;
            SealArt.PropertyChanged += NotifyCalculator;

            for (var i = 1; i <= maxLevel; i++)
            {
                LevelVariants.Add(i);
            }
        }



        public void UpdateTitanImage()
        {
            if (TitanSourceInfo == null)
            {
                TitanFaceImg = new BitmapImage();
                TitanBorderImg = new BitmapImage();
            }
            else
            {
                TitanFaceImg = TitanSourceInfo.GetFaceImage();
                TitanBorderImg = TitanSourceInfo.GetBorderImage();
            }

            PropertyChangedByName(nameof(TitanFaceImg));
            PropertyChangedByName(nameof(TitanBorderImg));
        }

        public void Update(TitanInfoDbo titanInfoDbo)
        {
            Name = titanInfoDbo.Name;
            Level = titanInfoDbo.Level;
            StarCount = titanInfoDbo.StarCount;
            Attack = titanInfoDbo.Attack;
            Hp = titanInfoDbo.Hp;
            FirstArt.Update(titanInfoDbo.FirstArtefact);
            SecondArt.Update(titanInfoDbo.SecondArtefact);
            SealArt.Update(titanInfoDbo.SealArtefact);
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

            var (hp, atack) = TitanSourceInfo.GetHpAndAttack(Level, StarCount);
            Hp = hp;
            Attack = atack;
        }

        private void NotifyCalculator(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedByMember();
        }
    }
}