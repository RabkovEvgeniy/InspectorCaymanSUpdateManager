using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InspectorCaymanSUpdater.Services;
using Microsoft.WindowsAPICodePack.Dialogs;

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

            IMainWindowViewModelDataSource viewModelDataSource = new MainWindowViewModelDataSource();
            var dialog = new CommonOpenFileDialog() 
            {
                Title = "Выбор папки сохранения обновлений",
                IsFolderPicker = true,
            };
            IUpdateLoader dbUpdateLoader = new DbUpdateLoader();
            IUpdateLoader softwereUpdateLoader = new SoftwereUpdateLoader();
            INotifyChangedLogger logger = new Logger();

            DataContext = new MainWindowViewModel(viewModelDataSource, dbUpdateLoader, softwereUpdateLoader, logger, dialog);
        }
    }
}