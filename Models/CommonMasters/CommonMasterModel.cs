using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCMAPI.Models.CommonMasters
{
    public class CommonMasterListModel
    {
        public CommonMasterListModel()
        {
            objCommonMasterData = new List<CommonMasterModel>();
        }
        public string Status { get; set; }       
        public int StatusCode { get; set; }

        public List<CommonMasterModel> objCommonMasterData { get; set; }
    }
    public class CommonMasterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name2l { get; set; }
        public string Code { get; set; }
        public string MasterType { get; set; }
    }
}