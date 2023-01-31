using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RCMAPI.DBManagers;
using RCMAPI.Managers;
using System;
using System.Threading.Tasks;

namespace RCMAPI.Controllers
{
    [ApiController]
    [System.Web.Mvc.Route("[controller]")]
    public class FTPController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFileOperationsManager _fileOpsManager;
        public FTPController(ILogger logger, IFileOperationsManager fileOpsManager)
        {
            _logger = logger;
            _fileOpsManager = fileOpsManager;

        }

        // GET: FTP
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("FileUpload")]
        // [Authorize]
        public async Task<IActionResult> PatientPhotoUpload([FromForm] RemoteFileUpload remoteFileUpload)
        {
            try
            {
                _logger.LogDebug("Upload call Start.");
                var response = await _fileOpsManager.FileUpload(remoteFileUpload);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError("Upload:" + e);
                return BadRequest(e.Message.ToString());
            }
        }
    }
}