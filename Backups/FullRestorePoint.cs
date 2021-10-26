using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups
{
    public class FullRestorePoint : RestorePoint
    {
        public FullRestorePoint(int id, Backup backup)
            : base(id, backup)
        {
            Id = id;
            RestoreFiles = new List<RestoreFile>();
            if (backup.RestorePoints.Count != 0)
            {
                int i = 0;
                foreach (var oldRestorePoints in backup.RestorePoints)
                {
                    if (i == backup.RestorePoints.Count - 1)
                    {
                        foreach (var restoreFile in oldRestorePoints.RestoreFiles)
                        {
                            var value = Id.ToString();
                            var fileName = restoreFile.FilePath.Substring(0, 5);
                            var result = fileName + "_" + value;
                            RestoreFile rFile = new RestoreFile(result);
                            RestoreFiles.Add(rFile);
                        }
                    }

                    i++;
                }
            }
        }

        public override void AddFile(RestoreFile file)
        {
            var value = Id.ToString();
            var fileName = file.FilePath.Substring(0, 5);
            var result = fileName + "_" + value;
            file = new RestoreFile(result);
            RestoreFiles.Add(file);
        }

        public override void RemoveFile(int index)
        {
            RestoreFiles.RemoveAt(index);
        }
    }
}