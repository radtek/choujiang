using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using XGTech.Config;
using XGTech.Web.Common;

namespace XGTech.Web.Database
{
    public class DBHelper
    {

        public static string ConnectionString = SiteConfig.GetSite("DefaultConnection");

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true });
                db.Ado.IsEnableLogEvent = true;
                db.Ado.LogEventStarting = (sql, pars) =>
                {
                    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
                return db;
        }
    }
}
