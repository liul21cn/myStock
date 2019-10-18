using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RiziFrame.Utility.Db
{
    public class DbHelper : IDbHelper
    {
        public void TestConn()
        {
            //MessageBox.Show(DbProvider.dbProviderString + "\n"
            //    + DbProvider.connectionString, "提示");
            DbProvider dbProvider = new DbProvider ();

            MessageBox.Show(dbProvider.TypeName + "\n"
                + dbProvider.ConnectionString, "提示");
        }
    }
}
