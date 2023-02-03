using RCMAPI.Models;
using RCMAPI.Models.ContractManagement;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> SaveCompany(Company company)
        {
            try
            {
                ContractManagementService objContractMgmtService = new ContractManagementService();
                var result = await  objContractMgmtService.SaveCompany(company);

                var output = new
                {
                    status = result.Item2,
                    companyId = result.Item1
                };                
                return OkOrNotFound(output);
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
                ContractManagementService objContractMgmtService = new ContractManagementService();
                var result = objContractMgmtService.FetchCompany(id);
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
                ContractManagementService objContractMgmtService = new ContractManagementService();
                var result = objContractMgmtService.FetchAllCompanies();
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

        [HttpGet]
        [Route("API/sscompanies")]
        public IHttpActionResult FetchCompaniesSS(string value)
        {
            try
            {
                ContractManagementService objContractMgmtService = new ContractManagementService();
                var result = objContractMgmtService.FetchCompaniesSS(value);
                return Ok(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in  FetchCompanies()");
            }
            return OkOrNotFound(objBase);
        }
    }
}