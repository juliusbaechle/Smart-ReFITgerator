using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace SmartFridge
{
    internal class DB
    {
        #if DEBUG
            // Relativ zur Debug.exe
            private const string PATH = "URI=file:../../smartfridge.db";
        #else
            //Relativ zur Release.exe
            private const string PATH = "URI=file:smartfridge.db";
        #endif

        public static DbConnection CreateLocalConnection()
        {
            //Falls Datenbank nicht vorhanden ist, wird eine neue Datenbank erstellt
            DbConnection conn = new SQLiteConnection(PATH);
            conn.Open();
            return conn;
        }

        public static DbConnection CreateWebConnection()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString =
                "Server=sql7.freesqldatabase.com; " +
                "Port=3306; " +
                "Database=sql7338189; " +
                "UID=sql7338189; " +
                "PWD=dx2EuT3yDD;";
            conn.Open();
            return conn;
        }
    }
}
