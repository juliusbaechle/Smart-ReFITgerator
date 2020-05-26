using SmartFridge.ProductNS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.ContentNS
{
    class DBContent
    {

        public DbConnection DbConnection { get; private set; }
        public DBContent(DbConnection dbConnection)
        {
            DbConnection = dbConnection;
            if (dbConnection.State == ConnectionState.Open)
            {
                CreateTable();
            }
            else
            {
                dbConnection.StateChange += (object sender, StateChangeEventArgs e) => { if (e.CurrentState == ConnectionState.Open) CreateTable(); };
            }
        }

        internal List<Item> LoadAll()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tblContent";
            DbDataReader reader = cmd.ExecuteReader();

            List<Item> Items = new List<Item>();
            while (reader.Read())
            {
                Item items = new Item();
                items.ID = reader.GetString(0);
                items.ProductID = reader.GetString(1);
                items.Amount = (UInt32)reader.GetInt32(2);
                items.ExpiryDate = DateTime.Parse(reader.GetString(3));
                Items.Add(items);
            }

            reader.Close();
            return Items;
        }

        internal void Delete(Item items)
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"DELETE FROM tblContent WHERE Id='{items.ID}'";
            cmd.ExecuteNonQuery();
           
        }

        internal void Save(Item items)
        {
            DbCommand cmd = DbConnection.CreateCommand();

            if (!Contains(items.ID))
            {
                cmd.CommandText = $"INSERT INTO tblContent (Id,ProductID, Amount, ExpiryDate) " +
                    $"VALUES ('{items.ID}', '{items.ProductID}',{(UInt16)items.Amount} , '{items.ExpiryDate.ToShortDateString()}')";
            }
            else
            {
                cmd.CommandText = $"UPDATE tblContent SET " +
                    $"ID = '{items.ID}', ProductID = '{items.ProductID}', Amount ={(UInt16)items.Amount} , ExpiryDate = '{items.ExpiryDate.ToShortDateString()}' " +
                    $"WHERE Id = '{items.ID}'";
            }

            cmd.ExecuteNonQuery();

        }

        internal void Clear()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "DELETE FROM tblContent";
            cmd.ExecuteNonQuery();
        }

        private bool Contains(string Id)
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM tblContent WHERE Id = '{Id}'"; // von Content oder Item ?
            bool contained = cmd.ExecuteScalar() != null;
            return contained;
        }


        private void CreateTable()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS tblContent (Id VARCHAR(50) PRIMARY KEY, ProductId VARCHAR(50), Amount INT, ExpiryDate TEXT )";
            cmd.ExecuteNonQuery();
        }
        
      
    }
}
