using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Web.Http;


namespace RCMAPI.Controllers
{
    public class BillSummaryController : BaseController
    {
        [HttpPost]
        [Route("API/PatientBillSummary")]
        public IHttpActionResult Post([FromBody] PatientBillList objAppointmentList)
        {
            try
            {
                BillSummary mobileAPPService = new BillSummary();
                var result = mobileAPPService.PatientBillSummary(objAppointmentList);
                return OkOrNotFound(null);
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
