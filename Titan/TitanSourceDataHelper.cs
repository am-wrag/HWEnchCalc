using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.TitanArtefactData;

namespace HWEnchCalc.Titan
{
    public class TitanSourceDataHelper
    {
        public const string FirstArtExceptedName = "FirstArt";
        public const string SecondArtExceptedName = "SecondArt";
        public const string SealSuppArtExceptedName = "Support";
        public const string SealOffenceArtExceptedName = "Offence";
        public const string SealDefenceExceptedName = "Defence";

        private readonly List<TitanSourceInfo> _titanSourceInfo = new List<TitanSourceInfo>();
        private readonly Dictionary<ArtefactType, List<TitanArtSourceInfo>> _titanArtSourceInfo
            = new Dictionary<ArtefactType, List<TitanArtSourceInfo>>();

        private readonly Dictionary<ArtefactType, List<TitanArtLevelUpInfo>> _titanArtLvlUpInfo
            = new Dictionary<ArtefactType, List<TitanArtLevelUpInfo>>();

        public TitanSourceDataHelper(Configuration config)
        {
            if (config?.GameInfo?.TitanDatas != null)
            {
                _titanSourceInfo = GetSourceTitanInfo(config.GameInfo.TitanDatas).ToList();
                GetTitanArtefactInfos(config.GameInfo.TitanDatas);
            }
        }

        public List<string> GetTitanNames()
        {
            return _titanSourceInfo.Select(t => t.Name).ToList();
        }

        public TitanArtLevelUpInfo GetTitanArtLevelUpInfo(string levelInfo, ArtefactType artefactType)
        {
            if (!_titanArtSourceInfo.ContainsKey(artefactType))
            {
                return TitanArtLevelUpInfo.Empty();
            }

            return _titanArtLvlUpInfo[artefactType].Find(t => t.LevelInfo == levelInfo);
        }

        public TitanSourceInfo GetTitanSourseInfo(string titanName)
        {
            return _titanSourceInfo.Find(t => t.Name == titanName);
        }

        public void GetTitanArtefactInfos(TitanDatas titanDatas)
        {
            var titansArtDir = new DirectoryInfo(titanDatas.ArtefactLevelDataFolder);

            foreach (var titanArt in titansArtDir.GetFiles())
            {
                var artPath = titanArt.FullName;

                if(!File.Exists(artPath)) continue;

                var artString = File.ReadAllLines(artPath);

                GetTitanArtefactInfo(titanArt.Name, artString);
            }
        }
        public List<string> GetArtLvlUpVariants(ArtefactType artefactType)
        {
            if (!_titanArtLvlUpInfo.ContainsKey(artefactType)) return new List<string>();

            return _titanArtLvlUpInfo[artefactType]
                .Select(a => a.LevelInfo)
                .ToList();
        }
        
        public List<string> GetSealArtLvlVariants(ArtefactType artefactType)
        {
            return _titanArtSourceInfo.ContainsKey(artefactType) 
                ? _titanArtSourceInfo[artefactType].Select(v => v.Level.ToString()).ToList()
                : new List<string>();
        }

        public double GetArtefactStarRaito(ArtefactType artefactType, int starCount)
        {
            if (artefactType == ArtefactType.ElementalDefence
                || artefactType == ArtefactType.ElementalOffence)
            {
                return GetElementalArtRaito(starCount);
            }

            return GetSealArtRaito(starCount);
        }

        private void GetTitanArtefactInfo(string fileName, IReadOnlyList<string> levelsData)
        {
            var artInfos = new List<TitanArtSourceInfo>();
            var artLvlUpInfos = new List<TitanArtLevelUpInfo>();
            var artefactType = GetArtefactTypeByFileName(fileName);

            //последний уровень нам не нужен, поскольку выше его прокачать артефакт нельзя. Поэтому -2
            for (var i = 0; i < levelsData.Count - 2; i++)
            {
                var levelData = levelsData[i].Split('\t');
                var nextLevelData = levelsData[i + 1].Split('\t');

                var level = int.Parse(levelData[0]);
                var statValue = int.Parse(levelData[1]);

                var nextLevelStatValue = int.Parse(nextLevelData[1]);
                var costToLevelUpValue = int.Parse(nextLevelData[2]);

                var levelUpInfo = $"{level}->{level + 1}";
                var increaseStatValue = nextLevelStatValue - statValue;

                artInfos.Add(new TitanArtSourceInfo(level, statValue, costToLevelUpValue));

                artLvlUpInfos.Add(new TitanArtLevelUpInfo(
                    levelUpInfo,
                    statValue,
                    costToLevelUpValue,
                    increaseStatValue));
            }

            _titanArtSourceInfo[artefactType] = artInfos;
            _titanArtLvlUpInfo[artefactType] = artLvlUpInfos;
        }

        private ArtefactType GetArtefactTypeByFileName(string fileName)
        {
            if (fileName.Contains(FirstArtExceptedName))
            {
                return ArtefactType.ElementalOffence;
            }
            if (fileName.Contains(SecondArtExceptedName))
            {
                return ArtefactType.ElementalDefence;
            }
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
    

        private IEnumerable<TitanSourceInfo> GetSourceTitanInfo(TitanDatas titanDatas)
        {
            var titansDir = new DirectoryInfo(titanDatas.TitansFolder);

            foreach (var titanFile in titansDir.GetFiles())
            {
                var titanString = File.ReadAllText(titanFile.FullName);
                yield return JsonParser<TitanSourceInfo>.Deserialize(titanString);
            }
        }
        private double GetElementalArtRaito(int starCount)
        {
            switch (starCount)
            {
                case 1:
                    return 1;
                case 2:
                    return 1.2;
                case 3:
                    return 1.5;
                case 4:
                    return 2;
                case 5:
                    return 2.5;
                case 6:
                    return 3;
                default: return 0;
            }
        }
        private double GetSealArtRaito(int starCount)
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
    }
}