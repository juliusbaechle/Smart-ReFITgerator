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
        Shopping
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
                case EPage.Home:
                    m_window.SetContent(new Page());
                    break;
                case EPage.Products:
                    m_window.SetContent(new ProductOverview(m_products));
                    break;
                case EPage.Content:
                    var productForm = new ProductForm();
                    productForm.Finished += m_products.AddOrEdit;
                    m_window.SetContent(productForm);
                    break;
            }
        }

        MainWindow m_window;
        Products m_products;
    }
}
