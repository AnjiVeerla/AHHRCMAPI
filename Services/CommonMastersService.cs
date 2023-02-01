using RCMAPI.DAL;
using RCMAPI.CommonUtilities;
using RCMAPI.Models;
using System;
using System.Collections.Generic;
using RCMAPI.Models.CommonMasters;

namespace RCMAPI.Services
{
    public class CommonMastersService
    {
        public CommonMasterListModel GetCompanyTypes()
        {
            CommonMastersDAL objDAL = new CommonMastersDAL();
            CommonMasterListModel objRetunObject = objDAL.GetCompanyTypes();
            return objRetunObject;
        }

        public CommonMasterListModel GetMasterData(string type)
        {
            CommonMastersDAL objDAL = new CommonMastersDAL();
            CommonMasterListModel objRetunObject = objDAL.GetMasterData(type);
            return objRetunObject;
        }

        public CommonMasterListModel GetCities(int countryId)
        {
            CommonMastersDAL objDAL = new CommonMastersDAL();
            CommonMasterListModel objRetunObject = objDAL.GetCities(countryId);
            return objRetunObject;
        }

    }
}