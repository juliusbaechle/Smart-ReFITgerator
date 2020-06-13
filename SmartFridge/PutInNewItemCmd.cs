using SmartFridge.ContentNS;
using SmartFridge.ProductNS;

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

            m_itemForm = new ItemForm(m_item);
            m_itemForm.Finished += OnItemFinished;
            m_mediator.MainWindow.SetContent(m_itemForm);
        }

        private void OnItemFinished()
        {
            if (!m_item.IsValid()) return;

            m_mediator.Content.Add(m_item);
            m_mediator.MainWindow.SetContent(m_mediator.ContentOverview);
            Dispose();
        }

        private Item m_item;
        private Mediator m_mediator;
        private ItemForm m_itemForm;
    }
}
