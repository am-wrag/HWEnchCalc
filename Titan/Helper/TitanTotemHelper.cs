using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.Titan.Helper
{
    public class TotemHelper
    {
        private const string EarthTotemExceptedName = "Earth";
        private const string WaterTotemExceptedName = "Water";
        private const string FireTotemExceptedName = "Fire";

        private readonly Dictionary<TotemType, List<TotemLevelInfo>> _totemLevels = new Dictionary<TotemType, List<TotemLevelInfo>>();
        
        private const double TotemNoBonusValue = 0;

        public TotemHelper(Configuration config)
        {
            if (config?.GameInfo?.TitanDatas?.TotemFolder != null)
            {
                _totemLevels = ParseTotemData(config.GameInfo.TitanDatas.TotemFolder);
            }
        }

        public (double hp, double attack) GetTotemHpAndAttackBonus(TotemType totemType, int totemLevel, int totemStars)
        {
            var hp = TotemNoBonusValue;
            var attack = TotemNoBonusValue;

            if (!_totemLevels.ContainsKey(totemType) || totemLevel == 0)
            {
                return (hp, attack);
            }

            var totemLevelInfo = _totemLevels[totemType].Find(t => t.Level == totemLevel);
            var starRaito = GetStarRaito(totemStars);

            switch (totemType)
            {
                case TotemType.Earth:
                    hp = totemLevelInfo.StatBonus * starRaito;
                break;
                case TotemType.Water:
                    attack = totemLevelInfo.StatBonus * starRaito;
                break;
                case TotemType.Fire:
                    attack = totemLevelInfo.StatBonus * starRaito;
                break;
            }

            return (hp, attack);
        }

        private Dictionary<TotemType, List<TotemLevelInfo>> ParseTotemData(string titanDatasTotemFolder)
        {
            var result = new Dictionary<TotemType, List<TotemLevelInfo>>();

            var totemFiles = new DirectoryInfo(titanDatasTotemFolder).GetFiles();

            foreach (var file in totemFiles)
            {
                if (file.Name.Contains(EarthTotemExceptedName))
                {
                    result[TotemType.Earth] = GetTotemLevels(file).ToList();
                }
                if (file.Name.Contains(WaterTotemExceptedName))
                {
                    result[TotemType.Water] = GetTotemLevels(file).ToList();
                }
                if (file.Name.Contains(FireTotemExceptedName))
                {
                    result[TotemType.Fire] = GetTotemLevels(file).ToList();
                }
            }

            return result;
        }

        private IEnumerable<TotemLevelInfo> GetTotemLevels(FileInfo file)
        {
            var totemLevels = File.ReadAllLines(file.FullName);

            foreach (var levelRow in totemLevels)
            {
                var levelData = levelRow.Split('\t');
                var level = int.Parse(levelData[0]);
                var statBonus = int.Parse(levelData[1]);

                yield return new TotemLevelInfo(level, statBonus);
            }
        }


        private double GetStarRaito(int starCount)
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

        public string GetName(TitanSourceInfo titanSourceInfo)
        {
            if (titanSourceInfo == null) return string.Empty;
           
            switch (titanSourceInfo.TotemType)
            {
                case TotemType.Earth:
                    return "Земля";
                case TotemType.Fire:
                    return "Огонь";
                case TotemType.Water:
                    return "Вода";
                default: return string.Empty;;
            }
        }
    }
}