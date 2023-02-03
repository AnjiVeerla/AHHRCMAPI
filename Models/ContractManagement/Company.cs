using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCMAPI.Models.ContractManagement
{
    public class Company
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyName2L { get; set; }
        public int CompanyTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CityId { get; set; }
        public string PinCode { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int CreditDays { get; set; }
        public string LicenseNo { get; set; }
        public int UserID { get; set; }
        public int WorkStationID { get; set; }
        public int Blocked { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CompanyType { get; set; }
    }
}