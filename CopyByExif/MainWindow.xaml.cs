using System;
using System.ComponentModel;
using System.Windows;

using CopyByExif.Core;

namespace CopyByExif
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker copyWorker;

        public CopySettings CopySettings
        {
            get => (CopySettings)DataContext;
            set => DataContext = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeCopyWorker();
            CopySettings = new CopySettings();
        }

        private void Btn_BrowseFrom_Click(object sender, RoutedEventArgs e)
        {
            var fd = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = Properties.Resources.Lbl_FromDirectory, UseDescriptionForTitle = true
            };
            if (fd.ShowDialog().GetValueOrDefault())
                CopySettings.FromDirectoryPath = fd.SelectedPath;
        }

        private void Btn_BrowseTo_Click(object sender, RoutedEventArgs e)
        {
            var fd = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = Properties.Resources.Lbl_ToDirectory, UseDescriptionForTitle = true
            };
            if (fd.ShowDialog().GetValueOrDefault())
                CopySettings.ToDirectoryPath = fd.SelectedPath;
        }

        private void Btn_ControlCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!copyWorker.IsBusy)
            {
                btn_ControlCopy.Content = Properties.Resources.CancelCopy;
                copyWorker.RunWorkerAsync(CopySettings);
            }
            else
            {
                copyWorker.CancelAsync();
                btn_ControlCopy.Content = Properties.Resources.StartCopy;
            }
        }

        private void CopyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CopyPhotos.Copy(e.Argument as CopySettings, sender as BackgroundWorker);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, Properties.Resources.ErrorCopyingFiles, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeCopyWorker()
        {
            if (copyWorker != null)
                copyWorker.Dispose();

            copyWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            copyWorker.DoWork += CopyWorker_DoWork;
            copyWorker.ProgressChanged += (_, pcea) => progressBar.Value = pcea.ProgressPercentage;
            copyWorker.RunWorkerCompleted += (_, rwcea) => btn_ControlCopy.Content = Properties.Resources.StartCopy;
        }
    }
}
