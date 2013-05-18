using System;
using System.IO;
using System.Threading;

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

                // make sure file has finished copying before trying to use it
                while (IsFileLocked(fi))
                {
                    Thread.Sleep(1000);
                }

                string tmp = fi.Directory.ToString();

                // get the label. This is the parent directory of the file created
                string eventParent = tmp.Substring(tmp.LastIndexOf('\\') + 1);

                // make sure it is not the download folder as utorrent does these
                // possible problem if there is a folder inside the download folder named the same
                // but this is a issue for another day
                if (downloadFolder != eventParent)
                {
                    // successfully loaded
                    if (uAL.Program.t.AddTorrent(e.FullPath, eventParent))
                    {
                        if (string.Compare(uAL.Program.settings.DeleteFileOnAdd, "y", true) == 0)
                        {
                            MoveProcessedFile(e.FullPath);
                        }
                        else
                        {
                            RenameAfterLoading(e.FullPath);
                        }
                    }
                }
            };
        }

        // We should rename the torrent file to a .loaded 
        private bool RenameAfterLoading(string source)
        {
            bool renamed = false;
            int renumber = 2;
            string renameExtension = ".loaded";
            while (!renamed)
            {
                try
                {
                    if (File.Exists(source))
                    {
                        File.Move(source, source + renameExtension);
                        Console.WriteLine("Renamed to {0}.", source + renameExtension);
                        renamed = true;
                        return true;
                    }                    
                }
                catch (Exception)
                {
                    Console.WriteLine("File already exists {0} incrementing number...", source + renameExtension);
                    renameExtension = "_" + renumber.ToString() + ".loaded";
                    renumber += 1;
                    renamed = false;
                }
            }
            return false;
        }

        // once torrent's been added, move torrent file to temp directory
        // (safer than just deleting the file)
        private bool MoveProcessedFile(string source)
        {
            string fileName = Path.GetFileName(source);
            string destination = Path.Combine(Path.GetTempPath(), fileName);
            try
            {
                if (File.Exists(source))
                {
                    File.Move(source, destination);
                }
            }
            catch (Exception) { return false; }
            return true;
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException) { return true; }
            finally
            {
                if (stream != null) { stream.Close(); }
            }
            return false;
        }
    }
}
