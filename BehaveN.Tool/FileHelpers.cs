namespace BehaveN.Tool
{
    using System.Collections.Generic;
    using System.IO;

    public static class FileHelpers
    {
        public static List<string> ExpandWildcards(List<string> files)
        {
            var expandedFiles = new List<string>();

            foreach (var file in files)
            {
                if (file.Contains("*"))
                {
                    string path = Directory.GetCurrentDirectory();

                    if (!file.StartsWith("*"))
                    {
                        path = Path.GetDirectoryName(file);
                    }

                    string searchPattern = Path.GetFileName(file);

                    expandedFiles.AddRange(Directory.GetFiles(path, searchPattern));
                }
                else
                {
                    expandedFiles.Add(file);
                }
            }

            return expandedFiles;
        }
    }
}
