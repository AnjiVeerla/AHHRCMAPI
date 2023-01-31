using Microsoft.AspNetCore.Http;
using RCM.PortalAPI.Infrastructure.DBManagers;
using RCMAPI.DBManagers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace RCMAPI.Managers
{
    public interface IFileOperationsManager
    {
        //Task<ICollection<FileAttributes>> ProfileUpload(ProfileData file);
        Task<(string, string, string)> ProfileDownload(int userID);
        Task<(string, string, string)> FileUpload(RemoteFileUpload remoteFileUpload);
        Task<(string, string, string)> ImageFileDownload(RemoteFileUpload remoteFileUpload);
        Task<(string, byte[], string)> FileDownload(RemoteFileUpload remoteFileUpload);
        Task<(string, string)> FileDelete(RemoteFileUpload remoteFileUpload);
    }

    public class FileAttributes
    {
        public string FileType { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
    }

 
}
