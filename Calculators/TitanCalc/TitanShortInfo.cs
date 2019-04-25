using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanShortInfo : NotifyPropertyChangedBase
    {
        [Column("Id", true)]
        public int Id { get; }
        [Column("Имя")]
        public string Name { get; }

        [Column("Время записи")]
        public string DateTime { get; }

        public TitanShortInfo(string name, int id, long ticks)
        {
            Name = name;
            Id = id;
            DateTime = new DateTime(ticks).ToString("yy:MM:dd HH:mm");
        }
    }
}