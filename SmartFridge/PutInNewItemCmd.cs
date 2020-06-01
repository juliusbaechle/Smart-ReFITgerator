using SmartFridge.ContentNS;
using SmartFridge.ProductNS;

namespace SmartFridge
{
    class PutInNewItemCmd
    {
        public PutInNewItemCmd(Mediator mediator)
        {
            mediator.UserChangedPage += Dispose;
            mediator.ProductOverview.Selected += OnSelected;

            m_mediator = mediator;            
            m_item = new Item();
            
            m_mediator.MainWindow.SetContent(m_mediator.ProductOverview);
        }

        private void Dispose()
        {
            m_mediator.UserChangedPage -= Dispose;
            m_mediator.ProductOverview.Selected -= OnSelected;
            if (m_itemForm != null) m_itemForm.Finished -= OnFinish;
        }

        private void OnSelected(Product product)
        {
            m_item.Product = product;
            m_item.ProductID = product.ID;

            m_itemForm = new ItemForm(m_item);
            m_itemForm.Finished += OnFinish;
            m_mediator.MainWindow.SetContent(m_itemForm);
        }

        private void OnFinish()
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
