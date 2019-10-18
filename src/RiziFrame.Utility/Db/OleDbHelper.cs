using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiziFrame.Utility.Db
{
    public class OleDbHelper
    {
        public OleDbHelper() { }

        private static OleDbConnection Conn;
        private static OleDbCommand Cmd;
        private static OleDbDataAdapter Da;
        private static DataSet Ds;
        private static DataTable Dt;
        private static string strConn = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=c:\\a.mdb";
                

        /// <summary>
        /// 设置execl文件连接
        /// </summary>
        /// <param name="FileName"></param>
        public static void SetConn(string FileName)
        {
            strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + FileName;
            //strConn = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + FileName;
        }

        /// <summary>  
        /// 打开连接  
        /// </summary>  
        public static void Open()
        {
            Conn = new OleDbConnection();
            Cmd = new OleDbCommand();
            if (Conn.State.Equals(ConnectionState.Closed))
            {
                Conn.ConnectionString = strConn;
                Conn.Open();
            }
            Cmd.Connection = Conn;
        }

        /// <summary>  
        /// 测试连接  
        /// </summary>  
        public static void TestConn()
        {
            Conn = new OleDbConnection();            
            if (Conn.State.Equals(ConnectionState.Closed))
            {
                Conn.ConnectionString = strConn;

                try
                {
                    Conn.Open();
                    MessageBox.Show("数据库连接成功！", "提示",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库连接失败！\n" + ex.Message, "提示", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw;
                }
                finally
                {
                    Conn.Close();
                }                                
            }
            
        }
        
        /// <summary>  
        /// 关闭连接  
        /// </summary>  
        public static void Close()
        {
            if (Conn.State.Equals(ConnectionState.Open))
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        /// <summary>  
        /// 执行ExecuteNonQuery()  
        /// </summary>  
        /// <param name="sql">SQL语句</param>  
        /// <returns></returns>  
        public static int ExecuteCmd(string sql)
        {
            try
            {
                Open();
                Cmd.CommandText = sql;
                return Cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                Close();
            }
        }
        
        /// <summary>  
        /// DataSet类  
        /// </summary>  
        /// <param name="sql">SQL语句</param>  
        /// <returns></returns>  
        public static DataSet GetDataSet(string sql)
        {
            try
            {
                Open();
                Cmd.CommandText = sql;
                Da = new OleDbDataAdapter();
                Da.SelectCommand = Cmd;
                Ds = new DataSet();
                Da.Fill(Ds);
                return Ds;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>  
        /// DataTable 类  
        /// </summary>  
        /// <param name="sql">SQL语句</param>  
        /// <returns></returns>  
        public static DataTable GetDataTable(string sql)
        {
            try
            {
                Open();
                Cmd.CommandText = sql;
                Da = new OleDbDataAdapter();
                Da.SelectCommand = Cmd;
                Dt = new DataTable();
                Da.Fill(Dt);
                return Dt;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                Close();
            }
        }

        /// <summary>  
        /// 执行 ExecuteScalar  
        /// </summary>  
        /// <param name="sql">SQL语句</param>  
        /// <returns></returns>  
        public static object ExecuteScalar(string sql)
        {
            try
            {
                Open();
                Cmd.CommandText = sql;
                return Cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
            finally
            {
                Close();
            }
        }



        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OleDbDataReader ExecuteReader(string strSQL)
        {
            OleDbConnection connection = new OleDbConnection(strConn);
            OleDbCommand cmd = new OleDbCommand(strSQL, connection);
            try
            {
                connection.Open();
                OleDbDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (OleDbException ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }  
}
