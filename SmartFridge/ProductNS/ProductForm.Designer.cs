namespace SmartFridge {
    partial class ProductForm {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent() {
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inpEnergy = new System.Windows.Forms.NumericUpDown();
            this.inpDurability = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbBoxCategory = new System.Windows.Forms.ComboBox();
            this.cmbBoxQuantity = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.inpEnergy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inpDurability)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(122, 5);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(100, 20);
            this.txtBoxName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Produktname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Energie";
            // 
            // inpEnergy
            // 
            this.inpEnergy.Location = new System.Drawing.Point(122, 35);
            this.inpEnergy.Name = "inpEnergy";
            this.inpEnergy.Size = new System.Drawing.Size(100, 20);
            this.inpEnergy.TabIndex = 4;
            // 
            // inpDurability
            // 
            this.inpDurability.Location = new System.Drawing.Point(122, 64);
            this.inpDurability.Name = "inpDurability";
            this.inpDurability.Size = new System.Drawing.Size(100, 20);
            this.inpDurability.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Haltbarkeit in Tagen";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Kategorie";
            // 
            // cmbBoxCategory
            // 
            this.cmbBoxCategory.FormattingEnabled = true;
            this.cmbBoxCategory.Items.AddRange(new object[] {
            "-",
            "",
            "Obst",
            "",
            "Gemüse",
            "",
            "Fleisch / Fisch",
            "",
            "Milchprodukt",
            "",
            "Frühstück",
            "",
            "Andere"});
            this.cmbBoxCategory.Location = new System.Drawing.Point(122, 91);
            this.cmbBoxCategory.Name = "cmbBoxCategory";
            this.cmbBoxCategory.Size = new System.Drawing.Size(100, 21);
            this.cmbBoxCategory.TabIndex = 8;
            // 
            // cmbBoxQuantity
            // 
            this.cmbBoxQuantity.FormattingEnabled = true;
            this.cmbBoxQuantity.Items.AddRange(new object[] {
            "-",
            "Gramm",
            "Anzahl",
            "Milliliter"});
            this.cmbBoxQuantity.Location = new System.Drawing.Point(122, 118);
            this.cmbBoxQuantity.Name = "cmbBoxQuantity";
            this.cmbBoxQuantity.Size = new System.Drawing.Size(100, 21);
            this.cmbBoxQuantity.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Mengenangabe";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(71, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 187);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbBoxQuantity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBoxCategory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inpDurability);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inpEnergy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxName);
            this.Name = "ProductForm";
            this.Text = "Produkt Formular";
            ((System.ComponentModel.ISupportInitialize)(this.inpEnergy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inpDurability)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown inpEnergy;
        private System.Windows.Forms.NumericUpDown inpDurability;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBoxCategory;
        private System.Windows.Forms.ComboBox cmbBoxQuantity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnOK;
    }
}

