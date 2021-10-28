using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using InspectorCaymanSUpdater.Services;
using System.Windows;

namespace InspectorCaymanSUpdater
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly INotifyChangedLogger _logger;
        private readonly IUpdateLoader _dbUpdateLoader;
        private readonly IUpdateLoader _softwereUpdateLoader;
        private readonly CommonFileDialog _fileDialog;

        public INotifyChangedLogger Logger => _logger;
        public LoadUpdateCommand LoadDbUpdateCommand => _loadDbUpdateCommand ?? 
            (_loadDbUpdateCommand = new LoadUpdateCommand(_dbUpdateLoader, _logger, _fileDialog));
        public LoadUpdateCommand LoadSoftwereUpdateCommand=>_loadSoftwereUpdateCommand ??
                    (_loadSoftwereUpdateCommand = new LoadUpdateCommand(_softwereUpdateLoader, _logger, _fileDialog));
        public string LastSoftwereUpdateDate
        {
            get => _lastSoftwereUpdateDate;
            set
            {
                _lastSoftwereUpdateDate = value;
                OnPropertyChanged(nameof(LastSoftwereUpdateDate));
            }
        }
        public string LastDbUpdateDate
        {
            get => _lastDbUpdateDate;
            set
            {
                _lastDbUpdateDate = value;
                OnPropertyChanged(nameof(LastDbUpdateDate));
            }
        }

        private string _lastSoftwereUpdateDate;
        private string _lastDbUpdateDate;
        private LoadUpdateCommand _loadDbUpdateCommand;
        private LoadUpdateCommand _loadSoftwereUpdateCommand;
        
        public MainWindowViewModel(IMainWindowViewModelDataSource dataSource, IUpdateLoader dbUpdateLoader, IUpdateLoader softwereUpdateLoader,
            INotifyChangedLogger logger,CommonFileDialog fileDialog)
        {
            if (dataSource == null || dbUpdateLoader == null || softwereUpdateLoader == null || logger == null)
            {
                throw new ArgumentNullException();
            }

            _logger = logger;
            _dbUpdateLoader = dbUpdateLoader;
            _softwereUpdateLoader = softwereUpdateLoader;
            _fileDialog = fileDialog;

            InitializeLastUpdateDates(dataSource);
        }

        private async void InitializeLastUpdateDates(IMainWindowViewModelDataSource dataSource)
        {
            try
            {
                await Task.Run(() =>
                {
                    _logger.LogInformation("Извлекаю даты последних обновлений");
                    LastDbUpdateDate = dataSource.GetLastDbUpdateDate();
                    LastSoftwereUpdateDate = dataSource.GetLastSoftwereUpdateDate();
                    _logger.LogInformation("Даты последних обновлений получены");

                });
            }
            catch (Exception ex)
            {
                string exceptionMessage = $"Произошла ошибка:{ex.Message}\nПожалуйста, устраните проблему и запустите программу еще раз";
                MessageBox.Show(exceptionMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
            }
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
