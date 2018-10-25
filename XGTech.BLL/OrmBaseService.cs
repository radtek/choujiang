using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using XGTech.Config;

namespace XGTech.BLL
{
    public class OrmBaseService
    {
  


        public static SqlSugarClient GetInstance()
        {
            var cnnectionString = "Database=jdj;Data Source=121.40.94.98;User Id=jdjuser;Password=h9RpRmw34tnyPLu9;pooling=false;CharSet=utf8;port=3306";
            //var cnnectionString = "Database=jdj;Data Source=10.12.78.182;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306";
           // var cnnectionString = "Database=jundui;Data Source=192.168.2.106;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306";
            //var cnnectionString = "Database=jundui;Data Source=127.0.0.1;User Id=root;Password=root;pooling=false;CharSet=utf8;port=3306";
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = cnnectionString, DbType = DbType.MySql, IsAutoCloseConnection = true });
            db.Ado.IsEnableLogEvent = true;
            db.Ado.LogEventStarting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(s => s.ParameterName, s => s.Value)));
                Console.WriteLine();
            };
            return db;
        }

        public static SqlSugarClient G_GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = "", DbType = DbType.MySql, IsAutoCloseConnection = true });
            db.Ado.IsEnableLogEvent = true;
            db.Ado.LogEventStarting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(s => s.ParameterName, s => s.Value)));
                Console.WriteLine();
            };
            return db;
        }
    }
}
