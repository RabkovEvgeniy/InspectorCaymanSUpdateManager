using InspectorCaymanSUpdater.Services;

namespace InspectorCaymanSUpdater
{
    interface IUpdateLoader
    {
        void LoadUpdate(string targetDirectoryName, INotifyChangedLogger logger);
    }
}
