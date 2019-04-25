using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.DB
{

    public class SealArtInfoDbo
    {
        public int Id { get; set; }
        public ArtefactType ArtefactType { get; set; }
        public int Level { get; set; }
        public double Hp { get; set; }
        public double Attack { get; set; }
        public int StarCount { get; set; }
        public int LevelUpCost { get; set; }
    }
}