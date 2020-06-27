using SmartFridge.ProductNS;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartFridge.ContentNS
{
    /// <summary>
    /// Interaktionslogik für ItemForm.xaml
    /// </summary>
    public partial class ItemFormManual : Page, IItemForm
    {
        public event Action<Item> Finished;

        private Item m_item;

        public ItemFormManual(Item item)
        {
            m_item = item;
            DataContext = item;
            InitializeComponent();
            InitializeExpiryDate();
            InitializeAmountSlider();
        }

        private void InitializeExpiryDate()
        {
            txtInputDate.LostFocus += OnDateInputFinished;
            var initDT = DateTime.Now.AddDays(m_item.Product.Durability).Date;
            txtInputDate.Text = initDT.ToString("dd.MM.yyyy");
            m_item.ExpiryDate = initDT;
        }

        private void InitializeAmountSlider()
        {
            sliderAmount.ValueChanged += OnSliderChanged;
            if (m_item.Product.Quantity == EQuantity.Count)
                sliderAmount.Value = sliderAmount.Maximum;
            else
                sliderAmount.Value = 500;
        }

        private void OnConfirm(object o, RoutedEventArgs e)
        {
            Finished?.Invoke(m_item);
        }

        private void OnDateInputFinished(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = txtInputDate.Text;
                DateTime dt = DateTime.Parse(text);
                m_item.ExpiryDate = dt;
            }
            catch
            {
                InitializeExpiryDate();
            }
        }

        private void OnSliderChanged(object sender, RoutedEventArgs e)
        {
            uint amount = 0;
            if(m_item.Product.Quantity == EQuantity.Count)
                amount = (UInt32)(sliderAmount.Value * 100 / sliderAmount.Maximum);
            else
                amount = (UInt32)sliderAmount.Value;

            m_item.Amount = amount;
            txtAmount.Text = m_item.AmountText;
        }
    }
}
