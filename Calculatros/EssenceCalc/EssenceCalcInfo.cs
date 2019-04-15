using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculatros.EssenceCalc
{
    public class EssenceCalcInfo : NotifyPropertyChangedBase
    {
        [Key] public int Id { get; set; }
        public long Ticks { get; set; }
        public ArtefactInfo DefArt { get; set; } = new ArtefactInfo { ArtefactType = ArtefactType.ElementalDef };
        public ArtefactInfo AtackArt { get; set; } = new ArtefactInfo { ArtefactType = ArtefactType.ElementalAtc };
        public TitanBaseStats TitanBaseStats { get; set; } = new TitanBaseStats();

        public EssenceCalcInfo()
        {
            Ticks = DateTime.Now.Ticks;

            CreatePath();
        }

        public void CreatePath()
        {
            DefArt.PropertyChanged += Calculate;
            AtackArt.PropertyChanged += Calculate;
        }

        public EssenceCalcShortInfo ToShortInfo()
        {
            return new EssenceCalcShortInfo(Id, Ticks);
        }

        private void Calculate(object sender, PropertyChangedEventArgs args)
        {
            PropertyChangedByMember();
        }
    }
}