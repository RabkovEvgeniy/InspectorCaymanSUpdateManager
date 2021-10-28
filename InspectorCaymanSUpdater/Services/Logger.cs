using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater.Services
{
    public class Logger : INotifyLogChangedLogger
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Log => _log.ToString();

        private readonly object _syncRoot = new();
        private readonly StringBuilder _log = new(500);

        public void LogInformation(string information)
        {
            lock (_syncRoot)
            {
                _log.AppendLine($"> {information};");
                OnPropertyChanged(nameof(Log));
            }
        }
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
