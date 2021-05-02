using Prism.Commands;
using Prism.Mvvm;
using SSH_FileTree_Viewer.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSH_FileTree_Viewer.ViewModel
{
    class SettingViewModel : BindableBase
    {
        #region Props

        private int _selectedElement;

        public List<string> Category { get; private set; }

        #endregion

        #region Commands

        private DelegateCommand _applyCommand;
        public DelegateCommand ApplyCommand =>
            _applyCommand ?? (_applyCommand = new DelegateCommand(ExecuteApplyCommand));

        #endregion

        public SettingViewModel()
        {
            Category = new List<string>();
            Category.Add("Connection");
            Category.Add("Security");
            SelectedElement = 0;

            Settings.Default.Reload();
        }

        #region Getter/Setter

        public int SelectedElement
        {
            get { return _selectedElement; }
            set { SetProperty(ref _selectedElement, value); }
        }

        #endregion

        #region ExecuteCommand

        void ExecuteApplyCommand()
        {
            Settings.Default.Save();
        }

        #endregion
    }
}
