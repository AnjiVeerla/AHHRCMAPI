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
    public class CommonMasterController : BaseController
    {
        [HttpGet]
        [Route("API/CompanyTypes")]
        public IHttpActionResult GetCompanyTypes()
        {
            try
            {
                CommonMastersService objService = new CommonMastersService();
                var result = objService.GetCompanyTypes();
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in Fetching GetCompanyTypes");
            }
            return OkOrNotFound(null);
        }       

        [HttpGet]
        [Route("API/MasterData")]
        public IHttpActionResult GetMasterData(string type)
        {
            try
            {
                CommonMastersService objService = new CommonMastersService();
                var result = objService.GetMasterData(type);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in Fetching GetMasterData()");
            }
            return OkOrNotFound(null);
        }

        [HttpGet]
        [Route("API/Cities")]
        public IHttpActionResult GetCities(int countryId)
        {
            try
            {
                CommonMastersService objService = new CommonMastersService();
                var result = objService.GetCities(countryId);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in Fetching GetCities()");
            }
            return OkOrNotFound(null);
        }
    }
}
