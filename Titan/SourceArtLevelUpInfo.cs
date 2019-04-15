using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Common;
using HWEnchCalc.Config;

namespace HWEnchCalc.Titan
{
    public class SourceArtLevelUpInfo
    {
        public static List<TitanArtLevelUp> FirstArtLevels = new List<TitanArtLevelUp>();
        public static List<TitanArtLevelUp> SecondArtLevels = new List<TitanArtLevelUp>();
        public static List<TitanArtLevelUp> ThirdAtcArtLevels = new List<TitanArtLevelUp>();
        public static List<TitanArtLevelUp> ThirdDefArtLevels = new List<TitanArtLevelUp>();
        public static List<TitanArtLevelUp> ThirdSuppArtLevels = new List<TitanArtLevelUp>();

        public SourceArtLevelUpInfo(Configuration config)
        {
            GetArtInfo(config);
        }

        private void GetArtInfo(Configuration config)
        {
            FirstArtLevels = ParseToTitanLevels(config.CalculatorInfo.ArtefactLevelInfo.FirstArtLevelsPath).ToList();
            SecondArtLevels = ParseToTitanLevels(config.CalculatorInfo.ArtefactLevelInfo.SecondArtLevelsPath).ToList();
            ThirdAtcArtLevels = ParseToTitanLevels(config.CalculatorInfo.ArtefactLevelInfo.ThirdArtActTypeLevelsPath)
                .ToList();
            ThirdDefArtLevels = ParseToTitanLevels(config.CalculatorInfo.ArtefactLevelInfo.ThirdArtDefTypeLevelsPath)
                .ToList();
            ThirdSuppArtLevels = ParseToTitanLevels(config.CalculatorInfo.ArtefactLevelInfo.ThirdArtSuppTypeLevelsPath)
                .ToList();
        }

        public static IEnumerable<TitanArtLevelUp> ParseToTitanLevels(string levelsFilePath)
        {
            var levelsData = File.ReadAllLines(levelsFilePath);

            var levels = levelsData
                .Select(l =>
                {
                    var levelData = l.Split('\t');
                    return new
                    {
                        level = levelData[0],
                        statValue = int.Parse(levelData[1]),
                        essenceValue = int.Parse(levelData[2]),
                    };
                })
                //.OrderBy(l => l.level)
                .ToList();

            //последний уровень нам не нужен, поскольку выше его прокачать артефакт нельзя. Поэтому -2
            for (var i = 0; i < levels.Count - 2; i++)
            {
                var levelInfo = $"{levels[i].level}->{levels[i + 1].level}";
                var increaseStatValue = levels[i + 1].statValue - levels[i].statValue;

                yield return new TitanArtLevelUp(
                    levelInfo,
                    levels[i].statValue,
                    levels[i + 1].essenceValue,
                    increaseStatValue);
            }
        }

        public static TitanArtLevelUp GetArtefactLevelUpInfo(string levelInfo, ArtefactType artefactType)
        {
            switch (artefactType)
            {
                case ArtefactType.ElementalAtc:
                    return FirstArtLevels.First(a => a.LevelInfo == levelInfo);
                case ArtefactType.ElementalDef:
                    return SecondArtLevels.First(a => a.LevelInfo == levelInfo);
                case ArtefactType.ThirdAtcArt:
                    return ThirdAtcArtLevels.First(a => a.LevelInfo == levelInfo);
                case ArtefactType.ThirdDefArt:
                    return ThirdDefArtLevels.First(a => a.LevelInfo == levelInfo);
                case ArtefactType.ThirdSuppArt:
                    return ThirdSuppArtLevels.First(a => a.LevelInfo == levelInfo);
                default:
                    return TitanArtLevelUp.Empty();
            }
        }
    }
}