﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        private IUpdateLoader _updateLoader;
        private CommonOpenFileDialog _folderPickerDialog; 

        public LoadUpdateCommand(IUpdateLoader updateLoader, CommonOpenFileDialog folderPickerDialog) 
        {
            _folderPickerDialog = folderPickerDialog;
            _updateLoader = updateLoader;
        }

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            CommonFileDialogResult result;
            do
            {
                result = _folderPickerDialog.ShowDialog();
            } while (result != CommonFileDialogResult.Ok);
            string targetDirectory = _folderPickerDialog.FileName;

            await _updateLoader.LoadUpdateAsync(targetDirectory);
        }
    }
}
