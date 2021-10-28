using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace InspectorCaymanSUpdater
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
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
                OnPropertyChanged("LastSoftwereUpdateDate");
            }
        }
        public string LastDbUpdateDate
        {
            get => _lastDbUpdateDate;
            set
            {
                _lastDbUpdateDate = value;
                OnPropertyChanged("LastDbUpdateDate");
            }
        }
        public string LogText
        {
            get => _logText.ToString();
        }

        private string _lastSoftwereUpdateDate;
        private string _lastDbUpdateDate;
        private StringBuilder _logText;
        private LoadUpdateCommand _loadDbUpdateCommand;
        private LoadUpdateCommand _loadSoftwereUpdateCommand;

        public  MainWindowViewModel(IMainWindowViewModelDataSource dataSource, IUpdateLoader dbUpdateLoader, IUpdateLoader softwereUpdateLoader,
            CommonFileDialog fileDialog) 
        {
            if (dataSource == null || dbUpdateLoader == null || softwereUpdateLoader == null)
            {
                throw new ArgumentNullException();
            }

            _logText = new StringBuilder(500);
            _dbUpdateLoader = dbUpdateLoader;
            _softwereUpdateLoader = softwereUpdateLoader;
            _fileDialog = fileDialog;

            Task.Run(() =>
            {
                LogInformation("Извлекаю дату последнего обновления базы данных...");
                LastDbUpdateDate = dataSource.GetLastDbUpdateDate();
                LogInformation("Дата последнего обновления базы данных получена.");
           
                LogInformation("Извлекаю дату последнего обновления ПО...");
                LastSoftwereUpdateDate = dataSource.GetLastSoftwereUpdateDate();
                LogInformation("Дата последнего обновления ПО получена.");
            });
        }
        
        public void LogInformation(string information) 
        {
            _logText.AppendLine("> " + information);
            OnPropertyChanged("LogText");
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
