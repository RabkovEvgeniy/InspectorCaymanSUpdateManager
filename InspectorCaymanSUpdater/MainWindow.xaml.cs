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

            //var dialog = new CommonOpenFileDialog();
            //dialog.IsFolderPicker = true;
            //dialog.Title = "Выбор папки сохранения обновлений";
            //CommonFileDialogResult result = dialog.ShowDialog();
            //string file = dialog.FileName;
        }
    }
}
