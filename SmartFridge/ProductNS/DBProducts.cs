using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    class DBProducts
    {
        internal DBProducts(DbConnection db, ImageRepository imageRepository)
        {
            DbConnection = db;
            ImageRepository = imageRepository;

            if(db.State == ConnectionState.Open) {
                CreateTable();
            } else {
                db.StateChange += (object sender, StateChangeEventArgs e) => { if (e.CurrentState == ConnectionState.Open) CreateTable(); };
            }
        }

        internal void Save(Product product)
        {
            DbCommand cmd = DbConnection.CreateCommand();

            if (!Contains(product.ID))
            {                
                cmd.CommandText = $"INSERT INTO tblProducts (Id, Name, Durability, Energy, Category, ImageId) " +
                    $"VALUES ('{product.ID}', '{product.Name}', {product.Durability}, {product.Energy}, {(UInt16)product.Category}, '{product.Image.ID}')";    
            }
            else
            {
                cmd.CommandText = $"UPDATE tblProducts SET " +
                    $"Name = '{product.Name}', Durability = {product.Durability}, Energy = {product.Energy}, Category = {(UInt16)product.Category}, ImageId = '{product.Image.ID}' " +
                    $"WHERE Id = '{product.ID}'";
            }

            cmd.ExecuteNonQuery();

            ImageRepository.Save(product.Image);       
        }

        internal List<Product> LoadAll()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "SELECT * FROM tblProducts";
            DbDataReader reader = cmd.ExecuteReader();

            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product     = new Product();
                product.ID          = reader.GetString(0);
                product.Name        = reader.GetString(1);
                product.Durability  = (UInt16)reader.GetInt32(2);
                product.Energy      = (UInt16)reader.GetInt32(3);
                product.Category    = (ECategory)reader.GetInt16(4);
                product.Image.ID    = reader.GetString(5);
                ImageRepository.Load(product.Image);
                products.Add(product);
            }

            reader.Close();
            return products;
        }

        internal void Clear()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "DELETE FROM tblProducts";
            cmd.ExecuteNonQuery();
        }

        internal void Delete(Product product)
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"DELETE FROM tblProducts WHERE Id='{product.ID}'";
            cmd.ExecuteNonQuery();

            ImageRepository.Delete(product.Image);
        }

        private bool Contains(string productId)
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM tblProducts WHERE Id = '{productId}'";
            bool contained = cmd.ExecuteScalar() != null;
            return contained;
        }

        private void CreateTable()
        {
            DbCommand cmd = DbConnection.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS tblProducts (Id VARCHAR(50) PRIMARY KEY, Name TEXT, Durability INT, Energy INT, Category INT, ImageId VARCHAR(50) )";
            cmd.ExecuteNonQuery();
        }

        internal DbConnection DbConnection { get; private set; }
        internal ImageRepository ImageRepository { get; private set; }
    }
}
