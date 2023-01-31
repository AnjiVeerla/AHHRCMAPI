using RCMAPI.CommonUtilities;
using System.Collections.Generic;

namespace RCMAPI.Models
{

    public class PatientRegMasterDataList : Base
    {
        List<DemoGraphics> MasterData = new List<DemoGraphics>();
        public List<DemoGraphics> MasterDataList { get { return MasterData; } set { MasterData = value; } }
    }
    public class GetMasterDataList
    {       
        public string Type { get; set; }       
        public int UserID { get; set; }
        public int LanguageID { get; set; }
    }
    public class GetCompanyTypesDataList
    {
        public string Type { get; set; }
        public string Filter { get; set; }
        public int UserID { get; set; }
        public int WorkStationId { get; set; }
        public int LanguageID { get; set; }
    }
    public class DemoGraphics
    {
        public string Type { get; set; }
        public string TableName { get; set; }
        public List<DemoGraphicsData> DemoGraphicsData { get; set; } = new List<DemoGraphicsData>();
    }
    public class DemoGraphicsData
    {
        public string Type { get; set; }
        public string Names { get; set; } = null;
        public string Names2L { get; set; } = null;
        public int Id { get; set; } 
       
    }

    public class FetchPatientDataList
    {
        public string PatientID { get; set; }
        public string RegCode { get; set; }
        public string TBL { get; set; }
        public string UserId { get; set; }
        public string WORKSTATIONID { get; set; }
        public string LanguageID { get; set; }
    }
}