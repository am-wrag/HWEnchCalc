using System.Collections.Generic;
using System.Windows.Documents;

namespace HWEnchCalc.Common
{
    public class TitanArtLevelUp
    {
        public string LevelInfo { get; }
        /// <summary>
        /// Значение характеристики для данного уровня
        /// </summary>
        public int StatValue { get; }
        /// <summary>
        /// Цена в эссенциях для повышение уровня с текущего на +1
        /// </summary>
        public int EssenceValue { get; }

        /// <summary>
        /// На сколько изменится стат если увеличить ему уровень с текущего на +1
        /// </summary>
        public int IncreaseStatValue { get; }

        public TitanArtLevelUp(string levelInfo, int statValue, int essenceValue, int increaseStatValue)
        {
            LevelInfo = levelInfo;
            StatValue = statValue;
            EssenceValue = essenceValue;
            IncreaseStatValue = increaseStatValue;
        }

        public static TitanArtLevelUp Empty()
        {
            return new TitanArtLevelUp(string.Empty, new int(), new int(), new int());
        }
    }
}