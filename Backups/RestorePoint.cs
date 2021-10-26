using System.Collections.Generic;

namespace Backups
{
    public abstract class RestorePoint
    {
        protected RestorePoint(int id, Backup backup)
        {
            Id = id;
        }

        public List<RestoreFile> RestoreFiles { get; set; }
        protected int Id { get; set; }

        public abstract void AddFile(RestoreFile file);
        public abstract void RemoveFile(int index);
    }
}