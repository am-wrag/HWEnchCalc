using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.Core;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;

namespace HWEnchCalc.Calculators.EssenceCalc
{
    public class EssCalcResultManager : NotifyPropertyChangedBase
    {
        public EssenceCalcResult Result { get; set; }
        public ObservableCollection<EssenceCalcResultShort> Results { get; private set; }
        public WpfCommand AddNewEntryCommand { get; set; }
        public WpfCommand DeleteEntryCommand { get; set; }
        public WpfCommand ClearCommand { get; set; }

        public bool HasAddNewTitanToCalc => TitanEssenceCalc.CheckAllTitanStatsIsNoZero(Result);
        public int SelectedTableIndex
        {
            get => _selectedTableIndex;
            set
            {
                _selectedTableIndex = value;
                UpdateCurrentCaltInfo();
            }
        }

        private int _selectedTableIndex = -1;
        private readonly TitanStats _titanBaseStats;
        private readonly Configuration _config;

        public EssCalcResultManager(TitanStats titanBaseStats, Configuration config)
        {
            _titanBaseStats = titanBaseStats;
            _config = config;

            Result = new EssenceCalcResult { TitanBaseStats = titanBaseStats };
            Result.PropertyChanged += Notify;

            AddNewEntryCommand = new WpfCommand(SaveNewCalcResult);
            DeleteEntryCommand = new WpfCommand(DeleteCalcResult);
            ClearCommand = new WpfCommand(ClearCalcInfo);

            GetPreviosCalcResult();
        }
        private void SaveNewCalcResult()
        {
            if (!TitanEssenceCalc.CheckAllTitanStatsIsNoZero(Result)) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                dbOper.AddEssenceCalcInfo(Result);

                Results = dbOper.GetShortCalcInfos();
            }
            PropertyChangedByName(nameof(Results));
        }

        private void GetPreviosCalcResult()
        {
            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                Results = dbOper.GetShortCalcInfos();
            }
        }

        private void ClearCalcInfo()
        {
            Result = new EssenceCalcResult(){ TitanBaseStats = _titanBaseStats };
            Result.PropertyChanged += Notify;
            PropertyChangedByName(nameof(Result));
        }

        private void DeleteCalcResult()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                var selectedCalcInfoId = Results[SelectedTableIndex].Id;
                dbOper.DeleteEssenceCalcInfoById(selectedCalcInfoId);
                Results.Remove(Results.First(d => d.Id == selectedCalcInfoId));
            }
        }

        private void UpdateCurrentCaltInfo()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;

            using (var dbOper = new DbOperator(_config.ConnectionInfo.DefaultConnection))
            {
                var selectedCalcInfoId = Results[SelectedTableIndex].Id;

                var essData = dbOper.GetEssenceCalcData().First(_ => _.Id == selectedCalcInfoId);
                essData.CreatePath();

                Result.Update(essData);
            }
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
        }

        private void Notify(object sender, PropertyChangedEventArgs args)
        {
            PropertyChangedByName(args.PropertyName);
        }
    }
}