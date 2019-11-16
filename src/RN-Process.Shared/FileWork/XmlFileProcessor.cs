using System;
using System.IO.Abstractions;

namespace RN_Process.Shared.FileWork
{
    public class XmlFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public XmlFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        public XmlFileProcessor(string inputFilePath, string outputFilePath, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}
