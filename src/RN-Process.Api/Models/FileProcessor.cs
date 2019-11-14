﻿using System;
using System.IO;
using static System.Console;

namespace RN_Process.Api.Models
{
    internal class FileProcessor
    {
        private static readonly string BackupDirectoryName = "backup";
        private static readonly string InProgressDirectoryName = "processing";
        private static readonly string CompletedDirectoryName = "complete";

        public string InputFilePath { get; }

        public FileProcessor(string filePath)
        {
            InputFilePath = filePath;
        }

        public void Process()
        {
            WriteLine($"Begin process of {InputFilePath}");

            // Check if file exists
            if (!File.Exists(InputFilePath))
            {
                WriteLine($"ERROR: file {InputFilePath} does not exist.");
                return;
            }

            string rootDirectoryPath = new DirectoryInfo(InputFilePath).Parent.Parent.FullName;
            WriteLine($"Root data path is {rootDirectoryPath}");

            // Check if backup dir exists
            string inputFileDirectoryPath = Path.GetDirectoryName(InputFilePath);
            string backupDirectoryPath = Path.Combine(rootDirectoryPath, BackupDirectoryName);

            Directory.CreateDirectory(backupDirectoryPath);

            // Copy file to backup dir
            string inputFileName = Path.GetFileName(InputFilePath);
            string backupFilePath = Path.Combine(backupDirectoryPath, inputFileName);
            WriteLine($"Copying {InputFilePath} to {backupFilePath}");
            File.Copy(InputFilePath, backupFilePath, true);

            // Move to in progress dir
            Directory.CreateDirectory(Path.Combine(rootDirectoryPath, InProgressDirectoryName));

            string inProgressFilePath = 
                Path.Combine(rootDirectoryPath, InProgressDirectoryName, inputFileName);

            if (File.Exists(inProgressFilePath))
            {
                WriteLine($"ERROR: a file with the name {inProgressFilePath} is already being processed.");
                return;
            }

            WriteLine($"Moving {InputFilePath} to {inProgressFilePath}");
            File.Move(InputFilePath, inProgressFilePath);

            // Determine type of file
            string extension = Path.GetExtension(InputFilePath);

            string completedDirectoryPath = Path.Combine(rootDirectoryPath, CompletedDirectoryName);
            Directory.CreateDirectory(completedDirectoryPath);
            var dataToName = DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF");

            var completedFileName =
                $"{Path.GetFileNameWithoutExtension(InputFilePath)}-{Guid.NewGuid()}-{dataToName}{extension}";

            var completedFilePath = Path.Combine(completedDirectoryPath, completedFileName);

            switch (extension)
            {
                case ".txt":
                    var textProcessor = new TextFileProcessor(inProgressFilePath, completedFilePath);
                    textProcessor.Process();
                    break; 
                case ".xml":
                    var xmlProcessor = new XmlFileProcessor(inProgressFilePath, completedFilePath);
                    xmlProcessor.Process();
                    break;
                case ".xlsx":
                    var excelProcessor = new ExcelFileProcessor(inProgressFilePath, completedFilePath);
                    excelProcessor.Process();
                    break;
                case ".data":
                    var binaryProcessor = new BinaryFileProcessor(inProgressFilePath, completedFilePath);
                    binaryProcessor.Process();
                    break;
                case ".csv":
                    var csvProcessor = new CsvFileProcessor(inProgressFilePath, completedFilePath);
                    csvProcessor.Process();
                    break;
                default:
                    WriteLine($"{extension} is an unsupported file type.");
                    break;
            }

            WriteLine($"Completed processing of {inProgressFilePath}");

            WriteLine($"Deleting {inProgressFilePath}");
            File.Delete(inProgressFilePath);
        }

    }
}