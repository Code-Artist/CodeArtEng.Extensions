using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    /// <summary>
    /// Provides extended functionality for the <see cref="DataGridView"/> control, including change tracking,
    /// smart editing features, and enhanced clipboard operations. This static class offers methods
    /// to enable advanced controls, manage changes, and customize the behavior of DataGridView objects.
    /// Features include highlighting modified cells, committing or reverting changes, and implementing
    /// efficient editing shortcuts.
    /// </summary>
    public static class DataGridViewEx
    {
        internal class DataGridViewExHandler
        {
            public DataGridView DataGrid { get; private set; }
            public Color ModifiedCellColor { get; set; } = Color.LightYellow;
            public Dictionary<DataGridViewCell, DataGridViewCellTracker> CellTrackers = new Dictionary<DataGridViewCell, DataGridViewCellTracker>();
            public DataGridViewExHandler(DataGridView sender) { DataGrid = sender; }

            public event EventHandler<DataGridViewCellEventArgs> CellValuePasted;
            public void OnCellValuePasted(DataGridViewCell sender)
            {
                DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(sender.ColumnIndex, sender.RowIndex);
                CellValuePasted?.Invoke(this, e);
            }
        }

        internal class DataGridViewCellTracker
        {
            public DataGridViewCell Cell { get; set; }
            public object OriginalValue { get; set; } = null;
            public bool IsModified { get; set; } = false;
        }

        #region [ Internal Methods ]

        private static readonly Dictionary<DataGridView, DataGridViewExHandler> DataGrids = new Dictionary<DataGridView, DataGridViewExHandler>();
        private static DataGridViewExHandler GetDataGridViewExtended(DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (DataGrids.ContainsKey(sender)) return DataGrids[sender];
            return EnableAdvanceControlInt(sender);
        }
        private static DataGridViewExHandler EnableAdvanceControlInt(this DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (DataGrids.ContainsKey(sender)) return DataGrids[sender];
            DataGridViewExHandler ptrData = DataGrids[sender] = new DataGridViewExHandler(sender);

            sender.KeyDown += Sender_KeyDown;
            sender.CellBeginEdit += Sender_CellBeginEdit;
            sender.CellEndEdit += Sender_CellEndEdit;
            sender.RowsRemoved += Sender_RowsRemoved;
            return ptrData;
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Enables advanced control features for a DataGridView.
        /// </summary>
        /// <param name="sender">The DataGridView instance to enable advanced control for.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="sender"/> is null.</exception>
        public static void EnableAdvanceControl(this DataGridView sender)
        {
            EnableAdvanceControlInt(sender);
        }

        /// <summary>
        /// Commits all changes made to the DataGridView.
        /// </summary>
        /// <remarks>
        /// This method clears the tracking of modified cells.
        /// </remarks>
        public static void CommitChanges(this DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);

            foreach (DataGridViewCellTracker t in data.CellTrackers.Where(n => n.Value.IsModified).Select(n => n.Value))
            {
                t.OriginalValue = t.Cell.Value;
                t.IsModified = false;
                t.Cell.Style.BackColor = sender.DefaultCellStyle.BackColor;
            }
        }

        /// <summary>
        /// Reverts all changes made to the DataGridView since the last commit.
        /// </summary>
        /// <remarks>
        /// This method restores the original values of modified cells and clears the tracking.
        /// </remarks>
        public static void RevertChanges(this DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);

            foreach (DataGridViewCellTracker t in data.CellTrackers.Where(n => n.Value.IsModified).Select(n => n.Value))
            {
                t.Cell.Value = t.OriginalValue;
                t.IsModified = false;
                t.Cell.Style.BackColor = sender.DefaultCellStyle.BackColor;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are any uncommitted changes in the DataGridView.
        /// </summary>
        public static bool HasChanges(this DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(sender);

            return dgvEx.CellTrackers.Any(n => n.Value.IsModified == true);
        }

        public static void CellValuePastedEventAdd(this DataGridView sender, EventHandler<DataGridViewCellEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValuePasted += eventHandler;
        }

        public static void CellValuePastedEventRemove(this DataGridView sender, EventHandler<DataGridViewCellEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValuePasted -= eventHandler;
        }

        #endregion

        #region [ DataGridView Events Subscription ]

        private static void Sender_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dg = sender as DataGridView;
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(dg);
            if (dgvEx == null) return;

            if ((e.KeyCode == Keys.Delete) && ((dg.SelectedRows.Count == 0) || !dg.AllowUserToDeleteRows))
            {
                //Delete key shall not be handle when full row delete is possible
                DeleteSelectedCellsContent(dgvEx);
                e.Handled = true;
            }
            else if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        dgvEx.CopyDown();
                        e.Handled = true;
                        break;
                    case Keys.R:
                        dgvEx.CopyRight();
                        e.Handled = true;
                        break;

                    case Keys.V:
                        dgvEx.PasteFromClipboard();
                        e.Handled = true;
                        break;
                }
            }
        }

        private static void Sender_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(dgv);
            if (dgvEx == null) return;

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            dgvEx.BeforeCellChange(dgv[e.ColumnIndex, e.RowIndex]);
        }

        private static void Sender_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(dgv);

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            AfterCellChange(dgvEx, dgv[e.ColumnIndex, e.RowIndex]);
        }

        private static void Sender_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(dgv);

            //Clean up cell tracker objects
            DataGridViewCell[] cells = dgvEx.CellTrackers.Where(n => n.Key.RowIndex == -1).Select(n => n.Key).ToArray();
            foreach (DataGridViewCell c in cells) dgvEx.CellTrackers.Remove(c);
        }
        #endregion


        #region [ Smart Editing ]

        /// <summary>
        /// Delete content of selected cells
        /// </summary>
        private static void DeleteSelectedCellsContent(DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;

            if (dgv.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell c in dgv.SelectedCells)
                {
                    if (!c.ReadOnly && c.OwningColumn.CellType == typeof(DataGridViewTextBoxCell))
                    {
                        dgvEx.BeforeCellChange(c);
                        c.Value = null;
                        dgvEx.AfterCellChange(c);
                    }
                }
            }
        }

        /// <summary>
        /// Copies cell values from top to bottom within the selected range when CTRL+D is pressed.
        /// </summary>
        /// <remarks>
        /// If a single cell or row is selected, it copies the value from the cell above.
        /// If multiple rows are selected, it copies values from top to bottom within the selection.
        /// The method does nothing if the topmost row is selected.
        /// </remarks>
        private static void CopyDown(this DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;

            DataGridViewCell[] selectedCells = dgv.SelectedCells.Cast<DataGridViewCell>().ToArray();
            int startRow = selectedCells.Min(n => n.RowIndex);
            int endRow = selectedCells.Max(n => n.RowIndex);
            int startCol = selectedCells.Min(n => n.ColumnIndex);
            int endCol = selectedCells.Max(n => n.ColumnIndex);
            int totalRow = endRow - startRow + 1;

            if ((selectedCells.Length == 1 || totalRow == 1) && (selectedCells.First().RowIndex > 0))
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    DataGridViewCell cellAbove = dgv[c, startRow - 1];
                    dgv[c, startRow].Value = cellAbove.Value;
                }
            }
            else if (totalRow > 1)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    for (int r = startRow + 1; r <= endRow; r++)
                    {
                        DataGridViewCell cell = dgv[c, r];
                        dgvEx.PasteCellValue(cell, dgv[c - 1, r].Value);
                    }
                }
            }
        }

        /// <summary>
        /// Copies cell values from left to right within the selected range when CTRL+R is pressed.
        /// </summary>
        /// <remarks>
        /// If a single cell or column is selected, it copies the value from the cell to the left.
        /// If multiple columns are selected, it copies values from left to right within the selection.
        /// The method does nothing if the leftmost column is selected.
        /// </remarks>
        private static void CopyRight(this DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;

            DataGridViewCell[] selectedCells = dgv.SelectedCells.Cast<DataGridViewCell>().ToArray();
            int startRow = selectedCells.Min(n => n.RowIndex);
            int endRow = selectedCells.Max(n => n.RowIndex);
            int startCol = selectedCells.Min(n => n.ColumnIndex);
            int endCol = selectedCells.Max(n => n.ColumnIndex);
            int totalCol = endCol - startCol + 1;

            if ((selectedCells.Length == 1 || totalCol == 1) && (selectedCells.First().ColumnIndex > 0))
            {
                for (int r = startRow; r <= endRow; r++)
                {
                    DataGridViewCell cellToLeft = dgv[startCol - 1, r];
                    dgv[startCol, r].Value = cellToLeft.Value;
                }
            }
            else if (totalCol > 1)
            {
                for (int r = startRow; r <= endRow; r++)
                {
                    for (int c = startCol + 1; c <= endCol; c++)
                    {
                        DataGridViewCell cell = dgv[c, r];
                        dgvEx.PasteCellValue(cell, dgv[c - 1, r].Value);
                    }
                }
            }
        }

        private static void PasteFromClipboard(this DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;
            if (dgv.SelectedCells.Count == 0) return;

            //Start paste value from TopLeft Cell of selected cells
            int row = dgv.SelectedCells.Cast<DataGridViewCell>().Min(n => n.RowIndex);
            int col = dgv.SelectedCells.Cast<DataGridViewCell>().Min(n => n.ColumnIndex);

            string clipboardText = Clipboard.GetText();
            if (!clipboardText.Contains('\n') && !clipboardText.Contains('\t'))
            {
                //Clipboard contains single value, paste to all cells
                foreach (DataGridViewCell cell in dgv.SelectedCells)
                    dgvEx.PasteCellValue(cell, clipboardText);
                return;
            }

            string[] lines = clipboardText.Split('\n');
            foreach (string line in lines)
            {
                if (row < dgv.Rows.Count)
                {
                    string[] clipBoardCells = line.Split('\t');
                    for (int i = 0; i < clipBoardCells.Length && col + i < dgv.Columns.Count; i++)
                    {
                        DataGridViewCell ptrCell = dgv[col + i, row];
                        dgvEx.PasteCellValue(ptrCell, clipBoardCells[i]);
                    }
                    row++;
                }
                else
                {
                    break;
                }
            }
            dgv.RefreshEdit();
        }

        private static void PasteCellValue(this DataGridViewExHandler dgvEx, DataGridViewCell ptrCell, object value)
        {
            BeforeCellChange(dgvEx, ptrCell);
            if (ptrCell is DataGridViewCheckBoxCell)
            {
                bool result = false;
                if (bool.TryParse(value.ToString(), out result))
                    ptrCell.Value = result;
            }
            else
                ptrCell.Value = value;
            AfterCellChange(dgvEx, ptrCell);
            dgvEx.OnCellValuePasted(ptrCell);
        }

        #endregion

        #region [ Cell Tracking ]

        private static Color ModifiedCellColor = Color.LightYellow;

        /// <summary>
        /// Represents the change tracking information for a cell.
        /// </summary>
        private static DataGridViewCellTracker GetCellTracker(this DataGridViewExHandler dgvEx, DataGridViewCell cell)
        {
            Debug.WriteLine($"Col = {cell.ColumnIndex}, row = {cell.RowIndex}");
            if (!dgvEx.CellTrackers.ContainsKey(cell)) return null;
            return dgvEx.CellTrackers[cell];
        }

        private static void BeforeCellChange(this DataGridViewExHandler dgvEx, DataGridViewCell c)
        {
            DataGridViewCellTracker ptrTracker = dgvEx.GetCellTracker(c);
            if (ptrTracker == null)
            {
                ptrTracker = new DataGridViewCellTracker() { Cell = c };
                dgvEx.CellTrackers[c] = ptrTracker;
                ptrTracker.OriginalValue = c.Value;
            }
        }

        private static void AfterCellChange(this DataGridViewExHandler dgvEx, DataGridViewCell c)
        {
            DataGridView dgv = dgvEx.DataGrid;

            DataGridViewCellTracker ptrTracker = dgvEx.GetCellTracker(c);
            if (ptrTracker.Cell.Value == null)
                ptrTracker.IsModified = ptrTracker.OriginalValue != null;
            else
                ptrTracker.IsModified = !ptrTracker.Cell.Value.Equals(ptrTracker.OriginalValue);
            ptrTracker.Cell.Style.BackColor = ptrTracker.IsModified ? ModifiedCellColor : dgv.DefaultCellStyle.BackColor;
        }

        public static Color GetModifiedCellColor(this DataGridView sender) => GetDataGridViewExtended(sender).ModifiedCellColor;
        public static void SetModifiedCellColor(this DataGridView sender, Color color) => GetDataGridViewExtended(sender).ModifiedCellColor = color;

        #endregion

    }
}
