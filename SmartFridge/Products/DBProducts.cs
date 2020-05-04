using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartFridge.Products
{
    class DBProducts
    {
        public DBProducts(SQLiteConnection db)
        {
            m_db = db;
            CreateTable();
        }

        public void Save(Product product)
        {
            SQLiteCommand cmd = new SQLiteCommand(m_db);

            if (!Contains(product.ID))
            {                
                cmd.CommandText = $"INSERT INTO tblProducts (Id, Name, Durability, Energy, Category) " +
                    $"VALUES ('{product.ID}', '{product.Name}', {product.Durability}, {product.Energy}, {(UInt16)product.Category})";    
            }
            else
            {
                cmd.CommandText = $"UPDATE tblProducts SET " +
                    $"Name = '{product.Name}', Durability = {product.Durability}, Energy = {product.Energy}, Category = {(UInt16)product.Category} " +
                    $"WHERE Id = '{product.ID}'";
            }

            cmd.ExecuteNonQuery();
        }

        public List<Product> LoadAll()
        {
            SQLiteCommand cmd = new SQLiteCommand(m_db);
            cmd.CommandText = "SELECT * FROM tblProducts";
            SQLiteDataReader reader = cmd.ExecuteReader();

            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product();
                product.ID          = reader.GetGuid(0);
                product.Name        = reader.GetString(1);
                product.Durability  = (UInt16)reader.GetInt32(2);
                product.Energy      = (UInt16)reader.GetInt32(3);
                product.Category    = (EProductCategory)reader.GetInt16(4);
                products.Add(product);
            }
            return products;
        }

        public void Clear()
        {
            SQLiteCommand cmd = new SQLiteCommand(m_db);
            cmd.CommandText = "DELETE FROM tblProducts";
            cmd.ExecuteNonQuery();
        }

        private bool Contains(Guid productId)
        {
            SQLiteCommand cmd = new SQLiteCommand(m_db);
            cmd.CommandText = $"SELECT * FROM tblProducts WHERE Id = '{productId}'";
            bool contained = cmd.ExecuteScalar() != null;
            return contained;
        }

        private void CreateTable()
        {
            SQLiteCommand cmd = new SQLiteCommand(m_db);
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS tblProducts (Id TEXT PRIMARY KEY, Name TEXT, Durability INT, Energy INT, Category INT )";
            cmd.ExecuteNonQuery();
        }

        private SQLiteConnection m_db;
    }
}
