using SSH_FileTree_Viewer.View;
using SSH_FileTree_Viewer.ViewModel;
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

namespace SSH_FileTree_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = (FileTreeViewModel)DataContext;
            viewModel.CloseCommand.Execute();
        }

        private void Directory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (FileTreeViewModel)DataContext;
            viewModel.CdCommand.Execute();
        }

        private void OptionOpen_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.Owner = this;
            setting.ApplyButton.Click += Apply_Click;
            setting.ShowDialog();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (FileTreeViewModel)DataContext;
            viewModel.SetConnectionInfoCommand.Execute();
            viewModel.ConnectCommand.RaiseCanExecuteChanged();
        }
    }
}
