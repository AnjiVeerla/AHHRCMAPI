using HISDataAccess;
using RCMAPI.AgeCalculator;
using RCMAPI.CommonUtilities;
using RCMAPI.Messages;
using RCMAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace RCMAPI.DAL
{
    public class BillSummaryDAL
    {
        static string MODULE_NAME = "WebAPIDAL";
        const int DEFAULTWORKSTATION = 0;
        static String strConnString = ConfigurationManager.ConnectionStrings["DBConnectionStringMasters"].ConnectionString;
        //private IDBManager _dbManager;
        string UHID = string.Empty;
        DataTable HISCONFIG = null;
        string hdnPatientID = string.Empty;
        bool blnCreditpinBlock = false; string ReturnMessage = string.Empty; string hdnNationalityId = string.Empty;
        string strGradeID = string.Empty;
        string strCompanyID = string.Empty; DateTime dtpCardValid; string ViewStateLetterid = string.Empty; DataTable dtPatientDetails;
        DataTable ViewStatedtPatientDetails; DataSet DsPatient = null;
        DataTable dtCompanyblock = null;
        DataSet dsPayerforLOA1678 = null; DataSet dsPayerforLOA678 = null;
        DataTable dtCompanyContract; DataTable dtDocOrders = null; DataTable gdvSearchResultData = null;
        bool hdnrblbilltypeCredit = false; bool hdnrblbilltypecash = false;
        bool hdnradordeTypeRoutine = true; bool hdnradordeTypeASAP = false; bool hdnradordeTypeStat = false;
        string DoctorsConsultations = string.Empty; string FixedConsultation = string.Empty;
        DataTable dtConfollowupM = new DataTable(); bool CheckType; int FollowupDaysM = 0; int FollowupLimitM = 0; int intFollowupID = 0; int ParentLetterId = 0;
        string Followuplimit, CONSFollowupDAYS, CRDiscountOnCash, LatestIPID, SPLFOLLOWUPLMT, SPLFOLLOWUPDAYS, Letterid;
        DataTable PredefinedDiscount;
        DataTable dtGradeValidation;
        int intTariffID = -1; string strTariffID = string.Empty; string hdnTariffID;

        string hdnblnCOmpanyBlocked = string.Empty; string hdnblnCOmpanyBlockedReason = string.Empty; string hdnblnCompanyExpired = string.Empty; string hdnblnInsuranceExpired = string.Empty;
        string CompanyReturnMessage, CompanyReturnMessage2L = string.Empty; string SessionID = string.Empty;
        string OrderTypeVisit = string.Empty; string DocAvail = string.Empty; string hdnblnYes, hdnblnNo = string.Empty;
        string OrderTypeM = string.Empty; string ddlservices = "2"; bool IsAppliedProfessionalCharge = false;
        string hdnPrice = string.Empty; string Priority = string.Empty; string DocCode = string.Empty; string hdnordertypeName = string.Empty; string hdnintBillDocEmpId = string.Empty;
        DataTable invtTable = null; string hdnScheduleID = string.Empty; int ProfChargeServiceId; string ProfChargeServiceName; DataTable DTView = null; DataTable GridContents = null;
        string Servicefilter = " and ServiceId=";
        string Validation, ValidationProfile = string.Empty; DataTable MiscDoctorEntry = null;
        string OrderVisitType = string.Empty;
        DataTable dtPayments = new DataTable();
        DataTable dtMultiPayment = new DataTable();
        string hdnAge, hdnGenderID, hdAgeUomid, hdnTitleID, hdnMarStatusID, hdnDOB, hdnPhno, hdnNationalID, hdnFamilyName, lblAddress, hdnMLC = string.Empty;

        static String strDefaultHospitalId = string.Empty;

        DataTable DTTem;
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
        public FetchPriceDataList FetchServicePrice(FetchServicePrice DocParams)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master);
            FetchPriceDataList objGetMasterData = new FetchPriceDataList();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ServiceId", DocParams.ServiceId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@ItemID", DocParams.ServiceTypeID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TariffId", DocParams.TariffId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Priority", DocParams.VisitType, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@SpecialisationId", DocParams.SpecialisationId, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@BedTypeID", DocParams.BedTypeID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UserID", DocParams.UserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WorkStationID", DocParams.WorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", null, DbType.Int32, ParameterDirection.Input));
                using (DataSet dsDocList = objDataHelper.RunSPReturnDS("pr_FetchPrice", objIDbDataParameters.ToArray()))
                {
                    if (dsDocList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDocList.Tables[0].Rows)
                        {
                            GetFetchPriceDataListOutput obj = new GetFetchPriceDataListOutput();
                            obj.ServiceId = Convert.ToInt32(dr["ServiceId"].ToString());
                            obj.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            obj.ServiceName = dr["ServiceName"].ToString();
                            if (dr["BasePrice"] != DBNull.Value)
                                obj.BasePrice = Convert.ToDecimal(dr["BasePrice"].ToString());
                            if (dr["EligiblePrice"] != DBNull.Value)
                                obj.EligiblePrice = Convert.ToDecimal(dr["EligiblePrice"].ToString());
                            if (dr["BillablePrice"] != DBNull.Value)
                                obj.BillablePrice = Convert.ToDecimal(dr["BillablePrice"].ToString());
                            obj.Procedure = dr["ItemName"].ToString();
                            obj.OrderTypeID = Convert.ToInt32(dr["OrderTypeID"].ToString());
                            obj.DeptID = Convert.ToInt32(dr["DeptID"].ToString());
                            obj.DeptName = dr["DeptName"].ToString();
                            obj.SpecialiseID = Convert.ToInt32(dr["SpecialiseID"].ToString());
                            obj.Specialisation = dr["Specialisation"].ToString();
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
        PatientBillRInfoList objPatientList = new PatientBillRInfoList();
        public PatientBillRInfoList ValidationRegCode(ValidationRegCode PatientBillList)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            PatientBillRInfoList objGetMasterData = new PatientBillRInfoList();
            try
            {
                DataSet dsSearchResults = null;
                int PatId = 0; string strSearch = string.Empty;
                string strFilterCond = string.Empty; string PatientID = string.Empty; int intprev = 0;
                DataSet DsPatientDetails = null; string hdnPackageUtil = string.Empty;
                string Letterid = string.Empty;
                string hdnIsCashBill = string.Empty;
                string hdnGradeFilterCond = string.Empty;
                bool HasDefaultLOA = false; string hdnHasDefaultLOA = string.Empty;
                string hdnIsfamilyLOA = string.Empty;
                int TagId = 0; string hdnPatientType = string.Empty; int intPatientType = 0; DataTable DtSch = null; DataTable DtSchRef;
                DataTable dtDtDoc = null; DataSet dsDtDoc = null;
                DataTable dtBankDetails = null; DataTable DtConsultationTypes;
                DataTable dtService;
                DataTable DtOtherOders; DataTable DtDocOrders; DataSet dsBankDetails = null; DataTable dtVisitDates;
                string hdnotheroders = string.Empty; string hdnspecialiseID = string.Empty;
                DataTable DtScheduleders;
                DataTable DtReferralOrders;
                DataTable DtSelectedSched;
                DataTable DtSelectedDocOrders;
                ArrayList intarray = new ArrayList(2);
                int intUserID = Convert.ToInt32(PatientBillList.UserID); int intWorkStationid = Convert.ToInt32(PatientBillList.WorkStationID);
                UHID = PatientBillList.UHID;
                strSearch = "OPBillRegCodeSearch";
                string HISConfigValues = ConfigurationSettings.AppSettings["HISConfig"].ToString();

                FetchHisConfiguation(Convert.ToInt32(PatientBillList.UserID), Convert.ToInt32(PatientBillList.WorkStationID), PatientBillList, HISConfigValues);
                PatId = GetRootPatientIdPerformance(UHID, Convert.ToInt32(PatientBillList.WorkStationID));
                strFilterCond = " patBlocked=0 " + "AND  PatientID = " + PatId + "";
                string strUHIDFilterCond = string.Empty; string SearchName = string.Empty; string PatientUHID = string.Empty;
                SearchName = "IPBillPatientSearch";
                string strAdmissionNumber = string.Empty;
                PatientUHID = UHID;
                strUHIDFilterCond = "RegCode is not null and RegCode = '" + UHID.Trim() + "' and Status in (0,2,3) and PatientType in(2,3,4) and Blocked = 0 and HospitalID='" + PatientBillList.HospitalId + "'";

                DataSet dsUhidSearchResults = CheckPatientIPorEMR(PatientBillList.UHID.Trim(), PatientBillList.HospitalId, Convert.ToInt32(PatientBillList.WorkStationID), PatientBillList);
                if (dsUhidSearchResults.Tables[0].Rows.Count > 0)
                {
                    if (dsUhidSearchResults.Tables[0].Rows[0]["PatienttypeID"].ToString() == "2")
                    {
                        objPatientList.Code = ProcessStatus.Fail;
                        objPatientList.Status = ProcessStatus.Fail.ToString();
                        objPatientList.Message = "Patient Is Currently IP";
                        return objPatientList;
                    }
                    if (dsUhidSearchResults.Tables[0].Rows[0]["PatienttypeID"].ToString() == "3")
                    {
                        objPatientList.Code = ProcessStatus.Fail;
                        objPatientList.Status = ProcessStatus.Fail.ToString();
                        objPatientList.Message = "Patient is currently EMR";
                        return objPatientList;
                    }
                }
                PatientID = PatId.ToString();
                hdnPatientID = PatId.ToString();
                string strRoleCheck = ConfigurationSettings.AppSettings["PatientOutstandingAproval"].ToString();
                if (strRoleCheck.ToUpper() == "YES")
                {
                    DataTable dtOutStand = FetchOutstandingAmount(PatId, 0, PatientBillList).Tables[0];
                    if (dtOutStand.Rows.Count > 0)
                    {
                        intprev = Convert.ToInt32(dtOutStand.Compute("sum(PBalanceReceipt)", ""));
                    }
                    if (intprev > 0)
                    {
                        objPatientList.Code = ProcessStatus.Fail;
                        objPatientList.Status = ProcessStatus.Fail.ToString();
                        objPatientList.Message = "Patient is having Outstanding Amount:" + Convert.ToDecimal(intprev.ToString()) + "";
                        return objPatientList;
                    }
                }
                //PinBLock
                if (CheckPinBlock(Convert.ToInt32(PatientID), PatientBillList) == false)
                {
                    if (blnCreditpinBlock == false)
                    {
                        objPatientList.Code = ProcessStatus.Fail;
                        objPatientList.Status = ProcessStatus.Fail.ToString();
                        objPatientList.Message = ReturnMessage;
                        objPatientList.Message2L = ReturnMessage;
                        return objPatientList;
                    }
                }

                DsPatientDetails = FetchPatientDetails(PatientID, PatientBillList.UHID.Trim(), false, Convert.ToInt32(PatientBillList.UserID), Convert.ToInt32(PatientBillList.WorkStationID), 0, 0, PatientBillList);
                if (DsPatientDetails.Tables[0].Rows.Count > 0)
                {
                    hdnNationalityId = DsPatientDetails.Tables[0].Rows[0]["NationalityID"].ToString();
                    if (DsPatientDetails.Tables[4].Rows.Count > 0)
                    {
                        if (DsPatientDetails.Tables[4].Columns.Contains("ActiveStatus") && DsPatientDetails.Tables[4].Columns.Contains("GradeBlocked"))
                        {
                            DataRow[] dr = DsPatientDetails.Tables[4].Select("ActiveStatus =1 and GradeBlocked=1");
                            if (dr.Length > 0)
                            {

                                dr[0]["ActiveStatus"] = 0;
                                DsPatientDetails.Tables[4].AcceptChanges();
                                objPatientList.Code = ProcessStatus.Fail;
                                objPatientList.Status = ProcessStatus.Fail.ToString();
                                objPatientList.Message = "This Grade has been Blocked";                               
                                return objPatientList;
                            }
                        }
                        if (DsPatientDetails.Tables[4].Columns.Contains("ActiveStatus"))
                        {
                            DataRow[] dr = DsPatientDetails.Tables[4].Select("ActiveStatus =1");
                            if (dr.Length > 0)
                            {
                                strCompanyID = dr[0]["PAYERID"].ToString();
                                strGradeID = dr[0]["GradeID"].ToString(); if (dr[0]["CardValidity"] != DBNull.Value)
                                {
                                    dtpCardValid = Convert.ToDateTime(dr[0]["CardValidity"]);
                                }
                            }
                        }
                        else
                        {
                            strCompanyID = DsPatientDetails.Tables[0].Rows[0]["Companyid"].ToString();
                            strGradeID = DsPatientDetails.Tables[0].Rows[0]["GradeID"].ToString();
                            if (DsPatientDetails.Tables[0].Columns.Contains("InsuranceCardExpiry") && DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"] != DBNull.Value)
                            {
                                dtpCardValid = Convert.ToDateTime(DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"]);
                            }
                        }
                    }
                    else
                    {
                        strCompanyID = DsPatientDetails.Tables[0].Rows[0]["Companyid"].ToString();
                        strGradeID = DsPatientDetails.Tables[0].Rows[0]["GradeID"].ToString();
                        if (DsPatientDetails.Tables[0].Columns.Contains("InsuranceCardExpiry") && DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"] != DBNull.Value)
                        {
                            dtpCardValid = Convert.ToDateTime(DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"]);
                        }
                    }
                    if (DsPatientDetails.Tables[5].Rows.Count > 0)
                    {
                        if (DsPatientDetails.Tables[5].Columns.Contains("Letterid") && DsPatientDetails.Tables[5].Rows[0]["Letterid"] != null)
                        {
                            DataRow[] dr = DsPatientDetails.Tables[5].Select("blocked=0");
                            if (dr.Length > 0)
                            {
                                ViewStateLetterid = dr[0]["Letterid"].ToString();
                            }
                        }
                    }
                }
                // package utilization need to implment ******************************************
                if (DsPatientDetails.Tables[0].Rows.Count > 0)
                {
                    #region package utilization need to implment

                    #endregion
                    dtPatientDetails = DsPatientDetails.Tables[0].Copy();
                    ViewStatedtPatientDetails = dtPatientDetails.Copy();
                }
                // CheckingDetail need to implment ******************************************
                #region CheckingDetail Calling

                // need to implement

                #endregion


                if (!string.IsNullOrEmpty(strCompanyID.Trim()))
                {
                    hdnIsCashBill = "true";
                    int HospitalID = Convert.ToInt32(PatientBillList.HospitalId);
                    DataSet dsCompanyBlock = FetchHospitalCompanyDetails(Convert.ToInt32(strCompanyID), "C", "1,6,7,8", Convert.ToInt32(PatientBillList.UserID), Convert.ToInt32(PatientBillList.WorkStationID), 0, HospitalID);
                    dtCompanyblock = dsCompanyBlock.Tables[0].Copy();
                    dsPayerforLOA1678 = null;
                    dsPayerforLOA1678 = new DataSet();
                    dsPayerforLOA1678.Tables.Add(dsCompanyBlock.Tables["table1"].Copy());

                    dsPayerforLOA1678.Tables.Add(dsCompanyBlock.Tables["table2"].Copy());
                    dsPayerforLOA1678.Tables.Add(dsCompanyBlock.Tables["table3"].Copy());
                    dsPayerforLOA1678.Tables.Add(dsCompanyBlock.Tables["table4"].Copy());

                    dsPayerforLOA1678.AcceptChanges();
                    hdnrblbilltypeCredit = true;
                    hdnrblbilltypecash = false;

                    if (dsCompanyBlock.Tables[0].Rows.Count > 0)
                    {
                        FixedConsultation = dsCompanyBlock.Tables[0].Rows[0]["FixedConsultationCharge"].ToString();
                        DoctorsConsultations = dsCompanyBlock.Tables[0].Rows[0]["DoctorConsultationsPerDay"].ToString();
                    }
                    if (DsPatientDetails.Tables[4].Rows.Count > 0)
                    {
                        if (DsPatientDetails.Tables[4].Columns.Contains("ActiveStatus"))
                        {
                            DataRow[] dr = DsPatientDetails.Tables[4].Select("ActiveStatus =1");
                            if (dr.Length > 0)
                            {
                                if (dr[0]["CardValidity"] != DBNull.Value && Convert.ToInt32(strCompanyID) > 0)
                                {
                                    dtpCardValid = Convert.ToDateTime(dr[0]["CardValidity"]);
                                }
                            }
                        }
                    }

                    if (dsPayerforLOA1678.Tables[3].Rows.Count > 0)
                    {
                        if (dsPayerforLOA1678.Tables[3].Rows[0]["Followuplimits"].ToString() != "")
                        {
                            Followuplimit = dsPayerforLOA1678.Tables[3].Rows[0]["Followuplimits"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dsPayerforLOA1678.Tables[3].Rows[0]["ApprovalDays"].ToString()))
                        {
                            CONSFollowupDAYS = dsPayerforLOA1678.Tables[3].Rows[0]["ApprovalDays"].ToString();
                        }
                    }
                    if (dsPayerforLOA1678.Tables[2].Rows.Count > 0)
                    {
                        if (dsPayerforLOA1678.Tables[2].Rows[0]["IsDiscountonCashOP"].ToString() != "")
                        {
                            CRDiscountOnCash = dsPayerforLOA1678.Tables[2].Rows[0]["IsDiscountonCashOP"].ToString();
                        }
                    }
                    DoctorsConsultations = string.Empty;
                    if (dsCompanyBlock != null && dsCompanyBlock.Tables[0].Rows.Count > 0)
                    {
                        dtCompanyContract = dsCompanyBlock.Tables[0].Copy();
                        PredefinedDiscount = dsPayerforLOA1678.Tables["table2"];
                        dtGradeValidation = dsPayerforLOA1678.Tables["table2"];
                        dtCompanyContract = dsPayerforLOA1678.Tables["table1"];

                        strTariffID = dsPayerforLOA1678.Tables["table1"].Rows[0]["TariffID"].ToString();

                        if (!string.IsNullOrEmpty(strTariffID.Trim()))
                        {
                            hdnTariffID = strTariffID;
                        }
                        else
                            strTariffID = "-1";

                        if (dsPayerforLOA1678.Tables["table2"].Rows.Count > 0)
                        {
                            string strGrades = "0";
                            strGrades = "";
                            int intprevId = 0;
                            DataRow[] dr1 = dsPayerforLOA1678.Tables["table2"].Select("PatientType=1", " GradeId Asc");
                            for (int ictr = 0; ictr < dr1.Length; ictr++)
                            {
                                if (intprevId != Convert.ToInt32(dr1[ictr]["GradeID"]))
                                {
                                    strGrades = strGrades + dr1[ictr]["GradeID"] + ",";
                                    intprevId = Convert.ToInt32(dr1[ictr]["GradeID"]);
                                }
                            }
                            if (strGrades.Length > 0)
                                strGrades = strGrades.Substring(0, strGrades.Length - 1);
                            else
                                strGrades = "0";

                            hdnGradeFilterCond = " GradeID in (" + strGrades + ") and Status = 0 and blocked=0 ";
                        }
                        else
                        {

                        }
                    }
                    if (dsPayerforLOA1678.Tables["table4"].Rows.Count > 0)
                    {
                        HasDefaultLOA = false;
                        HasDefaultLOA = Convert.ToBoolean(dsPayerforLOA1678.Tables["table4"].Rows[0]["IsDefaultLOA"]);
                        if (HasDefaultLOA)
                            hdnHasDefaultLOA = "true";
                        else
                            hdnHasDefaultLOA = "false";
                        if (dsPayerforLOA1678.Tables["table4"].Columns.Contains("IsFamilyLOA"))
                        {
                            if (dsPayerforLOA1678.Tables["table4"].Rows[0]["IsFamilyLOA"] != DBNull.Value)
                                hdnIsfamilyLOA = dsPayerforLOA1678.Tables[3].Rows[0]["IsFamilyLOA"].ToString();
                        }
                    }
                    DataRow[] drrblock = dtCompanyblock.Select("Blocked=0 and ISCompanyExpired=0 and ISInsuranceExpired=0");
                    if (drrblock.Length > 0)
                    {

                    }
                    else
                    {
                        drrblock = dtCompanyblock.Select();
                        string blockedMessage = string.Empty; string blockedMessage2L = string.Empty;
                        if (Convert.ToString(drrblock[0]["Blocked"]) == "1")
                        {
                            hdnblnCOmpanyBlocked = "true";
                            blockedMessage = "Company assigned to the Patient is Blocked." + "< br/>";
                            blockedMessage2L = "Company assigned to the Patient is Blocked." + "< br/>";
                            if (dtCompanyblock.Rows.Count > 0 && dtCompanyblock.Rows[0]["BlockedReason"] != DBNull.Value)
                            {
                                hdnblnCOmpanyBlockedReason = dtCompanyblock.Rows[0]["BlockedReason"].ToString();
                                blockedMessage = blockedMessage + " Reason: " + dtCompanyblock.Rows[0]["BlockedReason"].ToString();
                                blockedMessage2L = blockedMessage2L + " Reason: " + dtCompanyblock.Rows[0]["BlockedReason"].ToString();
                            }
                        }
                        else if (Convert.ToString(drrblock[0]["ISCompanyExpired"]) == "1")
                        {
                            hdnblnCompanyExpired = "true";
                            hdnblnCOmpanyBlockedReason = "";
                            blockedMessage = "Company assigned to the Patient is Expired.";
                            blockedMessage2L = "Insurance associated with Patient Company is Expired.";
                            hdnblnCOmpanyBlockedReason = "Company assigned to the Patient is Expired.";
                        }
                        else if (Convert.ToString(drrblock[0]["ISInsuranceExpired"]) == "1")
                        {
                            hdnblnInsuranceExpired = "true";
                            hdnblnCOmpanyBlockedReason = "";
                            blockedMessage = "Insurance associated with Patient Company is Expired.";
                            blockedMessage2L = "Insurance associated with Patient Company is Expired.";
                            hdnblnCOmpanyBlockedReason = "Insurance associated with Patient Company is Expired.";
                        }
                        objPatientList.Code = ProcessStatus.Fail;
                        objPatientList.Status = ProcessStatus.Fail.ToString();
                        objPatientList.Message = blockedMessage;
                        objPatientList.Message2L = blockedMessage2L;
                        return objPatientList;

                    }
                    if (!string.IsNullOrEmpty(strGradeID.Trim()))
                    {
                        if (dsPayerforLOA1678.Tables["table2"].Columns.Contains("GradeId"))
                        {
                            DataRow[] drGradeActive = dsPayerforLOA1678.Tables["table2"].Select("GradeId=" + Convert.ToInt32(strGradeID));
                            if (drGradeActive.Length == 0)
                            {
                                if (Convert.ToInt32(strGradeID) > 0)
                                {
                                    objPatientList.Code = ProcessStatus.Fail;
                                    objPatientList.Status = ProcessStatus.Fail.ToString();
                                    objPatientList.Message = "This Grade has been Blocked";                                   
                                    return objPatientList;
                                }
                            }
                        }
                    }
                    else
                    {
                        strGradeID = "0";
                    }
                    dtCompanyContract = dsPayerforLOA1678.Tables["table1"];
                    if (hdnrblbilltypeCredit == true && ValidateExpiryDate(dtCompanyContract, "ValidTo") == false)
                    {
                        objPatientList.Code = ProcessStatus.Success;
                        objPatientList.Status = ProcessStatus.Success.ToString();
                        objPatientList.Message = "Company Contract Date has been Expired";                     
                        return objPatientList;
                    }

                    if (DsPatientDetails.Tables[0].Rows.Count > 0)
                    {
                        if (DsPatientDetails.Tables[0].Columns.Contains("InsuranceCardExpiry") && DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"] != DBNull.Value && Convert.ToInt32(strCompanyID) > 0)
                        {
                            dtpCardValid = Convert.ToDateTime(DsPatientDetails.Tables[0].Rows[0]["InsuranceCardExpiry"]);
                            if ((Convert.ToInt32(strCompanyID) > 0) || (Convert.ToInt32(strGradeID) > 0))
                            {
                                if (Convert.ToInt32(strCompanyID) > 0 && dtpCardValid < System.DateTime.Today.Date)
                                {
                                    objPatientList.Code = ProcessStatus.Success;
                                    objPatientList.Status = ProcessStatus.Success.ToString();
                                    objPatientList.Message = "Insurance Card Expired";                                   
                                    return objPatientList;
                                }
                            }
                        }
                    }
                }
                else
                {
                    hdnIsCashBill = "false";
                    strCompanyID = "0";
                    strTariffID = "-1";
                    hdnrblbilltypeCredit = false;
                    hdnrblbilltypecash = true;
                    objPatientList.Code = ProcessStatus.Success;
                    objPatientList.Status = ProcessStatus.Success.ToString();
                    objPatientList.Message = ReturnMessage;                 
                    return objPatientList;
                }
                if (hdnPatientType != null && (!string.IsNullOrEmpty(hdnPatientType.Trim())) && hdnPatientType != "0")
                {
                    intPatientType = Convert.ToInt32(hdnPatientType);
                }
                else
                    intPatientType = 1;



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
        private void FetchHisConfiguation(int intUserID, int intWorkStationID, ValidationRegCode PatientBillList, string HISConfigValues)
        {
            try
            {
                DataTable dtSpecCofig = GetSpecializationConfig(HISConfigValues, 0, intUserID, intWorkStationID, 0, PatientBillList).Tables[0];
                HISCONFIG = dtSpecCofig.Copy();
                HISCONFIG.AcceptChanges();
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in GetSpecConfig", "");

            }
        }
        public DataSet GetSpecializationConfig(string HISConfigValues, int intResult, int intUserID, int intWorkstationId, int intError, ValidationRegCode PatientBillList)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            DataSet dsSpecConfig = new DataSet("Specialization Configuratin");
            try
            {

                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Type", HISConfigValues.ToString(), DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Hospitalid", PatientBillList.HospitalId, DbType.Int32, ParameterDirection.Input));
                dsSpecConfig = objDataHelper.RunSPReturnDS("Pr_HISConfiguration_MAPI", objIDbDataParameters.ToArray());
                return dsSpecConfig;
            }
            finally
            {

            }
        }
        public int GetRootPatientIdPerformance(string UHID, int iWStationId)
        {
            DataHelper objDataHelper = new DataHelper(iWStationId, (int)Database.Master);
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@RegCode", UHID, DbType.String, ParameterDirection.Input));
                using (DataSet ds = objDataHelper.RunSPReturnDS("PR_FetchPatientRoot", objIDbDataParameters.ToArray()))
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if ((Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]) == Convert.ToInt32(ds.Tables[0].Rows[0]["RootPatientID"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["RootPatientID"])) || ds.Tables[0].Rows[0]["RootPatientID"] == DBNull.Value)
                        { return Convert.ToInt32(ds.Tables[0].Rows[0]["PatientID"]); }
                        else if (Convert.ToInt32(ds.Tables[0].Rows[0]["PatientId"]) != Convert.ToInt32(ds.Tables[0].Rows[0]["RootPatientID"]))
                        { return Convert.ToInt32(ds.Tables[0].Rows[0]["RootPatientID"]); }
                    }
                }
            }
            finally
            {

            }
            return 0;
        }
        public DataSet CheckPatientIPorEMR(string UHID, int HospitalID, int intWorkstationId, ValidationRegCode PatientBillList)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            DataSet dsSpecConfig = new DataSet("ChkPatient");
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@UHID", UHID.ToString(), DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Hospitalid", HospitalID, DbType.Int32, ParameterDirection.Input));
                dsSpecConfig = objDataHelper.RunSPReturnDS("Pr_CheckPatientIPorEMR_MAPI", objIDbDataParameters.ToArray());
                return dsSpecConfig;
            }
            finally
            {

            }
        }
        public DataSet FetchOutstandingAmount(int intPatientID, int intPatientType, ValidationRegCode PatientBillList)
        {
            DataSet ds = new DataSet();
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            DataTable DtGetOutsAmt = new DataTable("GetOutsAmt");
            try
            {
                DataColumn dcMasters = DtGetOutsAmt.Columns.Add("ParamName", typeof(string));
                DtGetOutsAmt.Columns.Add("ParamValue", typeof(string));
                DtGetOutsAmt.Columns.Add("ParamDBtype", typeof(DbType));
                DtGetOutsAmt.Columns.Add("ParamDirection", typeof(Int32));
                DtGetOutsAmt.Rows.Add(new object[] { "@PatientID", intPatientID, DbType.Int32, 1 });
                DtGetOutsAmt.Rows.Add(new object[] { "@PatientType", intPatientType, DbType.Int32, 1 });
                int icnt = 0;
                IDbDataParameter[] IPrm = new IDbDataParameter[DtGetOutsAmt.Rows.Count];
                foreach (DataRow oDr in DtGetOutsAmt.Rows)
                {
                    IPrm[icnt] = objDataHelper.CreateDataParameter();
                    IPrm[icnt].ParameterName = oDr["ParamName"].ToString();
                    IPrm[icnt].Value = oDr["ParamValue"].ToString();
                    IPrm[icnt].DbType = (DbType)oDr["ParamDBtype"];
                    IPrm[icnt].Direction = (ParameterDirection)oDr["ParamDirection"];
                    icnt++;
                }
                ds = objDataHelper.RunSPReturnDS("pr_fetchbilldata_Perf", IPrm);
                return ds;
            }
            finally
            {

            }
        }
        private bool CheckPinBlock(int PatId, ValidationRegCode PatientBillList)
        {
            try
            {
                string str = "PatientID = " + PatId + " and Status = 0";
                DataSet dtPinBlock = FetchPinBlockMAPI(PatId, Convert.ToInt32(PatientBillList.HospitalId), Convert.ToInt32(PatientBillList.WorkStationID), PatientBillList);
                if (dtPinBlock.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strInternal;
                    DataRow[] dr = dtPinBlock.Tables[0].Select("", "EffectiveDate Desc");
                    if (dr.Length > 0)

                    {
                        if ((dr[0]["Blocktype"].ToString()) == "0")
                        {
                            strInternal = new StringBuilder();
                            strInternal.Append("UHID is Blocked.</br>");
                            strInternal.Append("Reason :" + dr[0]["BlockReason"].ToString() + "</br>");
                            strInternal.Append("Block Message :" + dr[0]["Discription"].ToString() + "");
                            blnCreditpinBlock = false;
                            if (dr[0]["blocktype"].ToString() == "2")
                                blnCreditpinBlock = true;
                            ReturnMessage = strInternal.ToString();
                            return false;
                        }
                        else if ((dr[0]["Blocktype"].ToString()) == "1")
                        {
                            strInternal = new StringBuilder();
                            strInternal.Append("UHID is Credit Blocked.</br>");
                            strInternal.Append("Reason :" + dr[0]["BlockReason"].ToString() + "</br>");
                            strInternal.Append("Block Message :" + dr[0]["Discription"].ToString() + "");
                            blnCreditpinBlock = true;
                            ReturnMessage = strInternal.ToString();
                            return false;
                        }
                        else if ((dr[0]["Blocktype"].ToString()) == "2")
                        {
                            strInternal = new StringBuilder();
                            strInternal.Append("UHID is Blocked.</br>");
                            strInternal.Append("Reason :" + dr[0]["BlockReason"].ToString() + "</br>");
                            strInternal.Append("Block Message :" + dr[0]["Discription"].ToString() + "");
                            blnCreditpinBlock = true;
                            ReturnMessage = strInternal.ToString();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in CheckPinBlock", "");
                return false;
            }
            finally
            {

            }
        }
        public DataSet FetchPinBlockMAPI(int PatientID, int HospitalID, int intWorkstationId, ValidationRegCode PatientBillList)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            DataSet dsSpecConfig = new DataSet();
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientID", PatientID.ToString(), DbType.Int32, ParameterDirection.Input));
                dsSpecConfig = objDataHelper.RunSPReturnDS("Pr_FetchPinBlock_MAPI", objIDbDataParameters.ToArray());
                return dsSpecConfig;
            }
            finally
            {

            }
        }
        public DataSet FetchPatientDetails(string strPatId, string strRegCode, bool BlnDeleted, int intUserId, int intWorkStnId, int intError, byte IsReg, ValidationRegCode PatientBillList)
        {
            DataSet ds = new DataSet();
            DataTable dtPatientTable = new DataTable("Patient");
            DataHelper objDataHelper = new DataHelper(intWorkStnId, (int)Database.Master, PatientBillList.HospitalId);
            try
            {
                DataColumn dcMasters = dtPatientTable.Columns.Add("ParamName", typeof(string));
                dtPatientTable.Columns.Add("ParamValue", typeof(string));
                dtPatientTable.Columns.Add("ParamDBtype", typeof(DbType));
                dtPatientTable.Columns.Add("ParamDirection", typeof(Int32));
                if (IsReg == 0 || IsReg == 1)
                {
                    if (strPatId.Length != 0)
                    { dtPatientTable.Rows.Add(new object[] { "@PatientID", strPatId, DbType.String, 1 }); }
                    else
                    { dtPatientTable.Rows.Add(new object[] { "@PatientID", DBNull.Value, DbType.Int32, 1 }); }
                    if (IsReg == 0)
                    {
                        dtPatientTable.Rows.Add(new object[] { "@RegCode", strRegCode, DbType.StringFixedLength, 1 });
                    }
                }
                else if (IsReg == 2)
                {
                    dtPatientTable.Rows.Add(new object[] { "@IPID", strPatId == "0" ? null : strPatId, DbType.Int32, 1 });
                    dtPatientTable.Rows.Add(new object[] { "@RegCode", strRegCode, DbType.StringFixedLength, 1 });
                }
                dtPatientTable.Rows.Add(new object[] { "@TBL", null, DbType.StringFixedLength, 1 });
                dtPatientTable.Rows.Add(new object[] { "@Deleted", BlnDeleted, DbType.Boolean, 1 });
                dtPatientTable.Rows.Add(new object[] { "@UserId", intUserId, DbType.Int32, 1 });
                dtPatientTable.Rows.Add(new object[] { "@WORKSTATIONID", intWorkStnId, DbType.Int32, 1 });
                dtPatientTable.Rows.Add(new object[] { "@Error", intError, DbType.Int32, 2 });
                int icnt = 0;
                IDbDataParameter[] IPrm = new IDbDataParameter[dtPatientTable.Rows.Count];
                foreach (DataRow oDr in dtPatientTable.Rows)
                {
                    IPrm[icnt] = objDataHelper.CreateDataParameter();
                    IPrm[icnt].ParameterName = oDr["ParamName"].ToString();
                    IPrm[icnt].Value = oDr["ParamValue"];
                    IPrm[icnt].DbType = (DbType)oDr["ParamDBtype"];
                    IPrm[icnt].Direction = (ParameterDirection)oDr["ParamDirection"];
                    if ((int)oDr["ParamDirection"] == 1)
                    { IPrm[icnt].Direction = ParameterDirection.Input; }
                    else if ((int)oDr["ParamDirection"] == 2)
                    { IPrm[icnt].Direction = ParameterDirection.InputOutput; }
                    icnt++;
                }
                if (IsReg == 0) //Registration
                { ds = objDataHelper.RunSPReturnDS("Pr_FetchPatients", IPrm); }
                else if (IsReg == 1) //PreRegistration
                { ds = objDataHelper.RunSPReturnDS("Pr_FetchPrePatients", IPrm); }
                else if (IsReg == 2) //Daycare/Emergency patients
                { ds = objDataHelper.RunSPReturnDS("Pr_FetchInPatient", IPrm); }
                ds.Tables[0].TableName = "Patient";
                ds.Tables[1].TableName = "OtherAllergies";
                ds.Tables[2].TableName = "FoodAllergies";
                ds.Tables[3].TableName = "DrugAllergies";
                return ds;
            }
            finally
            {
                ds.Dispose();
                dtPatientTable.Dispose();
                objDataHelper = null;
            }
        }
        public DataSet FetchHospitalCompanyDetails(int CompanyID, string CompanyType, string Tables, int intUserID, int intWorkStationID, int intError, int HospitalID)
        {
            DataHelper objDataHelper = new DataHelper(intWorkStationID, (int)Database.Master, HospitalID);
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyID", CompanyID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Companyname", "", DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CompanyType", CompanyType, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@TBL", Tables, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@USERID", intUserID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WORKSTATIONID", intWorkStationID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", intError, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@HospitalID", HospitalID, DbType.Int32, ParameterDirection.Input)); 
                return objDataHelper.RunSPReturnDS("Pr_FetchHospitalCompany", objIDbDataParameters.ToArray());

            }
            finally
            {
                objDataHelper = null;
            }
        }
        private bool ValidateExpiryDate(DataTable dtValidate, string strColumnName)
        {
            try
            {
                DataRow[] drValidate = dtValidate.Select();
                if (drValidate.Length == 0)
                { return true; }
                else
                {
                    DateTime dtExpiryDate = Convert.ToDateTime(drValidate[0][strColumnName]);
                    if ((DateTime.Compare(DateTime.Today, dtExpiryDate)) == 1)
                    { return false; }
                    else
                    { return true; }
                }
            }
            catch (Exception ex)
            {
                HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(ex, MODULE_NAME, "Error in ValidateExpiryDate", "");
                return false;
            }
        }

        public PatientBillSummaryDataList PatientBillSummary(PatientBillList PatientBillList)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, PatientBillList.HospitalId);
            PatientBillSummaryDataList objGetMasterData = new PatientBillSummaryDataList();
            
            int intUserID = Convert.ToInt32(PatientBillList.UserID); int intWorkStationid = Convert.ToInt32(PatientBillList.WorkStationID);
            strDefaultHospitalId = PatientBillList.HospitalId.ToString();

            decimal decERExtraLoaLimit = 0;
            DataTable dtEmpty = null; DataTable dtpatient = null; DataTable DtOPServices = null;
            DataTable DtCashBillItems = null; DataTable DtBillsummary = null;
            DataTable DtCreditBillItems = null; DataTable dtTempOut = null;
            DataTable dtCompanyCreditContribution = null; DataTable dtOrders = null;
            DataTable dtTestProfile = null; DataRow[] rowarray = null;
            DataTable dtPinBlock = null;

            try
            {
                bool blnValidateQty;
                string strCreditMsg = string.Empty;
                string strCashMsg = string.Empty;

                //DataTable DTTem;
                if (PatientBillList.ServiceOrderXML.ToArray().Length > 0)
                {
                    DTTem = Utilities.ToDataSetFromArrayOfObject(PatientBillList.ServiceOrderXML.ToArray()).Tables[0];                   
                }
                if (PatientBillList.rblbilltypeCredit == true)
                {
                    string strCreditBlock = "Patientid=" + Convert.ToInt32(PatientBillList.PatientID) + " and status =0";
                    dtPinBlock = GetPINBlockDetail(PatientBillList.PatientID, Convert.ToInt32(intUserID), Convert.ToInt32(intWorkStationid), 0, 775, -2, "Fetch PIN Block", Convert.ToInt32(PatientBillList.HospitalId)).Tables[0].Copy();
                   
                    if (dtPinBlock.Rows.Count > 0)
                    {
                        if ((dtPinBlock.Rows[0]["Blocktype"].ToString()) == "0")
                        {
                            objGetMasterData.Code = ProcessStatus.Success;
                            objGetMasterData.Status = ProcessStatus.Success.ToString();
                            objGetMasterData.Message = Resources.English.ResourceManager.GetString("Creditbillnotpossibleforpatient");
                            objGetMasterData.Message2L = Resources.Arabic.ResourceManager.GetString("Creditbillnotpossibleforpatient");
                            return objGetMasterData;
                        }

                        if ((dtPinBlock.Rows[0]["Blocktype"].ToString()) == "1")
                        {
                            if (DTTem != null)
                            {
                                DataTable dt = DTTem.Copy();
                                dt.AcceptChanges();
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow drview in dt.Rows)
                                    {
                                        if (Convert.ToString(drview["serviceid"]) == "2")
                                        {
                                            foreach (DataRow dr in dtPinBlock.Rows)
                                            {
                                                if (Convert.ToString(drview["SpecialiseID"]) == Convert.ToString(dr["SpecialiseID"].ToString()))
                                                {
                                                    StringBuilder strInternal = new StringBuilder();
                                                    strInternal.Append("Credit bill not possible for  " + dr["SpecialiseName"] + " Specialistion </br>");
                                                    strInternal.Append("Reason :" + dr["BlockReason"].ToString() + "</br>");
                                                    strInternal.Append("Block Message :" + dr["Discription"].ToString() + "");
                                                    objPatientList.Code = ProcessStatus.Success;
                                                    objPatientList.Status = ProcessStatus.Success.ToString();
                                                    objPatientList.Message = strInternal.ToString();
                                                    objPatientList.Message2L = strInternal.ToString();
                                                    return objGetMasterData;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
       
        public DataSet GetPINBlockDetail(string PatientID, int intUserId, int intWorkStationId, int intError, int intFeatureId, int intFunctionID, string strCallContext, int HospitalId)
        {
            DataHelper objDataHelper = new DataHelper(DEFAULTWORKSTATION, (int)Database.Master, HospitalId);
            try
            {
                List<IDbDataParameter> objIDbDataParameters = new List<IDbDataParameter>();
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@PatientID", PatientID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Tbl", 1, DbType.String, ParameterDirection.Input));            
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FeatureId", intFeatureId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@FunctionId", intFunctionID, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@CallContext", strCallContext, DbType.String, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@USERID", intUserId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@WORKSTATIONID", intWorkStationId, DbType.Int32, ParameterDirection.Input));
                objIDbDataParameters.Add(CreateParam(objDataHelper, "@Error", intError, DbType.Int32, ParameterDirection.Input));
                DataSet dsTemp = objDataHelper.RunSPReturnDS("Pr_FetchPinBlock_Patient", objIDbDataParameters.ToArray());
                return dsTemp;
            }
            finally
            {
                objDataHelper = null;
            }
        }

    }
}