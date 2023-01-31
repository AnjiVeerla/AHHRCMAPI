using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RCM.PortalAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RCMAPI.DBManagers
{
    /// <summary>
    /// This class implementes the generic database operations.
    /// </summary>
    public class DBManager : IDBManager
    {
        private readonly IConfiguration _config;

        private RCMDBAPortalContext _dbContext;
        public int intConnectionTimeOut = 30;
        public DBManager(RCMDBAPortalContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }


        public SqlParameter CreateParameter(DbType parameterType, string name, object value, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = parameterType,
                ParameterName = name,
                Value = value,
                Direction = direction
            };
        }

        public SqlParameter CreateDbDataParameter(string name, DbType parameterType, ParameterDirection direction, object value)
        {
            return new SqlParameter
            {
                DbType = parameterType,
                ParameterName = name,
                Value = value,
                Direction = direction
            };
        }

        public SqlParameter CreateParameterWithSize(DbType parameterType, string name, object value, ParameterDirection direction,int Size)
        {
            return new SqlParameter
            {
                DbType = parameterType,
                ParameterName = name,
                Value = value,
                Direction = direction,
                Size = Size,
            };
        }
        /// <summary>
        /// This method retrieves the data from database through stored procedure with parameters.
        /// </summary>
        /// <param name="storedprocedureName">Name of Stored Procedure</param>
        /// <param name="paramList">List of input parameters.</param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSet(string storedprocedureName, List<SqlParameter> paramList)
        {
            SqlDataAdapter adp;
            DataSet ds;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = intConnectionTimeOut;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedprocedureName;
            
            foreach (SqlParameter param in paramList)
            {
                command.Parameters.AddWithValue(param.ParameterName, param.Value);
            }
            adp = new SqlDataAdapter(command);
            ds = new DataSet();
                adp.Fill(ds);

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return ds;
        }

        /// <summary>
        /// This method retrieves the data from database through stored procedure which does not have any parameter.
        /// </summary>
        /// <param name="storedprocedureName">Name of Stored Procedure</param>
        /// <param name="paramList">List of input parameters.</param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSet(string storedprocedureName)
        {
            SqlDataAdapter adp;
            DataSet ds;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = intConnectionTimeOut;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = storedprocedureName;
            adp = new SqlDataAdapter(command);
            ds = new DataSet();
            adp.Fill(ds);
            return ds;
        }

        /// <summary>
        /// This method retrieves the data from database through raw select staement.
        /// </summary>
        /// <param name="strRawSQL">Select statement</param>
        /// <returns></returns>
        public async Task<DataSet> GetDataSetByRawSQL(string strRawSQL)
        {
            SqlDataAdapter adp;
            DataSet ds;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = intConnectionTimeOut;
            command.CommandType = CommandType.Text;
            command.CommandText = strRawSQL;
            adp = new SqlDataAdapter(command);
            ds = new DataSet();
            adp.Fill(ds);
            return ds;
        }

        public async Task<int> SaveData(string procName, List<SqlParameter> paramList)
        {
            // Declarations.	
            int noOfRecordsAffected = 0;
            int intConnectionTimeOut = 0;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(procName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = intConnectionTimeOut;
                foreach (SqlParameter param in paramList)
                {
                    command.Parameters.AddWithValue(param.ParameterName, param.Value);
                    //command.Parameters.Add(param);
                }
                noOfRecordsAffected = await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return noOfRecordsAffected;
        }

        public async Task<int> SaveDataWithReturnVal(string procName, List<SqlParameter> paramList)
        {
            // Declarations.	
            int spReturnVal = 0;
            int intConnectionTimeOut = 0; 
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(procName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = intConnectionTimeOut;
                SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                foreach (SqlParameter param in paramList)
                {
                    //command.Parameters.AddWithValue(param.ParameterName, param.Value);
                    command.Parameters.Add(param);
                }
                await command.ExecuteNonQueryAsync();

                spReturnVal = (int)returnParameter.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return spReturnVal;
        }

        public async Task<int> RunSQL(string sqlText, List<SqlParameter> paramList)
        {
            // Declarations.	
            int spReturnVal = 0;
            int intConnectionTimeOut = 30;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(sqlText, connection);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = intConnectionTimeOut;
                SqlParameter returnParameter = command.Parameters.Add("RetVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                foreach (SqlParameter param in paramList)
                {
                    //command.Parameters.AddWithValue(param.ParameterName, param.Value);
                    command.Parameters.Add(param);
                }
                await command.ExecuteNonQueryAsync();

                spReturnVal = (int)returnParameter.Value;
                command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return spReturnVal;
        }


        public async Task<List<object>> ExecuteWithOutput(string procName, List<SqlParameter> paramList)
        {
            var result = _config["DBConnectionTimeout"].ToString();
            List<object> returnObj = new List<object>();
            int intConnectionTimeOut = 30;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(procName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = intConnectionTimeOut;
                List<SqlParameter> returnParameter = new List<SqlParameter>();
                foreach (var item in paramList)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        returnParameter.Add(item);
                    }
                }
                foreach (SqlParameter param in paramList)
                {
                    command.Parameters.Add(param);
                }
                await command.ExecuteNonQueryAsync();
                foreach (var item in returnParameter)
                {
                    if (item.Value != (object)DBNull.Value)
                    {
                        returnObj.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return returnObj;
        }
        /// <summary>
        /// This method retrieves the data from database through raw select staement.
        /// </summary>
        /// <param name="strRawSQL">Select statement</param>
        /// <returns></returns>
        public async Task<string> GetStringByRawSQL(string strRawSQL)
        {

            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
         
            //using (SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection())
            //{
            SqlCommand command = connection.CreateCommand();
                command.CommandTimeout = intConnectionTimeOut;
                command.CommandType = CommandType.Text;
                command.CommandText = strRawSQL;
                connection.Open();
                object o = command.ExecuteScalar();
                if (o != null)
                {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return o.ToString();
                }
           // }



            return string.Empty;
        }

        /// <summary>
        /// This property is set to connection time out period for command object. 
        /// By default it is 30. 
        /// </summary>
        public int ConnectionTimeOut
        {
            get { return intConnectionTimeOut; }
            set { intConnectionTimeOut = (int)value; }
        }


        public async Task<DataSet> GetDataSetByRawSqlWithParam(string rawSql, List<SqlParameter> paramList)
        {
            SqlDataAdapter adp;
            DataSet ds;
            SqlConnection connection = (SqlConnection)_dbContext.Database.GetDbConnection();
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = intConnectionTimeOut;
            command.CommandType = CommandType.Text;
            command.CommandText = rawSql;

            foreach (SqlParameter param in paramList)
            {
                command.Parameters.AddWithValue(param.ParameterName, param.Value);
            }
            adp = new SqlDataAdapter(command);
            ds = new DataSet();
            adp.Fill(ds);

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return ds;
        }


    }
}
