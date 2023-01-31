using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Http;

namespace RCMAPI.CommonUtilities
{
    public class SendSMS
    {
        //        SET @strURL = 'http://www.strust.com/httpSmsProvider.aspx?username=ahhsw&password=ah123321&mobile=' + @MobileNo + '&unicode=E&message=' + @msgText + '&sender=ALHAMMADI'
        //http://www.strust.com/httpSmsProvider.aspx?username=ahhsw&password=ah123321&mobile=966500984269&unicode=E&message=TestMessage&sender=ALHAMMADI


        public static string SendOTP(string mobileno, string message, bool IsArabic)
        {
            using (var web = new System.Net.WebClient())
            {
                try
                {
                    string Mobileurl = ConfigurationManager.AppSettings["MobileURL"].ToString(); //"http://www.strust.com/httpSmsProvider.aspx?"
                    string userName = ConfigurationManager.AppSettings["MobileUserUser"].ToString();// "ahhsw";
                    string userPassword = ConfigurationManager.AppSettings["MobilePassword"].ToString();// "ah123321";
                    string sender = ConfigurationManager.AppSettings["MobileSender"].ToString();// "ALHAMMADI";//ALHAMMADI
                    string ISDCode = ConfigurationManager.AppSettings["ISDCode"].ToString();// "ALHAMMADI";//ALHAMMADI
                    string msgRecepient = mobileno.StartsWith("0") ? (ISDCode + mobileno.Remove(0, 1)) : (ISDCode + mobileno);
                    string msgText = message;
                    string url = string.Empty;

                    if (IsArabic)
                    {
                        url = Mobileurl +
                       "user=" + userName +
                       "&pwd=" + userPassword +
                       "&mobileno=" + msgRecepient +
                       "&msgtext=" + msgText +
                       "&senderid=" + sender +
                       "&priority=High" +
                       "&CountryCode=ALL";
                    }
                    else
                    {
                        url = Mobileurl +
                               "user=" + userName +
                               "&pwd=" + userPassword +
                               "&mobileno=" + msgRecepient +
                               "&msgtext=" + System.Web.HttpUtility.UrlEncode(msgText, System.Text.Encoding.GetEncoding("ISO-8859-1")) +//msgText+// + 
                               "&senderid=" + sender +
                               "&priority=High" +
                               "&CountryCode=ALL";
                    }
                    //HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(null, "SendOTP", "URL ==> " + url, "");
                    Stream data = web.OpenRead(url);
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                    //HIS.TOOLS.Logger.ErrorLog.ErrorRoutine(null, "SendOTP", "Response ==> " + s, "");
                    if (s.Contains("Send Successful"))
                    {
                        return "SMSSent";
                    }
                    else
                    {
                        return "SMSNotSent";
                    }
                }
                catch (Exception ex)
                {
                    return "SMSSendingCodeError";

                }
            }
        }
    }
}