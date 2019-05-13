using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using HWEnchCalc.Config;
using HWEnchCalc.Titan.Guise;

namespace HWEnchCalc.Titan.Helper
{
    public class TitanGuiseHelper
    {
        private readonly Dictionary<GuiseType, List<GuiseSourseInfo>> _guiseLevels
            = new Dictionary<GuiseType, List<GuiseSourseInfo>>();

        public TitanGuiseHelper(TitanDatas data)
        {
            GetGuiseLevels(data);
        }

        private void GetGuiseLevels(TitanDatas titanData)
        {
            var guisesDir = new DirectoryInfo(titanData.GuiseLevelsFolder);

            foreach (var file in guisesDir.GetFiles())
            {
                var guiseType = GetGuiseTypeByFileName(file);
                _guiseLevels[guiseType] = GetGuiseLevelsInfo(file).ToList();
            }
        }

        private GuiseType GetGuiseTypeByFileName(FileSystemInfo file)
        {
            var guiseName = file.Name;

            if (guiseName.Contains(GuiseType.Hp.ToString()))
            {
                return GuiseType.Hp;
            }

            if (guiseName.Contains(GuiseType.Offence.ToString()))
            {
                return GuiseType.Offence;
            }

            if (guiseName.Contains(GuiseType.ElementalDefence.ToString()))
            {
                return GuiseType.ElementalDefence;
            }

            if (guiseName.Contains(GuiseType.ElementalOffence.ToString()))
            {
                return GuiseType.ElementalOffence;
            }

            throw new ArgumentException($"Невозможно сопоставить облик из файла {guiseName}");
        }

        public int GetTotalGuiseStoneCount(GuiseType guiseType, int level)
        {
            if (!_guiseLevels.ContainsKey(guiseType))
            {
                return 0;
            }

            var guiseLevels = _guiseLevels[guiseType];

            var result = 0;

            for (var i = 1; i <= level; i++)
            {
                result += guiseLevels[i - 1].LvlUpCost;
            }

            return result;
        }

        private IEnumerable<GuiseSourseInfo> GetGuiseLevelsInfo(FileSystemInfo file)
        {
            var levels = File.ReadAllLines(file.FullName);
            var statValue = 0;
            for (var i = 0; i < levels.Length - 1; i++)
            {
                var levelData = levels[i].Split('\t');
                var nextLevelData = levels[i + 1].Split('\t');

                var level = int.Parse(levelData[0]);
                statValue = int.Parse(levelData[2]);
                var nextLevelStatValue = int.Parse(nextLevelData[2]);

                var increaseStatValue = nextLevelStatValue - statValue;

                var lvlUpCost = int.Parse(nextLevelData[1]);

                yield return new GuiseSourseInfo(level, statValue, increaseStatValue, lvlUpCost);
            }

            yield return new GuiseSourseInfo(levels.Length - 1, statValue, 0, 0);
        }

        public List<int> GetLevelVariants(GuiseType guiseType)
        {
            if (_guiseLevels.ContainsKey(guiseType))
            {
                return _guiseLevels[guiseType].Select(g => g.Level).ToList();
            }

            return new List<int>();
        }

        public GuiseSourseInfo GetGuiseLevelInfo(int level, GuiseType guiseType)
        {
            if (!_guiseLevels.ContainsKey(guiseType))
            {
                return GuiseSourseInfo.Empty();
            }

            return _guiseLevels[guiseType].Find(g => g.Level == level);
        }

        public static string GetShowName(GuiseType guiseType)
        {
            return $"Облик: {GetShowNameCore(guiseType)}";
        }

        public static string GetShowNameCore(GuiseType guiseName)
        {
            switch (guiseName)
            {
                case GuiseType.Hp: return "Здоровье";
                case GuiseType.Offence: return "Атака";
                case GuiseType.ElementalDefence: return "Стихийная защита";
                case GuiseType.ElementalOffence: return "Стихийная атака";
                default: return string.Empty;
            }
        }

        public (double hp, double attack) GetGuisesHpAndAttackBonus(ObservableCollection<GuiseInfo> guises)
        {
            double hp = 0;
            double attack = 0;

            foreach (var guise in guises)
            {
                if (guise.GuiseType == GuiseType.Offence)
                {
                    attack += guise.StatValue;
                }

                if (guise.GuiseType == GuiseType.Hp)
                {
                    hp += guise.StatValue;
                }
            }

            return (hp, attack);
        }
    }
}