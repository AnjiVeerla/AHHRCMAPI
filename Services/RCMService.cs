using RCMAPI.DAL;
using RCMAPI.CommonUtilities;
using RCMAPI.Models;
using System;
using System.Collections.Generic;
using Moyasar;

using Moyasar.Services;

namespace RCMAPI.Services
{
    public class RCMService
    {
        public Base GetPatientRegMasterData(GetMasterDataList docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.GetPatientRegMasterData(docParams);
            return obj;
        }
        public Base SmartSearchData(SmartSearch docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.SmartSearchData(docParams);
            return obj;
        }

      

        public Base FetchPatientRoot(FetchPatientRoot docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchPatientRoot(docParams);
            return obj;
        }
        public Base FetchPatientData(FetchPatientDataList docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchPatientData(docParams);
            return obj;
        }
        public Base SavePatientData(PatientRegistrationDetails docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.SavePatientData(docParams);
            return obj;
        }
        public Base FetchCityArea(FetchCityArea docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchCityArea(docParams);
            return obj;
        }
        public Base UpdatePatientData(PatientRegistrationDetails docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.UpdatePatientData(docParams);
            return obj;
        }
        public Base FetchDoctorWiseAvailability(GetDoctorWiseAvailability docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchDoctorWiseAvailability(docParams);
            return obj;
        }
        public Base ValidateNationalID(ValidateNationalID docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.ValidateNationalID(docParams);
            return obj;
        }
        public Base ValidatePinBlock(ValidatePinBlock docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.ValidatePinBlock(docParams);
            return obj;
        }
        public Base CurrentDayDoctorSpecAvailability(CurrentDayDoctorAvailability docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.CurrentDayDoctorSpecAvailability(docParams);
            return obj;
        }
        public Base FetchHospitalDoctors(HospitalDoctors docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchHospitalDoctors(docParams);
            return obj;
        }
        public Base FetchHospitalSpecialisations(HospitalSpecialisations docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchHospitalSpecialisations(docParams);
            return obj;
        }
        public Base FetchAllCities(SmartSearch docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchAllCities(docParams);
            return obj;
        }
        public Base FetchAgeCalculate(FetchAge docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchAgeCalculate(docParams);
            return obj;
        }
        public Base FetchDOB(FetchDOB docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchDOB(docParams);
            return obj;
        }
        public Base FetchHijri(FetchHijri docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchHijri(docParams);
            return obj;
        }
        public Base FetchEnglishDate(FetchEnglishDate docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchEnglishDate(docParams);
            return obj;
        }
        public Base UploadToFTP(UploadToFTP docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.UploadToFTP(docParams);
            return obj;
        }
        public Base FetchAllCities(FetchAllCities docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchAllCities(docParams);
            return obj;
        }

        public Base FetchCompanyTypes(GetCompanyTypesDataList docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchCompanyTypes(docParams);
            return obj;
        }
        public Base PATIENTDICTIONARY(PATIENTDICTIONARY docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.PATIENTDICTIONARY(docParams);
            return obj;
        }
        public Base FetchNationalitiesPriority(FetchNationalitiesPriority docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchNationalitiesPriority(docParams);
            return obj;
        }
        public Base FetchCityMasters(FetchCityMasters docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchCityMasters(docParams);
            return obj;
        }
        public Base FetchPatientModificationAudit(FetchPatientModificationAudit docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchPatientModificationAudit(docParams);
            return obj;
        }
        public Base GetAllServices(HospitalServicesInputs docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.GetAllServices(docParams);
            return obj;
        }
        public Base FetchConsultants(FetchConsultants docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchConsultants(docParams);
            return obj;
        }
        public string FileUpload(string base64, string fileName)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            return RCMDALObj.FileUpload(base64, fileName);
        }
        public string FileDownload(string FileName)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            return RCMDALObj.FileDownload(FileName);
        }
        public Base FetchEmployees(FetchEmployee docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchEmployees(docParams);
            return obj;
        }
        public Base FetchEmployee(FetchSpeficEmployee docParams)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.FetchEmployees(docParams);
            return obj;
        }
        public LoginDetails ValidateLoginCredentials(string username, string password, string location)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            LoginDetails obj = RCMDALObj.ValidateLoginCredentials(username, password, location);
            return obj;
        }
        public Base PatientInfoByYakeenService(string iqamaNumber, string dateOfBirth)
        {
            RCMDAL RCMDALObj = new RCMDAL();
            Base obj = RCMDALObj.PatientInfoByYakeenService(iqamaNumber, dateOfBirth);
            return obj;
        }     

        public int GetPayment()
        {          

            MoyasarService.ApiKey = "pk_test_WNzVNDPgy4T2Ka7U9M5zXY8bHYJe6zQiUA6vQFnQ";


            MoyasarService.ApiKey = "sk_test_MrtwozLJAuFmLKWWSaRaoaLX";

            var payment = Payment.Fetch("28fc9c0b-ecc3-4d75-9117-722eaab35505");
            return 1;
        }


    }
}