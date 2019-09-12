using System.ComponentModel;
using System.IO;

namespace CopyByExif.Core
{
    public static class CopyPhotos
    {
        public static void Copy(CopySettings settings, BackgroundWorker copyWorker)
        {
            copyWorker.ReportProgress(0);

            int i = 0;
            var allPhotos = settings.FromDirectory.GetFiles("*.jp*g", SearchOption.AllDirectories);
            foreach (var photoFile in allPhotos)
            {
                var photoToCopy = new PhotoToCopy(photoFile);
                photoToCopy.CopyPhoto(settings);

                copyWorker.ReportProgress(100 * ++i / allPhotos.Length);
            }
        }
    }
}
