namespace HWEnchCalc.Titan.ArtefactData
{
    public class ElementArtLevelUpInfo
    {
        public string LevelInfo { get; }
        /// <summary>
        /// Значение характеристики для данного уровня
        /// </summary>
        public int StatValue { get; }
        /// <summary>
        /// Цена в эссенциях для повышение уровня с текущего на +1
        /// </summary>
        public int LvlUpCostValue { get; }

        /// <summary>
        /// На сколько изменится стат если увеличить ему уровень с текущего на +1
        /// </summary>
        public int IncreaseStatValue { get; }

        public ElementArtLevelUpInfo(string levelInfo, int statValue, int lvlUpCostValue, int increaseStatValue)
        {
            LevelInfo = levelInfo;
            StatValue = statValue;
            LvlUpCostValue = lvlUpCostValue;
            IncreaseStatValue = increaseStatValue;
        }

        public static ElementArtLevelUpInfo Empty()
        {
            return new ElementArtLevelUpInfo(string.Empty, new int(), new int(), new int());
        }
    }
}