using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RiziFrame.Utility.Db
{
    public class SQLHelper
    {
        // 创建数据库连接字符串
        //private static string connectionString = "Server=.;DataBase=CourseManageDB;Uid=sa;Pwd=sa123";

        private static string connectionString; // = DbProvider.connectionString;

        public SQLHelper()
        {
            DbProvider dbProvider = new DbProvider();
            connectionString = dbProvider.ConnectionString;
        }

        #region 【执行增、删、改操作】
        /// <summary>
        /// 执行增、删、改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行Update(string sql) 方法时发生异常: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 【查询】简单的sql执行, 返回一个结果集的查询, 容易引起SQL注入
        /// <summary>
        /// 执行返回一个结果集的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>

        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行GetReader(string sql) 方法时发生异常: " + ex.Message);
            }
            // 此处不关闭，但需要在程序中关闭。
            //finally
            //{
            //    conn.Close();
            //}
        }
        #endregion
        
        #region 【查询】执行返回单一结果的查询
        /// <summary>
        /// 执行返回单一结果的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行GetSingleResult(string sql) 方法时发生异常: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion      

        // -- 以下是带参数的sql执行

        #region 【执行增、删、改操作】带参数的sql执行
        /// <summary>
        /// 执行增、删、改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Update(string sql, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            PrepareCommand(cmd, conn, null, sql, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行Update(string sql) 方法时发生异常: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 【查询】带参数的sql执行,执行返回单一结果的查询
        /// <summary>
        /// 执行返回单一结果的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleResult(string sql, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            PrepareCommand(cmd, conn, null, sql, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行GetSingleResult(string sql) 方法时发生异常: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region 【查询】带参数的sql执行, 返回一个结果集的查询, 防止SQL注入
        /// <summary>
        /// 带参数的sql执行, 返回一个结果集的查询, 防止SQL注入
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            PrepareCommand(cmd, conn, null, sql, cmdParms);
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                // 写入日志...
                throw new Exception("执行GetReader(string sql) 方法时发生异常: " + ex.Message);
            }
        }
        #endregion

        
        #region 准备带参数的sqlcmd命令
        /// <summary>
        /// 准备带参数的sqlcmd命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
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
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion
    }
}
