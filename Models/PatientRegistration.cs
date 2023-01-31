using RCMAPI.CommonUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCMAPI.Models
{
    public class PatientRegistration:Base 
    {
        public int PatientId { get; set; }
        public string RegCode { get; set; }
    }


    public class PatientRegistrationDetails
    {
        public string tbl { get; set; }
        public string PatientID { get; set; }
        public string RegCode { get; set; }
        //public string RegistrationDate { get; set; }
        public int TitleID { get; set; }
        public string GenderID { get; set; }
        public string Familyname { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string GrandFatherName { get; set; }
        public string Familyname2l { get; set; }
        public string FirstName2l { get; set; }
        public string MiddleName2l { get; set; }
        public string GrandFatherName2L { get; set; }
        public int ISVIP { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        public int AgeUOMID { get; set; }
        public string IsAgeByDOB { get; set; }
        public int ReligionID { get; set; }
        public int MaritalStatusID { get; set; }
        public string Address01 { get; set; }
        public int CityID { get; set; }
        public int CityAreaId { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int NationalityID { get; set; }
        public string SSN { get; set; }
        public string PassportNo { get; set; }
        public string ContactName { get; set; }
        public int ContRelationID { get; set; }
        public string ContPhoneNo { get; set; }
        public int PatientEmpID { get; set; }
        public string PhotoPath { get; set; }
        public string StrPath { get; set; }
        public int HospitalID { get; set; }      

        public List<MultipleInsurance> multipleInsurance { get; set; }
        public string UserID { get; set; }
        public int WorkStationID { get; set; }
        public int IsServicePatient { get; set; }
        public int PatientType { get; set; }

        public List<AuditPatientXML> AuditPatientXML { get; set; }
    }

    public class MultipleInsurance
    {
        public string PID { get; set; }
        public string GID { get; set; }
        public string INSNO { get; set; }
        public string CVAL { get; set; }
        public string VFR { get; set; }
        public string VTO { get; set; }
        public string MEM { get; set; }
        public string ASTA { get; set; }
        //public string DPAH { get; set; }
        public string CNO { get; set; }
        public string RCODE { get; set; }
        public string EID { get; set; }
        public string MNO { get; set; }
        public string RLID { get; set; }
        public string RBNO { get; set; }
        public string PCD { get; set; }
        public string PPNO { get; set; }
        public string PFAX { get; set; }
        public string INSN { get; set; }
        public string EML { get; set; }
        //public string PADD { get; set; }
        //public string MAXC { get; set; }
        //public string PNO { get; set; }
        //public string RB { get; set; }
    }
    public class AuditPatientXML
    {
        public string APPLN { get; set; }
        public string DBC { get; set; }
        public string UPV { get; set; }
        public string PRV { get; set; }
        public string UID { get; set; }
        public string WID { get; set; }
        public string HName { get; set; }
        
    }


    public class FetchDoctorWiseAvailabilityM : Base
    {
        List<GetDoctorWiseAvailabilityListOutput> MasterData = new List<GetDoctorWiseAvailabilityListOutput>();
        public List<GetDoctorWiseAvailabilityListOutput> DoctorAvailCode { get { return MasterData; } set { MasterData = value; } }
    }

    public class FetchHospitalDoctorsM : Base
    {
        List<FetchHospitalDoctorsListOutput> MasterData = new List<FetchHospitalDoctorsListOutput>();
        public List<FetchHospitalDoctorsListOutput> DoctorAvailCode { get { return MasterData; } set { MasterData = value; } }
    }
    public class FetchHospitalSpecialisationsM : Base
    {
        List<FetchHospitalSpecialisationsListOutput> MasterData = new List<FetchHospitalSpecialisationsListOutput>();
        public List<FetchHospitalSpecialisationsListOutput> DoctorAvailCode { get { return MasterData; } set { MasterData = value; } }
    }

    public class ValidateNationalIDM : Base
    {
        List<ValidateNationalIDListOutput> MasterData = new List<ValidateNationalIDListOutput>();
        public List<ValidateNationalIDListOutput> ValidateCode { get { return MasterData; } set { MasterData = value; } }
    }

    public class ValidatePinBlockM : Base
    {
        List<ValidatePinBlockListOutput> MasterData = new List<ValidatePinBlockListOutput>();
        public List<ValidatePinBlockListOutput> ValidateCode { get { return MasterData; } set { MasterData = value; } }
    }

    //public class CurrentDayDoctorAvailabilityM : Base
    //{
    //    List<ValidatePinBlockListOutput> MasterData = new List<ValidatePinBlockListOutput>();
    //    public List<ValidatePinBlockListOutput> ValidateCode { get { return MasterData; } set { MasterData = value; } }
    //}



    public class GetDoctorWiseAvailability
    {
        public int Specialiseid { get; set; }
        public int DoctorID { get; set; }
        public int HospitalID { get; set; }
        //public int WorkStationID { get; set; }
    }
    public class GetDoctorWiseAvailabilityListOutput
    {
        public string DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string DoctorName2L { get; set; }
        public string SpecialiseID { get; set; }
        public string Specialisation { get; set; }
        public string Specialisation2L { get; set; }
        public string MaxConsultation { get; set; }
        public string TotalAppointments { get; set; }
        public string AppointmentBills { get; set; }
        public string TotalBills { get; set; }
        public string Visited { get; set; }
        public string ServiceItemID { get; set; }
        
    }
    public class ValidateNationalID
    {
        public string NationalID { get; set; }
    }
    public class ValidatePinBlock
    {
        public string PatientID { get; set; }
    }

    public class HospitalDoctors
    {
        public int SpecialiseID { get; set; }
        public int HospitalID { get; set; }
    }
    public class HospitalSpecialisations
    {
        public string Type { get; set; }
        public string Filter { get; set; }
        public int UserID { get; set; }
        public int WorkStationID { get; set; }
    }


    public class CurrentDayDoctorAvailability
    {
        public int Specialiseid { get; set; }
        public int DoctorID { get; set; }
        public string DoctorCode { get; set; }
        public int HospitalID { get; set; }
        //public int WorkStationID { get; set; }
    }
    public class ValidateNationalIDListOutput
    {
        public string PatientID { get; set; }
        public string RegCode { get; set; }
        public string SSN { get; set; }      

    }
    public class ValidatePinBlockListOutput
    {
        public string Blocktype { get; set; }
        public string Blockreason { get; set; }
        public string Discription { get; set; }

        public string EffectiveDate { get; set; }
        public string STATUS { get; set; }

    }

    public class FetchHospitalDoctorsListOutput
    {
        public int EmpID { get; set; }
        public string FullName { get; set; }
        public string FullName2L { get; set; }
        public int HospDeptId { get; set; }
        public string ServiceDocID { get; set; }
        

    }
    public class FetchHospitalSpecialisationsListOutput
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name2L { get; set; }
        public string code { get; set; }
        public int blocked { get; set; }
        public int BitBlocked { get; set; }

    }
   
}