﻿using System;
using System.Windows.Controls;
using SmartFridge.ContentNS;
using SmartFridge.ProductNS;
using SmartFridge.Arduino;

namespace SmartFridge
{
    // Bindeglied zwischen Logik und View
    // Erzeugt Fenster in Abhängigkeit der Members wie z.B. Products 
    // und setzt diese im MainWindow

    public enum EPage
    {
        Home,
        Products,
        Content,
        Nutrition,
        Messages,
        Shopping
    }

    class Mediator
    {
        public Action UserChangedPage;

        public Mediator(MainWindow window, Products products, Content content, IScale scale)
        {
            MainWindow = window;
            MainWindow.OpenPage += ShowPage;

            Products = products;
            ProductOverview = CreateProductOverview(products);

            Content = content;            
            ContentOverview = CreateContentOverview(content);

            m_scale = scale;
        }

        public void ShowPage(EPage page) {
            switch (page)
            {
                case EPage.Products:
                    MainWindow.SetContent(ProductOverview);
                    break;

                case EPage.Content:
                    MainWindow.SetContent(ContentOverview);
                    break;

                case EPage.Home:
                    MainWindow.SetContent(new ImagePage("home.jpg"));
                    break;

                case EPage.Nutrition:
                    MainWindow.SetContent(new ImagePage("nutrition.jpg"));
                    break;

                case EPage.Messages:
                    MainWindow.SetContent(new ImagePage("messages.jpg"));
                    break;

                case EPage.Shopping:
                    MainWindow.SetContent(new ImagePage("shopping.jpg"));
                    break;

                default:
                    MainWindow.SetContent(new Page());
                    break;
            }

            UserChangedPage?.Invoke();
        }

        private ProductOverview CreateProductOverview(Products products)
        {
            var productOverview = new ProductOverview(products);
            productOverview.EditProduct += (Product product) => { MainWindow.SetContent(CreateProductForm(product)); };
            productOverview.DeleteProduct += Products.Delete;
            productOverview.SelectedProduct += Products.Selected;
            productOverview.AddProduct += () => { MainWindow.SetContent(CreateProductForm(null)); };
            return productOverview;
        }

        private ProductForm CreateProductForm(Product product)
        {
            var productForm = new ProductForm();

            if (product == null) 
                productForm.DataContext = new Product();
            else 
                productForm.DataContext = new Product(product);

            productForm.Finished += (Product) => {
                Products.AddOrEdit(Product);
                ShowPage(EPage.Products);
            };

            return productForm;
        }

        public IItemForm CreateItemForm(Item item)
        {
            if(m_scale.Connected && item.Product.Quantity != EQuantity.Count)
                return new ItemFormScale(item, m_scale);
            else
                return new ItemFormManual(item);
        }

        private ContentOverview CreateContentOverview(Content content)
        {
            var contentOverview = new ContentOverview(content);
            contentOverview.Add += () => { new PutInNewItemCmd(this); };
            contentOverview.Edit += (Item) => { new EditItemCmd(Item, this); };
            contentOverview.Delete += Content.Delete;
            return contentOverview;
        }
        
        public Products Products { get; private set; }
        public Content Content { get; private set; }

        public MainWindow MainWindow { get; private set; }
        public ProductOverview ProductOverview { get; private set; }
        public ContentOverview ContentOverview { get; private set; }

        public IScale m_scale;
    }
}
