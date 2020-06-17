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
    public partial class frmAddProd : Form
    {
        public frmAddProd()
        {
            InitializeComponent();
            List<string> sel = new List<string>();
            //prepare the existing categories in the category combobox
            sel = frmLogin.Stock.select_all(1, "categories");
            for(int i= 0; i < sel.Count() ;i++)
            {
                cmbCat.Items.Add(sel[i]);
            }
        }
        //insert the new product in database
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = "'" + textBox1.Text + "'";
            string barcode = "'" + textBox2.Text + "'";
            string price = "'" + numericUpDown1.Value.ToString() + "'";
            string qty = "'" + numericUpDown2.Value.ToString() + "'";
            string cat = "'" + cmbCat.SelectedItem.ToString() + "'";
            string valus = name + "," + price + "," + qty + "," + barcode + "," + cat;
            if (frmLogin.Stock.Insert("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", valus))
                this.Hide();
            else
                MessageBox.Show("error invalid inputs");

        }
    }
}
