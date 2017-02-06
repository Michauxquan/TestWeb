//===============================================================================
// This file is based on the Microsoft Data Access Application Block for .NET
// For more information please go to 
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//===============================================================================
using System;
using System.Text;
using System.Data;
using System.Data.Common;

namespace OWZX.Core
{
    /// <summary>
    /// 关系数据库帮助类
    /// </summary>
    public partial class RDBSHelper
    {
        private static object _locker = new object();//锁对象

        private static DbProviderFactory _factory;//抽象数据工厂

        private static string _rdbstablepre;//关系数据库对象前缀
        private static string _connectionstring;//关系数据库连接字符串

        /// <summary>
        /// 关系数据库对象前缀
        /// </summary>
        public static string RDBSTablePre
        {
            get { return _rdbstablepre; }
        }

        /// <summary>
        /// 关系数据库连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionstring; }
        }

#if DEBUG
        private static int _executecount = 0;
        private static string _executedetail = string.Empty;

        /// <summary>
        /// 数据库执行次数
        /// </summary>
        public static int ExecuteCount
        {
            get { return _executecount; }
            set { _executecount = value; }
        }

        /// <summary>
        /// 数据库执行细节
        /// </summary>
        public static string ExecuteDetail
        {
            get { return _executedetail; }
            set { _executedetail = value; }
        }

        /// <summary>
        /// 设置数据库执行细节
        /// </summary>
        /// <param name="commandText">数据库执行语句</param>
        /// <param name="startTime">数据库执行开始时间</param>
        /// <param name="endTime">数据库执行结束时间</param>
        /// <param name="commandParameters">数据库执行参数列表</param>
        /// <returns></returns>
        private static string GetExecuteDetail(string commandText, DateTime startTime, DateTime endTime, DbParameter[] commandParameters)
        {
            if (commandParameters != null && commandParameters.Length > 0)
            {
                StringBuilder paramdetails = new StringBuilder("<div style=\"display:block;clear:both;margin-left:auto;margin-right:auto;width:100%;\"><table cellspacing=\"0\" cellpadding=\"0\" style=\"border: 1px solid black;background:rgb(255, 255, 255) none repeat scroll 0%;font-size:12px;display:block;margin-left:auto;margin-right:auto;margin-top:5px;margin-bottom:5px;width:960px;\">");
                paramdetails.AppendFormat("<tr><td colspan=\"3\">执行SQL：{0}</td></tr>", commandText);
                paramdetails.AppendFormat("<tr><td colspan=\"3\">执行时间：{0}</td></tr>", endTime.Subtract(startTime).TotalMilliseconds / 1000);
                foreach (DbParameter param in commandParameters)
                {
                    if (param == null)
                        continue;

                    paramdetails.Append("<tr>");
                    paramdetails.AppendFormat("<td width=\"250px\">参数名称：{0}</td>", param.ParameterName);
                    paramdetails.AppendFormat("<td width=\"250px\">参数类型：{0}</td>", param.DbType);
                    paramdetails.AppendFormat("<td>参数值：{0}</td>", param.Value);
                    paramdetails.Append("</tr>");
                }
                return paramdetails.Append("</table></div>").ToString();
            }
            return string.Empty;
        }
#endif

        static RDBSHelper()
        {
            //设置数据工厂
            _factory = BSPData.RDBS.GetDbProviderFactory();
            //设置关系数据库对象前缀
            _rdbstablepre = BSPConfig.RDBSConfig.RDBSTablePre;
            //设置关系数据库连接字符串
            _connectionstring = BSPConfig.RDBSConfig.RDBSConnectString;
        }

