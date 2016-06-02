using System;
using System.Collections.Generic;
using System.IO;

namespace GXTExtractor
{
    class Program
    {
        private static string outputPath;

        private static void recurseDirectories(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            List<DirectoryInfo> directories = new List<DirectoryInfo>(info.EnumerateDirectories());
            foreach (DirectoryInfo directory in directories)
            {
                recurseDirectories(directory.FullName);
                parseFiles(directory.GetFiles());
            }
        }

        private static void parseFiles(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".gxt")
                {
                    Console.WriteLine("Processing " + file.Name);
                    try
                    {
                        GXTFile gxtFile = new GXTFile(file.FullName);
                        for (int i = 1; i <= gxtFile.FileCount; i++)
                        {
                            gxtFile.Export(i, outputPath);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid GTX file: " + file.Name);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                outputPath = args[1];
                recurseDirectories(args[0]);
            }
            else
            {
                Console.WriteLine("Usage: GTXExtractor <input directory> <output directory>");
            }
        }
    }
}
