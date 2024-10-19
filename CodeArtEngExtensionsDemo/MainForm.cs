using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeArtEngExtensionsDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitDgv();

            this.SetDoubleBufferingForAllControls(true);
        }

        private void InitDgv()
        {
            Random rand = new Random();
            for (int x = 0; x < 20; x++)
            {
                DataGridViewRow ptrRow = Dgv.Rows[Dgv.Rows.Add()];
                foreach (DataGridViewCell cell in ptrRow.Cells)
                {
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        cell.Value = rand.NextDouble() > 0.5;
                    }
                    else cell.Value = rand.Next(100);
                }
            }
        }

        private void DgvEnable_Click(object sender, EventArgs e)
        {
            Dgv.EnableAdvanceControl();
            Dgv.CellValuePastedEventAdd(valuePasted);
        }

        private void valuePasted(object sender, DataGridViewCellEventArgs e)
        {
            Trace.WriteLine($"Value pasted at cell {e.RowIndex}, {e.ColumnIndex} = {Dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()}");
        }

        private void DgvCommit_Click(object sender, EventArgs e)
        {
            Dgv.CommitChanges();
        }

        private void DgvRevert_Click(object sender, EventArgs e)
        {
            Dgv.RevertChanges();
        }

        private void Dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
