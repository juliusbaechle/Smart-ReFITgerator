using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Products
{
    delegate void ProductHandler(Product product);

    internal interface IProductView
    {
        event ProductHandler SelectedProduct;
        event ProductHandler EditProduct;

        void ShowCategories(List<EProductCategory> categories);
        void ShowProducts(List<Product> products);
        void Show(bool show);
    }
}
