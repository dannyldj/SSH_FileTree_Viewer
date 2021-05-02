using Prism.Commands;
using Prism.Mvvm;
using SSH_FileTree_Viewer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSH_FileTree_Viewer.ViewModel
{
    class SetConnectionViewModel : BindableBase
    {
        #region Props

        private int _selectedIpIndex;

        private string _enteredIp;

        #endregion

        #region Commands

        private DelegateCommand _ipSaveCommand;
        public DelegateCommand IpAddCommand =>
            _ipSaveCommand ?? (_ipSaveCommand = new DelegateCommand(ExecuteIpAddCommand));

        private DelegateCommand _ipLoadCommand;
        public DelegateCommand IpLoadCommand =>
            _ipLoadCommand ?? (_ipLoadCommand = new DelegateCommand(ExecuteIpLoadCommand));

        private DelegateCommand _ipDelCommand;
        public DelegateCommand IpDelCommand =>
            _ipDelCommand ?? (_ipDelCommand = new DelegateCommand(ExecuteIpDelCommand));

        #endregion

        public SetConnectionViewModel()
        {
            if (Settings.Default.savedIps == null)
            {
                Settings.Default.savedIps = new ObservableCollection<string>();
            }
        }

        #region Getter/Setter

        public int SelectedIpIndex
        {
            get { return _selectedIpIndex; }
            set { _selectedIpIndex = value; }
        }

        public string EnteredIp
        {
            get { return _enteredIp; }
            set { SetProperty(ref _enteredIp, value); }
        }

        #endregion

        #region ExecuteCommand

        void ExecuteIpAddCommand()
        {
            Settings.Default.savedIps.Add(EnteredIp);
            EnteredIp = String.Empty;
        }

        void ExecuteIpLoadCommand()
        {
            Settings.Default.connectingIp = Settings.Default.savedIps[SelectedIpIndex];
        }

        void ExecuteIpDelCommand()
        {
            Settings.Default.savedIps.RemoveAt(SelectedIpIndex);
        }

        #endregion
    }
}
