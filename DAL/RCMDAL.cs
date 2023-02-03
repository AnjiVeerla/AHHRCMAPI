using HISDataAccess;
using Microsoft.Graph;
using RCMAPI.AgeCalculator;
using RCMAPI.CommonUtilities;
using RCMAPI.Messages;
using RCMAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using Azure.Identity;


namespace RCMAPI.DAL
{
    public class RCMDAL
    {
        static string MODULE_NAME = "WebAPIDAL";
        const int DEFAULTWORKSTATION = 0;
        static String strConnString = ConfigurationManager.ConnectionStrings["DBConnectionStringMasters"].ConnectionString;
        //private IDBManager _dbManager;
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

        private void BuildParams(IDbDataParameter[] IParam)
        {
            string str = "";
            for (int intCnt = 0; intCnt < IParam.Length; intCnt++)
            {
                if (str.Trim().Length == 0)
                { str = IParam[intCnt].ParameterName.ToString() + " = " + IParam[intCnt].Value.ToString(); }
                else
                { str += ", " + IParam[intCnt].ParameterName.ToString() + " = " + IParam[intCnt].Value.ToString(); }
            }
        }
        public PatientRegMasterDataList GetPatientRegMasterData(GetMasterDataList DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            PatientRegMasterDataList objGetMasterData = new PatientRegMasterDataList();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Demographic", DocParams.Type, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserID", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@LanguageID", DocParams.LanguageID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchDemoGraphics", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        Dictionary<string, string> mydictionary = new Dictionary<string, string>();
                        mydictionary.Add("type", "tableName");
                        mydictionary.Add("1", "Genders");
                        mydictionary.Add("2", "AgeUOMs");
                        mydictionary.Add("3", "BloodGroups");
                        mydictionary.Add("5", "MaritalStatus");
                        mydictionary.Add("6", "Nationalities");
                        mydictionary.Add("7", "Occupations");
                        mydictionary.Add("8", "Qualifications");
                        mydictionary.Add("9", "Relations");
                        mydictionary.Add("10", "Religions");
                        mydictionary.Add("11", "Specializations");
                        mydictionary.Add("12", "Titles");
                        mydictionary.Add("15", "Countries");
                        mydictionary.Add("26", "BedTypes");
                        mydictionary.Add("29", "ReferalTypes");
                        mydictionary.Add("38", "AdmissionSource");
                        mydictionary.Add("39", "AdmissionType");
                        mydictionary.Add("56", "PaymentModes");
                        mydictionary.Add("57", "Banks");
                        mydictionary.Add("58", "cardTypes");
                        mydictionary.Add("61", "MLCType");
                        mydictionary.Add("62", "ModeOfTransport");
                        mydictionary.Add("66", "PaymentBlockTypes");
                        mydictionary.Add("112", "DietTypes");
                        mydictionary.Add("148", "VoucherNames");
                        mydictionary.Add("228", "BedAllocationRemarks");
                        mydictionary.Add("232", "RefundReasons");

                        for (int i = 0; i < dsDocList.Tables.Count; i++)
                        {
                            DemoGraphics obj = new DemoGraphics();
                            foreach (DataRow dr in dsDocList.Tables[i].Rows)
                            {
                                DemoGraphicsData objData = new DemoGraphicsData();
                                obj.Type = dr["Type"].ToString().Trim();
                                obj.TableName = mydictionary[dr["Type"].ToString().Trim()];
                                obj.Type = dr["Type"].ToString().Trim();
                                objData.Type = dr["Type"].ToString();
                                objData.Names = dr["Names"].ToString();
                                objData.Names2L = dr["Names2L"].ToString();
                                objData.Id = Convert.ToInt32(dr["Id"].ToString());
                                obj.DemoGraphicsData.Add(objData);
                            }
                            objGetMasterData.MasterDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.MasterDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in GetPatientRegMasterData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public SmartSearchDataList SmartSearchData(SmartSearch DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            SmartSearchDataList objGetMasterData = new SmartSearchDataList();
            string ProcName = "Pr_" + DocParams.ProcName;
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Tbl", DocParams.Tbl, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@name", DocParams.Name, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@languageID", DocParams.LanguageID, DbType.Int32, ParameterDirection.Input));

                if (DocParams.Param1 != -1)
                {
                    if (DocParams.Param1 != -0)
                        objIDbDataParameters.Add(CreateParam(objDataHelper, "@param1", DocParams.Param1, DbType.Int32, ParameterDirection.Input));
                    else
                        objIDbDataParameters.Add(CreateParam(objDataHelper, "@param1", null, DbType.Int32, ParameterDirection.Input));
                }
                if (DocParams.Param2 != -1)
                {
                    if (DocParams.Param2 != -0)
                        objIDbDataParameters.Add(CreateParam(objDataHelper, "@param2", DocParams.Param2, DbType.Int32, ParameterDirection.Input));
                    else
                        objIDbDataParameters.Add(CreateParam(objDataHelper, "@param2", null, DbType.Int32, ParameterDirection.Input));
                }


                using (DataSet dsDocList = objDataHelper.RunSPReturnDS(ProcName, objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetSmartSearchDataListOutput obj = new GetSmartSearchDataListOutput();
                            obj.ID = Convert.ToInt32(dr["ID"].ToString());
                            obj.Name = dr["Name"].ToString();
                            if (dr.Table.Columns.Contains("StateID"))
                                obj.StateID = dr["StateID"].ToString();
                            if (dr.Table.Columns.Contains("StateName"))
                                obj.StateName = dr["StateName"].ToString();
                            if (dr.Table.Columns.Contains("CountryId"))
                                obj.CountryId = dr["CountryId"].ToString();
                            if (dr.Table.Columns.Contains("Country"))
                                obj.Country = dr["Country"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in SmartSearchData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchPatientRootDataList FetchPatientRoot(FetchPatientRoot DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchPatientRootDataList objGetMasterData = new FetchPatientRootDataList();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientSearch", DocParams.RegCode, DbType.String, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchPatientIds", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetFetchPatientRootDataListOutput obj = new GetFetchPatientRootDataListOutput();
                            obj.PatientID = dr["PatientID"].ToString();
                            obj.RegCode = dr["RegCode"].ToString();
                            obj.RegistrationDate = dr["RegistrationDate"].ToString();
                            obj.PatientName = dr["PatientName"].ToString();
                            obj.SSN = dr["SSN"].ToString();
                            //obj.RootPatientID = dr["RootPatientID"].ToString();
                            //obj.PatBitblocked = dr["PatBitblocked"].ToString();
                            objGetMasterData.PatientIDRegCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.PatientIDRegCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "No Records Found";
                        objGetMasterData.Message2L = "No Records Found";

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchPatientRoot", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }


        public FetchCityAreaDataList FetchCityArea(FetchCityArea DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchCityAreaDataList objGetMasterData = new FetchCityAreaDataList();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", DocParams.Type, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Filter", DocParams.Filter, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_FetchCityAreaAdv", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetFetchCityAreaDataListOutput obj = new GetFetchCityAreaDataListOutput();
                            obj.CityAreaID = Convert.ToInt32(dr["CityAreaID"].ToString());
                            obj.CityID = Convert.ToInt32(dr["CityID"].ToString());
                            obj.ZoneID = Convert.ToInt32(dr["ZoneID"].ToString());
                            obj.Zone = dr["Zone"].ToString();
                            obj.Zone2L = dr["Zone2L"].ToString();
                            obj.Area = dr["Area"].ToString();
                            obj.Area2L = dr["Area2L"].ToString();
                            obj.CODE = dr["CODE"].ToString();
                            obj.City = dr["City"].ToString();
                            obj.Blocked = Convert.ToInt32(dr["Blocked"].ToString());
                            obj.BitBlocked = Convert.ToBoolean(dr["BitBlocked"].ToString());
                            objGetMasterData.CityAreaCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.CityAreaCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchCityArea", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchPatientInfoDataList FetchPatientData(FetchPatientDataList DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchPatientInfoDataList objPatientData = new FetchPatientInfoDataList();
            //FetchPatientInsuranceInfoDataList objPatientInsuranceData = new FetchPatientInsuranceInfoDataList();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientID", DocParams.PatientID, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@RegCode", DocParams.RegCode, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TBL", DocParams.TBL, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Deleted", 0, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WORKSTATIONID", DocParams.WORKSTATIONID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@LanguageID", DocParams.LanguageID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchPatients", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetFetchPatientInfoDataListOutput obj = new GetFetchPatientInfoDataListOutput();
                            obj.TBL = Convert.ToInt32(dr["TBL"].ToString());
                            obj.PatientID = Convert.ToInt32(dr["PatientID"].ToString());
                            obj.RegCode = dr["RegCode"].ToString();
                            if (dr["TitleID"].ToString().Trim() != "")
                                obj.TitleID = Convert.ToInt32(dr["TitleID"].ToString());
                            obj.Title = dr["Title"].ToString();
                            obj.Title2l = dr["Title2l"].ToString();
                            if (dr["GenderID"].ToString().Trim() != "")
                                obj.GenderID = Convert.ToInt32(dr["GenderID"].ToString());
                            obj.Gender = dr["Gender"].ToString();
                            obj.Gender2l = dr["Gender2l"].ToString();
                            obj.FullName = dr["PatientName"].ToString();
                            obj.Familyname = dr["Familyname"].ToString();
                            obj.FirstName = dr["FirstName"].ToString();
                            obj.MiddleName = dr["MiddleName"].ToString();
                            obj.GrandFatherName = dr["GrandFatherName"].ToString();
                            obj.Familyname2l = dr["Familyname2l"].ToString();
                            obj.FirstName2l = dr["FirstName2l"].ToString();
                            obj.MiddleName2l = dr["MiddleName2l"].ToString();
                            obj.GrandFatherName2L = dr["GrandFatherName2L"].ToString();
                            obj.RegistrationDate = dr["RegistrationDate"].ToString();
                            if (dr["ISVIP"].ToString().Trim() != "")
                            {
                                int ISVIP = Convert.ToInt32(dr["ISVIP"].ToString());
                                obj.ISVIP = Convert.ToBoolean(ISVIP);
                            }

                            if (dr["DOB"].ToString().Trim() != "")
                                obj.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                            if (dr["Age"].ToString().Trim() != "")
                                obj.Age = Convert.ToInt32(dr["Age"].ToString());
                            if (dr["AgeUOMID"].ToString().Trim() != "")
                                obj.AgeUOMID = Convert.ToInt32(dr["AgeUOMID"].ToString());
                            obj.AgeType = dr["AgeType"].ToString();
                            obj.AgeType2l = dr["AgeType2l"].ToString();
                            if (dr["IsAgeByDOB"].ToString().Trim() != "")
                                obj.IsAgeByDOB = Convert.ToBoolean(dr["IsAgeByDOB"].ToString());
                            if (dr["ReligionID"].ToString().Trim() != "")
                                obj.ReligionID = Convert.ToInt32(dr["ReligionID"].ToString());
                            obj.Religion = dr["Religion"].ToString();
                            obj.Religion2l = dr["Religion2l"].ToString();
                            if (dr["MaritalStatusID"].ToString().Trim() != "")
                                obj.MaritalStatusID = Convert.ToInt32(dr["MaritalStatusID"].ToString());
                            obj.MarStatus = dr["MarStatus"].ToString();
                            obj.MarStatus2l = dr["MarStatus2l"].ToString();
                            obj.Address01 = dr["Address01"].ToString();
                            obj.CityID = Convert.ToInt32(dr["CityID"].ToString());
                            if (dr["CityAreaID"].ToString() != "")
                                obj.CityAreaID = Convert.ToInt32(dr["CityAreaID"].ToString());
                            obj.City = dr["City"].ToString();
                            obj.CityArea = dr["CityArea"].ToString();
                            obj.City2L = dr["City2L"].ToString();
                            obj.District = dr["District"].ToString();
                            obj.District2L = dr["District2L"].ToString();
                            obj.State = dr["State"].ToString();
                            obj.State2L = dr["State2L"].ToString();
                            obj.CountryID = Convert.ToInt32(dr["CountryID"].ToString());
                            obj.Country = dr["Country"].ToString();
                            obj.Country2L = dr["Country2L"].ToString();
                            obj.PhoneNo = dr["PhoneNo"].ToString();
                            obj.MobileNo = dr["MobileNo"].ToString();
                            obj.Email = dr["Email"].ToString();
                            if (dr["NationalityID"].ToString().Trim() != "")
                                obj.NationalityID = Convert.ToInt32(dr["NationalityID"].ToString());
                            obj.Nationality = dr["Nationality"].ToString();
                            obj.Nationality2l = dr["Nationality2l"].ToString();
                            obj.SSN = dr["SSN"].ToString();
                            obj.PassportNo = dr["PassportNo"].ToString();
                            obj.ContactName = dr["ContactName"].ToString();
                            obj.ContactName2l = dr["ContactName2l"].ToString();
                            obj.ContAddress = dr["ContAddress"].ToString();
                            obj.ContAddress2l = dr["ContAddress2l"].ToString();
                            obj.ContPhoneNo = dr["ContPhoneNo"].ToString();
                            if (dr["ContRelationID"].ToString().Trim() != "")
                                obj.ContRelationID = Convert.ToInt32(dr["ContRelationID"].ToString());
                            obj.ContRelation = dr["ContRelation"].ToString();
                            obj.ContRelation2l = dr["ContRelation2l"].ToString();
                            obj.PatientEmpID = dr["PatientEmpID"].ToString();
                            obj.PhotoPath = dr["PhotoPath"].ToString();
                            obj.StrPath = dr["StrPath"].ToString();
                            if (dr["HospitalID"].ToString().Trim() != "")
                                obj.HospitalID = Convert.ToInt32(dr["HospitalID"].ToString());
                            if (dr["UserID"].ToString().Trim() != "")
                                obj.UserID = Convert.ToInt32(dr["UserID"].ToString());
                            obj.UserName = dr["UserName"].ToString();
                            if (dr["WorkStationID"].ToString().Trim() != "")
                                obj.WorkStationID = Convert.ToInt32(dr["WorkStationID"].ToString());
                            obj.WorkStationName = dr["WorkStationName"].ToString();
                            obj.IsServicePatient = 0;
                            if (dr["RegPatienttype"].ToString().Trim() != "")
                                obj.RegPatienttype = Convert.ToInt32(dr["RegPatienttype"].ToString());
                            obj.ConsultantID = dr["ConsultantID"].ToString();
                            obj.Consultant = dr["Consultant"].ToString();
                            obj.DoctorID = dr["IntDoctorID"].ToString();
                            obj.Doctor = dr["IntDoctor"].ToString();
                            obj.Doctor2L = dr["IntDoctor2L"].ToString();
                            obj.FullAge = dr["FullAge"].ToString();
                            obj.FullAge2L = dr["FullAge2L"].ToString();
                            if (dr["Status"].ToString().Trim() != "")
                                obj.Status = Convert.ToInt32(dr["Status"].ToString());
                            if (dr["CalcAge"].ToString().Trim() != "")
                                obj.CalcAge = Convert.ToInt32(dr["CalcAge"].ToString());
                            if (dr["CalcAgeUoMID"].ToString().Trim() != "")
                                obj.CalcAgeUoMID = Convert.ToInt32(dr["CalcAgeUoMID"].ToString());
                            obj.CalcAgeUoM = dr["CalcAgeUoM"].ToString();
                            if (dr["PatientEmpID"].ToString() != "")
                            {
                                int IsEmployee = 1;
                                obj.IsEmployee = Convert.ToBoolean(IsEmployee);
                            }
                            else
                            {
                                int IsEmployee = 0;
                                obj.IsEmployee = Convert.ToBoolean(IsEmployee);
                            }


                            obj.EmployeeName = dr["EmployeeName"].ToString();


                            foreach (DataRow drIns in dsDocList.Tables[1].Rows)
                            {
                                GetFetchPatientInsuranceInfoDataListOutput objInsurance = new GetFetchPatientInsuranceInfoDataListOutput();
                                objInsurance.MultiINSID = Convert.ToInt32(drIns["MultiINSID"].ToString());
                                objInsurance.PatientID = Convert.ToInt32(drIns["PatientID"].ToString());
                                objInsurance.PayerID = Convert.ToInt32(drIns["PayerID"].ToString());

                                objInsurance.PayersCard = drIns["PayersCard"].ToString();
                                objInsurance.GradeID = Convert.ToInt32(drIns["GradeID"].ToString());
                                objInsurance.ValidFrom = drIns["ValidFrom"].ToString();
                                objInsurance.ValidTo = drIns["ValidTo"].ToString();
                                objInsurance.CardValidity = drIns["CardValidity"].ToString();
                                if (drIns["InsurenceCompanyID"].ToString().Trim() != "")
                                    objInsurance.InsurenceCompanyID = Convert.ToInt32(drIns["InsurenceCompanyID"].ToString());
                                if (drIns["TPAID"].ToString() != "")
                                    objInsurance.TPAID = Convert.ToInt32(drIns["TPAID"].ToString());
                                if (drIns["ActiveStatus"].ToString().Trim() != "")
                                    objInsurance.ActiveStatus = Convert.ToInt32(drIns["ActiveStatus"].ToString());

                                objInsurance.InsuranceNo = drIns["InsuranceNo"].ToString();
                                objInsurance.ReceiverID = drIns["ReceiverID"].ToString();
                                objInsurance.AgreemenID = drIns["AgreemenID"].ToString();
                                objInsurance.MembershIPNO = drIns["MembershIPNO"].ToString();
                                objInsurance.Collectables = drIns["Collectables"].ToString();
                                objInsurance.Deductables = drIns["Deductables"].ToString();
                                objInsurance.IsPercollectables = drIns["IsPercollectables"].ToString();
                                objInsurance.IsPerdeductables = drIns["IsPerdeductables"].ToString();
                                objInsurance.PayerName = drIns["PayerName"].ToString();
                                objInsurance.ReceiverName = drIns["ReceiverName"].ToString();
                                objInsurance.TPAName = drIns["TPAName"].ToString();
                                objInsurance.InsurenceCompanyName = drIns["InsurenceCompanyName"].ToString();
                                objInsurance.GradeName = drIns["GradeName"].ToString();
                                if (drIns["Blocked"].ToString().Trim() != "")
                                    objInsurance.Blocked = Convert.ToInt32(drIns["Blocked"].ToString());
                                objInsurance.Documentpath = drIns["Documentpath"].ToString();
                                objInsurance.PolicyNo = drIns["PolicyNo"].ToString();
                                objInsurance.ReferalBasisNo = drIns["ReferalBasisNo"].ToString();
                                objInsurance.PatientEmpID = drIns["PatientEmpID"].ToString();

                                objInsurance.RelationCode = drIns["RelationCode"].ToString();
                                objInsurance.InsuranceCardExpiry = drIns["InsuranceCardExpiry"].ToString();
                                objInsurance.ContractNo = drIns["ContractNo"].ToString();
                                objInsurance.PolicyValidFrom = drIns["PolicyValidFrom"].ToString();
                                objInsurance.MRNo = drIns["MRNo"].ToString();
                                objInsurance.EmpRelation = drIns["EmpRelation"].ToString();
                                if (drIns["RelationID"].ToString() != "")
                                    objInsurance.RelationID = Convert.ToInt32(drIns["RelationID"].ToString());
                                objInsurance.CompanyName = drIns["CompanyName"].ToString();
                                objInsurance.CompanyCode = drIns["CompanyCode"].ToString();
                                objInsurance.PhoneNo = drIns["PhoneNo"].ToString();
                                objInsurance.CompanyEmail = drIns["CompanyEmail"].ToString();
                                objInsurance.CompanyFax = drIns["CompanyFax"].ToString();
                                objInsurance.Companyaddress = drIns["Companyaddress"].ToString();
                                objInsurance.ContractValidFrom = drIns["ContractValidFrom"].ToString();
                                objInsurance.ContractValidTo = drIns["ContractValidTo"].ToString();
                                objInsurance.InsurenceCompanyCode = drIns["InsurenceCompanyCode"].ToString();
                                if (drIns["MAXCollectable"].ToString().Trim() != "")
                                    objInsurance.MAXCollectable = Convert.ToInt32(drIns["MAXCollectable"].ToString());
                                if (drIns["GradeBlocked"].ToString().Trim() != "")
                                    objInsurance.GradeBlocked = Convert.ToInt32(drIns["GradeBlocked"].ToString());

                                obj.InsurancePatientData.Add(objInsurance);
                            }
                            objPatientData.PatientData.Add(obj);
                        }
                    }
                    if (objPatientData.PatientData.Count > 0)
                    {
                        objPatientData.Code = ProcessStatus.Success;
                        objPatientData.Status = ProcessStatus.Success.ToString();
                        objPatientData.Message = "";
                        objPatientData.Message2L = "";

                    }
                    else
                    {
                        objPatientData.Code = ProcessStatus.Success;
                        objPatientData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objPatientData;
            }
            catch (Exception ex)
            {
                objPatientData.Code = ProcessStatus.Fail;
                objPatientData.Status = ProcessStatus.Fail.ToString();
                objPatientData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchPatientData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objPatientData;
        }
        public DataSet GetADVProcedure(string ProcedureName, string Type, string Filter, int UserId, int WstationId, int intError)
        {
            DataSet ds = new DataSet();
            DataTable dtTitleRules = new DataTable("TitleRules");
            DataHelper objDataHelper = new DataHelper(WstationId, (int)Database.Master);
            try
            {
                DataColumn dcMasters = dtTitleRules.Columns.Add("ParamName", typeof(string));
                dtTitleRules.Columns.Add("ParamValue", typeof(string));
                dtTitleRules.Columns.Add("ParamDBtype", typeof(DbType));
                dtTitleRules.Columns.Add("ParamDirection", typeof(Int32));
                dtTitleRules.Rows.Add(new object[] { "@Type", Type, DbType.StringFixedLength, 1 });
                dtTitleRules.Rows.Add(new object[] { "@Filter", Filter, DbType.StringFixedLength, 1 });
                dtTitleRules.Rows.Add(new object[] { "@UserId", UserId, DbType.Int32, 1 });
                dtTitleRules.Rows.Add(new object[] { "@WorkStationID", WstationId, DbType.Int32, 1 });
                dtTitleRules.Rows.Add(new object[] { "@Error", intError, DbType.Int32, 2 });
                int icnt = 0;
                IDbDataParameter[] IPrm = new IDbDataParameter[dtTitleRules.Rows.Count];
                foreach (DataRow oDr in dtTitleRules.Rows)
                {
                    IPrm[icnt] = objDataHelper.CreateDataParameter();
                    IPrm[icnt].ParameterName = oDr["ParamName"].ToString();
                    IPrm[icnt].Value = oDr["ParamValue"];
                    IPrm[icnt].DbType = (DbType)oDr["ParamDBtype"];
                    IPrm[icnt].Direction = (ParameterDirection)oDr["ParamDirection"];
                    icnt++;
                }
                ds = objDataHelper.RunSPReturnDS(ProcedureName, IPrm);
                return ds;
            }
            finally
            {
                ds.Dispose();
                dtTitleRules.Dispose();
                objDataHelper = null;
            }
        }
        private bool CheckNationalID(string PatId, PatientRegistrationDetails PatientBillList)
        {
            DataTable dtmerged = new DataTable();
            bool value = true;
            try
            {
                string strFilter = string.Empty;
                if (!string.IsNullOrEmpty(PatId))
                    strFilter = "patblocked=0 and SSN='" + PatientBillList.SSN + "' and PatientID <> '" + PatId + "' and ( RootPatientId is null OR RootPatientId <> '" + PatId + "')";
                else
                {
                    PatId = "0";
                    strFilter = "patblocked=0 and SSN='" + PatientBillList.SSN + "'";
                }

                dtmerged = GetADVProcedure("Pr_FetchPatientsAdv", "", strFilter, Convert.ToInt32(PatientBillList.UserID), PatientBillList.WorkStationID, 0).Tables[0];
                if (dtmerged.Rows.Count > 0)
                {
                    DataRow[] Drpatient = dtmerged.Select();
                    if (Drpatient[0]["PatientID"].ToString() == PatId.Trim())
                    {
                        value = true;
                    }
                    else
                    {
                        value = false;
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in CheckPinBlock", "");
                return false;
            }
        }
        public DataSet FetchNationalityData(string ProcedureName, string Type, string Filter, int UserId, int WstationId, int intError)
        {
            DataSet ds = new DataSet();
            DataTable dtTitleRules = new DataTable("TitleRules");
            DataHelper objDataHelper = new DataHelper(WstationId, (int)Database.Master);
            try
            {
                DataColumn dcMasters = dtTitleRules.Columns.Add("ParamName", typeof(string));
                dtTitleRules.Columns.Add("ParamValue", typeof(string));
                dtTitleRules.Columns.Add("ParamDBtype", typeof(DbType));
                dtTitleRules.Columns.Add("ParamDirection", typeof(Int32));
                dtTitleRules.Rows.Add(new object[] { "@Type", Type, DbType.StringFixedLength, 1 });
                dtTitleRules.Rows.Add(new object[] { "@Filter", Filter, DbType.StringFixedLength, 1 });
                dtTitleRules.Rows.Add(new object[] { "@UserId", UserId, DbType.Int32, 1 });
                dtTitleRules.Rows.Add(new object[] { "@WorkStationID", WstationId, DbType.Int32, 1 });
                dtTitleRules.Rows.Add(new object[] { "@Error", intError, DbType.Int32, 2 });
                int icnt = 0;
                IDbDataParameter[] IPrm = new IDbDataParameter[dtTitleRules.Rows.Count];
                foreach (DataRow oDr in dtTitleRules.Rows)
                {
                    IPrm[icnt] = objDataHelper.CreateDataParameter();
                    IPrm[icnt].ParameterName = oDr["ParamName"].ToString();
                    IPrm[icnt].Value = oDr["ParamValue"];
                    IPrm[icnt].DbType = (DbType)oDr["ParamDBtype"];
                    IPrm[icnt].Direction = (ParameterDirection)oDr["ParamDirection"];
                    icnt++;
                }
                ds = objDataHelper.RunSPReturnDS(ProcedureName, IPrm);
                return ds;
            }
            finally
            {
                ds.Dispose();
                dtTitleRules.Dispose();
                objDataHelper = null;
            }
        }
        public PatientRegistration SavePatientData(PatientRegistrationDetails RegParams)
        {
            PatientRegistration objSave = new PatientRegistration();
            if (CheckNationalID(RegParams.PatientID, RegParams) == false)
            {
                string ReturnMessage = string.Empty;
                DataTable dtNationity = new DataTable();
                dtNationity = FetchNationalityData("Pr_FetchPatientsAdv", "", "patblocked=0 and SSN='" + RegParams.SSN + "'", Convert.ToInt32(RegParams.UserID), Convert.ToInt32(RegParams.WorkStationID), 0).Tables[0];
                ReturnMessage = "Nationality ID Already Exist to :" + dtNationity.Rows[0]["RegCode"];
                objSave.Code = ProcessStatus.Success;
                objSave.Status = ProcessStatus.Success.ToString();
                objSave.Message = ReturnMessage;
                return objSave;
            }
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);

            string strXMLMultipleInsurance = string.Empty;
            if (RegParams.multipleInsurance.ToArray().Length > 0)
            {
                DataSet dtMultipleInsurance = Utilities.ToDataSetFromArrayOfObject(RegParams.multipleInsurance.ToArray());
                if (dtMultipleInsurance != null && dtMultipleInsurance.Tables.Count > 0 && dtMultipleInsurance.Tables[0].Rows[0]["PID"].ToString() != "")
                {
                    strXMLMultipleInsurance = Utilities.ConvertDTToXML("PATIENT", "MULINS", dtMultipleInsurance.Tables[0]);
                }
            }

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientId", (object)DBNull.Value, DbType.Int32, ParameterDirection.Output));
                objIDbDataParameters.Add(CreateParam1(objDataHelper, "@RegCode", null, 100, DbType.String, ParameterDirection.Output));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TitleID", RegParams.TitleID == 0 ? (object)DBNull.Value : RegParams.TitleID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GenderID", RegParams.GenderID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Familyname", RegParams.Familyname, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FirstName", RegParams.FirstName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MiddleName", RegParams.MiddleName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GrandFatherName", RegParams.GrandFatherName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Familyname2l", string.IsNullOrEmpty(RegParams.Familyname2l) ? (object)DBNull.Value : RegParams.Familyname2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FirstName2L", string.IsNullOrEmpty(RegParams.FirstName2l) ? (object)DBNull.Value : RegParams.FirstName2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MiddleName2L", string.IsNullOrEmpty(RegParams.MiddleName2l) ? (object)DBNull.Value : RegParams.MiddleName2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GrandFatherName2L", RegParams.GrandFatherName2L, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Isvip", RegParams.ISVIP, DbType.Boolean, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@DOB", Convert.ToDateTime(RegParams.DOB).ToString("dd-MMM-yyyy"), DbType.DateTime, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Age", RegParams.Age, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@AgeUOMID", RegParams.AgeUOMID == 0 ? (object)DBNull.Value : RegParams.AgeUOMID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@IsAgeByDOB", RegParams.IsAgeByDOB == "" ? (object)DBNull.Value : RegParams.IsAgeByDOB, DbType.Byte, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ReligionID", RegParams.ReligionID == 0 ? (object)DBNull.Value : RegParams.ReligionID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MaritalStatusID", RegParams.MaritalStatusID == 0 ? (object)DBNull.Value : RegParams.MaritalStatusID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Address01", RegParams.Address01, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CityID", RegParams.CityID == 0 ? (object)DBNull.Value : RegParams.CityID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CityAreaId", RegParams.CityAreaId == 0 ? (object)DBNull.Value : RegParams.CityAreaId, DbType.Int32, ParameterDirection.Input));

                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PhoneNo", RegParams.PhoneNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MobileNo", RegParams.MobileNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@EMail", RegParams.Email == "undefined" ? (object)DBNull.Value : RegParams.Email, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@NationalityId", RegParams.NationalityID == 0 ? (object)DBNull.Value : RegParams.NationalityID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@SSN", RegParams.SSN == "0" || string.IsNullOrEmpty(RegParams.SSN) ? (object)DBNull.Value : RegParams.SSN, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PassportNo", RegParams.PassportNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContactName", RegParams.ContactName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContRelationId", RegParams.ContRelationID == 0 ? (object)DBNull.Value : RegParams.ContRelationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContPhoneNo", RegParams.ContPhoneNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientEmpId", RegParams.PatientEmpID == 0 ? (object)DBNull.Value : RegParams.PatientEmpID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PhotoPath", RegParams.PhotoPath, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@strpath", RegParams.StrPath, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", RegParams.HospitalID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MultipleI", strXMLMultipleInsurance, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@USERID", RegParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WORKSTATIONID", RegParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", 0, DbType.Int32, ParameterDirection.Output));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@IsServicePatient", RegParams.IsServicePatient, DbType.Byte, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientType", RegParams.PatientType, DbType.Int32, ParameterDirection.Input));

                int intRes = objDataHelper.RunSP("pr_InsertPatients", objIDbDataParameters.ToArray());

                if (intRes == -1 || intRes > 0)
                {
                    if (ConfigurationManager.AppSettings["EnableSMS"].ToString() == "YES")
                    {
                        bool IsArabic = false;
                        DataSet dsNationalityID = FetchNationalityID();
                        if (dsNationalityID.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drN = dsNationalityID.Tables[0].Select("NationalityID=" + RegParams.NationalityID);
                            if (drN.Length > 0)
                                IsArabic = Convert.ToBoolean(drN[0]["IsArabic"]);

                        }

                        string Message = string.Empty;
                        string EnglishSMS = ConfigurationManager.AppSettings["EnglishSMS"].ToString();
                        string ArabicSMS = ConfigurationManager.AppSettings["ArabicSMS"].ToString();
                        string HospitalURL = ConfigurationManager.AppSettings["HospitalURL"].ToString();
                        string PatientName = string.Empty;
                        if (IsArabic)
                            PatientName = RegParams.FirstName2l + " " + RegParams.MiddleName2l + " " + RegParams.Familyname2l;
                        else
                            PatientName = RegParams.FirstName + " " + RegParams.MiddleName + " " + RegParams.Familyname;
                        string RegistrationDate = Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy");
                        string HijriDate = string.Empty;
                        if (IsArabic)
                        {
                            string ConvertDateString = string.Empty;
                            CultureInfo higri_format = new CultureInfo("ar-SA");
                            higri_format.DateTimeFormat.Calendar = new HijriCalendar();
                            DateTime CurrentDate;
                            DateTime dt;
                            if (DateTime.TryParse(RegistrationDate.ToString(), out dt))
                            {
                                CurrentDate = Convert.ToDateTime(RegistrationDate);
                                string CurrentTime = Convert.ToDateTime(RegistrationDate).TimeOfDay.ToString();
                                ConvertDateString = CurrentDate.Date.ToString("dd-MM-yyyy", higri_format);
                                ConvertDateString = ConvertDateString.Replace("10:11:12", CurrentTime);
                                HijriDate = ConvertDateString;
                            }
                        }
                        if (IsArabic)
                        {
                            Message = ArabicSMS.Replace("{PatientFullName2L}", PatientName);
                            Message = Message.Replace("{RegistrationDate}", HijriDate);
                            Message = Message.Replace("{SSN}", RegParams.SSN.ToString());
                        }
                        else
                        {
                            Message = EnglishSMS.Replace("{PatientFullName}", PatientName);
                            Message = Message.Replace("{RegistrationDate}", RegistrationDate);
                            Message = Message.Replace("{SSN}", RegParams.SSN.ToString());
                        }

                        Message = Message + Environment.NewLine + HospitalURL;
                        string SMSStatus = CommonUtilities.SendSMS.SendOTP(RegParams.MobileNo, Message, IsArabic);
                        if (SMSStatus == "SMSSent")
                        {
                            objSave.Code = ProcessStatus.Success;
                            objSave.Status = ProcessStatus.Success.ToString();
                            objSave.Message = "Registered Successfully";
                            objSave.PatientId = Convert.ToInt32(objIDbDataParameters[0].Value.ToString());
                            objSave.RegCode = objIDbDataParameters[1].Value.ToString();
                        }
                        else if (SMSStatus == "SMSNotSent")
                        {
                            objSave.Message = Resources.English.ResourceManager.GetString("OTPSentFailed");
                            objSave.Message2L = Resources.Arabic.ResourceManager.GetString("OTPSentFailed");
                        }
                        else
                        {
                            objSave.Message = Resources.English.ResourceManager.GetString("OTPSentFailedError");
                            objSave.Message2L = Resources.Arabic.ResourceManager.GetString("OTPSentFailedError");
                        }
                    }
                    else
                    {
                        objSave.Code = ProcessStatus.Success;
                        objSave.Status = ProcessStatus.Success.ToString();
                        objSave.Message = "Registered Successfully";
                        objSave.PatientId = Convert.ToInt32(objIDbDataParameters[0].Value.ToString());
                        objSave.RegCode = objIDbDataParameters[1].Value.ToString();
                    }

                }
                else
                {
                    objSave.Code = ProcessStatus.Fail;
                    objSave.Status = ProcessStatus.Fail.ToString();
                    objSave.Message = "Error occured while Saving";
                }
                return objSave;

            }
            catch (Exception ex)
            {
                objSave.Code = ProcessStatus.Fail;
                objSave.Status = ProcessStatus.Fail.ToString();
                objSave.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in SavePatientData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objSave;

        }



        public PatientRegistration UpdatePatientData(PatientRegistrationDetails RegParams)
        {
            PatientRegistration objSave = new PatientRegistration();
            if (CheckNationalID(RegParams.PatientID, RegParams) == false)
            {
                string ReturnMessage = string.Empty;
                DataTable dtNationity = new DataTable();
                dtNationity = FetchNationalityData("Pr_FetchPatientsAdv", "", "patblocked=0 and SSN='" + RegParams.SSN + "'", Convert.ToInt32(RegParams.UserID), Convert.ToInt32(RegParams.WorkStationID), 0).Tables[0];
                ReturnMessage = "Nationality ID Already Exist to :" + dtNationity.Rows[0]["RegCode"];
                objSave.Code = ProcessStatus.Success;
                objSave.Status = ProcessStatus.Success.ToString();
                objSave.Message = ReturnMessage;
                return objSave;
            }
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);

            string strXMLMultipleInsurance = string.Empty; string strXMLAuditXML = string.Empty;
            if (RegParams.multipleInsurance.ToArray().Length > 0)
            {
                DataSet dtMultipleInsurance = Utilities.ToDataSetFromArrayOfObject(RegParams.multipleInsurance.ToArray());
                if (dtMultipleInsurance != null && dtMultipleInsurance.Tables.Count > 0 && dtMultipleInsurance.Tables[0].Rows[0]["PID"].ToString() != "")
                {
                    strXMLMultipleInsurance = Utilities.ConvertDTToXML("PATIENT", "MULINS", dtMultipleInsurance.Tables[0]);
                }
            }
            if (RegParams.AuditPatientXML != null)
            {
                if (RegParams.AuditPatientXML.ToArray().Length > 0)
                {
                    DataSet dtAuditPatientXML = Utilities.ToDataSetFromArrayOfObject(RegParams.AuditPatientXML.ToArray());
                    if (dtAuditPatientXML != null && dtAuditPatientXML.Tables.Count > 0)
                    {
                        strXMLAuditXML = Utilities.ConvertDTToXML("Audit", "Patient", dtAuditPatientXML.Tables[0]);
                    }
                }
            }
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientId", RegParams.PatientID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam1(objDataHelper, "@RegCode", RegParams.RegCode, 100, DbType.String, ParameterDirection.InputOutput));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TitleID", RegParams.TitleID == 0 ? (object)DBNull.Value : RegParams.TitleID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GenderID", RegParams.GenderID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Familyname", RegParams.Familyname, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FirstName", RegParams.FirstName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MiddleName", RegParams.MiddleName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GrandFatherName", string.Empty, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Familyname2l", string.IsNullOrEmpty(RegParams.Familyname2l) ? (object)DBNull.Value : RegParams.Familyname2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FirstName2L", string.IsNullOrEmpty(RegParams.FirstName2l) ? (object)DBNull.Value : RegParams.FirstName2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MiddleName2L", string.IsNullOrEmpty(RegParams.MiddleName2l) ? (object)DBNull.Value : RegParams.MiddleName2l, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@GrandFatherName2L", string.IsNullOrEmpty(RegParams.GrandFatherName2L) ? (object)DBNull.Value : RegParams.GrandFatherName2L, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Isvip", RegParams.ISVIP, DbType.Byte, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@DOB", Convert.ToDateTime(RegParams.DOB).ToString("dd-MMM-yyyy"), DbType.DateTime, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Age", RegParams.Age, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@AgeUOMID", RegParams.AgeUOMID == 0 ? (object)DBNull.Value : RegParams.AgeUOMID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@IsAgeByDOB", RegParams.IsAgeByDOB == "" ? (object)DBNull.Value : RegParams.IsAgeByDOB, DbType.Byte, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ReligionID", RegParams.ReligionID == 0 ? (object)DBNull.Value : RegParams.ReligionID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MaritalStatusID", RegParams.MaritalStatusID == 0 ? (object)DBNull.Value : RegParams.MaritalStatusID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Address01", RegParams.Address01, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CityID", RegParams.CityID == 0 ? (object)DBNull.Value : RegParams.CityID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CityAreaId", RegParams.CityAreaId == 0 ? (object)DBNull.Value : RegParams.CityAreaId, DbType.Int32, ParameterDirection.Input));

                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PhoneNo", RegParams.PhoneNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MobileNo", RegParams.MobileNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@EMail", RegParams.Email, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@NationalityId", RegParams.NationalityID == 0 ? (object)DBNull.Value : RegParams.NationalityID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@SSN", RegParams.SSN == "0" || string.IsNullOrEmpty(RegParams.SSN) ? (object)DBNull.Value : RegParams.SSN, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PassportNo", RegParams.PassportNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContactName", RegParams.ContactName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContRelationId", RegParams.ContRelationID == 0 ? (object)DBNull.Value : RegParams.ContRelationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ContPhoneNo", RegParams.ContPhoneNo, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientEmpId", RegParams.PatientEmpID == 0 ? (object)DBNull.Value : RegParams.PatientEmpID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PhotoPath", RegParams.PhotoPath, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@strpath", RegParams.StrPath, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", RegParams.HospitalID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@MultipleI", strXMLMultipleInsurance, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@USERID", RegParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WORKSTATIONID", RegParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Blocked", 0, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", 0, DbType.Int32, ParameterDirection.Output));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@IsServicePatient", RegParams.IsServicePatient, DbType.Byte, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientType", RegParams.PatientType, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@AuditXML", strXMLAuditXML, DbType.String, ParameterDirection.Input));


                int intRes = objDataHelper.RunSP("Pr_UpdatePatients", objIDbDataParameters.ToArray());

                if (intRes == -1 || intRes > 0)
                {
                    objSave.Code = ProcessStatus.Success;
                    objSave.Status = ProcessStatus.Success.ToString();
                    objSave.Message = "Modifed Successfully";
                    objSave.RegCode = objIDbDataParameters[1].Value.ToString();
                }
                else
                {
                    objSave.Code = ProcessStatus.Fail;
                    objSave.Status = ProcessStatus.Fail.ToString();
                    objSave.Message = "Error occured while Modifying";
                }
                return objSave;

            }
            catch (Exception ex)
            {
                objSave.Code = ProcessStatus.Fail;
                objSave.Status = ProcessStatus.Fail.ToString();
                objSave.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in UpdatePatientData", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objSave;

        }

        public bool CheckValidateUser(string UserName, string Password)
        {
            DataHelper objDataHelper = new DataHelper(0, 1);
            DataSet dsToken = new DataSet();
            bool UserExists;
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserName", UserName, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Password", Password, DbType.String, ParameterDirection.Input));
                dsToken = objDataHelper.RunSPReturnDS("PR_FetchAPIBASICAUTHORIZATIONUSERS_MAPI", objIDbDataParameters.ToArray());
                if (dsToken.Tables[0].Rows.Count > 0)
                    UserExists = true;
                else
                    UserExists = false;
                return UserExists;
            }
            finally
            { objDataHelper = null; }
        }

