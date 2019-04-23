using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using HWEnchCalc.Titan.TitanArtefactData;

namespace HWEnchCalc.Titan
{
    public class TitanSourceInfo
    {
        public string Name { get;  set; }
        public int HpStart { get;  set; }
        public int AttackStart { get;  set; }
        public string FaceImage { get; set; }
        public string BorderImage { get; set; }
        public ArtefactType SealArtefact { get; set; }

        public List<TitanStatPerStar> StatsPerLevelPerStar = new List<TitanStatPerStar>();

        public int MinStarLevel => StatsPerLevelPerStar.Min(t => t.StarCount);
        public int MaxStarLevel => StatsPerLevelPerStar.Max(t => t.StarCount);

        //взято из игровых формул
        private const double TitalLevelPowСoefficient = 1.5;

        public (double Hp, double Atack) GetHpAndAttack(int level, int starCount)
        {
            var levelModyfier = Math.Pow(level, TitalLevelPowСoefficient);
            var statsPerStar = StatsPerLevelPerStar.Find(s => s.StarCount == starCount);

            var hp = statsPerStar.Hp * levelModyfier + HpStart;
            var atack = statsPerStar.Attack * levelModyfier + AttackStart;

            return (Math.Round(hp, 1), Math.Round(atack, 1));
        }

        public BitmapImage GetFaceImage()
        {
            var fi = new FileInfo(FaceImage);
            return new BitmapImage(new Uri(fi.FullName, UriKind.Absolute));
        }
        public BitmapImage GetBorderImage()
        {
            var fi = new FileInfo(BorderImage);
            return new BitmapImage(new Uri(fi.FullName, UriKind.Absolute));
        }
    }
}