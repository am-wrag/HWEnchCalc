using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.EssenceCalc
{
    public class EssenceCalcResultShort : NotifyPropertyChangedBase
    {
        [Column("Id", true)]
        public int Id { get; set; }

        [Column("Дата/время записи")]
        public string DateTime { get; set; }

        public EssenceCalcResultShort(int id, long ticks)
        {
            Id = id;
            DateTime = new DateTime(ticks).ToString("yy-MM-dd HH:mm");
        }
    }
}