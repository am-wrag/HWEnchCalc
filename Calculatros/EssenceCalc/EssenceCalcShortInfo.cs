using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculatros.EssenceCalc
{
    public class EssenceCalcShortInfo : NotifyPropertyChangedBase
    {
        [Column("Id", true)]
        public int Id { get; set; }

        [Column("Дата/время записи")]
        public string DateTime { get; set; }

        public EssenceCalcShortInfo(int id, long ticks)
        {
            Id = id;
            DateTime = new DateTime(ticks).ToString("yy-MM-dd HH:mm");
        }
    }
}