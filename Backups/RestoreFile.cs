namespace Backups
{
    public class RestoreFile
    {
        public RestoreFile(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}