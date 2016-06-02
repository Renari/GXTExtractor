using System.IO;

namespace GXTExtractor
{
    class GXTFile
    {
        public readonly int HeaderSize;
        public readonly int FileCount;
        public readonly int FileSize;
        public readonly int[] ContentOffsets;
        public readonly string FileName;
        public readonly string Location;

        public GXTFile(string filePath)
        {
            try
            {
                FileName = Path.GetFileNameWithoutExtension(filePath);
                Location = filePath;
                using (FileStream stream = File.OpenRead(filePath))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    HeaderSize = reader.ReadInt32();
                    FileCount = reader.ReadInt32();
                    FileSize = reader.ReadInt32();
                    ContentOffsets = new int[FileCount];
                    for (int i = 0; i < FileCount; i++)
                    {
                        ContentOffsets[i] = reader.ReadInt32();
                    }
                }
            }
            catch
            {
                throw new InvalidGXTException();
            }
        }

        public void Export(int fileNumber, string exportPath)
        {
            using (FileStream stream = File.OpenRead(Location))
            using (FileStream writeStream = File.OpenWrite(exportPath + @"\" + FileName + "_" + fileNumber + ".dds"))
            {
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(writeStream);

                // seek to start location
                reader.BaseStream.Seek(ContentOffsets[fileNumber - 1] + HeaderSize, SeekOrigin.Begin);

                int fileSize;
                // if this is the last file the file ends at the end of the gxt file
                if (fileNumber == FileCount)
                {
                    fileSize = (int)reader.BaseStream.Length - ContentOffsets[fileNumber - 1] - HeaderSize;
                }
                else
                {
                    fileSize = ContentOffsets[fileNumber] - ContentOffsets[fileNumber - 1];
                }
                writeStream.Write(reader.ReadBytes(fileSize), 0, fileSize);
            }
        }
    }
}
