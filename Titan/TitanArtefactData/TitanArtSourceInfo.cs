namespace HWEnchCalc.Titan.TitanArtefactData
{
    public class TitanArtSourceInfo
    {
        public int Level { get; set; }
        public int StatValue { get; set; }
        public int UpToThisLevelCost { get; set; }
        public TitanArtSourceInfo(int level, int statValue, int upToThisLevelCost)
        {
            UpToThisLevelCost = upToThisLevelCost;
            StatValue = statValue;
            Level = level;
        }
    }
}