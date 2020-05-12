using SmartFridge.ProductNS;
using System;
using System.Windows.Forms;

namespace SmartFridge {
    public partial class ProductForm : Form {
        public delegate void ProductHandler(Product product);
        public event ProductHandler Finished;

        internal ProductForm()
        {
            InitializeComponent();
        }

        internal ProductForm(Product initialize)
        {
            txtBoxName.Text = initialize.Name;
            cmbBoxCategory.SelectedIndex = (int)initialize.Category;
            cmbBoxQuantity.SelectedIndex = (int)initialize.Quantity;
            inpDurability.Value = initialize.Durability;
            inpEnergy.Value = initialize.Energy;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            product.Name = txtBoxName.Text;
            product.Category = (ECategory)cmbBoxCategory.SelectedIndex;
            product.Quantity = (EQuantity)cmbBoxQuantity.SelectedIndex;
            product.Durability = (ushort)inpDurability.Value;
            product.Energy = (ushort)inpEnergy.Value;
            if (product.IsValid()) Finished?.Invoke(product);
        }
    }
}
