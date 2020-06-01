using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SmartFridge.ProductNS;

namespace SmartFridge.ContentNS
{
    /// <summary>
    /// Interaktionslogik für ContentOverview.xaml
    /// </summary>
    public partial class ContentOverview : Page
    {
        public event Action Add;
        public event Action<Item> Delete;

        private BindingList<Item> ItemList;
        private Content m_content;
        private ECategory m_category;
        private bool m_filterActive = false;

        public ContentOverview(Content content)
        {
            InitializeComponent();

            m_content = content;
            
            ItemList = new BindingList<Item>();
            listBoxItems.ItemsSource = ItemList;
            content.List.ListChanged += (object o, ListChangedEventArgs e) => { Refresh(); };

            Refresh();
            ConnectButtons();
        }

        private void btnAdd_Click(object o, RoutedEventArgs e)
        {
            Add?.Invoke();
        }

        private void btnDelete_Click(object o, RoutedEventArgs e)
        {
            if (CurrentItem() == null) return;
            Delete?.Invoke(CurrentItem());
        }

        private Item CurrentItem()
        {
            int index = listBoxItems.SelectedIndex;
            if (index < 0 || index >= ItemList.Count)
                return null;

            return ItemList.ElementAt(index);
        }

        private void ConnectButtons()
        {
            btnAll.Click += (object sender, RoutedEventArgs e) => { DeactivateFilter(); };
            btnVegetable_Fuit.Click += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Vegetable_Fruit); };
            btnDairy_Products.Click += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Dairy_Product); };
            btnMeat_Fish_Eggs.Click += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Meat_Fish_Eggs); };
            btnDrinks.Click += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Drinks); };
            btnOther.Click += (object sender, RoutedEventArgs e) => { SetFilter(ECategory.Other); };
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

        private bool Filter(Item item)
        {
            if (!m_filterActive) return true;
            return item.Product.Category == m_category;
        }

        private void Refresh()
        {
            ItemList.Clear();
            foreach (Item item in m_content.List)
            {
                if (Filter(item))
                {
                    ItemList.Add(item);
                }
            }
        }
    }
}