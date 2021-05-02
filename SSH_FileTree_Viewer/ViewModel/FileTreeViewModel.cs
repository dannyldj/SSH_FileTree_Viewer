using Prism.Commands;
using Prism.Mvvm;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using SSH_FileTree_Viewer.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SSH_FileTree_Viewer.ViewModel
{
    class FileTreeViewModel : BindableBase
    {
        #region Props

        private string _curDir;

        private SftpFile _selectedContent;

        public ObservableCollection<SftpFile> Contents { get; private set; }

        #endregion

        #region Commands

        private DelegateCommand _setConnectionInfoCommand;
        public DelegateCommand SetConnectionInfoCommand =>
            _setConnectionInfoCommand ?? (_setConnectionInfoCommand = new DelegateCommand(ExecuteSetConnectionInfoCommand));

        private DelegateCommand _connectCommand;
        public DelegateCommand ConnectCommand =>
            _connectCommand ?? (_connectCommand = new DelegateCommand(ExecuteConnectCommand, CanConnect));

        private DelegateCommand _cdCommand;
        public DelegateCommand CdCommand =>
            _cdCommand ?? (_cdCommand = new DelegateCommand(ExecuteCdCommand));

        private DelegateCommand _cdParentCommand;
        public DelegateCommand CdParentCommand =>
            _cdParentCommand ?? (_cdParentCommand = new DelegateCommand(ExecuteCdParentCommand, CanCdParent));

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(ExecuteCloseCommand));

        #endregion

        public ConnectionInfo connectionInfo;
        public SftpClient sftpClient;

        public FileTreeViewModel()
        {
            SetConnectionInfo();
            Contents = new ObservableCollection<SftpFile>();
            CurDir = "DISCONNECTED";
        }

        /// <summary>
        /// 연결 정보 설정
        /// </summary>
        protected void SetConnectionInfo()
        {
            if (CanSubsConnectionInfo())
            {
                if (sftpClient != null && sftpClient.IsConnected)
                {
                    sftpClient.Disconnect();
                    Contents.Clear();
                    CurDir = "DISCONNECTED";
                }
                connectionInfo = new ConnectionInfo(
                    Settings.Default.connectingIp,
                    Settings.Default.userId,
                    new PasswordAuthenticationMethod(
                        Settings.Default.userId,
                        Settings.Default.password));
                sftpClient = new SftpClient(connectionInfo);
            }
            ConnectCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// 현재 경로를 조회
        /// </summary>
        /// <returns>A current directory.</returns>
        protected string ShowDir()
        {
            return sftpClient.WorkingDirectory;
        }

        /// <summary>
        /// 현재 경로의 요소를 조회
        /// </summary>
        protected void LoadCurContents()
        {
            Contents.Clear();
            foreach (SftpFile file in sftpClient.ListDirectory(ShowDir()).ToList())
            {
                if (file.Name != "." && file.Name != "..")
                {
                    Contents.Add(file);
                }
            }
        }

        /// <summary>
        /// Sftp 연결 정보 대입 가능 여부
        /// </summary>
        /// <returns>true if can substitute ConnectionInfo; otherwise, false.</returns>
        protected bool CanSubsConnectionInfo()
        {
            return Settings.Default.connectingIp != null
                && string.IsNullOrEmpty(Settings.Default.userId) == false
                && string.IsNullOrEmpty(Settings.Default.password) == false;
        }

        #region Getter/Setter

        public string CurDir
        {
            get { return _curDir; }
            set
            {
                SetProperty(ref _curDir, value);
                CdParentCommand.RaiseCanExecuteChanged();
            }
        }

        public SftpFile SelectedContent
        {
            get { return _selectedContent; }
            set { _selectedContent = value; }
        }

        #endregion

        #region ExecuteMethod

        void ExecuteSetConnectionInfoCommand()
        {
            SetConnectionInfo();
        }

        void ExecuteConnectCommand()
        {
            try
            {
                sftpClient.Connect();
                LoadCurContents();
                CurDir = ShowDir();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            ConnectCommand.RaiseCanExecuteChanged();
            CdParentCommand.RaiseCanExecuteChanged();
        }

        void ExecuteCdCommand()
        {
            if (SelectedContent.IsDirectory)
            {
                sftpClient.ChangeDirectory(SelectedContent.Name);
                LoadCurContents();
                CurDir = ShowDir();
            }
        }

        void ExecuteCdParentCommand()
        {
            sftpClient.ChangeDirectory("..");
            LoadCurContents();
            CurDir = ShowDir();
        }

        void ExecuteCloseCommand()
        {
            sftpClient.Dispose();
        }

        #endregion

        #region CanExecuteMethod

        bool CanConnect()
        {
            return sftpClient != null
                && sftpClient.IsConnected == false
                && CanSubsConnectionInfo();
        }

        bool CanCdParent()
        {
            return CurDir != "/home"
                && sftpClient != null
                && sftpClient.IsConnected == true;
        }

        #endregion
    }
}
