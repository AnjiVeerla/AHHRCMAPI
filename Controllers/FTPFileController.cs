using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace RCMAPI.Controllers
{
    [Route("api/[controller]")]
    public class FTPFileController : Controller
    {
        // GET: FTPFile
        const string FILE_PATH = @"D:\Samples\";

        [HttpPost]
        public IActionResult Post([FromBody] FileToUpload theFile)
        {
            return Ok();
        }
    }
}