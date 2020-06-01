using SmartFridge.ProductNS;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartFridge.ContentNS
{
    /// <summary>
    /// Interaktionslogik für ItemForm.xaml
    /// </summary>
    public partial class ItemForm : Page
    {
        public Action Finished;

        private Item Item;

        public ItemForm(Item item)
        {
            Item = item;
            DataContext = item;
            InitializeComponent();

            txtInputDate.LostFocus += OnDateInputFinished;
            txtInputDate.Text = DateTime.Now.AddDays(item.Product.Durability).Date.ToString("dd.MM.yyyy");

            sliderAmount.ValueChanged += OnSliderChanged;
        }

        private void OnConfirm(object o, RoutedEventArgs e)
        {
            Finished?.Invoke();
        }

        private void OnDateInputFinished(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = txtInputDate.Text;
                DateTime dt = DateTime.Parse(text);
                Item.ExpiryDate = dt;
            }
            catch
            {
                //Parsing Fehler
                return;
            }
        }

        private void OnSliderChanged(object sender, RoutedEventArgs e)
        {
            switch (Item.Product.Quantity)
            {
                case EQuantity.Count:
                    Item.Amount = (UInt32)sliderAmount.Value / 20;
                    txtAmount.Text = $"{Item.Amount} %";
                    break;

                case EQuantity.Grams:
                    Item.Amount = (UInt32)sliderAmount.Value;
                    txtAmount.Text = $"{Item.Amount} g";
                    break;

                case EQuantity.Milliliters:
                    Item.Amount = (UInt32)sliderAmount.Value;
                    txtAmount.Text = $"{Item.Amount} ml";
                    break;
            }
        }
    }
}
