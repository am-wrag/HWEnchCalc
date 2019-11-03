using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HWEnchCalc.Common;
using HWEnchCalc.DB;
using HWEnchCalc.Titan;
using HWEnchCalc.Titan.Helper;

namespace HWEnchCalc.Calculators.TitanCalc
{
    public class TitanCalculationDataManager : NotifyPropertyChangedBase
    {
        public List<string> TitanVariants => _titanHelper.GetTitanNames();
        public ObservableCollection<TitanShowedData> TitanShowedData { get; private set; }
        public WpfCommand ShowTitanCompareWindowCommand { get; }
        public WpfCommand AddNewEntryCommand { get; }
        public WpfCommand DeleteEntryCommand { get; }
        public WpfCommand ClearCommand { get; }

        /// <summary>
        /// Для отображения кольца загрузки
        /// </summary>
        public bool IsDbLoading
        {
            get => _isDbLoading;
            private set
            {
                _isDbLoading = value;
                PropertyChangedByMember();
            }
        }

        public bool HasAddNewTitanToCalc => TitanCalculatorCore.CheckAllTitanStatsIsNoZero(TitanInfo);

        public int SelectedTableIndex
        {
            get => _selectedTableIndex;
            set
            {
                _selectedTableIndex = value;
                UpdateCurrentCalcInfo();
            }
        }

        public TitanInfo TitanInfo { get; private set; }

        private readonly TitanSourceDataHelper _titanHelper;
        private readonly TitanInfoMapper _titanBuilder;
        private readonly DbOperator _dbOperator;
        private int _selectedTableIndex = -1;
        private bool _isDbLoading = true;

        public TitanCalculationDataManager(TitanInfoMapper titanBuilder, TitanSourceDataHelper titanHelper, DbOperator dbOperator)
        {
            _titanBuilder = titanBuilder;
            _dbOperator = dbOperator;

            _titanHelper = titanHelper;

            TitanInfo = new TitanInfo(titanHelper);
            TitanInfo.PropertyChanged += NotifyToCalculation;

            ShowTitanCompareWindowCommand = new WpfCommand(ShowTitanCompareWindow);
            AddNewEntryCommand = new WpfCommand(AddNewCalcResult);
            DeleteEntryCommand = new WpfCommand(DeleteCalcResult);
            ClearCommand = new WpfCommand(ClearCalcInfo);

            GetPreviousCalculateResult();
        }
        private async void GetPreviousCalculateResult()
        {
            var titanDboInfos = await _dbOperator.GetTitanCalculatedInfoAsync();
            IsDbLoading = false;

            TitanShowedData = _titanBuilder.GetTitanShowedData(titanDboInfos);
            
            PropertyChangedByName(nameof(IsDbLoading));
            PropertyChangedByName(nameof(TitanShowedData));
        }
        private async void AddNewCalcResult()
        {
            if (!TitanCalculatorCore.CheckAllTitanStatsIsNoZero(TitanInfo)) return;
            IsDbLoading = true;

            var titanInfoDbo = _titanBuilder.GeTitanInfoDto(TitanInfo);

            await _dbOperator.AddTitanInfoAsync(titanInfoDbo);
            IsDbLoading = false;

            var titanDboInfos = await _dbOperator.GetTitanCalculatedInfoAsync();
            TitanShowedData = _titanBuilder.GetTitanShowedData(titanDboInfos);

            PropertyChangedByName(nameof(TitanShowedData));
        }
        private async void DeleteCalcResult()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;
            IsDbLoading = true;

            var selectedTitan = TitanShowedData[SelectedTableIndex];

            await _dbOperator.DeleteTitanInfoByIdAsync(selectedTitan.Id);
            IsDbLoading = false;

            TitanShowedData.Remove(selectedTitan);
        }

        private void ClearCalcInfo()
        {
            TitanInfo = new TitanInfo(_titanHelper);
            TitanInfo.PropertyChanged += NotifyToCalculation;
            PropertyChangedByName(nameof(TitanInfo));
        }

        private void ShowTitanCompareWindow()
        {
            new TitanCompareWindow(TitanShowedData, _titanHelper).ShowDialog();
        }

        private void UpdateCurrentCalcInfo()
        {
            //Отсутствие выделенной строки по умолчанию назначает SelectedIndex для DataGrid в -1
            if (SelectedTableIndex < 0) return;

            var selectedTitan = TitanShowedData[SelectedTableIndex];

            TitanInfo = selectedTitan.TitanInfo;
            TitanInfo.PropertyChanged += NotifyToCalculation;

            PropertyChangedByName(nameof(TitanInfo));
            NotifyToCalculation(new object(), new PropertyChangedEventArgs(string.Empty));
        }

        private void NotifyToCalculation(object sender, PropertyChangedEventArgs args)
        {
            PropertyChangedByName(nameof(HasAddNewTitanToCalc));
            PropertyChangedByName(CalculatorManager.PropertyWhenModifyNeedCalculate);
        }
    }
}