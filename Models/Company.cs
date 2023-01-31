using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCMAPI.Models
{
     public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public int CompanyType { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public int CityId { get; set; }
        public string PinCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public int CreditDays { get; set; }
        public string LicenseNumber { get; set; }
    }
}