using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace RCMAPI.Controllers
{
    public class SmartSearchController : BaseController
    {
        // GET: SmartSearch
        public IHttpActionResult Post([FromBody] SmartSearch DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.SmartSearchData(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving.");
            }
            return OkOrNotFound(objBase);
        }
    }
}