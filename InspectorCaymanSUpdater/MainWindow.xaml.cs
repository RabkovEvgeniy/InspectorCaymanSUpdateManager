using InspectorCaymanSUpdater.Services;
using System.Windows;

namespace InspectorCaymanSUpdater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            INotifyChangedLogger logger = new Logger();
            IUpdateLoader dbUpdateLoader = new DbUpdateLoader();
            IUpdateLoader softwereUpdateLoader = new SoftwereUpdateLoader();
            IMainWindowViewModelDataSource viewModelDataSource = new MainWindowViewModelDataSource();
            DataContext = new MainWindowViewModel(viewModelDataSource, dbUpdateLoader, softwereUpdateLoader, logger);
        }
    }
}