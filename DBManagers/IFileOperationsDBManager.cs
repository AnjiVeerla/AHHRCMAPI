
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace RCMAPI.DBManagers
{
    public interface IFileOperationsDBManager
    {    
        //Task ProfileUpload(ProfileData file);
        Task<(string, byte[], string)> ProfileDownload(int userID);
        Task<(string, byte[], string)> FileUpload(RemoteFileUpload remoteFileUpload);
        Task<(string, byte[], string)> FileDownload(RemoteFileUpload remoteFileUpload);
        Task<string> FileDelete(RemoteFileUpload remoteFileUpload);
        Task<(string,string)> SaveFileInformation(RemoteFileUpload fileUpload);


    }

    public class ProfileData
    {
        public int? UserID { get; set; }
        public IFormFile File { get; set; }
    }

    public class RemoteFileUpload
    {
        public string FileName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DirectoryFilePath { get; set; }
        public IFormFile File { get; set; }
        public int? UserID { get; set; }
        public string ServerType { get; set; }
        public string ScreenName { get; set; }
        public string FieldName { get; set; }
        public string ContentType { get; set; }
        public string FileSize { get; set; }
    }
}
