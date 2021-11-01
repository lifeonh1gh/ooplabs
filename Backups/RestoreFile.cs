using System;

namespace Backups
{
    public class RestoreFile
    {
        public RestoreFile(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}