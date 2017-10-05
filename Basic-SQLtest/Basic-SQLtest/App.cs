using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Basic_SQLtest.Properties;

namespace
    Basic_SQLtest
{
    public partial class
        App : Form
    {
        Database db = new Database();
        private DataGridView dataGridView1 = new DataGridView();
        List<Produit> l_produit = new List<Produit>();

        public App()
        {
            InitializeComponent();
            initializeDataGridView();
            display();
        }


        public void initializeDataGridView()
        {
            // Set up the DataGridView.
            dataGridView1.Dock = DockStyle.Fill;

            // Automatically generate the DataGridView columns.
            dataGridView1.AutoGenerateColumns = true;

            string query1 = "SELECT Produit.id, Produit.nom, SN.id_nom_produit, SN.num_serie FROM Produit INNER JOIN SN ON Produit.id = SN.id_nom_produit";

            db.GetData(query1, l_produit);

            // Automatically resize the visible rows.
            dataGridView1.AutoSizeRowsMode =
            DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Set the DataGridView control's border.
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;

            // Put the cells in edit mode when user enters them.
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        private void display()
        {
            foreach (Produit item in l_produit)
                dataGridView2.Rows.Add(item.Id, item.Nom, item.Num_serie);
        }



        private void App_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Delete definitively the row ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                int id;
                string num_serie = e.Row.Cells["num_serie"].Value.ToString();
                if (int.TryParse(e.Row.Cells["id"].Value.ToString(), out id))
                    db.deleteSN(id, num_serie);
            }
            else
                e.Cancel = true;
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells["nom"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value != null)
            {
                int id;
                if (!string.IsNullOrEmpty(dataGridView2.Rows[e.RowIndex].Cells["nom"].Value.ToString())
                 && !string.IsNullOrEmpty(dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value.ToString()))
                {
                   db.insertSN(dataGridView2.Rows[e.RowIndex].Cells["nom"].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value.ToString(), l_produit);
                }

            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells["nom"].Value != null && dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value != null)
            {
                if (!string.IsNullOrEmpty(dataGridView2.Rows[e.RowIndex].Cells["nom"].Value.ToString())
                  && !string.IsNullOrEmpty(dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value.ToString()))
                {
                    db.updateProduit(dataGridView2.Rows[e.RowIndex].Cells["nom"].Value.ToString(), dataGridView2.Rows[e.RowIndex].Cells["num_serie"].Value.ToString(), l_produit);
                }
            }
        }
    }
}

