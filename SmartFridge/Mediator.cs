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


            m_productOverview = CreateProductOverview(products);
            m_productForm = CreateProductForm();            
        }

        public void ShowPage(EPage page) {
            switch (page)
            {
                case EPage.Products:
                    m_window.SetContent(m_productOverview);
                    break;

                case EPage.ProductForm:
                    m_window.SetContent(m_productForm);
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

        private ProductOverview CreateProductOverview(Products products)
        {
            var productOverview = new ProductOverview(products);
            productOverview.Edit += ShowProductForm;
            productOverview.Delete += m_products.Delete;
            productOverview.Selected += m_products.Selected;
            productOverview.Add += () => { ShowProductForm(null); };
            return productOverview;
        }
        
        private ProductForm CreateProductForm()
        {
            var productForm = new ProductForm();
            m_window.SetContent(productForm);

            productForm.Finished += (Product) => {
                m_products.AddOrEdit(Product);
                ShowPage(EPage.Products);
            };

            return productForm;
        }

        private void ShowProductForm(Product product)
        {
            if(product != null)
                m_productForm.DataContext = new Product(product);
            else
                m_productForm.DataContext = new Product();

            ShowPage(EPage.ProductForm);
        }

        MainWindow m_window;
        Products m_products;

        ProductForm m_productForm;
        ProductOverview m_productOverview;
    }
}
