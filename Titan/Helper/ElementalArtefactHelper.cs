using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.Titan.Helper
{
    public class ElementalArtefactHelper
    {
        public const string FirstArtExceptedName = "FirstArt";
        public const string SecondArtExceptedName = "SecondArt";


        private readonly Dictionary<ArtefactType, List<ElementArtLevelUpInfo>> _titanArtLvlUpInfo
            = new Dictionary<ArtefactType, List<ElementArtLevelUpInfo>>();

        public ElementalArtefactHelper(Configuration config)
        {
            if (config?.GameInfo?.TitanDatas != null)
            {
                GetTitanArtefactInfos(config.GameInfo.TitanDatas);
            }
        }

        public ElementArtLevelUpInfo GetElementArtLevelUpInfo(string levelInfo, ArtefactType artefactType)
        {
            if (!_titanArtLvlUpInfo.ContainsKey(artefactType))
            {
                return ElementArtLevelUpInfo.Empty();
            }

            return _titanArtLvlUpInfo[artefactType].Find(t => t.LevelInfo == levelInfo);
        }

        public List<string> GetArtLvlUpVariants(ArtefactType artefactType)
        {
            if (!_titanArtLvlUpInfo.ContainsKey(artefactType)) return new List<string>();

            return _titanArtLvlUpInfo[artefactType]
                .Select(a => a.LevelInfo)
                .ToList();
        }

        public void GetTitanArtefactInfos(TitanDatas titanDatas)
        {
            var titansArtDir = new DirectoryInfo(titanDatas.ArtefactLevelDataFolder);

            foreach (var titanArt in titansArtDir.GetFiles())
            {
                var artPath = titanArt.FullName;

                if (!File.Exists(artPath)) continue;

                var artString = File.ReadAllLines(artPath);

                GetTitanArtefactInfo(titanArt.Name, artString);
            }
        }

        private void GetTitanArtefactInfo(string fileName, IReadOnlyList<string> levelsData)
        {
            var artLvlUpInfos = new List<ElementArtLevelUpInfo>();
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

                artLvlUpInfos.Add(new ElementArtLevelUpInfo(
                    levelUpInfo,
                    statValue,
                    costToLevelUpValue,
                    increaseStatValue));
            }
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

            return ArtefactType.None;
        }
       
        public double GetElementalArtRaito(int starCount)
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
    }
}