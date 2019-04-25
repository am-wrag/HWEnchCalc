using System;
using HWEnchCalc.Titan;

namespace HWEnchCalc.DB
{
    public class TitanInfoDbo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int StarCount { get; set; }
        public int TotemLevel { get; set; }
        public int TotemStars { get; set; }
        public double Attack { get; set; }
        public double Hp { get; set; }
        public long Ticks { get; set; }

        public ElementArtInfoDbo FirstArtefact { get; set; }
        public ElementArtInfoDbo SecondArtefact { get; set; }
        public SealArtInfoDbo SealArtefact { get; set; }
    }
}