using System;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            var fileA = new RestoreFile("FileA");
            var fileB = new RestoreFile("FileB");
            var fileC = new RestoreFile("FileC");
            Backup backup = new Backup(1);
            FullRestorePoint frp = new FullRestorePoint(1, backup);
            frp.AddFile(fileA);
            frp.AddFile(fileB);
            frp.AddFile(fileC);
            backup.AddRestorePoint(frp);
            FullRestorePoint frp2 = new FullRestorePoint(2, backup);
            frp2.RemoveFile(0);
            backup.AddRestorePoint(frp2);
        }
    }
}
