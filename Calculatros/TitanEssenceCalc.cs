using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using HWEnchCalc.Calculatros.EssenceCalc;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculatros
{
    public class TitanEssenceCalc : NotifyPropertyChangedBase
    {
        public ObservableCollection<EssenceCalcShortInfo> EssenceCalcTable { get; private set; }
        public EssenceCalcInfo CurrentCalcInfo { get; set; }
        public List<string> FirstArtLevelUpVariants => SourceArtLevelUpInfo.FirstArtLevels.Select(a => a.LevelInfo).ToList();
        public List<string> SecondArtLevelUpVariants => SourceArtLevelUpInfo.SecondArtLevels.Select(a => a.LevelInfo).ToList();
        public double Result { get; private set; }
        public string ResultDesc { get; private set; } = "Результат";
        public bool HasAddNewTitanToCalc => CheckAllTitanStatsIsNoZero();
        public int SelectedTableIndex
        {
            get => _selectedTableIndex;
            set
            {
                _selectedTableIndex = value;
                UpdateCurrentCaltInfo();
            }
        }
        public WpfCommand AddNewEntryCommand { get; set; }
        public WpfCommand DeleteEntryCommand { get; set; }
        public WpfCommand ClearCommand { get; set; }

        private readonly Configuration _config;
        private int _selectedTableIndex = -1;
        private const string AtackBetterThenDef = "Атака лучше защиты, результат в %";
        private const string DefBetterThenAtack = "Защита лучше атаки, результат в %";

        public TitanEssenceCalc(TitanBaseStats titanBaseStats, Configuration config)
        {
            CurrentCalcInfo = new EssenceCalcInfo { TitanBaseStats = titanBaseStats };

            _config = config;

            CurrentCalcInfo.PropertyChanged += Calculate;
            CurrentCalcInfo.TitanBaseStats.PropertyChanged += Calculate;

            AddNewEntryCommand = new WpfCommand(SaveNewCalcResult);
            DeleteEntryCommand = new WpfCommand(DeleteCalcResult);
            ClearCommand = new WpfCommand(ClearCalcInfo);

            GetPreviosCalcResult();
        }

        private void GetPreviosCalcResult()
        {
            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                EssenceCalcTable = dbOper.GetShortCalcInfoTable();
            }
        }

        private void SaveNewCalcResult()
        {
            if (!CheckAllTitanStatsIsNoZero()) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                dbOper.AddEssenceCalcInfo(CurrentCalcInfo);

                EssenceCalcTable = dbOper.GetShortCalcInfoTable();
            }
        }

        private void ClearCalcInfo()
        {
            CurrentCalcInfo = new EssenceCalcInfo();
            PropertyChangedByName(nameof(CurrentCalcInfo));
        }

        private void DeleteCalcResult()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                var selectedCalcInfoId = EssenceCalcTable[SelectedTableIndex].Id;
                dbOper.DeleteEssenceCalcInfoById(selectedCalcInfoId);
                EssenceCalcTable.Remove(EssenceCalcTable.First(d => d.Id == selectedCalcInfoId));
            }
        }

        private void UpdateCurrentCaltInfo()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                var selectedCalcInfoId = EssenceCalcTable[SelectedTableIndex].Id;

                var essData = dbOper.GetEssenceCalcData().First(_ => _.Id == selectedCalcInfoId);
                essData.CreatePath();

                CurrentCalcInfo = essData;
                CurrentCalcInfo.PropertyChanged += Calculate;
            }

            PropertyChangedByName(nameof(CurrentCalcInfo));
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
            Calculate(new object(), new PropertyChangedEventArgs(""));
        }


        private void Calculate(object sende, PropertyChangedEventArgs args)
        {
            if (!CheckAllTitanStatsIsNoZero()) return;

            var titanBaseStats = CurrentCalcInfo.TitanBaseStats;

            var effectiveHpStart = titanBaseStats.Hp * (1 + CurrentCalcInfo.DefArt.BaseStatValue / 300000);
            var effectiveHpFinish = titanBaseStats.Hp *
                                    (1 + (CurrentCalcInfo.DefArt.BaseStatValue +
                                          CurrentCalcInfo.DefArt.IncreaseStatValue) / 300000);
            var effectiveHpIncrease = effectiveHpFinish / effectiveHpStart;
            var effectiveHpIncreasePerEssence = (effectiveHpIncrease - 1) / CurrentCalcInfo.DefArt.EssenceValue;

            var effectiveAtackStart = titanBaseStats.Atack + CurrentCalcInfo.AtackArt.BaseStatValue;
            var effectiveAtackFinish = titanBaseStats.Atack + CurrentCalcInfo.AtackArt.BaseStatValue +
                                       CurrentCalcInfo.AtackArt.IncreaseStatValue;
            var effectiveAtackIncrease = effectiveAtackFinish / effectiveAtackStart;
            var effectiveAtackIncreasePerEssence = (effectiveAtackIncrease - 1) / CurrentCalcInfo.AtackArt.EssenceValue;

            var totalRait = effectiveHpIncreasePerEssence / effectiveAtackIncreasePerEssence;

            if (totalRait > 1)
            {
                ResultDesc = DefBetterThenAtack;
                Result = (totalRait - 1) * 100;
            }
            else
            {
                ResultDesc = AtackBetterThenDef;
                Result = (effectiveAtackIncreasePerEssence / effectiveHpIncreasePerEssence - 1) * 100;
            }

            Result = Math.Round(Result, 2);

            PropertyChangedByName(nameof(Result));
            PropertyChangedByName(nameof(ResultDesc));
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
        }

        private bool CheckAllTitanStatsIsNoZero()
        {
            return CurrentCalcInfo.TitanBaseStats.Hp != 0
                   && CurrentCalcInfo.TitanBaseStats.Atack != 0
                   && CurrentCalcInfo.DefArt.BaseStatValue != 0
                   && CurrentCalcInfo.DefArt.IncreaseStatValue != 0
                   && CurrentCalcInfo.DefArt.EssenceValue != 0
                   && CurrentCalcInfo.AtackArt.BaseStatValue != 0
                   && CurrentCalcInfo.AtackArt.IncreaseStatValue != 0
                   && CurrentCalcInfo.AtackArt.EssenceValue != 0;
        }
    }
}