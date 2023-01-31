using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace RCMAPI.Controllers
{
    public class CompanyMasterController : BaseController
    {
        [HttpPost]
        [Route("API/FetchCompanyTypes")]
        public IHttpActionResult Post([FromBody] GetCompanyTypesDataList DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchCompanyTypes(DocParams);
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
        [Route("API/savecompany")]
        public IHttpActionResult SaveCompany([FromBody] Company company)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.SaveCompany(company);
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

        [HttpGet]
        [Route("API/getcompany")]
        public IHttpActionResult FetchCompany(int id)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchCompany(id);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCompany");
            }
            return OkOrNotFound(objBase);
        }

        [HttpGet]
        [Route("API/allcompanies")]
        public IHttpActionResult FetchAllCompanyList()
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchAllCompanies();
                return Ok(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in  FetchAllCompanyList");
            }
            return OkOrNotFound(objBase);
        }
    }
}