using System;

namespace SmartFridge.ContentNS
{
    interface IItemForm
    {
        event Action<Item> Finished;
    }
}
