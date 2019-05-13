using System;
using HWEnchCalc.Common;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanShowedData : NotifyPropertyChangedBase
    {
        [Column("Id", true)]
        public int Id { get; }
        [Column("None", true)]
        public TitanInfo TitanInfo { get; }

        [Column("Имя")]
        public string Name { get; }

        [Column("Время записи")]
        public string DateTime { get; }


        public TitanShowedData(int id, long ticks, TitanInfo titanInfo)
        {
            Name = titanInfo.Name;
            Id = id;
            TitanInfo = titanInfo;
            DateTime = new DateTime(ticks).ToString("yy:MM:dd HH:mm");
        }
    }
}