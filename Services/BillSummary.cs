using RCMAPI.DAL;
using RCMAPI.CommonUtilities;
using RCMAPI.Models;

namespace RCMAPI.Services
{
    public class BillSummary
    {
        
        public Base FetchServicePrice(FetchServicePrice docParams)
        {
            BillSummaryDAL RCMDALObj = new BillSummaryDAL();
            Base obj = RCMDALObj.FetchServicePrice(docParams);
            return obj;
        }
        public Base PatientBillSummary(PatientBillList docParams)
        {
            BillSummaryDAL RCMDALObj = new BillSummaryDAL();
            Base obj = RCMDALObj.PatientBillSummary(docParams);
            return obj;
        }
        public Base ValidationRegCode(ValidationRegCode docParams)
        {
            BillSummaryDAL RCMDALObj = new BillSummaryDAL();
            Base obj = RCMDALObj.ValidationRegCode(docParams);
            return obj;
        }


    }
}