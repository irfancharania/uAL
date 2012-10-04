using System;
using System.IO;

namespace uAL
{
    class FileSystemMonitor
    {
        public FileSystemMonitor()
        {
            string downloadDir = uAL.Program.settings.Dir;
            string downloadFolder = downloadDir.Substring(downloadDir.LastIndexOf('\\') + 1);
            FileSystemWatcher w = new FileSystemWatcher(downloadDir);
            w.Filter = "*.torrent";
            w.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            w.EnableRaisingEvents = true;
            w.IncludeSubdirectories = true;

            w.Created += (s, e) =>
            {
                FileInfo fi = new FileInfo(e.FullPath);
                string tmp = fi.Directory.ToString();

                // get the label. This is the parent directory of the file created
                string eventParent = tmp.Substring(tmp.LastIndexOf('\\') + 1);

                // make sure it is not the download folder as utorrent does these
                // possible problem if there is a folder inside the download folder named the same
                // but this is a issue for another day
                if (downloadFolder != eventParent)
                    uAL.Program.t.AddTorrent(e.FullPath, eventParent);
            };
        }
    }
}
