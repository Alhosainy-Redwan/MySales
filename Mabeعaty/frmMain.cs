using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Mabeعaty
{
    public partial class frmMain : Form
    {
        public frmMain()
        {        
            InitializeComponent();
            this.MaximizedBounds = Screen.GetWorkingArea(this);
            this.MinimumSize = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width,
                                                       Screen.PrimaryScreen.WorkingArea.Height);
            this.CenterToScreen();
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width,
                                                Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            refresh_cat();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        //private void frmMain_Load(object sender, EventArgs e)
        //{
        //    //this.CenterToScreen();            
        //    //this.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width, 
        //    //                                    Screen.PrimaryScreen.WorkingArea.Height);            
        //}    

        #region ToolStrip
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmRemoveCat().ShowDialog();
        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmRemoveProd().ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            new Mabeعaty.frmAbout().ShowDialog();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmModifyCat(this).ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmAddCat(this).ShowDialog();
        }


        private void modifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmModifyProd().ShowDialog();
        }

        private void addToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Mabeعaty.frmAddProd().ShowDialog();
        }
    #endregion

        private void dgvBill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if click is on new row or header row
            if (e.RowIndex == dgvBill.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dgvBill.Columns["Remove"].Index)
            {
                dgvBill.Rows.RemoveAt(e.RowIndex);

                //TODO: Update other controls
                //DONE: In other event handler
                //...
            }
        }

        private void dgvBill_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dgvBill.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dgvBill.Columns["Qty"].Index)
            {
                dgvBill.Rows[e.RowIndex].Cells["Qty"].Value = Check_QTY(dgvBill.Rows[e.RowIndex].Cells["Product"].Value.ToString(), Convert.ToUInt32(dgvBill.Rows[e.RowIndex].Cells["Qty"].Value));
                dgvBill.Rows[e.RowIndex].Cells["TotPrice"].Value = 
                        Convert.ToSingle(dgvBill.Rows[e.RowIndex].Cells["Qty"].Value) * 
                        Convert.ToSingle(dgvBill.Rows[e.RowIndex].Cells["UnitPrice"].Value);
                UpdateNumOfProd();
            }

            UpdateTotPrice();
        }

        private void dgvBill_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateNumOfProd();
            UpdateTotPrice();
        }
        private void dgvBill_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateNumOfProd();
            UpdateTotPrice();
        }
        private void UpdateTotPrice()
        {
            //Sum total prices
            float TotPrice = 0f;
            for (int i = 0; i < dgvBill.Rows.Count; ++i)
            {
                TotPrice += Convert.ToSingle(dgvBill.Rows[i].Cells["TotPrice"].Value);
            }
            //0:#.00 or {0:N2}
            lblTotPrice.Text = "Total Price:  " + string.Format("{0:C2}", 
                                                    Convert.ToDecimal(TotPrice));
        }
        private void UpdateNumOfProd()
        {
            int prodnum = 0;
            for (int i = 0; i < dgvBill.Rows.Count; ++i)
            {
                prodnum += Convert.ToInt32(dgvBill.Rows[i].Cells["Qty"].Value);
            }
            lblNumOfProd.Text = "# Products:  " + prodnum.ToString();
        }
        private void AddProdToBill(string item, float price, uint Qty)
        {
            //Check if the product already exist in the bill
            for (int i = 0; i < dgvBill.Rows.Count; ++i)
            {
                if ((dgvBill.Rows[i].Cells["Product"].Value).ToString() == item)
                {
                    dgvBill.Rows[i].Cells["Qty"].Value = Convert.ToInt32(dgvBill.Rows[i].Cells["Qty"].Value) + Qty;
                    return;
                }
            }
            float TotPrice = price * Convert.ToSingle(Qty);
            this.dgvBill.Rows.Add(item, price, Qty, TotPrice);
        }

        // display existing products within selected category
        private void lstCat_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstCat.SelectedItem != null)
            {
                lstProd.Items.Clear();
                lblProd.Text = "Product: ";
                lblPrice.Text = "Price: ";
                lblStock.Text = "In Stock: ";
                lblCode.Text = "Code: ";
                lblCat.Text = "Category: ";
                List<string> products = new List<string>();
                products = frmLogin.Stock.Select_item("PRODUCTS", "NAME", 1, "cat='" + lstCat.SelectedItem.ToString() + "'");
                for (int i = 0; i < products.Count(); i++)
                {
                    lstProd.Items.Add(products[i]);
                }
            }
        }
        //display the details of the selcted prodcut
        private void lstProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProd.SelectedItem != null)
            {
                List<string> detials = new List<string>();
                detials = frmLogin.Stock.Select_item("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", 5, "NAME='" + lstProd.SelectedItem.ToString() + "'");
                lblProd.Text = "Product: " + detials[0];
                lblPrice.Text = "Price: " + detials[1];
                lblStock.Text = "In Stock: " + detials[2];
                lblCode.Text = "Code: " + detials[3];
                lblCat.Text = "Category: " + detials[4];
            }
         
        }
        /// <summary>
        /// refresh the category list with the categrois values
        /// </summary>
        public void refresh_cat()
        {
            lstCat.Items.Clear();
            List<string> sel = new List<string>();
            sel = frmLogin.Stock.select_all(1, "categories");
            for (int i = 0; i < sel.Count(); i++)
            {
                lstCat.Items.Add(sel[i]);
            }
        }
        //add selected product to bill in case of double click in it
        private void lstProd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lstProd.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches && index != null)
            {
                List<string> detials = new List<string>();
                detials = frmLogin.Stock.Select_item("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", 5, "NAME='" + lstProd.SelectedItem.ToString() + "'");
                
                float Price = float.Parse(detials[1],System.Globalization.CultureInfo.InvariantCulture);
                uint qnt=Convert.ToUInt32( detials[2]);
                if(qnt ==0)
                    MessageBox.Show(detials[0]+" is out of the stock");
                else
                AddProdToBill(detials[0], Price, 1);
            }
        }
        /// <summary>
        /// check if the quantity be chosen are availble or not 
        /// if available return the given quantity
        /// if not return the available quantity
        /// </summary>
        /// <param name="product"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
         public uint Check_QTY(string product,uint qty)
        {
            List<string> detials = new List<string>();
            detials = frmLogin.Stock.Select_item("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", 5, "NAME='" + product+ "'");
            uint q = Convert.ToUInt32(detials[2]);
            if (qty < q)
                return qty;
            else
            {
                MessageBox.Show("there  are  only "+ detials[2] +" of "+product+" in the stock");
                return q;

            }

        }
        /// <summary>
        /// finally out the product from the stock
        /// </summary>
        /// <param name="product"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool out_stock(string product, uint qty)
        {
            List<string> detials = new List<string>();
            detials = frmLogin.Stock.Select_item("PRODUCTS", "NAME,PRICE,QTY,BARCODE,CAT", 5, "NAME='" + product + "'");
            uint q = Convert.ToUInt32(detials[2]);
            q = q - qty;
            string valus = "QTY='" + q.ToString() + "'";
            if (frmLogin.Stock.Update("PRODUCTS", valus, "NAME='" + product + "'"))
                return true;
            else
            {
                return false;
            }

        }
    }
}
