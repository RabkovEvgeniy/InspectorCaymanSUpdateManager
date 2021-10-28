using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace InspectorCaymanSUpdater.Services
{
    public class Logger : INotifyChangedLogger
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
