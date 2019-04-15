using System.ComponentModel;
using HWEnchCalc.Calculatros;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Core
{
    public class HwEnchCalcViewModel : NotifyPropertyChangedBase
    {
        public TitanBaseStats TitanBaseStats { get; set; } = new TitanBaseStats();
        public TitanEssenceCalc TitanEssenceCalc { get; set; }
        public TitanGoldCalc TitanGoldCalc { get; set; }
        public CalculatorManager CalcManager { get; set; } = new CalculatorManager();

        public HwEnchCalcViewModel(Configuration config)
        {
            TitanEssenceCalc = new TitanEssenceCalc(TitanBaseStats, config);
            TitanGoldCalc = new TitanGoldCalc(TitanBaseStats, config);
            GetBaseArtefactInfo(config);

            TitanEssenceCalc.PropertyChanged += UpdateTitanBaseStats;
        }

        private void UpdateTitanBaseStats(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TitanEssenceCalc.CurrentCalcInfo))
            {
                TitanBaseStats = TitanEssenceCalc.CurrentCalcInfo.TitanBaseStats;
                TitanBaseStats.UpdateTitanImage();
                PropertyChangedByName(nameof(TitanBaseStats));
            }
        }

        private void GetBaseArtefactInfo(Configuration config)
        {
           var artLevelUpInfo = new SourceArtLevelUpInfo(config);
        }
    }
}