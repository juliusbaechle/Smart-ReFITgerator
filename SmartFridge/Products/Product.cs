using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Products
{
    enum EProductCategory
    {
        Other,
        Fruit,
        Vegetable,
        Meat_Fish,
        Dairy_Product,
        Breakfast
    }

    class Product
    {
        public Product()
        {
            ID = Guid.NewGuid();
            Barcodes = new List<UInt64>();            
        }


        public string Name {
            get;
            internal set;
        }

        public UInt16 Energy {
            get;
            internal set;
        }

        public UInt16 Durability {
            get;
            internal set;
        }

        public EProductCategory Category {
            get;
            internal set;
        }

        internal Guid ID {
            get;
            set;
        }

        internal List<UInt64> Barcodes {
            get;
            set;
        }
    }
}
