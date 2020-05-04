using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

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

        public static void Setup()
        {
            if (!File.Exists(PATH))
            {
                // TODO: Log Error and close application
            }

            FridgeDB = new SQLiteConnection(PATH);
            FridgeDB.Open();
        }

        public static SQLiteConnection FridgeDB {
            get;
            private set;
        }
    }
}
