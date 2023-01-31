using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace RCMAPI.DBManagers
{
	/// <summary>
	/// This interface defines the generic database operations.
	/// </summary>
	public interface IDBManager
	{
        int ConnectionTimeOut { get; set; }

        Task<DataSet> GetDataSet(string storedprocedureName, List<SqlParameter> param);
		Task<DataSet> GetDataSet(string storedprocedureName);
		Task<DataSet> GetDataSetByRawSQL(string strRawSQL);
		SqlParameter CreateParameter(DbType parameterType, string name, object value, ParameterDirection direction);
		Task<int> SaveData(string procName, List<SqlParameter> listOfParams);
		Task<int> SaveDataWithReturnVal(string procName, List<SqlParameter> paramList);
		Task<string> GetStringByRawSQL(string strRawSQL);
		SqlParameter CreateParameterWithSize(DbType parameterType, string name, object value, ParameterDirection direction, int Size);
		SqlParameter CreateDbDataParameter(string name, DbType parameterType, ParameterDirection direction, object value);
		Task<int> RunSQL(string sqlText, List<SqlParameter> paramList);
		Task<List<object>> ExecuteWithOutput(string procName, List<SqlParameter> paramList);
		Task<DataSet> GetDataSetByRawSqlWithParam(string rawSql, List<SqlParameter> paramList);
	}
}
