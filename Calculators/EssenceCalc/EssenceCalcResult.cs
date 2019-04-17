using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using HWEnchCalc.Common;
using HWEnchCalc.Core;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.EssenceCalc
{
    public class EssenceCalcResult : NotifyPropertyChangedBase
    {
        private TitanStats _titanBaseStats;
        [Key] public int Id { get; set; } = new int();
        public long Ticks { get; set; }
        public ArtefactInfo DefArt { get; set; } = new ArtefactInfo { ArtefactType = ArtefactType.ElementalDef };
        public ArtefactInfo AttackArt { get; set; } = new ArtefactInfo { ArtefactType = ArtefactType.ElementalAtc };

        public TitanStats TitanBaseStats
        {
            get => _titanBaseStats;
            set
            {
                _titanBaseStats = value;
                _titanBaseStats.PropertyChanged += Calculate;
            }
        }

        public EssenceCalcResult()
        {
            Ticks = DateTime.Now.Ticks;

            CreatePath();
        }

        public void CreatePath()
        {
            DefArt.PropertyChanged += Calculate;
            AttackArt.PropertyChanged += Calculate;
        }

        public EssenceCalcResultShort ToShortInfo()
        {
            return new EssenceCalcResultShort(Id, Ticks);
        }

        public void Update(EssenceCalcResult calcResult)
        {
            Ticks = DateTime.Now.Ticks;
            TitanBaseStats.Update(calcResult.TitanBaseStats);
            DefArt.Update(calcResult.DefArt);
            AttackArt.Update(calcResult.AttackArt);
        }

        private void Calculate(object sender, PropertyChangedEventArgs args)
        {
            PropertyChangedByName(CalculatorManager.PropertyWhenModifyNeedCalculate);
        }
    }
}