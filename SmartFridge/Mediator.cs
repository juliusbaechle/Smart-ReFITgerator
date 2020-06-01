using SmartFridge.ProductNS;
using SmartFridgeWPF;
using SmartFridgeWPF.ProductNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using SmartFridge.ContentNS;
using System.Windows.Forms;

namespace SmartFridge
{
    // Bindeglied zwischen Logik und View
    // Erzeugt Fenster in Abhängigkeit der Members wie z.B. Products 
    // und setzt diese im MainWindow

    public enum EPage
    {
        Home,
        Products,
        Content,
        Nutrition,
        Messages,
        Shopping
    }

    class Mediator
    {
        public Action UserChangedPage;

        public Mediator(MainWindow window, Products products, Content content)
        {
            MainWindow = window;
            MainWindow.OpenPage += ShowPage;

            Products = products;
            ProductOverview = CreateProductOverview(products);

            Content = content;            
            ContentOverview = CreateContentOverview(content);
        }

        public void ShowPage(EPage page) {
            switch (page)
            {
                case EPage.Products:
                    MainWindow.SetContent(ProductOverview);
                    break;

                case EPage.Content:
                    MainWindow.SetContent(ContentOverview);
                    break;

                default:
                    MainWindow.SetContent(new Page());
                    break;
            }

            UserChangedPage?.Invoke();
        }

        private ProductOverview CreateProductOverview(Products products)
        {
            var productOverview = new ProductOverview(products);
            productOverview.Edit += (Product product) => { MainWindow.SetContent(CreateProductForm(product)); };
            productOverview.Delete += Products.Delete;
            productOverview.Selected += Products.Selected;
            productOverview.Add += () => { MainWindow.SetContent(CreateProductForm(null)); };
            return productOverview;
        }

        private ProductForm CreateProductForm(Product product)
        {
            var productForm = new ProductForm();

            if (product == null) 
                productForm.DataContext = new Product();
            else 
                productForm.DataContext = new Product(product);

            productForm.Finished += (Product) => {
                Products.AddOrEdit(Product);
                ShowPage(EPage.Products);
            };

            return productForm;
        }

        private ContentOverview CreateContentOverview(Content content)
        {
            var contentOverview = new ContentOverview(content);
            contentOverview.Add += () => { new PutInNewItemCmd(this); };
            contentOverview.Delete += Content.Delete;
            return contentOverview;
        }
        
        public Products Products { get; private set; }
        public Content Content { get; private set; }

        public MainWindow MainWindow { get; private set; }
        public ProductOverview ProductOverview { get; private set; }
        public ContentOverview ContentOverview { get; private set; }
    }
}
