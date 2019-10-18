using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RiziFrame.Utility.Db
{
    /// <summary>
    /// 数据源提供者
    /// </summary>
    public class DbProvider
    {
        /// <summary>
        /// 数据源提供者：类型名称
        /// </summary>
        public string TypeName
        {
            get { return ConfigurationManager.AppSettings["DbProvider"].ToString();  }
        }

        /// <summary>
        /// 数据源提供者：数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get {
                string val = string.Empty;
                switch (TypeName.ToLower())
                {
                    case "oledb":
                        val = ConfigurationManager.ConnectionStrings["connectStringOledb"].ToString();
                        break;
                    case "sqlite":
                        val = ConfigurationManager.ConnectionStrings["connectStringSqlite"].ToString();
                        break;
                    default:
                        break;
                }

                return val; 
            }
        }



        //public DbProvider()
        //{
        //    dbProviderString = ""; ConfigurationManager.AppSettings["DbProvider"].ToString();

        //    switch (dbProviderString.ToLower())
        //    {
        //        case "oledb":
        //            connectionString = ConfigurationManager.ConnectionStrings["connectStringOledb"].ToString();
        //            break;
        //        case "sqlite":
        //            connectionString = ConfigurationManager.ConnectionStrings["connectStringSqlite"].ToString();
        //            break;
        //        default:
        //            break;
        //    }
        //}
    
    }
}
