using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorCaymanSUpdater.Services
{
    public interface INotifyChangedLogger: INotifyPropertyChanged
    {
        string Log { get; }
        void LogInformation(string information);
    }
}
