using System.ComponentModel;

namespace InspectorCaymanSUpdater.Services
{
    public interface INotifyChangedLogger : INotifyPropertyChanged
    {
        string Log { get; }
        void LogInformation(string information);
    }
}
