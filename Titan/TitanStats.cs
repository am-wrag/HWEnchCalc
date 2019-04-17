using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;
using HWEnchCalc.Common;

namespace HWEnchCalc.Titan
{
    public class TitanStats : NotifyPropertyChangedBase
    {
        [Key] public int Id { get; set; }
        [NotMapped] public List<string> TitanVariants => TitanHelper.TitanNames;
        [NotMapped] public List<int> LevelVariants => TitanHelper.LevelVariants;
        [NotMapped] public BitmapImage TitanFaceImg { get; set; }
        [NotMapped] public BitmapImage TitanBorderImg { get; set; }

        public string TitanName
        {
            get => _titanName;
            set
            {
                _titanName = value;
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

        private void UpdateTitanStats()
        {
        }

        public int Attack
        {
            get => _attack;
            set
            {
                _attack = value;
                PropertyChangedByMember();
            }
        }

        public int Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                PropertyChangedByMember();
            }
        }

        private int _attack;
        private int _hp;
        private string _titanName;
        private int _starCount = 1;
        private int _level = 1;

        public TitanStats()
        {
        }

        public void UpdateTitanImage()
        {
            TitanFaceImg = TitanHelper.GetTitanFaceImgByName(TitanName);
            TitanBorderImg = TitanHelper.GetTitanBorderImgByName(TitanName);
            PropertyChangedByName(nameof(TitanFaceImg));
            PropertyChangedByName(nameof(TitanBorderImg));
        }

        public void Update(TitanStats stats)
        {
            TitanName = stats.TitanName;
            Level = stats.Level;
            StarCount = stats.StarCount;
            Attack = stats.Attack;
            Hp = stats.Hp;
        }
    }
}