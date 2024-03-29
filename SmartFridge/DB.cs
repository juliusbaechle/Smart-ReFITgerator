﻿using System.Data.SQLite;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace SmartFridge
{
    public class DB
    {
        #if DEBUG
            // Relativ zur Debug.exe
            private const string PATH = "URI=file:../../smartfridge.db";
        #else
            //Relativ zur Release.exe
            private const string PATH = "URI=file:smartfridge.db";
        #endif

        public static DbConnection CreateLocalConnection(string path = PATH)
        {
            //Falls Datenbank nicht vorhanden ist, wird eine neue Datenbank erstellt
            DbConnection conn = new SQLiteConnection(path);
            conn.Open();
            return conn;
        }

        public static DbConnection CreateRemoteConnection()
        {
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString =
                "Server=ec2-3-125-104-251.eu-central-1.compute.amazonaws.com; " +
                "Port=3306; " + 
                "Database=smartfridge; " +
                "Password=smartfridge; " +
                "User=ec2-user; ";
            conn.OpenAsync();
            return conn;
        }
    }
}
