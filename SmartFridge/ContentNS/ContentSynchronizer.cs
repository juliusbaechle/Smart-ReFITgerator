using System.Collections.Generic;
using System.Windows;

namespace SmartFridge.ContentNS
{
    class ContentSynchronizer : ISynchronizer
    {
        private Content m_content;
        private DBContent m_db;

        private readonly List<Item> m_deletedItems = new List<Item>();
        private readonly List<Item> m_savedItems = new List<Item>();

        public ContentSynchronizer(Content content, DBContent remoteDbContent)
        {
            m_content = content;
            m_db = remoteDbContent;

            m_content.Added += m_savedItems.Add;
            m_content.Deleted += m_deletedItems.Add;
        }

        public void Synchronize()
        {
            try
            {
                OpenConnection();
                Push();
                Pull();
            }
            finally
            {
                // werden auch im Offline Fall zurückgesetzt
                //   -> Änderungen werden zurückgesetzt
                m_deletedItems.Clear();
                m_savedItems.Clear();
            }
        }

        private void OpenConnection()
        {
            if (m_db.DbConnection.State != System.Data.ConnectionState.Open)
            {
                m_db.DbConnection.Open();
            }
        }

        private void Push()
        {
            // Jedes gelöschte / gespeicherte Item in der Remote Datenbank löschen / speichern
            m_deletedItems.ForEach(m_db.Delete);
            m_savedItems.ForEach(m_db.Save);
        }

        private void Pull()
        {
            var remoteItems = m_db.LoadAll();
            var localItems = new List<Item>(m_content.List);
            var deletedItems = GetDeletedItems(localItems, remoteItems);
            var changedOrCreatedItems = GetCreatedOrChangedItems(localItems, remoteItems);

            // Oberfläche darf nur im Main-Thread (GUI Thread) verändert werden
            Application.Current.Dispatcher.Invoke(() => {
                foreach (Item item in deletedItems)
                    m_content.Delete(item);

                foreach (Item item in changedOrCreatedItems)
                    m_content.AddOrEdit(item);
            });
        }

        List<Item> GetDeletedItems(List<Item> localItems, List<Item> remoteItems)
        {
            List<Item> deletedItems = new List<Item>();

            foreach (Item localItem in localItems)
            {
                // Findet Item aus remoteItems, welches dieselbe ID wie localItem hat
                // Gibt null zurück wenn kein zugehöriges Remote-Item gefunden wurde
                var congruentRemoteItem = remoteItems.Find(item => item.ID == localItem.ID);

                // remoteItem wurde gelöscht
                if (congruentRemoteItem == null)
                    deletedItems.Add(localItem);
            }

            return deletedItems;
        }

        List<Item> GetCreatedOrChangedItems(List<Item> localItems, List<Item> remoteItems)
        {
            List<Item> changedOrCreatedItems = new List<Item>();

            foreach (Item remoteItem in remoteItems)
            {
                // Findet Item aus localItems, welches dieselbe ID wie remoteItem hat
                // Gibt null zurück wenn kein zugehöriges, lokales Item gefunden wurde
                var congruentLocalItem = localItems.Find(item => item.ID == remoteItem.ID);

                // remoteItem wurde neu erstellt
                if (congruentLocalItem == null)
                    changedOrCreatedItems.Add(remoteItem);

                // remoteItem verändert
                if (!congruentLocalItem.ValueEqual(remoteItem))
                    changedOrCreatedItems.Add(remoteItem);
            }

            return changedOrCreatedItems;
        }
    }
}
