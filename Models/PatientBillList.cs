using RCMAPI.CommonUtilities;
using System;
using System.Collections.Generic;

namespace RCMAPI.Models
{
    public class PatientBillList : Base
    {
        public string PatientID { get; set; }
        public int HospitalId { get; set; }
        public int ScheduleID { get; set; }
        public int ServiceID { get; set; }
        public bool rblbilltypeCredit { get; set; }
        public int ServiceTypeID { get; set; }
        public int UserID { get; set; }
        public int WorkStationID { get; set; }
        public List<ServiceOrderXML> ServiceOrderXML { get; set; }

    }
    public class ServiceOrderXML
    {
        public string ServiceName { get; set; }
        public string ServiceID { get; set; }
        public string Procedure { get; set; }
        public string ProcedureID { get; set; }
        public string Sample { get; set; }
        public string SampleID { get; set; }
        public string DeptID { get; set; }
        public string DeptName { get; set; }

        public string BedTypeID { get; set; }
        public string BedTypeName { get; set; }

        public string SpecialiseID { get; set; }
        public string SpecialiseName { get; set; }
        public bool IsGroup { get; set; }

        public string Quantity { get; set; }
        public string Amount { get; set; }
        
        public string PPAY { get; set; }
        public string CPAY { get; set; }
        public string SPAY { get; set; }
        public string ProfileID { get; set; }
        public string TariffID { get; set; }

        public string MQTY { get; set; }
        public string SQTY { get; set; }
        public string Status { get; set; }
        public decimal BasePrice { get; set; }
        public decimal BillablePrice { get; set; }
        public decimal EligiblePrice { get; set; }
        public decimal PAmount { get; set; }
        public decimal UnitRate { get; set; }
        public decimal Price { get; set; }
        public string DPAY { get; set; }
        public string ServiceTypeID { get; set; }
        public string PatientType { get; set; }
        public string Priority { get; set; }
        public decimal VAT { get; set; }
        public decimal CVAT { get; set; }
        public decimal PVAT { get; set; }
        public bool IsSaudi { get; set; }

    }
    public class FetchServicePrice
    {
        public int ServiceId { get; set; }
        public int ServiceTypeID { get; set; }
        public int TariffId { get; set; }
        public string VisitType { get; set; }
        public string SpecialisationId { get; set; }
        public int BedTypeID { get; set; }
        public int UserID { get; set; }
        public int WorkStationID { get; set; }
    }
    public class FetchPriceDataList : Base
    {
        List<GetFetchPriceDataListOutput> MasterData = new List<GetFetchPriceDataListOutput>();
        public List<GetFetchPriceDataListOutput> PatientIDRegCode { get { return MasterData; } set { MasterData = value; } }
    }
    public class PatientBillSummaryDataList : Base
    {
        List<PatientBillSummaryListOutput> MasterData = new List<PatientBillSummaryListOutput>();
        public List<PatientBillSummaryListOutput> PatientIDRegCode { get { return MasterData; } set { MasterData = value; } }
    }
    public class GetFetchPriceDataListOutput
    {
        public int ServiceId { get; set; }
        public int ItemId { get; set; }
        public string ServiceName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal EligiblePrice { get; set; }
        public decimal BillablePrice { get; set; }
        public string Procedure { get; set; }
        public int OrderTypeID { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public int SpecialiseID { get; set; }
        public string Specialisation { get; set; }
    }
    public class PatientBillSummaryListOutput
    {
        public int ServiceId { get; set; }
        public int ItemId { get; set; }
        public string ServiceName { get; set; }
        public decimal BasePrice { get; set; }
        public decimal EligiblePrice { get; set; }
        public decimal BillablePrice { get; set; }
        public string Procedure { get; set; }
        public int OrderTypeID { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public int SpecialiseID { get; set; }
        public string Specialisation { get; set; }
    }
    public class ValidationRegCode
    {
        public string UHID { get; set; }
        public int HospitalId { get; set; }
        public int UserID { get; set; }
        public int WorkStationID { get; set; }

    }
    public class PatientBillRInfoListN
    {
        public string RegCode { get; set; }
        public string BillAmount { get; set; }
        public string PayerAmount { get; set; }
        public string DiscountAmount { get; set; }
    }
    public class PatientBillRInfoList : Base
    {
        List<PatientBillRInfoListN> PatientBill = new List<PatientBillRInfoListN>();
        public List<PatientBillRInfoListN> BillSummary { get { return PatientBill; } set { PatientBill = value; } }

    }
}