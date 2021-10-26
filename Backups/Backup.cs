using System;
using System.Collections.Generic;

namespace Backups
{
    public class Backup
    {
        public Backup(int id)
        {
            Id = id;
            RestorePoints = new List<RestorePoint>();
        }

        public List<RestorePoint> RestorePoints { get; }
        private int Id { get; }

        public void AddRestorePoint(RestorePoint restorePoint)
        {
            RestorePoints.Add(restorePoint);
        }
    }
}