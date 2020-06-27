using SmartFridge.ContentNS;
using SmartFridge.ProductNS;
using System.Windows.Controls;

namespace SmartFridge
{
    class PutInNewItemCmd
    {
        public PutInNewItemCmd(Mediator mediator)
        {
            m_mediator = mediator;            
            m_item = new Item();

            mediator.UserChangedPage += Dispose;

            m_mediator.MainWindow.SetContent(m_mediator.ProductOverview);
            mediator.ProductOverview.SelectedProduct += OnProductSelected;
        }

        private void Dispose()
        {
            m_mediator.UserChangedPage -= Dispose;
            m_mediator.ProductOverview.SelectedProduct -= OnProductSelected;
            if (m_itemForm != null) m_itemForm.Finished -= OnItemFinished;
        }

        private void OnProductSelected(Product product)
        {
            m_item.Product = product;
            m_item.ProductID = product.ID;

            m_itemForm = m_mediator.CreateItemForm(m_item);
            m_itemForm.Finished += OnItemFinished;
            m_mediator.MainWindow.SetContent((Page)m_itemForm);
        }

        private void OnItemFinished(Item item)
        {
            if (!item.IsValid()) return;

            m_mediator.Content.Add(item);
            m_mediator.MainWindow.SetContent(m_mediator.ContentOverview);
            Dispose();
        }

        private Item m_item;
        private Mediator m_mediator;
        private IItemForm m_itemForm;
    }
}
