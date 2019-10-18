using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RiziFrame.Utility.Db
{
    public interface IDbHelper
    {
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        void TestConn();

        /// <summary>
        /// 得到数据库版本
        /// </summary>
        /// <returns></returns>
        string GetDbVersion();

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        object GetSingle(string sqlString);

        IDataReader ExecuteReader(string sqlString);


    }
}
