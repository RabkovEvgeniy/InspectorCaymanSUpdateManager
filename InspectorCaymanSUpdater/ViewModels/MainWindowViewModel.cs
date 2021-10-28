using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using InspectorCaymanSUpdater.Services;

namespace InspectorCaymanSUpdater
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private readonly INotifyChangedLogger _logger;
        private readonly IUpdateLoader _dbUpdateLoader;
        private readonly IUpdateLoader _softwereUpdateLoader;
        private readonly CommonFileDialog _fileDialog;
        public LoadUpdateCommand LoadDbUpdateCommand 
        {
            get 
            {
                return _loadDbUpdateCommand ??
                    (_loadDbUpdateCommand = new LoadUpdateCommand(_dbUpdateLoader, _fileDialog));
            }
        }
        public LoadUpdateCommand LoadSoftwereUpdateCommand
        {
            get
            {
                return _loadSoftwereUpdateCommand ??
                    (_loadSoftwereUpdateCommand = new LoadUpdateCommand(_softwereUpdateLoader, _fileDialog));
            }
        }
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
        public INotifyChangedLogger Logger
        {
            get => _logger;
        }

        private string _lastSoftwereUpdateDate;
        private string _lastDbUpdateDate;
        private LoadUpdateCommand _loadDbUpdateCommand;
        private LoadUpdateCommand _loadSoftwereUpdateCommand;
        public  MainWindowViewModel(IMainWindowViewModelDataSource dataSource, IUpdateLoader dbUpdateLoader, IUpdateLoader softwereUpdateLoader,
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

            Task.Run(() =>
            {
                _logger.LogInformation("Извлекаю дату последнего обновления базы данных...");
                LastDbUpdateDate = dataSource.GetLastDbUpdateDate();
                _logger.LogInformation("Дата последнего обновления базы данных получена.");
           
                _logger.LogInformation("Извлекаю дату последнего обновления ПО...");
                LastSoftwereUpdateDate = dataSource.GetLastSoftwereUpdateDate();
                _logger.LogInformation("Дата последнего обновления ПО получена.");
            });
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
