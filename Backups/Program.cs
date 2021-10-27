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
            var backup = new Backup(1);
            var frp = new FullRestorePoint(1, backup);
            frp.AddFile(fileA);
            frp.AddFile(fileB);
            frp.AddFile(fileC);
            backup.AddRestorePoint(frp);
            var frp2 = new FullRestorePoint(2, backup);
            frp2.RemoveFile(0);
            backup.AddRestorePoint(frp2);
        }
    }
}
