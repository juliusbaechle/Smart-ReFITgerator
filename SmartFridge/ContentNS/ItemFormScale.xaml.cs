using SmartFridge.Arduino;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SmartFridge.ContentNS
{
    /// <summary>
    /// Interaktionslogik für ItemForm.xaml
    /// </summary>
    public partial class ItemFormScale : Page, IItemForm
    {
        public event Action<Item> Finished;

        private Item m_item;
        private ScalePollingHelper m_scaleHelper;

        public ItemFormScale(Item item, IScale scale)
        {
            m_item = item;
            DataContext = item;
            InitializeComponent();
            InitializeExpiryDate();
            SetupScale(scale);
        }

        private void InitializeExpiryDate()
        {
            txtInputDate.LostFocus += OnDateInputFinished;
            var initDT = DateTime.Now.AddDays(m_item.Product.Durability).Date;
            txtInputDate.Text = initDT.ToString("dd.MM.yyyy");
            m_item.ExpiryDate = initDT;
        }

        private void SetupScale(IScale scale)
        {
            m_scaleHelper = new ScalePollingHelper(scale);            
            m_scaleHelper.WeightChanged += (ulong weight) => { 
                txtScaleMeasure.Text = weight + " " + m_item.Unit; 
            };
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

        private void OnWeight(object o, RoutedEventArgs e)
        {
            ulong weight = m_scaleHelper.GetWeightInGrams();
            m_item.Amount = (uint)weight;
            txtWeight.Text = m_item.AmountText;            
        }
    }
}
