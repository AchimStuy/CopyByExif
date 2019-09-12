using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

using CopyByExif.Core.Properties;

namespace CopyByExif.Core
{
    public class CopySettings : INotifyPropertyChanged
    {
        public string DirectoryStructure
        {
            get => Settings.Default.DirectoryStructure;
            set
            {
                if (Settings.Default.DirectoryStructure != value)
                {
                    Settings.Default.DirectoryStructure = value;
                    Settings.Default.Save();
                    NotifyPropertyChanged();
                }
            }
        }

        public DirectoryInfo FromDirectory
        {
            get => new DirectoryInfo(FromDirectoryPath);
        }

        public string FromDirectoryPath
        {
            get => Settings.Default.FromDirectory;
            set
            {
                if (Settings.Default.FromDirectory != value)
                {
                    Settings.Default.FromDirectory = value;
                    Settings.Default.Save();
                    NotifyPropertyChanged();
                }
            }
        }

        public DirectoryInfo ToDirectory
        {
            get => new DirectoryInfo(ToDirectoryPath);
        }

        public string ToDirectoryPath
        {
            get => Settings.Default.ToDirectory;
            set
            {
                if (Settings.Default.ToDirectory != value)
                {
                    Settings.Default.ToDirectory = value;
                    Settings.Default.Save();
                    NotifyPropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
