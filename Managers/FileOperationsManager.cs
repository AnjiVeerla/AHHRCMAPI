using Microsoft.Extensions.Configuration;
using RCMAPI.DBManagers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RCMAPI.Managers
{
    public class FileOperationsManager : IFileOperationsManager
    {
              
        public IFileOperationsDBManager _fileOperationsDBManager;
        public IConfiguration _configuration;
        public FileOperationsManager(IFileOperationsDBManager fileOperationsDBManager, IConfiguration configuration)
        {
            _fileOperationsDBManager = fileOperationsDBManager;
            _configuration = configuration;          
        }


        public async Task<(string, string, string)> ProfileDownload(int userID)
        {
            var file = await _fileOperationsDBManager.ProfileDownload(userID);
            if (file.Item2 != null)
            {
                var fileName = file.Item1;
                var data = Convert.ToBase64String(file.Item2);
                var contentType = file.Item3;
                return (fileName, data, contentType);
            }
            else
                return (null, null, null);

        }

       


        public async Task<(string, string, string)> FileUpload(RemoteFileUpload remoteFileUpload)
        {
            var filesData = new List<FileAttributes>();
            remoteFileUpload.Host = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Host").Value;
            remoteFileUpload.Port = Convert.ToInt32(_configuration.GetSection("RemoteFileUploadSettings").GetSection("Port").Value);
            remoteFileUpload.UserName = _configuration.GetSection("RemoteFileUploadSettings").GetSection("UserName").Value;
            remoteFileUpload.Password = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Password").Value;
            remoteFileUpload.ServerType = _configuration.GetSection("RemoteFileUploadSettings").GetSection("ServerType").Value;
            remoteFileUpload.DirectoryFilePath = _configuration.GetSection("RemoteFileUploadSettings").GetSection("DirectoryFilePath").Value;
           var file=  await _fileOperationsDBManager.FileUpload(remoteFileUpload);

            var fileName = file.Item1;
            var data = Convert.ToBase64String(file.Item2);
            var contentType = file.Item3;
            remoteFileUpload.ContentType = contentType;
            remoteFileUpload.FileName = fileName;
            remoteFileUpload.FileSize = remoteFileUpload.File.Length.ToString();
            await _fileOperationsDBManager.SaveFileInformation(remoteFileUpload);
            return (fileName, data, contentType);
        }

        public async Task<(string, string, string)> ImageFileDownload(RemoteFileUpload remoteFileUpload)
        {
            var filesData = new List<FileAttributes>();
            remoteFileUpload.Host = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Host").Value;
            remoteFileUpload.Port = Convert.ToInt32(_configuration.GetSection("RemoteFileUploadSettings").GetSection("Port").Value);
            remoteFileUpload.UserName = _configuration.GetSection("RemoteFileUploadSettings").GetSection("UserName").Value;
            remoteFileUpload.Password = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Password").Value;
            remoteFileUpload.ServerType = _configuration.GetSection("RemoteFileUploadSettings").GetSection("ServerType").Value;
            remoteFileUpload.DirectoryFilePath = _configuration.GetSection("RemoteFileUploadSettings").GetSection("DirectoryFilePath").Value;
            var file = await _fileOperationsDBManager.FileDownload(remoteFileUpload);

            var fileName = file.Item1;
            var data = Convert.ToBase64String(file.Item2);
            var contentType = file.Item3;
            return (fileName, data, contentType);
        }
        public async Task<(string, byte[], string)> FileDownload(RemoteFileUpload remoteFileUpload)
        {
            var filesData = new List<FileAttributes>();
            remoteFileUpload.Host = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Host").Value;
            remoteFileUpload.Port = Convert.ToInt32(_configuration.GetSection("RemoteFileUploadSettings").GetSection("Port").Value);
            remoteFileUpload.UserName = _configuration.GetSection("RemoteFileUploadSettings").GetSection("UserName").Value;
            remoteFileUpload.Password = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Password").Value;
            remoteFileUpload.ServerType = _configuration.GetSection("RemoteFileUploadSettings").GetSection("ServerType").Value;
            remoteFileUpload.DirectoryFilePath = _configuration.GetSection("RemoteFileUploadSettings").GetSection("DirectoryFilePath").Value;
            var file = await _fileOperationsDBManager.FileDownload(remoteFileUpload);

            var fileName = file.Item1;
           // var data = Convert.ToBase64String(file.Item2);
            var contentType = file.Item3;
            return (fileName, file.Item2, contentType);
        }

        public async Task<(string, string)> FileDelete(RemoteFileUpload remoteFileUpload)
        {
            var filesData = new List<FileAttributes>();
            remoteFileUpload.Host = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Host").Value;
            remoteFileUpload.Port = Convert.ToInt32(_configuration.GetSection("RemoteFileUploadSettings").GetSection("Port").Value);
            remoteFileUpload.UserName = _configuration.GetSection("RemoteFileUploadSettings").GetSection("UserName").Value;
            remoteFileUpload.Password = _configuration.GetSection("RemoteFileUploadSettings").GetSection("Password").Value;
            remoteFileUpload.ServerType = _configuration.GetSection("RemoteFileUploadSettings").GetSection("ServerType").Value;
            remoteFileUpload.DirectoryFilePath = _configuration.GetSection("RemoteFileUploadSettings").GetSection("DirectoryFilePath").Value;
            var response = await _fileOperationsDBManager.FileDelete(remoteFileUpload);              
            return (response, response);
        }

       
    }


}