        public FetchDoctorWiseAvailabilityM FetchDoctorWiseAvailability(GetDoctorWiseAvailability DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchDoctorWiseAvailabilityM objGetMasterData = new FetchDoctorWiseAvailabilityM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Specialiseid", DocParams.Specialiseid, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@DoctorID", DocParams.DoctorID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", DocParams.HospitalID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_FetchDoctorCurrentDayScheduleDetails", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetDoctorWiseAvailabilityListOutput obj = new GetDoctorWiseAvailabilityListOutput();
                            obj.DoctorID = dr["DoctorID"].ToString();
                            obj.DoctorCode = dr["DoctorCode"].ToString();
                            obj.DoctorName = dr["DoctorName"].ToString();
                            obj.DoctorName2L = dr["DoctorName2L"].ToString();
                            obj.SpecialiseID = dr["SpecialiseID"].ToString();
                            obj.Specialisation = dr["Specialisation"].ToString();
                            obj.Specialisation2L = dr["Specialisation2L"].ToString();
                            obj.MaxConsultation = dr["MaxConsultation"].ToString();
                            obj.TotalAppointments = dr["TotalAppointments"].ToString();
                            obj.AppointmentBills = dr["AppointmentBills"].ToString();
                            obj.TotalBills = dr["TotalBills"].ToString();
                            obj.Visited = dr["Visited"].ToString();
                            obj.ServiceItemID = dr["ServiceItemID"].ToString();
                            objGetMasterData.DoctorAvailCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.DoctorAvailCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchDoctorWiseAvailability", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public ValidateNationalIDM ValidateNationalID(ValidateNationalID DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            ValidateNationalIDM objGetMasterData = new ValidateNationalIDM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@NationalID", DocParams.NationalID, DbType.String, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_ValidateNationalId", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            ValidateNationalIDListOutput obj = new ValidateNationalIDListOutput();
                            obj.PatientID = dr["PatientID"].ToString();
                            obj.RegCode = dr["RegCode"].ToString();
                            obj.SSN = dr["SSN"].ToString();
                            objGetMasterData.ValidateCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.ValidateCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in ValidateNationalID", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public ValidatePinBlockM ValidatePinBlock(ValidatePinBlock DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            ValidatePinBlockM objGetMasterData = new ValidatePinBlockM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientID", DocParams.PatientID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_ValidatePinBlock", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            ValidatePinBlockListOutput obj = new ValidatePinBlockListOutput();
                            obj.Blocktype = dr["Blocktype"].ToString();
                            obj.Blockreason = dr["Blockreason"].ToString();
                            obj.Discription = dr["Discription"].ToString();
                            obj.EffectiveDate = dr["EffectiveDate"].ToString();
                            obj.STATUS = dr["STATUS"].ToString();
                            objGetMasterData.ValidateCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.ValidateCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in ValidatePinBlock", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }


        public FetchDoctorWiseAvailabilityM CurrentDayDoctorSpecAvailability(CurrentDayDoctorAvailability DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchDoctorWiseAvailabilityM objGetMasterData = new FetchDoctorWiseAvailabilityM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();

                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Specialiseid", DocParams.Specialiseid == 0 ? (object)DBNull.Value : DocParams.Specialiseid, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@DoctorCode", DocParams.DoctorCode == "0" ? (object)DBNull.Value : DocParams.DoctorCode, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", DocParams.HospitalID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserID", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                //objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_FetchDoctorCurrentDayScheduleDetails", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetDoctorWiseAvailabilityListOutput obj = new GetDoctorWiseAvailabilityListOutput();
                            obj.DoctorID = dr["DoctorID"].ToString();
                            obj.DoctorCode = dr["DoctorCode"].ToString();
                            obj.DoctorName = dr["DoctorName"].ToString();
                            obj.DoctorName2L = dr["DoctorName2L"].ToString();
                            obj.SpecialiseID = dr["SpecialiseID"].ToString();
                            obj.Specialisation = dr["Specialisation"].ToString();
                            obj.Specialisation2L = dr["Specialisation2L"].ToString();
                            obj.MaxConsultation = dr["MaxConsultation"].ToString();
                            obj.TotalAppointments = dr["TotalAppointments"].ToString();
                            obj.AppointmentBills = dr["AppointmentBills"].ToString();
                            obj.TotalBills = dr["TotalBills"].ToString();
                            obj.Visited = dr["Visited"].ToString();
                            obj.ServiceItemID = dr["ServiceItemID"].ToString();
                            objGetMasterData.DoctorAvailCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.DoctorAvailCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in CurrentDayDoctorSpecAvailability", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }



        public FetchHospitalDoctorsM FetchHospitalDoctors(HospitalDoctors DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchHospitalDoctorsM objGetMasterData = new FetchHospitalDoctorsM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Specialiseid", DocParams.SpecialiseID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", DocParams.HospitalID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchHospitalDoctors", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchHospitalDoctorsListOutput obj = new FetchHospitalDoctorsListOutput();
                            obj.EmpID = Convert.ToInt32(dr["EmpID"].ToString());
                            obj.FullName = dr["FullName"].ToString();
                            obj.FullName2L = dr["FullName2L"].ToString();
                            obj.HospDeptId = Convert.ToInt32(dr["HospDeptId"].ToString());
                            obj.ServiceDocID = dr["ServiceDocID"].ToString();
                            objGetMasterData.DoctorAvailCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.DoctorAvailCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchHospitalDoctors", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }


        public FetchHospitalSpecialisationsM FetchHospitalSpecialisations(HospitalSpecialisations DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchHospitalSpecialisationsM objGetMasterData = new FetchHospitalSpecialisationsM();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", DocParams.Type, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Filter", DocParams.Filter, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));

                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchEmployeeSpecializationsAdv", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchHospitalSpecialisationsListOutput obj = new FetchHospitalSpecialisationsListOutput();
                            obj.id = Convert.ToInt32(dr["id"].ToString());
                            obj.name = dr["name"].ToString();
                            obj.name2L = dr["name2L"].ToString();
                            obj.code = dr["code"].ToString();
                            obj.blocked = Convert.ToInt32(dr["blocked"].ToString());
                            obj.BitBlocked = Convert.ToInt32(dr["BitBlocked"].ToString());
                            objGetMasterData.DoctorAvailCode.Add(obj);
                        }
                    }
                    if (objGetMasterData.DoctorAvailCode.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchHospitalSpecialisations", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }
        public SmartSearchDataList FetchAllCities(SmartSearch DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            SmartSearchDataList objGetMasterData = new SmartSearchDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();

                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchCities", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetSmartSearchDataListOutput obj = new GetSmartSearchDataListOutput();
                            obj.ID = Convert.ToInt32(dr["ID"].ToString());
                            obj.Name = dr["City"].ToString();
                            if (dr.Table.Columns.Contains("StateID"))
                                obj.StateID = dr["StateID"].ToString();
                            if (dr.Table.Columns.Contains("StateName"))
                                obj.StateName = dr["StateName"].ToString();
                            if (dr.Table.Columns.Contains("CountryId"))
                                obj.CountryId = dr["CountryId"].ToString();
                            if (dr.Table.Columns.Contains("Country"))
                                obj.Country = dr["Country"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }


        public FetchAgeDataList FetchAgeCalculate(FetchAge DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchAgeDataList objGetMasterData = new FetchAgeDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@DOB", DocParams.DOB, DbType.DateTime, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchAgeCalculate", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchAgeDataListOutput obj = new FetchAgeDataListOutput();
                            obj.Age = Convert.ToInt32(dr["Age"].ToString());
                            obj.Ageuomid = Convert.ToInt32(dr["Ageuomid"].ToString());
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchDOBDataList FetchDOB(FetchDOB DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchDOBDataList objGetMasterData = new FetchDOBDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@AgeUomID", DocParams.AgeUomID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@age", DocParams.Age, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_FetchDOB", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchDOBDataListOutput obj = new FetchDOBDataListOutput();
                            obj.DOB = Convert.ToDateTime(dr["DOB"].ToString());
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        private bool ValidateHijriKey()
        {
            if ((!string.IsNullOrEmpty(Convert.ToString(ConfigurationManager.AppSettings["ShowHijriDate"]))))
            {
                if (ConfigurationManager.AppSettings["ShowHijriDate"].ToString().ToUpper() == "YES")
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public FetchHijriDataList FetchHijri(FetchHijri DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchHijriDataList objGetMasterData = new FetchHijriDataList();

            try
            {
                FetchHijriDataListOutput obj = new FetchHijriDataListOutput();
                string ConvertDateString = string.Empty;
                CultureInfo higri_format = new CultureInfo("ar-SA");
                higri_format.DateTimeFormat.Calendar = new HijriCalendar();
                DateTime CurrentDate;
                DateTime dt;
                if (DateTime.TryParse(DocParams.GDOB.ToString(), out dt))
                {
                    CurrentDate = Convert.ToDateTime(DocParams.GDOB);
                    string CurrentTime = Convert.ToDateTime(DocParams.GDOB).TimeOfDay.ToString();
                    ConvertDateString = CurrentDate.Date.ToString("dd-MM-yyyy", higri_format);
                    ConvertDateString = ConvertDateString.Replace("10:11:12", CurrentTime);
                    obj.HijriDate = ConvertDateString;
                    obj.Message = "";
                    objGetMasterData.SmartDataList.Add(obj);
                }
                else
                {
                    if (ValidateHijriKey())
                    {
                        obj.HijriDate = "";
                        obj.Message = "Please Enter Valid Date to Show Hijri Date.Valid Pattern is (dd-MMM-yyyy)";
                    }
                }
                if (objGetMasterData.SmartDataList.Count > 0)
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                    objGetMasterData.Message = "";
                    objGetMasterData.Message2L = "";
                }
                else
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        private void CalculateAgeFromDob(string dobOfPatient)
        {
            FetchEnglishDataListOutput obj = new FetchEnglishDataListOutput();
            FetchEnglishDataList objGetMasterData = new FetchEnglishDataList();

        }
        public FetchEnglishDataList FetchEnglishDate(FetchEnglishDate DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchEnglishDataList objGetMasterData = new FetchEnglishDataList();

            try
            {
                FetchEnglishDataListOutput obj = new FetchEnglishDataListOutput();
                CultureInfo arSA = new CultureInfo("ar-SA");
                arSA.DateTimeFormat.Calendar = new HijriCalendar();
                string datehigiri = DocParams.HDOB.Replace('-', '/');
                string[] strarg = datehigiri.Split('/');
                if (strarg.Length == 3)
                {
                    if ((strarg[0].Length == 2) && (strarg[1].Length == 2) && (strarg[2].Length == 4))
                    {
                        var dateValue = DateTime.ParseExact(datehigiri, "dd/MM/yyyy", arSA);
                        obj.EnglishDate = dateValue.ToString("dd-MMM-yyyy");
                        //obj= CalculateAgeFromDob(dateValue.ToString("dd-MMM-yyyy"));

                        if (!string.IsNullOrEmpty(dateValue.ToString("dd-MMM-yyyy").Trim()))
                        {
                            if (!string.IsNullOrEmpty(dateValue.ToString("dd-MMM-yyyy").Trim()))
                            {
                                string strAge = string.Empty;
                                strAge = NxGAgeCalculator.CalculateAgeFromDob(dateValue.ToString("dd-MMM-yyyy"));
                                string[] sAge = strAge.Split('-');
                                switch (sAge[1])
                                {
                                    case "0":
                                        obj.Age = "";
                                        break;
                                    case "1":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 1;
                                        break;
                                    case "2":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 2;
                                        break;
                                    case "3":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 3;
                                        break;
                                    case "4":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 4;
                                        break;
                                    case "5":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 5;
                                        break;
                                    case "6":
                                        obj.Age = sAge[0];
                                        obj.AgeUOM = 6;
                                        break;
                                    default:
                                        if (!string.IsNullOrEmpty(sAge[2].ToString()))
                                        {
                                            obj.Age = string.Empty;
                                            obj.EnglishDate = string.Empty;
                                            obj.Message = "Future dates cannot be selected as Date Of Birth";
                                        }
                                        break;
                                }
                            }
                        }
                        objGetMasterData.SmartDataList.Add(obj);
                    }
                    else
                    {
                        obj.Message = "Please Enter Valid Hijri Date 11-11-1111";
                    }
                }
                else
                {
                    obj.Message = "Please Enter Valid Hijri Date 11-11-1111";
                }
                if (objGetMasterData.SmartDataList.Count > 0)
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                    objGetMasterData.Message = "";
                    objGetMasterData.Message2L = "";
                }
                else
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchEnglishDate", "");
            }
            finally
            {
            }
            return objGetMasterData;
        }

        public UploadToFTPDataList UploadToFTP(UploadToFTP DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            UploadToFTPDataList objGetMasterData = new UploadToFTPDataList();

            try
            {
                string strRemoteFileName = string.Empty;
                UploadToFTPDataListDataListOutput obj = new UploadToFTPDataListDataListOutput();
                FileInfo fileInf;
                fileInf = DocParams.FTPFilename.Contains("~") ? new FileInfo(DocParams.FTPFilename) : new FileInfo(DocParams.FTPFilename);
                string strIPAdd, strUser, strPwd, strRemote;
                strIPAdd = ConfigurationManager.AppSettings["strIPAdd"];
                strUser = ConfigurationManager.AppSettings["strUser"];
                strPwd = ConfigurationManager.AppSettings["strPwd"];
                strRemote = ConfigurationManager.AppSettings["strRemote"];
                string strTemp = DocParams.FTPFilename;
                int intl = strTemp.LastIndexOf("\\");
                strRemoteFileName = strTemp.Substring(intl + 1, (strTemp.Length - intl - 1));
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(@"ftp://" + strIPAdd + "/" + strRemote + "/" + strRemoteFileName);
                reqFTP.Credentials = new NetworkCredential(strUser, strPwd);
                reqFTP.KeepAlive = true;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                int buffLength = 4096;
                byte[] buff = new byte[4096];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                try
                {
                    contentLen = fs.Read(buff, 0, buffLength);
                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);
                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    obj.Save = "Success";
                }
                catch (Exception ex)
                {
                    objGetMasterData.Message = "Error while Uploading";
                }
                finally
                {
                    strm.Close();
                    fs.Close();
                }
                objGetMasterData.SmartDataList.Add(obj);
                if (objGetMasterData.SmartDataList.Count > 0)
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                    objGetMasterData.Message = "";
                    objGetMasterData.Message2L = "";
                }
                else
                {
                    objGetMasterData.Code = ProcessStatus.Success;
                    objGetMasterData.Status = ProcessStatus.Success.ToString();
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchEnglishDate", "");
            }
            finally
            {
            }
            return objGetMasterData;
        }

        public FetchAllCitiesDataList FetchAllCities(FetchAllCities DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchAllCitiesDataList objGetMasterData = new FetchAllCitiesDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchCities", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchAllCitiesDataListOutput obj = new FetchAllCitiesDataListOutput();
                            obj.CityID = Convert.ToInt32(dr["ID"].ToString());
                            obj.City = dr["City"].ToString();
                            obj.City2L = dr["City2L"].ToString();
                            obj.StateID = Convert.ToInt32(dr["StateID"].ToString());
                            obj.StateName = dr["StateName"].ToString();
                            obj.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                            obj.Country = dr["Country"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchCompanyTypesDataList FetchCompanyTypes(GetCompanyTypesDataList DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchCompanyTypesDataList objGetMasterData = new FetchCompanyTypesDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", DocParams.Type, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Filter", DocParams.Filter, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UseriD", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationId", DocParams.WorkStationId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@LanguageID", DocParams.LanguageID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchAdminMasters", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchCompanyTypesDataListOutput obj = new FetchCompanyTypesDataListOutput();
                            obj.id = Convert.ToInt32(dr["id"].ToString());
                            obj.name = dr["name"].ToString();
                            obj.name2L = dr["name2L"].ToString();
                            obj.code = dr["code"].ToString();
                            obj.blocked = Convert.ToBoolean(dr["blocked"]);
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public PATIENTDICTIONARYDataList PATIENTDICTIONARY(PATIENTDICTIONARY DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            PATIENTDICTIONARYDataList objGetMasterData = new PATIENTDICTIONARYDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Tbl", DocParams.Tbl, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@name", DocParams.name, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@languageID", DocParams.languageID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_SSPATIENTDICTIONARY", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            PATIENTDICTIONARYDataListOutput obj = new PATIENTDICTIONARYDataListOutput();
                            obj.ID = Convert.ToInt32(dr["ID"].ToString());
                            obj.Name = dr["Name"].ToString();
                            obj.Name2L = dr["Name2L"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchNationalitiesPriorityDataList FetchNationalitiesPriority(FetchNationalitiesPriority DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchNationalitiesPriorityDataList objGetMasterData = new FetchNationalitiesPriorityDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@NationalityID", DocParams.NationalityID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchNationalities_MAPI", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchNationalitiesPriorityDataListOutput obj = new FetchNationalitiesPriorityDataListOutput();
                            obj.Id = Convert.ToInt32(dr["NationalityID"].ToString());
                            obj.Names = dr["Nationality"].ToString();
                            obj.Names2L = dr["Nationality2L"].ToString();
                            obj.IsArabic = Convert.ToBoolean(dr["IsArabic"].ToString());
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }


        public FetchCityMastersDataList FetchCityMasters(FetchCityMasters DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchCityMastersDataList objGetMasterData = new FetchCityMastersDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CountryID", DocParams.CountryID, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("PR_FetchCityMasters", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchCityMastersDataListOutput obj = new FetchCityMastersDataListOutput();
                            obj.CityID = Convert.ToInt32(dr["CityID"].ToString());
                            obj.CityName = dr["CityName"].ToString();
                            obj.CityName2L = dr["CityName2L"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllCities", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }
        public DataSet FetchNationalityID()
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            DataSet dsNationalityID = new DataSet();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@NationalityID", 0, DbType.Int32, ParameterDirection.Input));
                dsNationalityID = objDataHelper.RunSPReturnDS("Pr_FetchNationalities_MAPI", objIDbDataParameters.ToArray());
                return dsNationalityID;
            }
            finally
            {

            }
        }
        public FetchPatientModificationAuditDataList FetchPatientModificationAudit(FetchPatientModificationAudit DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchPatientModificationAuditDataList objGetMasterData = new FetchPatientModificationAuditDataList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientID", DocParams.PatientID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FromDate", DocParams.FromDate, DbType.DateTime, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Todate", DocParams.Todate, DbType.DateTime, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchPatientModificationAudit", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchPatientModificationAuditListOutput obj = new FetchPatientModificationAuditListOutput();
                            obj.DBColumnName = dr["DBColumnName"].ToString();
                            obj.UpdatedValue = dr["UpdatedValue"].ToString();
                            obj.PreviousValue = dr["PreviousValue"].ToString();
                            obj.HostName = dr["HostName"].ToString();
                            obj.Createdate = Convert.ToDateTime(dr["Createdate"]).ToString("dd-MMM-yyyy");
                            obj.EmployeeName = dr["EmployeeName"].ToString();
                            obj.WorkStations = dr["WorkStations"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchPatientModificationAudit", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }
        public HospitalConsultants FetchConsultants(FetchConsultants DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            HospitalConsultants objGetMasterData = new HospitalConsultants();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Tbl", "1", DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@name", DocParams.Name, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@languageID", 0, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@param1", 3392, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@hospitalid", 3, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@param3", 2, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@param4", 1, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_SSHospitalFacilityDoctors", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchConsultantsOutput obj = new FetchConsultantsOutput();
                            obj.ConsultantID = dr["ID"].ToString();
                            obj.ConsultantName = dr["Name"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchConsultants", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }
        public HospitalServices GetAllServices(HospitalServicesInputs DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            HospitalServices objGetMasterData = new HospitalServices();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@type", "2", DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@filter", "Blocked = 0 and PatientType in (0, 1) and IsVisible=1", DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@userId", 4394, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@workstationId", 3392, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchServicesAdv", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            HospitalServicesOutput obj = new HospitalServicesOutput();
                            obj.ServiceID = Convert.ToInt32(dr["id"].ToString());
                            obj.ServiceName = dr["DisplayName"].ToString();
                            obj.ServiceName2L = dr["ServiceName2l"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchAllServices", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }
        public string FileUpload(string base64, string fileName)
        {
            try
            {
                string server = ConfigurationManager.AppSettings["FTPHost"].ToString();  // Just the IP Address 
                string path = ConfigurationManager.AppSettings["FTPDirectoryFilePath"].ToString();
                byte[] buffer = Convert.FromBase64String(base64);
                //WebRequest request = WebRequest.Create(server + path + fileName);
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(server + path + fileName);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"].ToString(), ConfigurationManager.AppSettings["FTPPassword"].ToString());
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();
                return "File Uploaded Successfully";
            }
            catch (Exception ex)
            {
                return "Error uploading in file :" + ex.Message;
            }

        }
        public string FileDownload(string FileName)
        {
            try
            {
                CreateMeeting();
                string server = ConfigurationManager.AppSettings["FTPHost"].ToString();  // Just the IP Address 
                string path = ConfigurationManager.AppSettings["FTPDirectoryFilePath"].ToString();
                /* Create an FTP Request */
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(server + path + FileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUserName"].ToString(), ConfigurationManager.AppSettings["FTPPassword"].ToString());
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                var stream = ftpResponse.GetResponseStream();
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                return "Download error";
            }
        }
        public LoginDetails ValidateLoginCredentials(string userName, string password, string location)
        {
            CreateMeeting();
            string SWlicenceStatus = string.Empty;
            LoginDetails obj = new LoginDetails();
            DataSet ds = new DataSet();
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, 3);
            try
            {
                IDbDataParameter PrmUserName = objDataHelper.CreateDataParameter();
                PrmUserName.ParameterName = "@UserName";
                PrmUserName.DbType = DbType.String;
                PrmUserName.Value = userName;
                PrmUserName.Direction = ParameterDirection.Input;

                IDbDataParameter prmUserID = objDataHelper.CreateDataParameter();
                prmUserID.ParameterName = "@USERID";
                prmUserID.DbType = DbType.Int32;
                prmUserID.Value = DBNull.Value;
                prmUserID.Direction = ParameterDirection.Input;

                IDbDataParameter prmWorkStationID = objDataHelper.CreateDataParameter();
                prmWorkStationID.ParameterName = "@WORKSTATIONID";
                prmWorkStationID.DbType = DbType.Int32;
                prmWorkStationID.Value = DBNull.Value;
                prmWorkStationID.Direction = ParameterDirection.Input;

                //int intErrorNum = 0;
                IDbDataParameter prmError = objDataHelper.CreateDataParameter();
                prmError.ParameterName = "@Error";
                prmError.DbType = DbType.Int32;
                prmError.Value = DBNull.Value;
                prmError.Direction = ParameterDirection.Output;

                IDbDataParameter prmFeatureId = objDataHelper.CreateDataParameter();
                prmFeatureId.ParameterName = "@FeatureId";
                prmFeatureId.DbType = DbType.Int32;
                prmFeatureId.Value = DBNull.Value;
                prmFeatureId.Direction = ParameterDirection.Input;

                IDbDataParameter prmFunctionId = objDataHelper.CreateDataParameter();
                prmFunctionId.ParameterName = "@FunctionId";
                prmFunctionId.DbType = DbType.Int32;
                prmFunctionId.Value = DBNull.Value;
                prmFunctionId.Direction = ParameterDirection.Input;

                IDbDataParameter prmCallContext = objDataHelper.CreateDataParameter();
                prmCallContext.ParameterName = "@CallContext";
                prmCallContext.DbType = DbType.Int32;
                prmCallContext.Value = DBNull.Value;
                prmCallContext.Direction = ParameterDirection.Input;

                ds = objDataHelper.RunSPReturnDS("Pr_FetchUserSecurityDtls", PrmUserName, prmUserID, prmWorkStationID, prmError, prmFeatureId, prmFunctionId, prmCallContext);
                //end of addition.

                if (ds == null)
                    return null;
                else if (ds.Tables.Count == 0)
                    return null;

                ds.Tables[0].Columns[0].ColumnName = "UserId";
                ds.Tables[0].Columns[1].ColumnName = "UserName";
                ds.Tables[0].Columns[2].ColumnName = "Password";
                ds.Tables[0].Columns[3].ColumnName = "ISLocked";
                ds.Tables[0].Columns[4].ColumnName = "ISPWDExpired";
                ds.Tables[0].Columns[5].ColumnName = "PWDSetDate";
                ds.Tables[0].Columns[6].ColumnName = "LoggedHostIP";
                ds.Tables[0].Columns[7].ColumnName = "LoggedHostName";
                ds.Tables[0].Columns[8].ColumnName = "ISLogged";
                ds.Tables[0].Columns[9].ColumnName = "PWDDays";
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        LoginDetailsOutput objdata = new LoginDetailsOutput();
                        objdata.UserName = dr["UserName"].ToString();
                        objdata.Password = dr["Password"].ToString();
                        objdata.UserId = dr["UserId"].ToString();
                        objdata.ISLocked = dr["ISLocked"].ToString();
                        objdata.ISPWDExpired = dr["ISPWDExpired"].ToString();
                        objdata.PWDSetDate = dr["PWDSetDate"].ToString();
                        objdata.LoggedHostIP = dr["LoggedHostIP"].ToString();
                        objdata.LoggedHostName = dr["LoggedHostName"].ToString();
                        objdata.ISLogged = dr["ISLogged"].ToString();
                        objdata.PWDDays = dr["PWDDays"].ToString();
                        obj.SmartDataList.Add(objdata);
                    }
                }
                else
                {
                    LoginDetailsOutput objdata = new LoginDetailsOutput();
                    objdata.CredentialsMessage = "Incorrect Username";
                    obj.SmartDataList.Add(objdata);

                }
            }
            finally
            { objDataHelper = null; }
            return obj;
        }
        public FetchEmployeeList FetchEmployees(FetchEmployee DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchEmployeeList objGetMasterData = new FetchEmployeeList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@min", 1, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@max", 100, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", "1", DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Filter", DocParams.Filter, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Featureid", 775, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Functionid", -1, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CallContext", null, DbType.String, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_Nxg_FetchGlobalEmployeeSpecializationsAdv", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[1].Rows)
                        {
                            FetchEmployeeOutput obj = new FetchEmployeeOutput();
                            obj.EmpID = Convert.ToInt32(dr["EmpID"].ToString());
                            obj.EmpNo = dr["EmpNo"].ToString();
                            obj.FullName = dr["FullName"].ToString();
                            obj.FullName2l = dr["FullName2l"].ToString();
                            obj.DepartmentName = dr["DepartmentName"].ToString();
                            obj.DepartmentName2l = dr["DepartmentName2l"].ToString();
                            obj.Specialisation = dr["Specialisation"].ToString();
                            obj.Specialisation2l = dr["Specialisation2l"].ToString();
                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchConsultants", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public FetchSpeficEmployeeList FetchEmployees(FetchSpeficEmployee DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchSpeficEmployeeList objGetMasterData = new FetchSpeficEmployeeList();

            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@EmpID", DocParams.EmpID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@EmpNo", null, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TBL", null, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Deleted", null, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserId", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("Pr_FetchEmployees", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            FetchSpeficEmployeeOutput obj = new FetchSpeficEmployeeOutput();
                            obj.EmpID = Convert.ToInt32(dr["EmpID"].ToString());
                            obj.EmpNo = dr["EmpNo"].ToString();
                            obj.TitleID = Convert.ToInt32(dr["TitleID"].ToString());
                            obj.Title = dr["Title"].ToString();

                            obj.FirstName = dr["FirstName"].ToString();
                            obj.FirstName2L = dr["FirstName2L"].ToString();
                            obj.MiddleName = dr["MiddleName"].ToString();
                            obj.MiddleName2L = dr["MiddleName2L"].ToString();
                            obj.FamilyName = dr["LastName"].ToString();
                            obj.FamilyName2L = dr["LastName2L"].ToString();
                            obj.SSN = dr["SSN"].ToString();
                            obj.MaritalStatusID = Convert.ToInt32(dr["MaritalStatusID"].ToString());
                            obj.MaritalStatus = dr["MaritalStatus"].ToString();
                            obj.NationalityID = Convert.ToInt32(dr["NationalityID"].ToString());
                            obj.Nationality = dr["Nationality"].ToString();
                            obj.ReligionID = Convert.ToInt32(dr["ReligionID"].ToString());
                            obj.Religion = dr["Religion"].ToString();
                            obj.Religion2L = dr["Religion2L"].ToString();
                            obj.DoB = Convert.ToDateTime(dr["DoB"]).ToString("dd-MMM-yyyy");
                            obj.CityID = Convert.ToInt32(dr["PermCityID"].ToString());
                            obj.City = dr["PermCity"].ToString();
                            obj.City2L = dr["PermCity2l"].ToString();
                            obj.GenderID = Convert.ToInt32(dr["GenderID"].ToString());
                            obj.Gender = dr["Gender"].ToString();
                            obj.MobileNo = dr["PermMobile"].ToString();

                            objGetMasterData.SmartDataList.Add(obj);
                        }

                    }
                    if (objGetMasterData.SmartDataList.Count > 0)
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();
                        objGetMasterData.Message = "";
                        objGetMasterData.Message2L = "";

                    }
                    else
                    {
                        objGetMasterData.Code = ProcessStatus.Success;
                        objGetMasterData.Status = ProcessStatus.Success.ToString();

                    }
                }
                return objGetMasterData;
            }
            catch (Exception ex)
            {
                objGetMasterData.Code = ProcessStatus.Fail;
                objGetMasterData.Status = ProcessStatus.Fail.ToString();
                objGetMasterData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in FetchConsultants", "");
            }
            finally
            {
                objDataHelper = null;
            }
            return objGetMasterData;
        }

        public UserData PatientInfoByYakeenService(string iqamaNumber, string dateOfBirth)
        {
            UserData userData = new UserData();
            try
            {
                
                var _url = ConfigurationManager.AppSettings["YakeenServiceURL"].ToString();
                var _action = ConfigurationManager.AppSettings["YakeenServiceAction"].ToString();

                string userName = ConfigurationManager.AppSettings["YakeenServiceUserName"].ToString();
                string password = ConfigurationManager.AppSettings["YakeenServicePassword"].ToString();
                string chargeCode = ConfigurationManager.AppSettings["YakeenServiceChargeCode"].ToString();
                string referenceNumber = ConfigurationManager.AppSettings["YakeenServiceReferenceNumber"].ToString();

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(iqamaNumber, userName, password, chargeCode, dateOfBirth, referenceNumber);

                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                //get the response from the completed web request.
                XmlDocument soapResult = new XmlDocument();


                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult.LoadXml(rd.ReadToEnd()); 

                        XmlNamespaceManager manager = new XmlNamespaceManager(soapResult.NameTable);

                        manager.AddNamespace("d", "http://someURL");

                        XmlNodeList xnList = soapResult.SelectNodes("//AlienInfoByIqamaResult", manager);
                        int nodes = xnList.Count;

                        foreach (XmlNode xn in xnList)
                        {
                            userData.Users.Add(new Models.User
                            {
                                DateOfBirth = xn.SelectSingleNode("dateOfBirthH").InnerText,
                                EnFirstName = xn.SelectSingleNode("englishFirstName").InnerText,
                                EnLastName = xn.SelectSingleNode("englishLastName").InnerText,
                                EnSecondName = xn.SelectSingleNode("englishSecondName").InnerText,
                                EnThirdName = xn.SelectSingleNode("englishThirdName").InnerText,
                                FirstName = xn.SelectSingleNode("firstName").InnerText,
                                Gender = xn.SelectSingleNode("gender").InnerText,
                                //IqamaExpiryDateG = Convert.ToDateTime(xn.SelectSingleNode("iqamaExpiryDateG").InnerText).ToString("dd-MMM-yyyy"),
                                IqamaExpiryDateG = Convert.ToDateTime(xn.SelectSingleNode("iqamaExpiryDateG").InnerText, System.Globalization.CultureInfo.GetCultureInfo("hi-IN").DateTimeFormat).ToString("dd-MMM-yyyy"),
                                LastInsuranceCompanyIdentity = xn.SelectSingleNode("lastInsuranceCompanyIdentity").InnerText,
                                LastInsurancePolicyEndGDate = xn.SelectSingleNode("lastInsurancePolicyEndGDate").InnerText,
                                LastInsurancePolicyStartGDate = xn.SelectSingleNode("lastInsurancePolicyStartGDate").InnerText,
                                LastName = xn.SelectSingleNode("lastName").InnerText,
                                LogId = xn.SelectSingleNode("logId").InnerText,
                                NationalityCode = xn.SelectSingleNode("nationalityCode").InnerText,
                                OccupationCode = xn.SelectSingleNode("occupationCode").InnerText,
                                PlaceOfBirthCode = xn.SelectSingleNode("placeOfBirthCode").InnerText,
                                SecondName = xn.SelectSingleNode("secondName").InnerText,
                                SponsorName = xn.SelectSingleNode("sponsorName").InnerText,
                                ThirdName = xn.SelectSingleNode("thirdName").InnerText
                            });
                        }
                    }
                }
                userData.Code = ProcessStatus.Success;
                userData.Status = ProcessStatus.Success.ToString();
                userData.Message = "";
                userData.Message2L = "";
            }
            catch (Exception ex)
            {
                userData.Code = ProcessStatus.Fail;
                userData.Status = ProcessStatus.Fail.ToString();
                userData.Message = ex.Message;
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in PatientInfoByYakeenService", "");
            }
            return userData;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            // webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string iqamaNumber, string userName, string password, string chargeCode, string dateOfBirth, string referenceNumber)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(
            @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:yak = ""http://yakeenforalhammadi.yakeen.elm.com/"" ><soapenv:Header><soapenv:Body><yak:getAlienInfoByIqama><AlienInfoByIqamaRequest><iqamaNumber>" + iqamaNumber + "</iqamaNumber><userName>" + userName + "</userName><password>"+ password + "</password><chargeCode>"+ chargeCode  + "</chargeCode><dateOfBirth>"+ dateOfBirth +"</dateOfBirth><referenceNumber>"+ referenceNumber +"</referenceNumber></AlienInfoByIqamaRequest></yak:getAlienInfoByIqama></soapenv:Body></soapenv:Header></soapenv:Envelope>");
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        

        public void CreateMeeting()
        {

            var scopes = new[] { "OnlineMeetings.ReadWrite" };

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = "64ec65dd-78f8-470c-802f-a9566db9d4a6";

            // Values from app registration
            var clientId = "38ce9080-225a-4fe3-8523-67de67d40f8c";
            var clientSecret = "bYR8Q~NAay~r.z5_vH_5VcfXAYc1vlzLyXISubui";

            // For authorization code flow, the user signs into the Microsoft
            // identity platform, and the browser is redirected back to your app
            // with an authorization code in the query parameters
            var authorizationCode = "AUTH_CODE_FROM_REDIRECT";

            // using Azure.Identity;
            TokenCredentialOptions options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://learn.microsoft.com/dotnet/api/azure.identity.authorizationcodecredential
            //var authCodeCredential = new AuthorizationCodeCredential(tenantId, clientId, clientSecret, authorizationCode, options);

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);


            GraphServiceClient graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            //GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            var onlineMeeting = new OnlineMeeting
            {
                StartDateTime = DateTimeOffset.Parse("2022-12-29T17:00:00.2444915+00:00"),
                EndDateTime = DateTimeOffset.Parse("2022-12-12T17:30:00.2464912+00:00"),
                Subject = "User Token Meeting"
            };

            var meeting = graphClient.Me.OnlineMeetings.Request().AddAsync(onlineMeeting);
        }


       

    }
}