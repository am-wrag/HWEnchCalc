namespace HWEnchCalc.Titan.ArtefactData
{
    public class SealArtSourceInfo
    {
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Hp { get; set; }
        public int UpToThisLevelCost { get; set; }
        public SealArtSourceInfo(int level, int attack, int hp, int upToThisLevelCost)
        {
            Level = level;
            Attack = attack;
            Hp = hp;
            UpToThisLevelCost = upToThisLevelCost;
        }

        public static SealArtSourceInfo Empty()
        {
            return  new SealArtSourceInfo(0, 0, 0, 0);
        }
    }
}