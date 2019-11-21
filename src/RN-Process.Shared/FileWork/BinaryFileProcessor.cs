using System.IO;
using System.IO.Abstractions;

namespace RN_Process.Shared.FileWork
{
    public class BinaryFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public BinaryFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem())
        {
        }


        public BinaryFileProcessor(string inputFilePath, string outputFilePath,
            IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public void Process()
        {
            using (var inputFileStream = _fileSystem.File.Open(
                InputFilePath, FileMode.Open, FileAccess.Read))
            using (var binaryStreamReader = new BinaryReader(inputFileStream))
            using (var outputFileStream = _fileSystem.File.Create(OutputFilePath))
            using (var binaryStreamWriter = new BinaryWriter(outputFileStream))
            {
                byte largest = 0;

                while (binaryStreamReader.BaseStream.Position < binaryStreamReader.BaseStream.Length)
                {
                    var currentByte = binaryStreamReader.ReadByte();

                    binaryStreamWriter.Write(currentByte);

                    if (currentByte > largest) largest = currentByte;
                }

                binaryStreamWriter.Write(largest);
            }
        }
    }
}