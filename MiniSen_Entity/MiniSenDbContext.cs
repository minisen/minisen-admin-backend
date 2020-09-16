using CoffeeSql.Mysql;
using MiniSen_Common.Helpers.Config;
using MiniSen_Common.Helpers.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniSen_Entity
{
    public class MiniSenDbContext : MysqlDbContext<MiniSenDbContext>
    {
        private static string connStr = ConfigHelper.GetConfig("mysqlDevelopTest");

        public MiniSenDbContext() : base(connStr)
        {
            this.OpenQueryCache = false;
            this.OpenTableCache = false;
            this.Log = context =>
            {
                LogHelper.Info($"sql:{context.SqlStatement}\r\n");
            };
        }
    }
}
