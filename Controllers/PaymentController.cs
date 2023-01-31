using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RCMAPI.Controllers
{
    public class PaymentController : BaseController
    {

        [HttpPost]
        [Route("API/getpayment")]
        public IHttpActionResult SaveCompany(Company company)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.GetPayment(company);
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
