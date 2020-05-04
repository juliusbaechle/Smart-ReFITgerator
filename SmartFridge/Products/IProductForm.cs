using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Products
{
    internal interface IProductForm
    {
        event Action Commit;

        void Reset();

        UInt16 Durability { get; set; }
        UInt16 Energy { get; set; }
        string Name { get; set; }
        EProductCategory Category { get; set; }
    }
}
