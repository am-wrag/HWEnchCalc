using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanShortInfo : NotifyPropertyChangedBase
    {
        [Column("Id", true)]
        public int Id { get; set; }
        [Column("Имя")]
        public string Name { get; set; }

        [Column("Дата/время записи")]
        public string DateTime { get; set; }

        public TitanShortInfo(string name, int id, long ticks)
        {
            Name = name;
            Id = id;
            DateTime = new DateTime(ticks).ToString("yy-MM-dd HH:mm");
        }
    }
}