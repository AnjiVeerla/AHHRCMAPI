using System.Collections.Generic;

namespace RCMAPI.CommonUtilities
{
    
    public class PatientBiillInfoListN
    {
        public string RegCode { get; set; }
        public string HospitalID { get; set; }
        public string ScheduleID { get; set; }

        public string BillAmount { get; set; }
        public string PayerAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string VAT { get; set; }
        public string DepositAmount { get; set; }
        public string RefundAmount { get; set; }
        public string ReceiptAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string Collectable { get; set; }
        public string OrderType { get; set; }

    }
    public class PatientBillInfoList : Base
    {
        List<PatientBiillInfoListN> PatientBill = new List<PatientBiillInfoListN>();
        public List<PatientBiillInfoListN> BillSummary { get { return PatientBill; } set { PatientBill = value; } }

    }
}
