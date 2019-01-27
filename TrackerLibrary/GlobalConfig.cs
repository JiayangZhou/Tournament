using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connections { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            /*switch (db)
            {
                case DatabaseType.Sql:
                    break;
                case DatabaseType.Text:
                    break;
                default:
                    break;
            }*/
            if (db == DatabaseType.Sql)
            {
                SqlConnector sql = new SqlConnector();
                Connections = sql;
                //Can't use interface unless you sign it to a specific class
            }
            else if (db == DatabaseType.Text)
            {
                TextConnector text = new TextConnector();
                Connections = text;
            }
        }
        public static string CnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
