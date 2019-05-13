using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.Titan.Helper
{
    public class ElementalArtefactHelper
    {
        public const string FirstArtExceptedName = "ElementalOffence";
        public const string SecondArtExceptedName = "ElementalDefence";

        private readonly Dictionary<ArtefactType, List<ElementArtLevelUpInfo>> _titanArtLvlUpInfo
            = new Dictionary<ArtefactType, List<ElementArtLevelUpInfo>>();

        public ElementalArtefactHelper(Configuration config)
        {
            GetTitanArtefactInfos(config.GameInfo.TitanDatas);
        }

        public ElementArtLevelUpInfo GetElementArtLevelUpInfo(int level, ArtefactType artefactType)
        {
            if (!_titanArtLvlUpInfo.ContainsKey(artefactType))
            {
                return ElementArtLevelUpInfo.Empty();
            }

            return _titanArtLvlUpInfo[artefactType].Find(t => t.Level == level);
        }

        public List<int> GetArtLvlUpVariants(ArtefactType artefactType)
        {
            if (!_titanArtLvlUpInfo.ContainsKey(artefactType)) return new List<int>();

            return _titanArtLvlUpInfo[artefactType]
                .Select(a => a.Level)
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

            var levelData = levelsData[levelsData.Count - 1].Split('\t');
            var statValue = int.Parse(levelData[1]);

            // последний увровень, выше его прокачать нельзя
            artLvlUpInfos.Add(new ElementArtLevelUpInfo(
                levelsData.Count - 1, statValue, new int(), new int()));

            //последний уровень нам не нужен, поскольку выше его прокачать артефакт нельзя.
            for (var i = levelsData.Count - 2; i >= 0 ; i--)
            {
                levelData = levelsData[i].Split('\t');
                var nextLevelData = levelsData[i + 1].Split('\t');

                var level = int.Parse(levelData[0]);
                statValue = int.Parse(levelData[1]);

                var nextLevelStatValue = int.Parse(nextLevelData[1]);
                var costToLevelUpValue = int.Parse(nextLevelData[2]);
                
                var increaseStatValue = nextLevelStatValue - statValue;

                artLvlUpInfos.Add(new ElementArtLevelUpInfo(level, statValue, costToLevelUpValue, increaseStatValue));
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

        public int GetTotalEssenceCount(ArtefactType artefactType, int level)
        {
            var result = 0;
            var levels = _titanArtLvlUpInfo[artefactType];

            for (var i = 1; i <= level; i++)
            {
                result += levels[i - 1].LvlUpCostValue;
            }

            return result;
        }
    }
}