        #region ExecuteNonQuery
        /// <summary>
        /// 受影响的行数
        /// </summary>
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, null);
        }
        /// <summary>
        /// 受影响的行数
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(cmdType, cmdText, null);
        }
        /// <summary>
        /// 受影响的行数
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif
            DbCommand cmd = _factory.CreateCommand();

            using (DbConnection conn = _factory.CreateConnection())
            {
                conn.ConnectionString = ConnectionString;
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                int val = cmd.ExecuteNonQuery();
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return val;
            }
        }


        /// <summary>
        /// /// <summary>
        /// 受影响的行数
        /// </summary>
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(trans, cmdType, cmdText, null);
        }
        /// <summary>
        /// 受影响的行数
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            int val = cmd.ExecuteNonQuery();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteReader
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="cmdText">脚本</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(string cmdText, params DbParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.Text, cmdText, commandParameters);
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(CommandType.Text, cmdText, null);
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            return ExecuteReader(cmdType, cmdText, null);
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = ConnectionString;

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteReader(trans, cmdType, cmdText, null);
        }
        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            DbDataReader rdr = cmd.ExecuteReader();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return rdr;

        }

        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(CommandType.Text, cmdText, null);
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(cmdType, cmdText, null);
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();

            using (DbConnection connection = _factory.CreateConnection())
            {
                connection.ConnectionString = ConnectionString;
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                object val = cmd.ExecuteScalar();
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(trans, cmdType, cmdText, null);
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            object val = cmd.ExecuteScalar();
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteDataset
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string cmdText)
        {
            return ExecuteDataset(CommandType.Text, cmdText, null);
        }
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText)
        {
            return ExecuteDataset(cmdType, cmdText, null);
        }
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbConnection conn = _factory.CreateConnection();
            conn.ConnectionString = ConnectionString;
            DbDataAdapter adapter = _factory.CreateDataAdapter();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();

#if DEBUG
                DateTime startTime = DateTime.Now;
#endif
                adapter.Fill(ds);
#if DEBUG
                DateTime endTime = DateTime.Now;
                _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                adapter.Dispose();
                conn.Close();
            }
        }


        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="cmdType">类型</param>
        /// <param name="cmdText">脚本</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(DbTransaction trans, CommandType cmdType, string cmdText)
        {
            return ExecuteDataset(trans, cmdType, cmdText, null);
        }

        private static DataSet ExecuteDataset(DbTransaction trans, CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
#if DEBUG
            _executecount++;
#endif

            DbCommand cmd = _factory.CreateCommand();
            DbDataAdapter adapter = _factory.CreateDataAdapter();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            adapter.SelectCommand = cmd;
            DataSet ds = new DataSet();

#if DEBUG
            DateTime startTime = DateTime.Now;
#endif
            adapter.Fill(ds);
#if DEBUG
            DateTime endTime = DateTime.Now;
            _executedetail += GetExecuteDetail(cmd.CommandText, startTime, endTime, commandParameters);
#endif
            cmd.Parameters.Clear();
            return ds;
        }

        #endregion

        #region ExecuteTable方法
        /// <summary>
        /// 返回DataTableCollection
        /// </summary>
        /// <param name="cmdTye">SqlCommand命令类型</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个表集合(DataTableCollection)表示查询得到的数据集</returns>
        public static DataTableCollection ExecuteTable(CommandType cmdTye, string cmdText, DbParameter[] commandParameters)
        {
            DataSet ds= ExecuteDataset(cmdTye, cmdText, commandParameters);
            DataTableCollection table = ds.Tables;
            return table;
        }


        /// <summary>
        /// 返回DataTableCollection ,存储过程专用
        /// </summary>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个表集合(DataTableCollection)表示查询得到的数据集</returns>
        public static DataTableCollection ExecuteTable(string cmdText, DbParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataset(CommandType.Text, cmdText, commandParameters);
            DataTableCollection table = ds.Tables;
            return table;
        }
        #endregion

        #region Exists
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns>bool结果</returns>
        public static bool Exists(string strSql)
        {
            int cmdresult = Convert.ToInt32(ExecuteScalar(strSql));
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>bool结果</returns>
        public static bool Exists(string strSql, params DbParameter[] cmdParms)
        {
            int cmdresult = Convert.ToInt32(ExecuteScalar(CommandType.Text,strSql, cmdParms));
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    if (parm != null)
                    {
                        if ((parm.Direction == ParameterDirection.InputOutput || parm.Direction == ParameterDirection.Input) &&
                            (parm.Value == null))
                        {
                            parm.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(parm);
                    }
                }
            }
        }

    }
}
