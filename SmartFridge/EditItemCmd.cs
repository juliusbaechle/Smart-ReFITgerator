using SmartFridge.ContentNS;
using System.Windows.Controls;

namespace SmartFridge
{
    class EditItemCmd
    {
        public EditItemCmd(Item item, Mediator mediator)
        {
            m_mediator = mediator;
            m_item = new Item(item);

            mediator.UserChangedPage += Dispose;

            m_itemForm = m_mediator.CreateItemForm(m_item);
            m_itemForm.Finished += OnItemFinished;
            m_mediator.MainWindow.SetContent((Page)m_itemForm);
        }

        private void Dispose()
        {
            m_mediator.UserChangedPage -= Dispose;
            if (m_itemForm != null) m_itemForm.Finished -= OnItemFinished;
        }

        private void OnItemFinished(Item item)
        {
            if (!item.IsValid()) return;

            m_mediator.Content.AddOrEdit(item);
            m_mediator.MainWindow.SetContent(m_mediator.ContentOverview);
            Dispose();
        }
                
        private Item m_item;
        private IItemForm m_itemForm;
        private Mediator m_mediator;
    }
}
