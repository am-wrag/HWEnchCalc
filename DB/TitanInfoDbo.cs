using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HWEnchCalc.DB
{
    //[Table("[TitanInfos]")]
    public class TitanInfoDbo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int StarCount { get; set; }
        public int TotemLevel { get; set; }
        public int TotemStars { get; set; }
        public double TotalAttack { get; set; }
        public double TotalHp { get; set; }
        public long Ticks { get; set; }

        public ElementArtInfoDbo ElementalOffenceArtefact { get; set; }
        public ElementArtInfoDbo ElementalDefenceAtrefact { get; set; }
        public SealArtInfoDbo SealArtefact { get; set; }
        public List<GuiseInfoDbo> Guises { get; set; }
    }
}