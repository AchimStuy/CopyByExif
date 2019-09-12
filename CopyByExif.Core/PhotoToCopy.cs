using System;
using System.IO;

using ExifLib;

namespace CopyByExif.Core
{
    class PhotoToCopy
    {
        ExifData _exifData;
        FileInfo _photoFile;

        public PhotoToCopy(FileInfo photoFile)
        {
            _photoFile = photoFile;
            _exifData = GetExifData();
        }

        private ExifData GetExifData()
        {
            var exifData = new ExifData();
            try
            {
                using (ExifReader exifReader = new ExifReader(_photoFile.FullName))
                {
                    if (exifReader.GetTagValue<DateTime>(ExifTags.DateTime, out var dateTime))
                        exifData.DateTime = dateTime;
                }
            }
            catch (ExifLibException)
            { }
            return exifData;
        }

        private DirectoryInfo GetTargetDirectory(DirectoryInfo targetBaseDirectory, string directoryStructure)
        {
            var targetSubdirectory = directoryStructure
                .Replace("{year}", _exifData.DateTime.Year.ToString())
                .Replace("{month}", _exifData.DateTime.Month.ToString())
                .Replace("{day}", _exifData.DateTime.Day.ToString());
            return new DirectoryInfo(
                Path.Combine(targetBaseDirectory.FullName, targetSubdirectory));
        }

        public void CopyPhoto(CopySettings copySettings)
        {
            var targetDirectory = GetTargetDirectory(copySettings.ToDirectory, copySettings.DirectoryStructure);
            var targetFile = new FileInfo(Path.Combine(targetDirectory.FullName, _photoFile.Name));
            if (!targetFile.Exists)
            {
                if (!targetDirectory.Exists)
                    targetDirectory.Create();
                _photoFile.CopyTo(targetFile.FullName);
            }
        }
    }
}
