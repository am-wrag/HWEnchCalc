using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using HWEnchCalc.Titan.ArtefactData;

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
        public TotemType TotemType { get; set; }

        public List<TitanStatPerStar> StatsPerLevelPerStar = new List<TitanStatPerStar>();

        public int MinStarLevel => StatsPerLevelPerStar.Min(t => t.StarCount);
        public int MaxStarLevel => StatsPerLevelPerStar.Max(t => t.StarCount);

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