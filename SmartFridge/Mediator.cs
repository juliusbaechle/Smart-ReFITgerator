using SmartFridge.ProductNS;
using SmartFridgeWPF;
using SmartFridgeWPF.ProductNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        Shopping,
        ProductForm
    }

    class Mediator
    {
        public Mediator(MainWindow window, Products products)
        {
            m_window = window;
            m_window.OpenPage += ShowPage;

            m_products = products;
        }

        public void ShowPage(EPage page) {
            switch (page)
            {
                case EPage.Products: 
                    ShowProductOverview(); 
                    break;

                case EPage.ProductForm: 
                    ShowProductForm(null); 
                    break;

                default: 
                    ShowEmptyPage(); 
                    break;                    
            }
        }

        private void ShowEmptyPage()
        {
            m_window.SetContent(new Page());
        }

        private void ShowProductOverview()
        {
            var productOverview = new ProductOverview { DataContext = m_products };
            productOverview.Edit += ShowProductForm;
            productOverview.Delete += m_products.Delete;
            productOverview.Selected += m_products.Selected;
            productOverview.Add += () => { ShowPage(EPage.ProductForm); };
            m_window.SetContent(productOverview);
        }

        private void ShowProductForm(Product product)
        {
            var productForm = new ProductForm();
            if (product != null) productForm.DataContext = product;
            productForm.Finished += m_products.AddOrEdit;
            productForm.Finished += (Product p) => { ShowPage(EPage.Products); };
            m_window.SetContent(productForm);
        }

        MainWindow m_window;
        Products m_products;
    }
}
