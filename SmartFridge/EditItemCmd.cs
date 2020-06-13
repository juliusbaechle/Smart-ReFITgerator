using SmartFridge.ContentNS;

namespace SmartFridge
{
    class EditItemCmd
    {
        public EditItemCmd(Item item, Mediator mediator)
        {
            m_mediator = mediator;
            m_item = new Item(item);

            mediator.UserChangedPage += Dispose;

            m_itemForm = new ItemForm(m_item);
            m_itemForm.Finished += OnItemFinished;
            m_mediator.MainWindow.SetContent(m_itemForm);
        }

        private void Dispose()
        {
            m_mediator.UserChangedPage -= Dispose;
            if (m_itemForm != null) m_itemForm.Finished -= OnItemFinished;
        }

        private void OnItemFinished()
        {
            if (!m_item.IsValid()) return;

            m_mediator.Content.AddOrEdit(m_item);
            m_mediator.MainWindow.SetContent(m_mediator.ContentOverview);
            Dispose();
        }
                
        private Item m_item;
        private ItemForm m_itemForm;
        private Mediator m_mediator;
    }
}
