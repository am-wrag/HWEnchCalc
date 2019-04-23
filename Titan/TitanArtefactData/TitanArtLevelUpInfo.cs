namespace HWEnchCalc.Titan.TitanArtefactData
{
    public class TitanArtLevelUpInfo
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

        public TitanArtLevelUpInfo(string levelInfo, int statValue, int lvlUpCostValue, int increaseStatValue)
        {
            LevelInfo = levelInfo;
            StatValue = statValue;
            LvlUpCostValue = lvlUpCostValue;
            IncreaseStatValue = increaseStatValue;
        }

        public static TitanArtLevelUpInfo Empty()
        {
            return new TitanArtLevelUpInfo(string.Empty, new int(), new int(), new int());
        }
    }
}