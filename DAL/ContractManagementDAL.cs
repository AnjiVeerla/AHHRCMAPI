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
using RCMAPI.Models.ContractManagement;
using System.Threading.Tasks;

namespace RCMAPI.DAL
{
    public class ContractManagementDAL
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
        private IDbDataParameter CreateParam1(DataHelper objDataHelper, string paramName, object paramVal, int size, DbType paramType, ParameterDirection paramDirection)
        {
            IDbDataParameter objIDbDataParameter = objDataHelper.CreateDataParameter();
            objIDbDataParameter.ParameterName = paramName;
            objIDbDataParameter.Value = paramVal;
            objIDbDataParameter.DbType = paramType;
            objIDbDataParameter.Direction = paramDirection;
            objIDbDataParameter.Size = size;

            return objIDbDataParameter;
        }
        public async Task<(int,string)> SaveCompany(Company company)
        {

            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyID", (company.CompanyID==0)? (object)DBNull.Value : company.CompanyID, DbType.Int32, ParameterDirection.InputOutput));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyCode", company.CompanyCode, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyName", company.CompanyName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyName2L", company.CompanyName2L, DbType.String, ParameterDirection.Input));                
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyTypeId", company.CompanyTypeId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Address1", company.Address1, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Address2", company.Address2, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CityId", company.CityId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PinCode", company.PinCode, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PhoneNo", company.PhoneNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MobileNo", company.MobileNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Fax", company.Fax, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Email", company.Email, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@LicenseNo", company.LicenseNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CreditDays", company.CreditDays, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserID", company.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", company.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Blocked", company.Blocked, DbType.Int32, ParameterDirection.Input));


                int intRes =  objDataHelper.RunSP("PR_Master_SaveCompanies", objIDbDataParameters.ToArray());

                if (intRes == -1 || intRes > 0)
                {

                    return (Convert.ToInt32(objIDbDataParameters[0].Value), ApiStatus.Success.ToString());
                }
                else
                {

                }
                return (Convert.ToInt32(objIDbDataParameters[0].Value), ApiStatus.Fail.ToString());

            }
            catch (Exception ex)
            {

                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in SaveCompany", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return (0, ApiStatus.Fail.ToString()); ;

        }


        public Company FetchCompany(int id)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            Company objCompany = new Company();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyID", id, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsCompanyList = objDataHelper.RunSPReturnDS("Pr_Master_FetchCompanies", objIDbDataParameters.ToArray()))
                {
                    if (dsCompanyList.Tables.Count > 0)
                    {

                        foreach (DataRow dr in dsCompanyList.Tables[0].Rows)
                        {

                            objCompany.CompanyID = Convert.ToInt32(dr["CompanyId"]);
                            objCompany.CompanyCode = Convert.ToString(dr["CompanyCode"]);
                            objCompany.CompanyName = Convert.ToString(dr["CompanyName"]);
                            objCompany.CompanyName2L = Convert.ToString(dr["CompanyName2L"]);
                            objCompany.CompanyTypeId = Convert.ToInt32(dr["CompanyTypeId"]);
                            objCompany.CityId = Convert.ToInt32(dr["CityId"]);
                            objCompany.Address1 = Convert.ToString(dr["Address1"]);
                            objCompany.Address2 = Convert.ToString(dr["Address2"]);
                            objCompany.PinCode = Convert.ToString(dr["PinCode"]);
                            objCompany.PhoneNo = Convert.ToString(dr["PhoneNo"]);
                            objCompany.MobileNo = Convert.ToString(dr["MobileNo"]);
                            objCompany.Fax = Convert.ToString(dr["Fax"]);
                            objCompany.Email = Convert.ToString(dr["Email"]);
                            objCompany.LicenseNo = Convert.ToString(dr["LicenseNo"]);
                            objCompany.CreditDays = Convert.ToInt32(dr["CreditDays"]);
                            objCompany.UserID = Convert.ToInt32(dr["UserID"]);
                            objCompany.WorkStationID = Convert.ToInt32(dr["WorkStationID"]);
                            objCompany.Blocked = Convert.ToInt32(dr["Blocked"]);
                            objCompany.CreditDays = Convert.ToInt32(dr["CreditDays"]);
                            objCompany.CountryId = Convert.ToInt32(dr["CountryID"]);
                            objCompany.CountryName = Convert.ToString(dr["CountryName"]);
                            objCompany.CityName = Convert.ToString(dr["CityName"]);
                            objCompany.CompanyType = Convert.ToString(dr["CompanyTypename"]);


                        }

                    }

                    //return objCompanyList;
                }

            }
            catch (Exception ex)
            {

                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchCompany", "");
            }
            finally
            {
                objDataHelper = null;
            }

            return objCompany;
        }


        public List<Company> FetchAllCompanies()
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            List<Company> objCompanyList = new List<Company>();

            try
            {

                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                using (DataSet dsCompanyList = objDataHelper.RunSPReturnDS("Pr_FetchAllCompanies", objIDbDataParameters.ToArray()))
                {
                    if (dsCompanyList.Tables.Count > 0)
                    {

                        foreach (DataRow dr in dsCompanyList.Tables[0].Rows)
                        {
                            Company objCompany = new Company();
                            objCompany.CompanyID = Convert.ToInt32(dr["ID"].ToString());
                            objCompany.CompanyCode = Convert.ToString(dr["City"]);
                            objCompany.CompanyName = Convert.ToString(dr["City2L"]);
                            objCompany.CompanyTypeId = Convert.ToInt32(dr["StateID"]);
                            objCompany.CityId = Convert.ToInt32(dr["StateName"]);
                            objCompany.Address1 = Convert.ToString(dr["StateName"]);
                            objCompany.Address2 = Convert.ToString(dr["StateName"]);

                            objCompany.PinCode = Convert.ToString(dr["StateName"]);
                            objCompany.PhoneNo = Convert.ToString(dr["StateName"]);
                            objCompany.MobileNo = Convert.ToString(dr["StateName"]);
                            objCompany.Fax = Convert.ToString(dr["StateName"]);
                            objCompany.Email = Convert.ToString(dr["StateName"]);
                            objCompany.LicenseNo = Convert.ToString(dr["StateName"]);
                            objCompany.CreditDays = Convert.ToInt32(dr["StateName"]);
                            // objCompany.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                            //objCompany.Country = dr["Country"].ToString();

                            objCompanyList.Add(objCompany);
                        }

                    }

                    //return objCompanyList;
                }
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objCompanyList;
        }


        public List<CompaniesSS> FetchCompaniesSS(string inputValue)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            List<CompaniesSS> objCompanyList = new List<CompaniesSS>();

            try
            {

                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Name", inputValue, DbType.String, ParameterDirection.Input));
                using (DataSet dsCompanyList = objDataHelper.RunSPReturnDS("Pr_Master_SSCompanies", objIDbDataParameters.ToArray()))
                {
                    if (dsCompanyList.Tables.Count > 0)
                    {

                        foreach (DataRow dr in dsCompanyList.Tables[0].Rows)
                        {
                            CompaniesSS objCompany = new CompaniesSS();
                            objCompany.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());
                            objCompany.CompanyCode = Convert.ToString(dr["CompanyCode"]);
                            objCompany.CompanyName = Convert.ToString(dr["CompanyName"]);
                            objCompany.CompanyName2L = Convert.ToString(dr["CompanyName2L"]);
                            objCompany.Blocked = Convert.ToInt32(dr["Blocked"]);
                            

                            objCompanyList.Add(objCompany);
                        }

                    }

                    //return objCompanyList;
                }
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objCompanyList;
        }

    }
}