using RCMAPI.Models.CommonMasters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HISDataAccess;
using Microsoft.Graph;
using RCMAPI.AgeCalculator;
using RCMAPI.CommonUtilities;
using RCMAPI.Messages;
using RCMAPI.Models;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using Azure.Identity;
using static RCMAPI.SharedData.Enums;

namespace RCMAPI.DAL
{
    public class CommonMastersDAL
    {
        static string MODULE_NAME = "WebAPIDAL";
        const int DEFAULTWORKSTATION = 0;
        internal enum Database
        {
            Master = 1,
            Transaction = 2
        }
        private IDbDataParameter CreateParam(DataHelper objDataHelper, string paramName, object paramVal, DbType paramType, ParameterDirection paramDirection)
        {
            IDbDataParameter objIDbDataParameter = objDataHelper.CreateDataParameter();
            objIDbDataParameter.ParameterName = paramName;
            objIDbDataParameter.Value = paramVal;
            objIDbDataParameter.DbType = paramType;
            objIDbDataParameter.Direction = paramDirection;

            return objIDbDataParameter;
        }
        public CommonMasterListModel GetCompanyTypes()
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            CommonMasterListModel objCompanyTypes = new CommonMasterListModel();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();               
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_Master_FetchCompanyType", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            CommonMasterModel objType = new CommonMasterModel();
                            objType.Id = Convert.ToInt32(dr["CompanyTypeID"]);
                            objType.Name = Convert.ToString(dr["CompanyTypename"]);
                            objType.Name2l = Convert.ToString(dr["CompanyTypename2L"]);
                            objType.Code = Convert.ToString(dr["CODE"]);
                            objCompanyTypes.objCommonMasterData.Add(objType);
                        }

                    }

                    objCompanyTypes.Status = ApiStatus.Success.ToString();
                    objCompanyTypes.StatusCode = Convert.ToInt32(ApiStatus.Success);
                }
                return objCompanyTypes;
            }
            catch (Exception ex)
            {
                objCompanyTypes.StatusCode = Convert.ToInt32(ApiStatus.Fail);
                objCompanyTypes.Status = ApiStatus.Fail.ToString();               
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in GetCompanyTypes", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objCompanyTypes;
        }

        public CommonMasterListModel GetMasterData(string type)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            CommonMasterListModel objCommonMasterDataList = new CommonMasterListModel();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", type, DbType.String, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_Master_FetchSingleFieldMasters", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataTable dtMaster in dsDocList.Tables)
                        {
                            foreach (DataRow dr in dtMaster.Rows)
                            {
                                CommonMasterModel objType = new CommonMasterModel();
                                objType.Id = Convert.ToInt32(dr["ID"]);
                                objType.Name = Convert.ToString(dr["Names"]);
                                objType.Name2l = Convert.ToString(dr["Names2L"]);
                                //objType.Code = Convert.ToString(dr["ID"]);
                                objType.MasterType = Convert.ToString(dr["Type"]);
                                objCommonMasterDataList.objCommonMasterData.Add(objType);
                            }

                        }
                       

                    }

                    objCommonMasterDataList.Status = ApiStatus.Success.ToString();
                    objCommonMasterDataList.StatusCode = Convert.ToInt32(ApiStatus.Success);
                }
                return objCommonMasterDataList;
            }
            catch (Exception ex)
            {
                objCommonMasterDataList.StatusCode = Convert.ToInt32(ApiStatus.Fail);
                objCommonMasterDataList.Status = ApiStatus.Fail.ToString();
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in GetMasterData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objCommonMasterDataList;
        }

        public CommonMasterListModel GetCities(int countryId)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            CommonMasterListModel objCommonMasterDataList = new CommonMasterListModel();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CountryID", countryId, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_Master_FetchCityMasters", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            CommonMasterModel objType = new CommonMasterModel();
                            objType.Id = Convert.ToInt32(dr["CityID"]);
                            objType.Name = Convert.ToString(dr["CityName"]);
                            objType.Name2l = Convert.ToString(dr["CityName2L"]);
                            //objType.Code = Convert.ToString(dr["CODE"]);
                            objCommonMasterDataList.objCommonMasterData.Add(objType);
                        }
                    }

                    objCommonMasterDataList.Status = ApiStatus.Success.ToString();
                    objCommonMasterDataList.StatusCode = Convert.ToInt32(ApiStatus.Success);
                }
                return objCommonMasterDataList;
            }
            catch (Exception ex)
            {
                objCommonMasterDataList.StatusCode = Convert.ToInt32(ApiStatus.Fail);
                objCommonMasterDataList.Status = ApiStatus.Fail.ToString();
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in GetCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objCommonMasterDataList;
        }
    }
}