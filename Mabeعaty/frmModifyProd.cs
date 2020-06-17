using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mabeعaty
{
    public partial class frmModifyProd : Form
    {
    
        public frmModifyProd()
        {
            
            InitializeComponent();
            //prepare the existing categories in the category combobox
            List<string> sel = new List<string>();
            sel = frmLogin.Stock.select_all(1, "categories");
            for (int i = 0; i < sel.Count(); i++)
            {
                cmbSelCat.Items.Add(sel[i]);
                cmbCat.Items.Add(sel[i]);
            }

        }

        
        // prepare the existing products within the selected category into products combobox
      
        private void cmbSelCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbProd.Items.Clear();
            lblProd.Text = "Product: ";
            lblPrice.Text = "Price: ";
            lblQty.Text = "Quantity: ";
            lblBarcode.Text = "Barcode: ";
            lblCat.Text = "Category: ";
            List<string> products = new List<string>();
            products = frmLogin.Stock.Select_item("PRODUCTS", "NAME", 1, "cat='" + cmbSelCat.SelectedItem.ToString() + "'");
            for (int i = 0; i < products.Count(); i++)
            {
                cmbProd.Items.Add(products[i]);
            }
        
        }
        //display the details of the selected product
        private void cmbProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> detials = new List<string>();
            detials = frmLogin.Stock.Select_item("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", 5, "NAME='" + cmbProd.SelectedItem.ToString() + "'");

            lblProd.Text = "Product: " + detials[0];
            lblPrice.Text = "Price: " + detials[1];
            lblQty.Text = "Quantity: " + detials[2];
            lblBarcode.Text = "Barcode: " + detials[3];
            lblCat.Text = "Category: " + detials[4];
            // dispaly data ito their control
            txtProd.Text = detials[0];
            txtBarcode.Text = detials[3];
            nudPrice.Value = Convert.ToDecimal(detials[1]);
            nudQty.Value = Convert.ToUInt32(detials[2]);
            cmbCat.SelectedItem = detials[4];
        }
        //modify the selected product with the new values 
        private void btnModify_Click_1(object sender, EventArgs e)
        {
           
            string name = "'" + txtProd.Text + "'";
            string barcode = "'" + txtBarcode.Text + "'";
            string price = "'" + nudPrice.Value.ToString() + "'";
            string qty = "'" + nudQty.Value.ToString() + "'";
            string cat = "'" + cmbCat.SelectedItem.ToString() + "'";
            string valus = "NAME=" + name + "," + "PRICE=" + price + "," + "QTY=" + qty + "," + "BARCODE=" + barcode + "," + "CAT=" + cat;
            if (frmLogin.Stock.Update("PRODUCTS",valus, "NAME='" + cmbProd.SelectedItem.ToString() + "'"))
                this.Hide();
            else
                MessageBox.Show("error invalid inputs");

        }
    }
}
