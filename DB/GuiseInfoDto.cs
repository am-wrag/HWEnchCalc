using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Guise;

namespace HWEnchCalc.DB
{
    public class GuiseInfoDto
    {
        public int Id { get; set; }
        public GuiseType GuiseType { get; set; }
        public int Level { get; set; }
        public double StatValue { get; set; }
        public double IncreaseStatValue { get; set; }
        public int LvlUpCost { get; set; }
    }
}