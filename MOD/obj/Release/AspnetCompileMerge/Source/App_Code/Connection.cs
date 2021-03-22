using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace MOD.App_Code
{
    public class Connection
    {
        public Connection()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static OleDbConnection DB()
        {
            OleDbConnection conn = new OleDbConnection("Provider=SQLOLEDB;Data Source=103.149.165.53;Initial Catalog=MOD;persist security info=True;user id=gip;password=gip@321");
            conn.Open();
            return conn;
        }
    }
}