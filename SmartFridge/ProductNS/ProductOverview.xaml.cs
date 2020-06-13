using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Equin.ApplicationFramework;

namespace SmartFridge.ProductNS
{
    public partial class ProductOverview : Page
    {
        public event Action AddProduct;
        public event Action<Product> EditProduct;
        public event Action<Product> DeleteProduct;
        public event Action<Product> SelectedProduct;

        private BindingList<Product> ProductList;
        private Products m_products;
        private ECategory m_category;
        private bool m_filterActive = false;

        public ProductOverview(Products products)
        {
            InitializeComponent();

            m_products = products;
            ProductList = new BindingList<Product>();
            listBoxProducts.ItemsSource = ProductList;

            products.List.ListChanged += (object o, ListChangedEventArgs e) => { Refresh(); };

            Refresh();
            ConnectButtons();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProduct?.Invoke();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            EditProduct?.Invoke(CurrentItem());
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            DeleteProduct?.Invoke(CurrentItem());
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            SelectedProduct?.Invoke(CurrentItem());
        }

        private Product CurrentItem()
        {
            int index = listBoxProducts.SelectedIndex;
            if (index < 0 || index >= ProductList.Count) 
                return null;
            
            return ProductList.ElementAt(index);
        }


        private void ConnectButtons()
        {
            btnAll.Click             += (object sender, RoutedEventArgs e) => { DeactivateFilter();                   };
            btnVegetable_Fuit.Click  += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Vegetable_Fruit); };
            btnDairy_Products.Click  += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Dairy_Product);   };
            btnMeat_Fish_Eggs.Click  += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Meat_Fish_Eggs);  };
            btnDrinks.Click          += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Drinks);          };
            btnOther.Click           += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Other);           };
        }

        private void SetFilter(ECategory category)
        {
            m_category = category;
            m_filterActive = true;
            Refresh();
        }

        private void DeactivateFilter()
        {
            m_filterActive = false;
            Refresh();
        }

        private bool Filter(Product product)
        {
            if (!m_filterActive) return true;
            return product.Category == m_category;
        }

        private void Refresh()
        {
            ProductList.Clear();
            foreach(Product product in m_products.List)
            {
                if(Filter(product))
                {
                    ProductList.Add(product);
                }
            }
        }
    }
}
