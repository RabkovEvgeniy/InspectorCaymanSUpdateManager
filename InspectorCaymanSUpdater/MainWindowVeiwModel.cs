using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater
{
    class MainWindowVeiwModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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

        public MainWindowVeiwModel() 
        {
            _logText = new StringBuilder("> Инициализирую модель представления", 100);
        }
        
        public void LogInformation(string information) 
        {
            _logText.AppendLine("> " + information);
            OnPropertyChanged("LogText");
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
