using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.Titan.Helper
{
    public class SealArtefactHelper
    {
        public const string SealSuppArtExceptedName = "Support";
        public const string SealOffenceArtExceptedName = "Offence";
        public const string SealDefenceExceptedName = "Defence";

        /// <summary>
        /// Не реализовано
        /// </summary>
        public const int DefaultLvlUpCost = 0; 

        private readonly Dictionary<ArtefactType, List<SealArtSourceInfo>> _artSourceInfo
            = new Dictionary<ArtefactType, List<SealArtSourceInfo>>();
       

        public SealArtefactHelper(Configuration config)
        {
            GetArtefactInfos(config.GameInfo.TitanDatas);
        }

        public SealArtSourceInfo GetLevelInfo(int level, ArtefactType artefactType)
        {
            if (!_artSourceInfo.ContainsKey(artefactType) || level == 0)
            {
                return SealArtSourceInfo.Empty();
            }

            return _artSourceInfo[artefactType].Find(t => t.Level == level);
        }

        public List<int> GetLevelVariants(ArtefactType artefactType)
        {
            if (!_artSourceInfo.ContainsKey(artefactType)) return new List<int>();

            return _artSourceInfo[artefactType]
                .Select(a => a.Level)
                .ToList();
        }

        private void GetArtefactInfos(TitanData titanData)
        {
            var titansArtDir = new DirectoryInfo(titanData.ArtefactLevelDataFolder);

            foreach (var titanArt in titansArtDir.GetFiles())
            {
                var artPath = titanArt.FullName;

                var artString = File.ReadAllLines(artPath);

                GetTitanArtefactInfo(titanArt.Name, artString);
            }
        }

        private void GetTitanArtefactInfo(string fileName, IReadOnlyList<string> levelsData)
        {
            var artLvlInfo = new List<SealArtSourceInfo>();
            var artefactType = GetArtefactTypeByFileName(fileName);

            for (var i = levelsData.Count - 1; i >= 0; i--)
            {
                var levelData = levelsData[i].Split('\t');

                var level = int.Parse(levelData[0]);
                var attack = int.Parse(levelData[1]);
                var hp = int.Parse(levelData[2]);

                artLvlInfo.Add(new SealArtSourceInfo(level, attack, hp, DefaultLvlUpCost));
            }
            _artSourceInfo[artefactType] = artLvlInfo;
        }

        private ArtefactType GetArtefactTypeByFileName(string fileName)
        {
            if (fileName.Contains(SealSuppArtExceptedName))
            {
                return ArtefactType.Support;
            }
            if (fileName.Contains(SealDefenceExceptedName))
            {
                return ArtefactType.Defence;
            }
            if (fileName.Contains(SealOffenceArtExceptedName))
            {
                return ArtefactType.Offence;
            }

            return ArtefactType.None;
        }
        
        public double GetSealArtRaito(int starCount)
        {
            switch (starCount)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.25;
                case 3:
                    return 1.5;
                case 4:
                    return 2;
                case 5:
                    return 2.5;
                case 6:
                    return 3.75;
                default: return 0;
            }
        }

        public BitmapImage GetImage(ArtefactType artefactType)
        {
            switch (artefactType)
            {
                case ArtefactType.Defence:
                    return new BitmapImage(new Uri("Titan/TitanPic/DefenceSeal.png", UriKind.RelativeOrAbsolute));
                case ArtefactType.Offence:
                    return new BitmapImage(new Uri("Titan/TitanPic/OffenceSeal.png", UriKind.RelativeOrAbsolute));
                case ArtefactType.Support:
                    return new BitmapImage(new Uri("Titan/TitanPic/SupportSeal.png", UriKind.RelativeOrAbsolute));
                default:
                    return new BitmapImage();
            }
        }
    }
}