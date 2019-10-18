using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using RiziFrame.Utility.Common;


namespace RiziFrame.Utility.Db
{
    /// <summary>
    /// 数据访问基础类(基于SQLite)
    /// 可以用户可以修改满足自己项目的需要。
    /// </summary>
    public class SQLiteHelper: IDbHelper
    {
        /* Sqlite连接串说明：
         * Data Source=.\data\zzsWork.db;Pooling=true;Cache Size=4096;FailIfMissing=false
         * Cache Size： 单位字节，默认2000
         */
        
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.    
        //private static string connectionString = DbProvider.connectionString;
        private static string connectionString;
        private SQLiteConnection _connection;

        public SQLiteHelper()
        {
            DbProvider dbProvider = new DbProvider();
            connectionString = dbProvider.ConnectionString;
            _connection = new SQLiteConnection(connectionString);
        }
        
    

        /// <summary>
        /// 获取数据文件.db绝对路径
        /// </summary>
        /// <returns></returns>
        private string GetDbFile()
        { 
            string dbFile = string.Empty;
            string[] arrTemp = connectionString.Split(';');
            
            foreach (string item in arrTemp)
            {
                //string result1 = str.Substring(2, 3); //CDE

                if (item.ToLower().IndexOf("source") > 0)
                {
                    string[] dsStr = item.Split('=');
                    dbFile = System.Windows.Forms.Application.StartupPath + dsStr[1];
                    break;
                }
            }

            //MessageBox.Show(dbFile + " --- " + File.Exists(@dbFile).ToString());

            return dbFile;
        }

        public void TestConn()
        {            
            string msg = string.Empty;            
            string dbFile = GetDbFile();

            // 判断数据文件.db是否存在
            if (!File.Exists(@dbFile))
            {
                msg = string.Format("数据库连接: 数据文件 \"{0}\" 不存在, 请检查！", dbFile);
                LogHelper.WriteLog(msg, LogLevel.Error);
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return ;
            }

            // 存在数据文件.db，则测试连接数据库，并在日志中打印 db.version
            // 注意：sqlite 的db文件即使不存在，一旦进行SQLiteConnection 连接，则会创建.db文件
            SQLiteConnection conn = new SQLiteConnection();
            if (conn.State.Equals(ConnectionState.Closed))
            {
                conn.ConnectionString = connectionString;

                try
                {
                    conn.Open();
                    msg = string.Format("数据库连接：success!  Version:{0}", GetDbVersion());
                    LogHelper.WriteLog(msg, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库连接失败！\n" + ex.Message, "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    LogHelper.WriteLog(msg, LogLevel.Error);

                    //throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 获取SQLite版本
        /// </summary>
        /// <returns></returns>
        public string GetDbVersion()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sqlite_version(*) ");
            string dbVersion = string.Empty;

            object obj = GetSingle(strSql.ToString());
            if (obj != null) { dbVersion = obj.ToString(); }
            return dbVersion;            
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string sqlString)
        {
            DataSet ds = new DataSet();            
            SQLiteCommand cmd = new SQLiteCommand(sqlString, _connection);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            try
            {
                this.Open();
                da.Fill(ds, "ds");
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteLog(ex.Message, LogLevel.Error);
                throw new Exception(ex.Message);
            }
            return ds;            
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// 用于delete, update 相关的sql
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string sqlString)
        {
            SQLiteCommand cmd = new SQLiteCommand(sqlString, _connection);
            try
            {
                _connection.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteLog(ex.Message, LogLevel.Error);
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// 用于查询并返回dataReader
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sqlString)
        {
            return this.ExecuteReader(sqlString, null);            
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>IDataReader</returns>
        public IDataReader ExecuteReader(string sqlString, params SQLiteParameter[] cmdParms)
        {         
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, _connection, null, sqlString, cmdParms);
                SQLiteDataReader reader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return reader;
            }
            catch (SQLiteException ex)
            {
                LogHelper.WriteLog(ex.Message, LogLevel.Error);
                throw new Exception(ex.Message);
            }
 
        }

        private void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public void Open()
        {
            if (_connection.State != ConnectionState.Open)
            _connection.Open();
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
            _connection.Close();
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public object GetSingle(string sqlString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException ex)
                    {
                        connection.Close();
                        LogHelper.WriteLog(ex.Message, LogLevel.Error);
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
