using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InspectorCaymanSUpdater.Services;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace InspectorCaymanSUpdater
{
    class LoadUpdateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private INotifyChangedLogger _logger;
        private IUpdateLoader _updateLoader;
        private CommonFileDialog _folderPickerDialog;

        public LoadUpdateCommand(IUpdateLoader updateLoader, INotifyChangedLogger logger, CommonFileDialog folderPickerDialog)
        {
            if (updateLoader == null || logger == null || folderPickerDialog == null) 
            {
                throw new ArgumentNullException();
            }

            _folderPickerDialog = folderPickerDialog;
            _updateLoader = updateLoader;
            _logger = logger;
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            _logger.LogInformation("Начинаю загрузку обновления");
            CommonFileDialogResult result;
            do
            {
                result = _folderPickerDialog.ShowDialog();
            } while (result != CommonFileDialogResult.Ok);
            string targetDirectoryName = _folderPickerDialog.FileName;

            await Task.Run(() => _updateLoader.LoadUpdate(targetDirectoryName));
            _logger.LogInformation("Операция прошла успешно");
        }
    }
}
