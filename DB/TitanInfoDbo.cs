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
        public double Attack { get; set; }
        public double Hp { get; set; }
        public long Ticks { get; set; }

        public TitatnArtefactInfoDbo FirstArtefact { get; set; }
        public TitatnArtefactInfoDbo SecondArtefact { get; set; }
        public TitatnArtefactInfoDbo SealArtefact { get; set; }

        public TitanInfoDbo()
        {

        }
    }
}