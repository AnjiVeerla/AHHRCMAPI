using System;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace RCMAPI.Messages
{
    public static class Utilities
    {
        public const string DefaultErrorMessage = "Request can't be processed this time, plese try again later.";
        public const string NoRecordsFound = "No Records found";
        public const string SucMessage = "Success";
        public const string FailMessage = "Success";

        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public static DataSet ToDataSetFromArrayOfObject(this object[] arrCollection)
        {
            DataSet ds = new DataSet();
            try
            {
                XmlSerializer serializer = new XmlSerializer(arrCollection.GetType());
                System.IO.StringWriter sw = new System.IO.StringWriter();
                serializer.Serialize(sw, arrCollection);
                System.IO.StringReader reader = new System.IO.StringReader(sw.ToString());
                ds.ReadXml(reader);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error While Converting Array of Object to Dataset."));
            }
            return ds;
        }
        public static string ConvertDTToXML(String RootTableName, string TableName, DataTable dt)
        {
            StringBuilder strXML = new StringBuilder();

            if (dt == null)
            {
                return strXML.ToString();
            }

            if ((RootTableName.Length == 0) || (TableName.Length == 0) || (dt.Rows.Count == 0))
            {
                return strXML.ToString();
            }

            strXML.Append("<" + RootTableName.ToUpper() + "> ");

            foreach (DataRow dr in dt.Rows)
            {
                strXML.Append(" <" + TableName.ToUpper() + " ");
                for (int intCol = 0; intCol < dt.Columns.Count; intCol++)
                {
                    if (dr[intCol] is System.DBNull)
                        //if collumn value is null no column information will be available in xml string 
                        strXML.Append(" ");
                    else
                    {                       
                        strXML.Append(dt.Columns[intCol].ColumnName.ToUpper());
                        strXML.Append(@"=""");
                        if (dt.Columns[intCol].DataType.ToString() == "System.Boolean")
                        {                           
                            if (dr[intCol].ToString() == "False")
                                strXML.Append("0" + @""" ");
                            else
                                strXML.Append("1" + @""" ");
                        }
                        else if (dt.Columns[intCol].DataType.ToString() == "System.String")
                        {                            
                            StringBuilder strtoconvert = new StringBuilder();
                            strtoconvert.Append(dr[intCol].ToString());
                            strtoconvert.Replace(@"&", "&amp;");
                            strtoconvert.Replace(@"""", "&quot;");
                            strtoconvert.Replace(@"<", "&lt;");
                            strtoconvert.Replace(@">", "&gt;");

                            strXML.Append(strtoconvert.ToString() + @""" ");

                        }
                        else
                            strXML.Append(dr[intCol] + @""" ");

                    }                    
                }
                strXML.Append("/> ");
            }
            strXML.Append("</" + RootTableName.ToUpper() + ">");
            return strXML.ToString();
        }
    }
}