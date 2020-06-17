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
    public partial class frmRemoveProd : Form
    {
        public frmRemoveProd()
        {
            InitializeComponent();
            //prepare the existing categories in the category combobox
            List<string> sel = new List<string>();
            sel = frmLogin.Stock.select_all(1, "categories");
            for (int i = 0; i < sel.Count(); i++)
            {
                cmbCat.Items.Add(sel[i]);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //delete the selected product
        private void btnRemove_Click(object sender, EventArgs e)
        {
            string del = cmbProd.SelectedItem.ToString();
            if (frmLogin.Stock.Delete("PRODUCTS", "NAME='" + del + "'"))
                this.Hide();
            else
                MessageBox.Show("error invalid inputs");

        }
        //prepare the existing products within the selected category in the products combobox
        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {

            cmbProd.Items.Clear();
            List<string> products = new List<string>();
            products = frmLogin.Stock.Select_item("PRODUCTS", "NAME", 1, "cat='" + cmbCat.SelectedItem.ToString() + "'");
            for (int i = 0; i < products.Count(); i++)
            {
                cmbProd.Items.Add(products[i]);
            }
        }
    }
}
