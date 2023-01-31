
using FluentFTP;
using RCMAPI.DBManagers;

namespace RCMAPI.Models
{
  public  class FtpRemoteFileContext 
    {
        public IFtpClient FtpClient { get; set; }
        public FtpRemoteFileContext(RemoteFileUpload setting)
        {         
            FtpClient = new FtpClient(setting.Host);
            FtpClient.Credentials = new System.Net.NetworkCredential(setting.UserName, setting.Password);
            FtpClient.Port = setting.Port;
            FtpClient.Connect();
        }



     
    }
}
