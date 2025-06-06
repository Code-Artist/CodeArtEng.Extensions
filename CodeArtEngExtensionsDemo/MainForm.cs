﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            Dgv.CellValueChangingEventAdd(valueChanging);
            Dgv.CellValueChangedEventAdd(valueChanged);
        }

        private void valueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Trace.WriteLine("Value Changed: " + Dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString());
        }

        private void valueChanging(object sender, DataGridViewCellExEventArgs e)
        {
            Trace.WriteLine($"Value updating at cell {e.RowIndex}, {e.ColumnIndex} = {e.NewValue?.ToString()}, " +
                $"Current Value = {Dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString()}");
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
            Trace.WriteLine("End edit triggered.");
        }

        private void BtCount_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("Modified Cells = " + Dgv.ModifiedCells().Count());
        }

        private void BtAddRow_Click(object sender, EventArgs e)
        {
            int r = Dgv.Rows.Add();
            Dgv.SetRowModified(r);
        }

        private void BtSetModified_Click_1(object sender, EventArgs e)
        {
            Dgv.SetCellModified(Dgv.CurrentCell.RowIndex, Dgv.CurrentCell.ColumnIndex);
        }
    }
}
