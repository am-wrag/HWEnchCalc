using System.ComponentModel;
using HWEnchCalc.Calculators;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Core
{
    public class HwEnchCalcViewModel : NotifyPropertyChangedBase
    {
        public TitanEssenceCalc TitanEssenceCalc { get; set; }
        public TitanGoldCalc TitanGoldCalc { get; set; }
        public CalculatorManager CalcManager { get; set; } = new CalculatorManager();
        public TitanStats TitanBaseStats { get; set; } = new TitanStats();

        public HwEnchCalcViewModel(Configuration config)
        {
            GetBaseArtefactInfo(config);
            TitanEssenceCalc = new TitanEssenceCalc(TitanBaseStats, config);
            TitanGoldCalc = new TitanGoldCalc(TitanBaseStats, config);
        }
       

        private void GetBaseArtefactInfo(Configuration config)
        {
           var artLevelUpInfo = new SourceArtLevelUpInfo(config);
        }
    }
}