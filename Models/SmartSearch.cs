using RCMAPI.CommonUtilities;
using System;
using System.Collections.Generic;

namespace RCMAPI.Models
{
    public class SmartSearchDataList : Base
    {
        List<GetSmartSearchDataListOutput> MasterData = new List<GetSmartSearchDataListOutput>();
        public List<GetSmartSearchDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchPatientRootDataList : Base
    {
        List<GetFetchPatientRootDataListOutput> MasterData = new List<GetFetchPatientRootDataListOutput>();
        public List<GetFetchPatientRootDataListOutput> PatientIDRegCode { get { return MasterData; } set { MasterData = value; } }
    }

    public class FetchCityAreaDataList : Base
    {
        List<GetFetchCityAreaDataListOutput> MasterData = new List<GetFetchCityAreaDataListOutput>();
        public List<GetFetchCityAreaDataListOutput> CityAreaCode { get { return MasterData; } set { MasterData = value; } }
    }

    public class FetchAgeDataList : Base
    {
        List<FetchAgeDataListOutput> MasterData = new List<FetchAgeDataListOutput>();
        public List<FetchAgeDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchDOBDataList : Base
    {
        List<FetchDOBDataListOutput> MasterData = new List<FetchDOBDataListOutput>();
        public List<FetchDOBDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchAllCitiesDataList : Base
    {
        List<FetchAllCitiesDataListOutput> MasterData = new List<FetchAllCitiesDataListOutput>();
        public List<FetchAllCitiesDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchCompanyTypesDataList : Base
    {
        List<FetchCompanyTypesDataListOutput> MasterData = new List<FetchCompanyTypesDataListOutput>();
        public List<FetchCompanyTypesDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class PATIENTDICTIONARYDataList : Base
    {
        List<PATIENTDICTIONARYDataListOutput> MasterData = new List<PATIENTDICTIONARYDataListOutput>();
        public List<PATIENTDICTIONARYDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchCityMastersDataList : Base
    {
        List<FetchCityMastersDataListOutput> MasterData = new List<FetchCityMastersDataListOutput>();
        public List<FetchCityMastersDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchNationalitiesPriorityDataList : Base
    {
        List<FetchNationalitiesPriorityDataListOutput> MasterData = new List<FetchNationalitiesPriorityDataListOutput>();
        public List<FetchNationalitiesPriorityDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchPatientModificationAuditDataList : Base
    {
        List<FetchPatientModificationAuditListOutput> MasterData = new List<FetchPatientModificationAuditListOutput>();
        public List<FetchPatientModificationAuditListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class HospitalServices : Base
    {
        List<HospitalServicesOutput> MasterData = new List<HospitalServicesOutput>();
        public List<HospitalServicesOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class HospitalConsultants : Base
    {
        List<FetchConsultantsOutput> MasterData = new List<FetchConsultantsOutput>();
        public List<FetchConsultantsOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchEmployeeList : Base
    {
        List<FetchEmployeeOutput> MasterData = new List<FetchEmployeeOutput>();
        public List<FetchEmployeeOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchSpeficEmployeeList : Base
    {
        List<FetchSpeficEmployeeOutput> MasterData = new List<FetchSpeficEmployeeOutput>();
        public List<FetchSpeficEmployeeOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }


    public class FetchHijriDataList : Base
    {
        List<FetchHijriDataListOutput> MasterData = new List<FetchHijriDataListOutput>();
        public List<FetchHijriDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchEnglishDataList : Base
    {
        List<FetchEnglishDataListOutput> MasterData = new List<FetchEnglishDataListOutput>();
        public List<FetchEnglishDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class UploadToFTPDataList : Base
    {
        List<UploadToFTPDataListDataListOutput> MasterData = new List<UploadToFTPDataListDataListOutput>();
        public List<UploadToFTPDataListDataListOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }


    public class FetchPatientInfoDataList : Base
    {
        List<GetFetchPatientInfoDataListOutput> MasterData = new List<GetFetchPatientInfoDataListOutput>();
        public List<GetFetchPatientInfoDataListOutput> PatientData { get { return MasterData; } set { MasterData = value; } }
    }
    public class LoginDetails : Base
    {
        List<LoginDetailsOutput> MasterData = new List<LoginDetailsOutput>();
        public List<LoginDetailsOutput> SmartDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class GetFetchCityAreaDataListOutput
    {       
        public int CityAreaID { get; set; }
        public int CityID { get; set; }
        public int ZoneID { get; set; }
        public string Zone { get; set; }
        public string Zone2L { get; set; }
        public string Area { get; set; }
        public string Area2L { get; set; }
        public string CODE { get; set; }
        public string City { get; set; }
        public string City2L { get; set; }
        public int Blocked { get; set; }
        public bool BitBlocked { get; set; }
    }


    public class SmartSearch
    {
        public string ProcName { get; set; }
        public string Tbl { get; set; }
        public string Name { get; set; }
        public int LanguageID { get; set; }
        public int Param1 { get; set; } = -1;
        public int Param2 { get; set; } = -1;
    }
    public class FetchAge
    {
        public DateTime DOB { get; set; }
       
    }
    public class FetchDOB
    {
        public int AgeUomID { get; set; }
        public int Age { get; set; }

    }
    public class FetchHijri
    {
        public string GDOB { get; set; }      

    }
    public class FetchEnglishDate
    {
        public string HDOB { get; set; }
      

    }
    public class UploadToFTP
    {
        public string FTPFilename { get; set; }


    }
    public class FetchAllCities
    {
        public string FTPFilename { get; set; }
    }
    public class PATIENTDICTIONARY
    {
        public string Tbl { get; set; }
        public string name { get; set; }
        public string languageID { get; set; }
    }
    public class FetchNationalitiesPriority
    {
        public int NationalityID { get; set; }
    }
    public class FetchCityMasters
    {
        public int CountryID { get; set; }
    }
    public class FetchPatientModificationAudit
    {
        public int PatientID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
    }
    

    public class FetchPatientRoot
    {
        public string RegCode { get; set; }
    }

   
    public class FetchCityArea
    {
        public string Type { get; set; }
        public string Filter { get; set; }        
        public int UserId { get; set; }
        public int WorkStationID { get; set; }
    }
    public class GetSmartSearchDataListOutput
    {
        public int ID { get; set; }
        public string Name { get; set; } = null;
        public string StateID { get; set; } = null;
        public string StateName { get; set; } = null;
        public string CountryId { get; set; } = null;
        public string Country { get; set; } = null;
    }
    public class FetchAgeDataListOutput
    {
        public int Age { get; set; }
        public int Ageuomid { get; set; } 
        
    }
    public class FetchDOBDataListOutput
    {
        public DateTime DOB { get; set; }
       

    }
    public class FetchAllCitiesDataListOutput
    {
        public int CityID { get; set; }
        public string City { get; set; }
        public string City2L { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }

    }
    public class FetchCompanyTypesDataListOutput
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name2L { get; set; }
        public string code { get; set; }
        public Boolean blocked { get; set; }        

    }
    public class PATIENTDICTIONARYDataListOutput
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Name2L { get; set; }       

    }
    public class FetchCityMastersDataListOutput
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string CityName2L { get; set; }

    }
    public class FetchPatientModificationAuditListOutput
    {
        public string DBColumnName { get; set; }
        public string UpdatedValue { get; set; }
        public string PreviousValue { get; set; }
        public string HostName { get; set; }
        public string Createdate { get; set; }
        public string EmployeeName { get; set; }
        public string WorkStations { get; set; }


    }
    public class FetchNationalitiesPriorityDataListOutput
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public string Names2L { get; set; }
        public bool IsArabic { get; set; }

    }
    public class FetchHijriDataListOutput
    {
        public string HijriDate { get; set; }
        public string Message { get; set; }

    }
    public class FetchEnglishDataListOutput
    {
        public string EnglishDate { get; set; }
        public string Message { get; set; }
        public string Age { get; set; }
        public int AgeUOM { get; set; }

    }
    public class UploadToFTPDataListDataListOutput
    {
       
        public string Save { get; set; }
        public string Message { get; set; }

    }

    public class GetFetchPatientRootDataListOutput
    {
        public string PatientID { get; set; }
        public string RegCode { get; set; } = null;
        public string RegistrationDate { get; set; } = null;
        public string PatientName { get; set; } = null;
        public string RootPatientID { get; set; } = null;
        public string PatBitblocked { get; set; } = null;
        public string SSN { get; set; } = null;
    }

    public class FetchPatientData
    {
        public string RegCode { get; set; }
    }

    public class GetFetchPatientInfoDataListOutput
    {
        public int TBL { get; set; }
        public int PatientID { get; set; }
        public string RegCode { get; set; }
        public string RegistrationDate { get; set; }
        public int TitleID { get; set; }
        public string Title { get; set; }
        public string Title2l { get; set; }

        public string FullName { get; set; }
        public string Familyname { get; set; }
        public string Familyname2l { get; set; }
        public string FirstName { get; set; }
        public string FirstName2l { get; set; }
        public string MiddleName { get; set; }
        public string MiddleName2l { get; set; }
        public string GrandFatherName { get; set; }
        public string GrandFatherName2L { get; set; }      
        public int GenderID { get; set; }
        public string Gender { get; set; }
        public string Gender2l { get; set; }       
        public DateTime? DOB { get; set; }
        public int? Age { get; set; }
        public int? AgeUOMID { get; set; }
        public string AgeType { get; set; }
        public string AgeType2l { get; set; }
        public bool? IsAgeByDOB { get; set; }
        public string Address01 { get; set; }       
        public int CityID { get; set; }
        public int CityAreaID { get; set; } = 0;
        public string CityArea { get; set; }
        public string City { get; set; }
        public string City2L { get; set; }
        public string District { get; set; }
        public string District2L { get; set; }
        public string State { get; set; }
        public string State2L { get; set; }

        public int CountryID { get; set; }
        public string Country { get; set; }
        public string Country2L { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public string ContactName { get; set; }
        public string ContactName2l { get; set; }
        public string ContAddress { get; set; }
        public string ContAddress2l { get; set; }
        public string ContPhoneNo { get; set; }     
        public int? ContRelationID { get; set; }
        public bool? ISVIP { get; set; }       
        public string PassportNo { get; set; }       
        public string ConsultantID { get; set; }
        public string Consultant { get; set; }      
        public string DoctorID { get; set; }
        public string Doctor { get; set; }
        public string Doctor2L { get; set; }      
        public int? MaritalStatusID { get; set; }
        public string MarStatus { get; set; }
        public string MarStatus2l { get; set; }
        public int? ReligionID { get; set; }
        public string Religion { get; set; }
        public string Religion2l { get; set; }
        public int? HospitalID { get; set; }
        public string ContRelation { get; set; }
        public string ContRelation2l { get; set; }      
        public int? NationalityID { get; set; }
        public string Nationality { get; set; }
        public string Nationality2l { get; set; }
        public string AdmSourceID { get; set; }       
        public string FullAge { get; set; }
        public string FullAge2L { get; set; }
        public int? Status { get; set; }       
        public string PatientEmpID { get; set; }        
        public string PhotoPath { get; set; }       
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string PlaceOfBirth { get; set; }      
       
        public int? RegPatienttype { get; set; }      
        public int? CalcAge { get; set; }
        public int? CalcAgeUoMID { get; set; }
        public string CalcAgeUoM { get; set; }       
        public int? WorkStationID { get; set; }
        public int? IsServicePatient { get; set; }
        public string WorkStationName { get; set; }
        public int? ParentPatientID { get; set; }       
        public bool? IsEmployee { get; set; }
        public string StrPath { get; set; }
        public string EmployeeName { get; set; }       
        public string PatientName { get; set; }       
        

        public List<GetFetchPatientInsuranceInfoDataListOutput> InsurancePatientData { get; set; } = new List<GetFetchPatientInsuranceInfoDataListOutput>();
    }

    public class GetFetchPatientInsuranceInfoDataListOutput
    {
        public int MultiINSID { get; set; }
        public int PatientID { get; set; }
        public int PayerID { get; set; }
        public string PayersCard { get; set; }

        public int GradeID { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string CardValidity { get; set; }
        public int InsurenceCompanyID { get; set; }
        public int TPAID { get; set; }
        public int ActiveStatus { get; set; }

        public string InsuranceNo { get; set; }
        public string ReceiverID { get; set; }
        public string AgreemenID { get; set; }
        public string MembershIPNO { get; set; }
        public string Collectables { get; set; }
        public string Deductables { get; set; }
        public string IsPercollectables { get; set; }
        public string IsPerdeductables { get; set; }
        public string PayerName { get; set; }
        public string ReceiverName { get; set; }
        public string TPAName { get; set; }
        public string InsurenceCompanyName { get; set; }

        public string GradeName { get; set; }
        public int Blocked { get; set; }
        public string Documentpath { get; set; }

        public string PolicyNo { get; set; }

        public string ReferalBasisNo { get; set; }
        public string PatientEmpID { get; set; }
        public string RelationCode { get; set; }
        public string InsuranceCardExpiry { get; set; }
        public string ContractNo { get; set; }
        public string PolicyValidFrom { get; set; }
        public string MRNo { get; set; }
        public string EmpRelation { get; set; }

        public int RelationID { get; set; }

        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string PhoneNo { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyFax { get; set; }
        public string Companyaddress { get; set; }

        public string ContractValidFrom { get; set; }
        public string ContractValidTo { get; set; }
        public string InsurenceCompanyCode { get; set; }
        public int MAXCollectable { get; set; }
        public int GradeBlocked { get; set; }
    }

    public class HospitalServicesInputs
    {
         public string type { get; set; }
        //public string filter { get; set; }
        //public string order { get; set; }
        //public int userId { get; set; }
        //public string workStationId { get; set; }
        //public string languageId { get; set; }
    }
    public class FetchEmployee
    {
        public string Filter { get; set; }
        public int UserID { get; set; }
        public int WorkStationId { get; set; }
        
    }
    public class FetchSpeficEmployee
    {
        public int EmpID { get; set; }
        public int UserID { get; set; }
        public int WorkStationId { get; set; }

    }
    public class HospitalServicesOutput
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceName2L { get; set; }
    }
    public class FetchConsultantsOutput
    {
        public string ConsultantID { get; set; }
        public string ConsultantName { get; set; }       
    }
    public class FetchEmployeeOutput
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public string FullName { get; set; }
        public string FullName2l { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentName2l { get; set; }
        public string Specialisation { get; set; }
        public string Specialisation2l { get; set; }
    }

    public class FetchSpeficEmployeeOutput
    {
        public int EmpID { get; set; }
        public string EmpNo { get; set; }
        public int TitleID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string FirstName2L { get; set; }
        public string MiddleName { get; set; }
        public string MiddleName2L { get; set; }
        public string FamilyName { get; set; }
        public string FamilyName2L { get; set; }
        public string SSN { get; set; }
        public int MaritalStatusID { get; set; }
        public string MaritalStatus { get; set; }
        public int NationalityID { get; set; }
        public string Nationality { get; set; }
        public int ReligionID { get; set; }
        public string Religion { get; set; }
        public string Religion2L { get; set; }
        public string DoB { get; set; }
        public int CityID { get; set; }
        public string City { get; set; }
        public string City2L { get; set; }
        public string MobileNo { get; set; }
        public int GenderID { get; set; }
        public string Gender { get; set; }

    }
    public class LoginDetailsOutput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public string ISLocked { get; set; }
        public string ISPWDExpired { get; set; }
        public string PWDSetDate { get; set; }
        public string LoggedHostIP { get; set; }
        public string LoggedHostName { get; set; }
        public string ISLogged { get; set; }
        public string PWDDays { get; set; }
        public string CredentialsMessage { get; set; }
    }
    public class FetchConsultants
    {    
        public string Name { get; set; }
    }
    public class FileUpload
    {
        public string FileName { get; set; }
        public string Base64 { get; set; }
    }

    public class UserData : Base
    {
        public List<User> Users { get; set; }

        public UserData()
        {
            Users = new List<User>();
        }
    }

    public class User : Base
    {
        //[XmlElement(ElementName = "dateOfBirthH")]
        public string DateOfBirth { get; set; }

        //[XmlElement(ElementName = "englishFirstName")]
        public string EnFirstName { get; set; }

        //[XmlElement(ElementName = "englishLastName")]
        public string EnLastName { get; set; }

        //[XmlElement(ElementName = "englishSecondName")]
        public string EnSecondName { get; set; }
        //[XmlElement(ElementName = "englishThirdName")]
        public string EnThirdName { get; set; }
        //[XmlElement(ElementName = "firstName")]
        public string FirstName { get; set; }
        //[XmlElement(ElementName = "gender")]
        public string Gender { get; set; }
        //[XmlElement(ElementName = "iqamaExpiryDateG")]
        public string IqamaExpiryDateG { get; set; }
        //[XmlElement(ElementName = "lastInsuranceCompanyIdentity")]
        public string LastInsuranceCompanyIdentity { get; set; }
        //[XmlElement(ElementName = "lastInsurancePolicyEndGDate")]
        public string LastInsurancePolicyEndGDate { get; set; }
        //[XmlElement(ElementName = "lastInsurancePolicyStartGDate")]
        public string LastInsurancePolicyStartGDate { get; set; }
        //[XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }
        //[XmlElement(ElementName = "logId")]
        public string LogId { get; set; }
        //[XmlElement(ElementName = "nationalityCode")]
        public string NationalityCode { get; set; }
        //[XmlElement(ElementName = "occupationCode")]
        public string OccupationCode { get; set; }
        //[XmlElement(ElementName = "placeOfBirthCode")]
        public string PlaceOfBirthCode { get; set; }
        //[XmlElement(ElementName = "secondName")]
        public string SecondName { get; set; }
        //[XmlElement(ElementName = "sponsorName")]
        public string SponsorName { get; set; }
        //[XmlElement(ElementName = "thirdName")]
        public string ThirdName { get; set; }
    }

}