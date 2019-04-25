namespace HWEnchCalc.Titan.ArtefactData
{
    public class TotemLevelInfo
    {
        public int Level { get; }
        public int StatBonus { get; }

        public TotemLevelInfo(int level, int statBonus)
        {
            Level = level;
            StatBonus = statBonus;
        }
    }
}