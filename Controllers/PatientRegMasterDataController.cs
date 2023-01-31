using RCMAPI.Models;
using RCMAPI.Services;
using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace RCMAPI.Controllers
{
    public class PatientRegMasterDataController : BaseController
    {
        // GET: PatientRegMasterData
        [HttpPost]
        [Route("API/GetPatientRegMasterData")]
        public IHttpActionResult Post([FromBody] GetMasterDataList DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.GetPatientRegMasterData(DocParams);
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
        [Route("API/FetchPatientData")]
        public IHttpActionResult Post([FromBody] FetchPatientDataList DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchPatientData(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchPatientData");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/SavePatientData")]
        public IHttpActionResult Post([FromBody] PatientRegistrationDetails DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.SavePatientData(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving SavePatientData");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchPatientRoot")]
        public IHttpActionResult Post([FromBody] FetchPatientRoot DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchPatientRoot(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchPatientRoot");
            }
            return OkOrNotFound(objBase);
        }

        [HttpPost]
        [Route("API/FetchCityArea")]
        public IHttpActionResult Post([FromBody] FetchCityArea DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchCityArea(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCityArea");
            }
            return OkOrNotFound(objBase);
        }


        [HttpPost]
        [Route("API/UpdatePatientData")]
        public IHttpActionResult Post1([FromBody] PatientRegistrationDetails DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.UpdatePatientData(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving UpdatePatientData");
            }
            return OkOrNotFound(objBase);
        }


        [HttpPost]
        [Route("API/FetchDoctorWiseAvailability")]
        public IHttpActionResult Post([FromBody] GetDoctorWiseAvailability DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchDoctorWiseAvailability(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchDoctorWiseAvailability");
            }
            return OkOrNotFound(objBase);
        }

        [HttpPost]
        [Route("API/ValidateNationalID")]
        public IHttpActionResult Post([FromBody] ValidateNationalID DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.ValidateNationalID(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving ValidateNationalID");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/ValidatePinBlock")]
        public IHttpActionResult Post([FromBody] ValidatePinBlock DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.ValidatePinBlock(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving ValidatePinBlock");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/CurrentDayDoctorSpecAvailability")]
        public IHttpActionResult Post([FromBody] CurrentDayDoctorAvailability DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.CurrentDayDoctorSpecAvailability(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving CurrentDayDoctorSpecAvailability");
            }
            return OkOrNotFound(objBase);
        }



        [HttpPost]
        [Route("API/FetchHospitalDoctors")]
        public IHttpActionResult Post([FromBody] HospitalDoctors DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchHospitalDoctors(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchHospitalDoctors");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchHospitalSpecialisations")]
        public IHttpActionResult Post([FromBody] HospitalSpecialisations DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchHospitalSpecialisations(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchHospitalSpecialisations");
            }
            return OkOrNotFound(objBase);
        }

        [HttpPost]
        [Route("API/FetchAllCities")]
        public IHttpActionResult Post([FromBody] SmartSearch DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchAllCities(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchHospitalSpecialisations");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchAgeCalculate")]
        public IHttpActionResult Post([FromBody] FetchAge DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchAgeCalculate(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchAgeCalculate");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchDOB")]
        public IHttpActionResult Post([FromBody] FetchDOB DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchDOB(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchDOB");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchHijri")]
        public IHttpActionResult Post([FromBody] FetchHijri DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchHijri(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchDOB");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchEnglishDate")]
        public IHttpActionResult Post([FromBody] FetchEnglishDate DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchEnglishDate(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchDOB");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/UploadToFTP")]
        public IHttpActionResult Post([FromBody] UploadToFTP DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.UploadToFTP(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchDOB");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchAllCitiesC")]
        public IHttpActionResult Post([FromBody] FetchAllCities DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchAllCities(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchAllCities");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/PATIENTDICTIONARY")]
        public IHttpActionResult Post([FromBody] PATIENTDICTIONARY DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.PATIENTDICTIONARY(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchAllCities");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchNationalitiesPriority")]
        public IHttpActionResult Post([FromBody] FetchNationalitiesPriority DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchNationalitiesPriority(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchAllCities");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchCityMasters")]
        public IHttpActionResult Post([FromBody] FetchCityMasters DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchCityMasters(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCityMasters");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchPatientModificationAudit")]
        public IHttpActionResult Post([FromBody] FetchPatientModificationAudit DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchPatientModificationAudit(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchPatientModificationAudit");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/HospitalServices")]
        public IHttpActionResult Post([FromBody] HospitalServicesInputs DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.GetAllServices(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCityMasters");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchConsultants")]
        public IHttpActionResult Post([FromBody] FetchConsultants DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchConsultants(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchPatientData");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FileUpload")]
        public IHttpActionResult FileUpload([FromBody] FileUpload fileUpload)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FileUpload(fileUpload.Base64, fileUpload.FileName);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "File upload error";
                SetErrorObject(objBase, ex, "FTPException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in uploading File");
            }
            return OkOrNotFound(objBase);
        }
        [HttpGet]
        [Route("API/FileDownload")]
        public IHttpActionResult FileDownload(string FileName)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FileDownload(FileName);
                return OkOrNotFound(result);

            }
            catch (SqlException ex)
            {
                objBase.Message = "File download error";
                SetErrorObject(objBase, ex, "FTPException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in downloading File");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchAllEmployees")]
        public IHttpActionResult Post([FromBody] FetchEmployee DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchEmployees(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCityMasters");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/FetchEmployee")]
        public IHttpActionResult Post([FromBody] FetchSpeficEmployee DocParams)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.FetchEmployee(DocParams);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchCityMasters");
            }
            return OkOrNotFound(objBase);
        }
        [HttpPost]
        [Route("API/ValidateLoginCredentials/{username}/{password}/{location}")]
        public IHttpActionResult ValidateLoginCredentials(string username, string password, string location)
        {
            try
            {
                RCMService RCMIService = new RCMService();
                var result = RCMIService.ValidateLoginCredentials(username, password, location);
                return OkOrNotFound(result);
            }
            catch (SqlException ex)
            {
                objBase.Message = "SqlException";
                SetErrorObject(objBase, ex, "SqlException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in saving FetchPatientData");
            }
            return OkOrNotFound(objBase);
        }
        [HttpGet]
        [Route("API/PatientInfoByYakeenService")]
        public IHttpActionResult PatientInfoByYakeenService(string iqamaNumber, string dateOfBirth)
        {
            try
            {
                RCMService svcObj = new RCMService();
                var result = svcObj.PatientInfoByYakeenService(iqamaNumber, dateOfBirth);
                return OkOrNotFound(result);

            }
            catch (SqlException ex)
            {
                objBase.Message = "Error in PatientInfoByYakeenService";
                SetErrorObject(objBase, ex, "FTPException");
            }
            catch (Exception ex)
            {
                SetErrorObject(objBase, ex, "Error in PatientInfoByYakeenService");
            }
            return OkOrNotFound(objBase);
        }
    }
}