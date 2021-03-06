﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RN_Process.Shared.Commun;

namespace RN_Process.Api.Models
{
    public class DueDetailConfiguration
    {
        public DueDetailConfiguration()
        {
            FileHeaderColumns = new List<string> {"CLIENT_COLUM1", "CLIENT_COLUM2", "CLIENT_COLUM3"};
            AvailableFieldsColumns = RnProcessConstant.AvailableColumnsIntrum.ToList();
           // FilesDataInContracts = new List<FileDataContract>();
        }

        public DueDetailConfiguration(string id, string codOrg, string communicationType, string linkToAccess,
            string linkToAccessType, string typeOfResponse, bool requiredLogin, string authenticationLogin,
            string authenticationPassword, string hostkeyFingerPrint, string authenticationCodeApp,
            string pathToOriginFile, string pathToDestinationFile, string pathToFileBackupAtClient,
            string fileDelimiter, bool hashearder, string fileProtectedPassword,
            string fileName,
            IList<string> fileHeaderColumns, IList<string> availableFieldsColumns)
        {
            Id = id;
            CodOrg = codOrg;
            CommunicationType = communicationType;
            LinkToAccess = linkToAccess;
            LinkToAccessType = linkToAccessType;
            TypeOfResponse = typeOfResponse;
            RequiredLogin = requiredLogin;
            AuthenticationLogin = authenticationLogin;
            AuthenticationPassword = authenticationPassword;
            HostkeyFingerPrint = hostkeyFingerPrint;
            AuthenticationCodeApp = authenticationCodeApp;
            PathToOriginFile = pathToOriginFile;
            PathToDestinationFile = pathToDestinationFile;
            PathToFileBackupAtClient = pathToFileBackupAtClient;
            FileDelimiter = fileDelimiter;
            HasHeader = hashearder;
            FileProtectedPassword = fileProtectedPassword;
            FileHeaderColumns = fileHeaderColumns;
            AvailableFieldsColumns = availableFieldsColumns;
            FileName = fileName;
        }

        public string Id { get; set; }

        public string CodOrg { get;  set; }
        /// <summary>
        ///     Type communication has been agreed
        /// </summary>
        [Required]
       // [JsonConverter(typeof(StringEnumConverter))]
        public string CommunicationType { get; set; }

        /// <summary>
        ///     link or path to access the file
        /// </summary>
        [Display(Name = "Address to Access file (LINK|IP)")]
        [Required]
        public string LinkToAccess { get; set; }

        /// <summary>
        ///     Type acces //SFTP HTTP HTTPS
        /// </summary>
        [Display(Name = "Type Access")]
        public string LinkToAccessType { get; set; }

        /// <summary>
        ///     File Format Agreed
        /// </summary>
        [Display(Name = "Type File Response")]
        public string TypeOfResponse { get; set; }

        /// <summary>
        ///     If need login
        /// </summary>
        [Display(Name = "Required Login?")]
        public bool RequiredLogin { get; set; }

        [Display(Name = "User Login")] 
        public string AuthenticationLogin { get; set; }

        [Display(Name = "Password")] 
        public string AuthenticationPassword { get; set; }


        [Display(Name = "SSH Host key Finger Print ")]
        public string HostkeyFingerPrint { get; set; }


        [Display(Name = "app code identification")]
        public string AuthenticationCodeApp { get; set; }


        [Display(Name = "Path to get file (client side)")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PathToOriginFile { get; set; }

        [Display(Name = "Path to send file back (client side)")]
        public string PathToDestinationFile { get; set; }

        [Display(Name = "Path to File backup (client side)")]
        public string PathToFileBackupAtClient { get; set; }

        [Display(Name = "File Delimiter")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FileDelimiter { get; set; }

        [Display(Name = "Has Header")]
        [Required]
        public bool HasHeader { get;  set; }

        [Display(Name = "File is Protected - Password")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FileProtectedPassword { get;  set; }

        [Display(Name = "File Name")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FileName { get;  set; }

        [Display(Name = "Columns Names at file")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public IList<string> FileHeaderColumns { get; set; }


        [Display(Name = "Intrum Columns Available")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public IList<string> AvailableFieldsColumns { get; set; }

       // [DisplayFormat(ConvertEmptyStringToNull = false)]
        //public IList<FileDataContract> FilesDataInContracts { get; set; }

        //public void AddFileIncontract(string id, string codOrg, string fileDescription, int fileSize, string fileFormat,
        //    string fileLocationOrigin, string locationToCopy, string status, List<BsonDocument> allDataInFile)
        //{
        //    FilesDataInContracts.Add(new FileDataContract(id,
        //        codOrg, fileDescription, fileSize, fileFormat, fileLocationOrigin, locationToCopy, status
        //        , false, null, allDataInFile));
        //}

    }
}