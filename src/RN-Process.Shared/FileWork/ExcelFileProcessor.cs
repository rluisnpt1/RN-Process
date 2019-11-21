using System;
using System.IO.Abstractions;

namespace RN_Process.Shared.FileWork
{
    public class ExcelFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public ExcelFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem())
        {
        }

        public ExcelFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}