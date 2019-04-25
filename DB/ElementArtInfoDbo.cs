using HWEnchCalc.Titan.ArtefactData;

namespace HWEnchCalc.DB
{
    public class ElementArtInfoDbo
    {
        public int Id { get; set; }
        public ArtefactType ArtefactType { get; set; }
        public string LevelInfo { get; set; }
        public double StatValue { get; set; }
        public double IncreaseStatValue { get; set; }
        public int StarCount { get; set; }
        public double LevelUpCost { get; set; }
    }
}