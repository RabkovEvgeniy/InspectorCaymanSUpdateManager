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

            CommonFileDialogResult result = _folderPickerDialog.ShowDialog();
            switch (result)
            {
                case CommonFileDialogResult.None:
                case CommonFileDialogResult.Cancel:
                    _logger.LogInformation("Операция была отменена");
                    return;
                case CommonFileDialogResult.Ok:
                    break;
            }
            string targetDirectoryName = _folderPickerDialog.FileName;

            await Task.Run(() => _updateLoader.LoadUpdate(targetDirectoryName, _logger));
        }
    }
}
