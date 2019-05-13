namespace HWEnchCalc.Titan.Guise
{
    public class GuiseSourseInfo
    {
        public int Level { get; set; }
        public int StatValue { get; set; }
        public int IncreaseStatValue { get; set; }
        public int LvlUpCost { get; set; }

        public GuiseSourseInfo(int level, int statValue, int increaseStatValue, int lvlUpCost)
        {
            Level = level;
            StatValue = statValue;
            IncreaseStatValue = increaseStatValue;
            LvlUpCost = lvlUpCost;
        }

        public static GuiseSourseInfo Empty()
        {
            return new GuiseSourseInfo(new int(), new int(), new int(), new int());
        }
    }
}