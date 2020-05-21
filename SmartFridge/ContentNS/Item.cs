using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    class Item
    {
        public Item()
        {
            ID = Guid.NewGuid().ToString("N").ToUpper();
            Product = null;
        }
        internal string ID { get; set; }

        private string ProductID { get; set; }

        public DateTime ExpiryDate { get; set; }

        public UInt32 Amount { get; set; }

        public Product Product { get; internal set; } 

        
    }
}
