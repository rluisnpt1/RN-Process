using System.IO.Abstractions;

namespace RN_Process.Shared.FileWork
{
    public class TextFileProcessor
    {
        private readonly IFileSystem _fileSystem;

        public string InputFilePath { get; }
        public string OutputFilePath { get; }

        public TextFileProcessor(string inputFilePath, string outputFilePath)
            : this(inputFilePath, outputFilePath, new FileSystem()) { }

        public TextFileProcessor(string inputFilePath, string outputFilePath, 
            IFileSystem fileSystem)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
            _fileSystem = fileSystem;
        }

        public void Process()
        {
            using (var inputStreamReader = _fileSystem.File.OpenText(InputFilePath))            
            using (var outputStreamWriter = _fileSystem.File.CreateText(OutputFilePath))
            {
                var currentLineNumber = 1;
                while (!inputStreamReader.EndOfStream)
                {
                    string line = inputStreamReader.ReadLine();

                    Write(currentLineNumber == 2 ? line.ToUpperInvariant() : line);

                    currentLineNumber++;

                    void Write(string content)
                    {
                        bool isLastLine = inputStreamReader.EndOfStream;

                        if (isLastLine)
                        {
                            outputStreamWriter.Write(content);
                        }
                        else
                        {
                            outputStreamWriter.WriteLine(content);
                        }
                    }

                }
            }
        }
    }
}