using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.DB
{
    public class ElementArtInfoDto
    {
        public int Id { get; set; }
        public ArtefactType ArtefactType { get; set; }
        public int Level { get; set; }
        public double StatValue { get; set; }
        public double IncreaseStatValue { get; set; }
        public int StarCount { get; set; }
        public double LevelUpCost { get; set; }
    }
}