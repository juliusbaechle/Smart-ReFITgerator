using SmartFridgeWPF;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Equin.ApplicationFramework;

namespace SmartFridge.ProductNS
{
    public partial class ProductOverview : Page
    {
        public event Action Add;
        public event Action<Product> Edit;
        public event Action<Product> Delete;
        public event Action<Product> Selected;

        private BindingListView<Product> ProductList;
        private ECategory m_category;
        private bool m_filterActive = false;

        public ProductOverview(Products products)
        {
            InitializeComponent();

            ProductList = new BindingListView<Product>(products.List);
            listBoxProducts.ItemsSource = ProductList;
            ProductList.ApplyFilter(Filter);

            ConnectButtons();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add?.Invoke();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            ProductList.RemoveFilter();
            Edit?.Invoke(CurrentItem());
            ProductList.ApplyFilter(Filter);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            ProductList.RemoveFilter();
            Delete?.Invoke(CurrentItem());
            ProductList.ApplyFilter(Filter);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            Selected?.Invoke(CurrentItem());
        }

        private Product CurrentItem()
        {
            int index = listBoxProducts.SelectedIndex;
            var product = ProductList.ElementAt(index);
            return product;
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
            ProductList.Refresh();
        }

        private void DeactivateFilter()
        {
            m_filterActive = false;
            ProductList.Refresh();
        }

        private bool Filter(object o)
        {
            if (!m_filterActive) return true;

            var product = o as Product;
            if (product == null) return true;

            return product.Category == m_category;
        }
    }
}
