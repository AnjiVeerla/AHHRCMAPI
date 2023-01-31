using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Web.Http;
namespace RCMAPI.Controllers
{
    public class ServicePriceController : BaseController
    {
        [HttpPost]
        [Route("API/FetchServicePrice")]
        public IHttpActionResult Post([FromBody] FetchServicePrice DocParams)
        {
            try
            {
                BillSummary RCMIService = new BillSummary();
                var result = RCMIService.FetchServicePrice(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving GetPatientRegMasterData");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/ValidationRegCode")]
        public IHttpActionResult Post([FromBody] ValidationRegCode DocParams)
        {
            try
            {
                BillSummary RCMIService = new BillSummary();
                var result = RCMIService.ValidationRegCode(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving GetPatientRegMasterData");
            }
            return OkOrNotFound(objBase);
        }


    }
}
