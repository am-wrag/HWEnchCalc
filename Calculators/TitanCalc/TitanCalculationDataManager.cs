using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HWEnchCalc.Common;
using HWEnchCalc.Config;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Calculators.TitanCalc
{ 
    public class TitanCalculationDataManager : NotifyPropertyChangedBase
    {
        public List<string> TitanVariants => _titanHelper.GetTitanNames();
        public ObservableCollection<TitanShortInfo> CalculatedTitanList { get; private set; }
        public WpfCommand AddNewEntryCommand { get; }
        public WpfCommand DeleteEntryCommand { get; }
        public WpfCommand ClearCommand { get; }
        public bool IsDbLoading { get; private set; } = true;

        public bool HasAddNewTitanToCalc => TitanCalculator.CheckAllTitanStatsIsNoZero(TitanInfo);
        public int SelectedTableIndex
        {
            get => _selectedTableIndex;
            set
            {
                _selectedTableIndex = value;
                UpdateCurrentCaltInfo();
            }
        }

        public TitanInfo TitanInfo { get; private set; }

        private readonly TitanSourceDataHelper _titanHelper;
        private readonly Configuration _config;
        private readonly DbOperator _dbOperator;
        private int _selectedTableIndex = -1;

        public TitanCalculationDataManager(TitanSourceDataHelper titanHelper, Configuration config)
        {
            _config = config;
            _dbOperator= new DbOperator(config.ConnectionInfo.DefaultConnection);

            _titanHelper = titanHelper;

            TitanInfo = new TitanInfo(_titanHelper, config.GameInfo.TitanDatas.TitanMaxLevel);
            TitanInfo.PropertyChanged += NotifyToCalculation;

            AddNewEntryCommand = new WpfCommand(SaveNewCalcResult);
            DeleteEntryCommand = new WpfCommand(DeleteCalcResult);
            ClearCommand = new WpfCommand(ClearCalcInfo);

            GetPreviosCalcResult();
        }

        private async void SaveNewCalcResult()
        {
            if (!TitanCalculator.CheckAllTitanStatsIsNoZero(TitanInfo)) return;
            
            await _dbOperator.AddEssenceCalcInfoAsync(TitanInfo);
            CalculatedTitanList = await _dbOperator.GetShortCalcInfosAsync();
            PropertyChangedByName(nameof(CalculatedTitanList));
        }

        private async void GetPreviosCalcResult()
        {
            CalculatedTitanList = await _dbOperator.GetShortCalcInfosAsync();
            IsDbLoading = false;
            PropertyChangedByName(nameof(IsDbLoading));
            PropertyChangedByName(nameof(CalculatedTitanList));
        }

        private void ClearCalcInfo()
        {
            TitanInfo = new TitanInfo(_titanHelper, _config.GameInfo.TitanDatas.TitanMaxLevel);
            TitanInfo.PropertyChanged += NotifyToCalculation;
            PropertyChangedByName(nameof(TitanInfo));
        }

        private async void DeleteCalcResult()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;
            
            var selectedCalcInfoId = CalculatedTitanList[SelectedTableIndex].Id;

            await _dbOperator.DeleteEssenceCalcInfoByIdAsync(selectedCalcInfoId);

            CalculatedTitanList.Remove(CalculatedTitanList.First(d => d.Id == selectedCalcInfoId));
        }

        private void UpdateCurrentCaltInfo()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;
            
            var selectedCalcInfoId = CalculatedTitanList[SelectedTableIndex].Id;

            var calcInfoDbo = _dbOperator.GetTitanCalculatedInfo();
            var selectedTitanInfo = calcInfoDbo.First(_ => _.Id == selectedCalcInfoId);
            
            TitanInfo.Update(selectedTitanInfo);
            
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
        }

        private void NotifyToCalculation(object sender, PropertyChangedEventArgs args)
        {
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
            PropertyChangedByName(CalculatorManager.PropertyWhenModifyNeedCalculate);
        }
    }
}