using Microsoft.Data.SqlClient;
using RCMAPI.DBManagers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;


namespace RCM.PortalAPI.Infrastructure.DBManagers
{
    public class FileOperationsDBManager : IFileOperationsDBManager
    {

        private RCMDBAPortalContext _dbContext;
        private IDBManager _dbManager;
        private readonly ILogger _logger;

        public FileOperationsDBManager(RCMDBAPortalContext dbContext, IDBManager dbManager, ILogger logger)
        {
            _dbContext = dbContext;
            _dbManager = dbManager;
            _logger = logger;
        }      

        public async Task<(string, byte[], string)> ProfileDownload(int userID)
        {
            int id = userID;
            byte[] bytes;
            string fileName;
            string contentType;
            var constr = (SqlConnection)_dbContext.Database.GetDbConnection();
            using (SqlConnection con = new SqlConnection(constr.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select FileName, Data,ContentType from profileImage where UserID=@Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        if (sdr.HasRows)
                        {
                            bytes = (byte[])sdr["Data"];
                            fileName = sdr["FileName"].ToString();
                            contentType = sdr["ContentType"].ToString();
                        }
                        else
                            return (null, null, null);
                    }
                    con.Close();
                }
            }
            return (fileName, bytes, contentType);
        }


        public async Task<(string, byte[], string)> FileUpload(RemoteFileUpload remoteFileUpload)
        {
            //  string filename = Path.GetFileName(file);
            _logger.Information("FileOperationsDBManager -> FileUpload Start.");
            string contentType = remoteFileUpload.File.ContentType;
            string strPath = remoteFileUpload.File.FileName.ToString();
            string filename = Path.GetFileName(strPath);
            if (remoteFileUpload.ServerType == "FTP")
            {
                FtpRemoteFileContext ftpRemoteFileContext = new FtpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.FtpClient.IsConnected)
                {

                    using (Stream fs = remoteFileUpload.File.OpenReadStream())
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);
                            string path = WriteFile(filename, bytes);
                            int num = new Random().Next(1000, 9999);
                            ftpRemoteFileContext.FtpClient.UploadFile(Path.Combine(path, filename), remoteFileUpload.DirectoryFilePath + num + "_" + filename);
                            File.Delete(Path.Combine(path, filename));
                            _logger.Information("FileOperationsDBManager -> FileUpload End.");
                            return (num + "_" + filename, bytes, contentType);
                        }

                    }
                }
            }
            else if (remoteFileUpload.ServerType == "SFTP")
            {
                SftpRemoteFileContext ftpRemoteFileContext = new SftpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.SftpClient.IsConnected)
                {
                    using (Stream fs = remoteFileUpload.File.OpenReadStream())
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);
                            string path = WriteFile(filename, bytes);
                            ftpRemoteFileContext.SftpClient.UploadFile(fs, remoteFileUpload.DirectoryFilePath + filename);
                            File.Delete(Path.Combine(path, filename));
                            _logger.Information("FileOperationsDBManager -> FileUpload End.");
                            return (filename, bytes, contentType);
                        }

                    }
                }
            }

            return (null, null, null);
        }
        public string WriteFile(string fileName, byte[] bytes)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // if (!path.EndsWith(@"\"))
            // path += @"\file";

            if (File.Exists(Path.Combine(path, fileName)))
                File.Delete(Path.Combine(path, fileName));

            using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.CreateNew, FileAccess.Write))
            {
                fs.Write(bytes, 0, (int)bytes.Length);
                fs.Close();
            }

            return path;
        }


        public async Task<(string, byte[], string)> FileDownload(RemoteFileUpload remoteFileUpload)
        {
            _logger.Information("FileOperationsDBManager -> FileDownload Start.");
            string fileName;
            string contentType = "";
            if (remoteFileUpload.ServerType == "FTP")
            {
                FtpRemoteFileContext ftpRemoteFileContext = new FtpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.FtpClient.IsConnected)
                {
                    // ftpRemoteFileContext.FtpClient.DownloadFile(remoteFileUpload.FileName, Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName));

                    using (Stream fs = ftpRemoteFileContext.FtpClient.OpenRead(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName)))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);
                            _logger.Information("FileOperationsDBManager -> FileDownload End.");
                            return (remoteFileUpload.FileName, bytes, contentType);
                        }

                    }
                }
            }

            else if (remoteFileUpload.ServerType == "SFTP")
            {
                SftpRemoteFileContext ftpRemoteFileContext = new SftpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.SftpClient.IsConnected)
                {
                    Stream fileStream = File.Create(remoteFileUpload.FileName);
                    // ftpRemoteFileContext.SftpClient.DownloadFile(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName), fileStream);

                    using (Stream fs = ftpRemoteFileContext.SftpClient.OpenRead(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName)))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);
                            _logger.Information("FileOperationsDBManager -> FileDownload End.");
                            return (remoteFileUpload.FileName, bytes, contentType);
                        }

                    }
                }
            }


            return (null, null, null);
        }

        public async Task<string> FileDelete(RemoteFileUpload remoteFileUpload)
        {
            _logger.Information("FileOperationsDBManager -> FileDelete Start.");
            if (remoteFileUpload.ServerType == "FTP")
            {
                FtpRemoteFileContext ftpRemoteFileContext = new FtpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.FtpClient.IsConnected)
                {
                    if (ftpRemoteFileContext.FtpClient.FileExists(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName)))
                    {
                        ftpRemoteFileContext.FtpClient.DeleteFile(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName));
                        _logger.Information("FileOperationsDBManager -> FileDelete End.");
                        return Constants.SUCCESS_MESSAGE;
                    }
                }
            }

            else if (remoteFileUpload.ServerType == "SFTP")
            {
                SftpRemoteFileContext ftpRemoteFileContext = new SftpRemoteFileContext(remoteFileUpload);
                if (ftpRemoteFileContext.SftpClient.IsConnected)
                {
                    if (ftpRemoteFileContext.SftpClient.Exists(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName)))
                    {
                        ftpRemoteFileContext.SftpClient.DeleteFile(Path.Combine(remoteFileUpload.DirectoryFilePath, remoteFileUpload.FileName));
                        _logger.Information("FileOperationsDBManager -> FileDelete End.");
                        return Constants.SUCCESS_MESSAGE;
                    }
                }
            }


            return null;
        }


        public async Task<(string, string)> SaveFileInformation(RemoteFileUpload fileUpload)
        {
            _logger.Information("FileOperationsDBManager -> SaveFileInformation Start.");


            List<SqlParameter> dbParams = new List<SqlParameter>
                 {
                     _dbManager.CreateParameter(DbType.String,"@ScreenName", fileUpload.ScreenName, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@FieldName", fileUpload.FieldName, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@FileName ", fileUpload.FileName, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@FileExtension", fileUpload.ContentType, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@FileSize", fileUpload.FileSize, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@FileLocation",fileUpload.ServerType, ParameterDirection.Input),
                     _dbManager.CreateParameter(DbType.String,"@CreatedBy", fileUpload.UserID, ParameterDirection.Input),

            };

            var response = await _dbManager.SaveDataWithReturnVal("Pr_SaveFileInformation", dbParams);
            _logger.Information("FileOperationsDBManager -> SaveFileInformation End.");
            if (response == 0)
                return (Constants.SUCCESS_MESSAGE, Constants.SUCCESS_MESSAGE);
            else
                return (response.ToString(), string.Empty);
        }


    }
}
