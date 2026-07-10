using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace System.Windows.Forms
{
    public class DataGridViewCellExEventArgs : DataGridViewCellEventArgs
    {
        /// <summary>
        /// New input value, update value if required.
        /// </summary>
        public object NewValue { get; set; }
        /// <summary>
        /// Cancel change, revert to old value
        /// </summary>
        public bool Cancel { get; set; } = false;
        public DataGridViewCellExEventArgs(int columnIndex, int rowIndex, object newValue)
            : base(columnIndex, rowIndex)
        {
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Provides extended functionality for the <see cref="DataGridView"/> control, including change tracking,
    /// smart editing features, and enhanced clipboard operations. This static class offers methods
    /// to enable advanced controls, manage changes, and customize the behavior of DataGridView objects.
    /// Features include highlighting modified cells, committing or reverting changes, and implementing
    /// efficient editing shortcuts.
    /// </summary>
    public static class DataGridViewEx
    {
        /// <summary>
        /// Represents a handler for extended functionality of a <see cref="DataGridView"/> control.
        /// </summary>
        internal class DataGridViewExHandler
        {
            public DataGridView DataGrid { get; private set; }
            public Color ModifiedCellColor { get; set; } = Color.LightYellow;
            public Dictionary<DataGridViewCell, DataGridViewCellTracker> CellTrackers = new Dictionary<DataGridViewCell, DataGridViewCellTracker>();
            public DataGridViewExHandler(DataGridView sender) { DataGrid = sender; }
            /// <summary>
            /// Cell value chaging event when cell value is pasted
            /// </summary>
            public event EventHandler<DataGridViewCellExEventArgs> CellValueChanging;
            public event EventHandler<DataGridViewCellEventArgs> CellValueChanged;

            public void OnCellValueChanging(DataGridViewCellExEventArgs arg)
            {
                CellValueChanging?.Invoke(DataGrid, arg);
            }

            public void OnCellValueChanged(DataGridViewCellEventArgs arg)
            {
                CellValueChanged?.Invoke(DataGrid, arg);
            }
        }

        /// <summary>
        /// Represents a tracker for a cell in a <see cref="DataGridView"/> control.
        /// </summary>
        internal class DataGridViewCellTracker
        {
            public DataGridViewCell Cell { get; set; }
            /// <summary>
            /// Original value
            /// </summary>
            public object OriginalValue { get; set; } = null;
            /// <summary>
            /// Cell is dirty, contain changes.
            /// </summary>
            public bool IsModified { get; set; } = false;
            /// <summary>
            /// Cell modified state set manually by user, revert changes is not possible.
            /// </summary>
            public bool CanRevert { get; set; } = true;
        }

        #region [ Internal Methods ]

        private static readonly Dictionary<DataGridView, DataGridViewExHandler> DataGrids = new Dictionary<DataGridView, DataGridViewExHandler>();
        private static DataGridViewExHandler GetDataGridViewExtended(DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            if (DataGrids.ContainsKey(sender)) return DataGrids[sender];
            return EnableAdvanceControlInt(sender);
        }
        /// <summary>
        /// Enables advanced control features for a DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
                if (!t.CanRevert) continue;
                t.Cell.Value = t.OriginalValue;
                t.IsModified = false;
                t.Cell.Style.BackColor = sender.DefaultCellStyle.BackColor;
            }
        }

        /// <summary>
        /// Manually set cell state as modified. No changes for cell which already in modified state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cell"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetCellModified(this DataGridView sender, int rowIndex, int cellIndex)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(sender);
            DataGridViewCell cell = dgvEx.DataGrid.Rows[rowIndex].Cells[cellIndex];

            DataGridViewCellTracker ptrTracker = dgvEx.GetCellTracker(cell);
            if (ptrTracker == null)
            {
                ptrTracker = new DataGridViewCellTracker() { Cell = cell };
                dgvEx.CellTrackers[cell] = ptrTracker;
            }

            if (!ptrTracker.IsModified)
            {
                ptrTracker.IsModified = true;
                ptrTracker.CanRevert = false;
            }
            AfterCellChange(dgvEx, cell);
        }

        /// <summary>
        /// Manually set cell state as modified. No changes for cell which already in modified state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rowIndex"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void SetRowModified(this DataGridView sender, int rowIndex)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(sender);
            DataGridViewRow row = dgvEx.DataGrid.Rows[rowIndex];
            foreach (DataGridViewCell cell in row.Cells)
            {
                SetCellModified(sender, cell.RowIndex, cell.ColumnIndex);
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

        /// <summary>
        /// Gets the modified cell color for the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static Color GetModifiedCellColor(this DataGridView sender) => GetDataGridViewExtended(sender).ModifiedCellColor;
        /// <summary>
        /// Sets the modified cell color for the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="color"></param>
        public static void SetModifiedCellColor(this DataGridView sender, Color color) => GetDataGridViewExtended(sender).ModifiedCellColor = color;

        /// <summary>
        /// Gets the list of modified cells in the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataGridViewCell[] ModifiedCells(this DataGridView sender)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler dgvEx = GetDataGridViewExtended(sender);
            return dgvEx.CellTrackers.Where(n => n.Value.IsModified).Select(n => n.Value.Cell).ToArray();
        }

        /// <summary>
        /// Subscribe to CellValuePasted event which is triggered when a cell value is pasted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CellValueChangingEventAdd(this DataGridView sender, EventHandler<DataGridViewCellExEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValueChanging += eventHandler;
        }

        /// <summary>
        /// Unsubscribe from CellValuePasted event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CellValueChangingEventRemove(this DataGridView sender, EventHandler<DataGridViewCellExEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValueChanging -= eventHandler;
        }

        public static void CellValueChangedEventAdd(this DataGridView sender, EventHandler<DataGridViewCellEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValueChanged += eventHandler;
        }

        public static void CellValueChangedEventRemove(this DataGridView sender, EventHandler<DataGridViewCellEventArgs> eventHandler)
        {
            if (sender == null) throw new ArgumentNullException("sender");
            DataGridViewExHandler data = GetDataGridViewExtended(sender);
            data.CellValueChanged -= eventHandler;
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
                        dgvEx.ChangeCellValue(c, null);
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
                    dgvEx.ChangeCellValue(dgv[c, startRow], cellAbove.Value);
                }
            }
            else if (totalRow > 1)
            {
                for (int c = startCol; c <= endCol; c++)
                {
                    for (int r = startRow + 1; r <= endRow; r++)
                    {
                        DataGridViewCell cell = dgv[c, r];
                        dgvEx.ChangeCellValue(cell, dgv[c, r - 1].Value);
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
                    dgvEx.ChangeCellValue(dgv[startCol, r], cellToLeft.Value);
                }
            }
            else if (totalCol > 1)
            {
                for (int r = startRow; r <= endRow; r++)
                {
                    for (int c = startCol + 1; c <= endCol; c++)
                    {
                        DataGridViewCell cell = dgv[c, r];
                        dgvEx.ChangeCellValue(cell, dgv[c - 1, r].Value);
                    }
                }
            }
        }

        private static void PasteFromClipboard(this DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;
            if (dgv.SelectedCells.Count == 0) return;

            DataGridViewCell[] selCells = GetSelectedVisibleCells(dgvEx);

            //Clipboard contains single value, paste to all cells
            string clipboardText = Clipboard.GetText();
            if (!clipboardText.Contains('\n') && !clipboardText.Contains('\t'))
            {
                foreach (DataGridViewCell cell in selCells)
                    dgvEx.ChangeCellValue(cell, clipboardText);
                return;
            }
            else
            {
                DataGridViewCell[,] cellsArray = ToCellGrid(selCells);

                //Clip board contains multiple values, split lines by carriage return.
                string[,] pasteValues = ParseDelimitedText(clipboardText);

                int pasteRows = pasteValues.GetLength(0);
                int pasteCols = pasteValues.GetLength(1);

                if (pasteRows == 0 || pasteCols == 0)
                    return;

                if (selCells.Length == 1)
                {
                    //Selected range is one cell, paste range to visible cells starting from selected cell within boundary of the DataGridView
                    int startRowIndex = selCells[0].RowIndex;
                    int startColIndex = selCells[0].ColumnIndex;

                    List<int> targetRows = GetVisibleIndices(dgv.Rows.Count, startRowIndex, pasteRows,
                        idx => dgv.Rows[idx].Visible);
                    List<int> targetCols = GetVisibleIndices(dgv.Columns.Count, startColIndex, pasteCols,
                        idx => dgv.Columns[idx].Visible);

                    for (int r = 0; r < targetRows.Count; r++)
                    {
                        for (int c = 0; c < targetCols.Count; c++)
                        {
                            SetCellValue(dgv, targetRows[r], targetCols[c], pasteValues[r, c]);
                        }
                    }
                }
                else
                {
                    //Selected range is multiple cells, paste range to selected range starting from selected cell within boundary of the DataGridView
                    int selRows = cellsArray.GetLength(0);
                    int selCols = cellsArray.GetLength(1);

                    // If paste block is smaller than (or equal to) the selection, tile it to fill the
                    // selection (Excel-style repeat). Otherwise, paste the full block starting at the
                    // top-left of the selection, extending past it but staying within grid bounds.
                    int outputRows = pasteRows <= selRows ? pasteRows : selRows;
                    int outputCols = pasteCols <= selCols ? pasteCols : selCols;

                    for (int r = 0; r < outputRows; r++)
                    {
                        for (int c = 0; c < outputCols; c++)
                        {
                            DataGridViewCell targetCell = cellsArray[r, c];
                            string value = pasteValues[r % pasteRows, c % pasteCols];
                            SetCellValue(dgv, targetCell.RowIndex, targetCell.ColumnIndex, value);
                        }
                    }
                }
            }

            dgv.RefreshEdit();
        }

        private static void ChangeCellValue(this DataGridViewExHandler dgvEx, DataGridViewCell ptrCell, object value)
        {
            BeforeCellChange(dgvEx, ptrCell);
            if (ptrCell is DataGridViewCheckBoxCell)
            {
                bool result = false;
                if (bool.TryParse(value.ToString(), out result))
                    value = result;
            }

            DataGridViewCellExEventArgs a = new DataGridViewCellExEventArgs(ptrCell.ColumnIndex, ptrCell.RowIndex, value);
            dgvEx.OnCellValueChanging(a);
            if (a.Cancel) return;

            ptrCell.Value = a.NewValue;
            AfterCellChange(dgvEx, ptrCell);
        }

        #endregion

        #region [ Cell Tracking ]

        private static Color ModifiedCellColor = Color.LightYellow;

        /// <summary>
        /// Represents the change tracking information for a cell.
        /// </summary>
        private static DataGridViewCellTracker GetCellTracker(this DataGridViewExHandler dgvEx, DataGridViewCell cell)
        {
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
            if (ptrTracker.CanRevert)
            {
                if (ptrTracker.Cell.Value == null)
                    ptrTracker.IsModified = ptrTracker.OriginalValue != null;
                else
                    ptrTracker.IsModified = !ptrTracker.Cell.Value.Equals(ptrTracker.OriginalValue);
            }
            ptrTracker.Cell.Style.BackColor = ptrTracker.IsModified ? ModifiedCellColor : dgv.DefaultCellStyle.BackColor;
            dgvEx.OnCellValueChanged(new DataGridViewCellEventArgs(c.ColumnIndex, c.RowIndex));
        }

        private static DataGridViewCell[] GetSelectedVisibleCells(this DataGridViewExHandler dgvEx)
        {
            DataGridView dgv = dgvEx.DataGrid;
            return dgv.SelectedCells.Cast<DataGridViewCell>().Where(n => n.Visible)
                        .OrderBy(n => n.RowIndex).ThenBy(n => n.ColumnIndex).ToArray();
        }

        #endregion

        #region [ Utility ]

        public static DataGridViewCell[,] ToCellGrid(IEnumerable<DataGridViewCell> cells)
        {
            List<DataGridViewCell> cellList = cells.ToList();
            if (cellList.Count == 0)
                return new DataGridViewCell[0, 0];

            List<int> rowIndices = cellList.Select(c => c.RowIndex).Distinct().OrderBy(r => r).ToList();
            List<int> colIndices = cellList.Select(c => c.ColumnIndex).Distinct().OrderBy(c => c).ToList();

            Dictionary<int, int> rowMap = new Dictionary<int, int>();
            for (int i = 0; i < rowIndices.Count; i++)
                rowMap[rowIndices[i]] = i;

            Dictionary<int, int> colMap = new Dictionary<int, int>();
            for (int i = 0; i < colIndices.Count; i++)
                colMap[colIndices[i]] = i;

            DataGridViewCell[,] grid = new DataGridViewCell[rowIndices.Count, colIndices.Count];

            foreach (DataGridViewCell cell in cellList)
                grid[rowMap[cell.RowIndex], colMap[cell.ColumnIndex]] = cell;

            return grid;
        }

        /// <summary>
        /// Parses tab/newline-delimited text into a 2D string array.
        /// Handles \r\n, \r, and \n line endings, and pads ragged rows
        /// with empty strings so the array is rectangular.
        /// </summary>
        public static string[,] ParseDelimitedText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new string[0, 0];

            // Normalize line endings, then split into rows
            string normalized = text.Replace("\r\n", "\n").Replace("\r", "\n");
            string[] rawLines = normalized.Split('\n');

            // Trim a single trailing blank line (common when copying from Excel/DataGridView)
            List<string> lines = new List<string>(rawLines);
            if (lines.Count > 0 && lines[lines.Count - 1].Length == 0)
                lines.RemoveAt(lines.Count - 1);

            if (lines.Count == 0)
                return new string[0, 0];

            // Split each row into columns and track the widest row
            List<string[]> rows = new List<string[]>();
            int maxCols = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                string[] cols = lines[i].Split('\t');
                rows.Add(cols);
                if (cols.Length > maxCols)
                    maxCols = cols.Length;
            }

            // Build the rectangular result, padding short rows with ""
            string[,] result = new string[rows.Count, maxCols];
            for (int r = 0; r < rows.Count; r++)
            {
                for (int c = 0; c < maxCols; c++)
                {
                    result[r, c] = c < rows[r].Length ? rows[r][c] : string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// Walks forward from startIndex over up to 'count' visible indices (0..maxCount-1),
        /// skipping any index for which isVisible returns false.
        /// </summary>
        private static List<int> GetVisibleIndices(int maxCount, int startIndex, int count, Func<int, bool> isVisible)
        {
            List<int> result = new List<int>();
            int idx = startIndex;

            while (result.Count < count && idx < maxCount)
            {
                if (idx >= 0 && isVisible(idx))
                    result.Add(idx);
                idx++;
            }

            return result;
        }

        /// <summary>
        /// Safely assigns a value to a DataGridView cell, skipping read-only cells,
        /// the "new row" placeholder, and out-of-range indices.
        /// </summary>
        private static void SetCellValue(DataGridView dgv, int rowIndex, int colIndex, string value)
        {
            if (rowIndex < 0 || rowIndex >= dgv.RowCount || colIndex < 0 || colIndex >= dgv.ColumnCount)
                return;

            DataGridViewRow row = dgv.Rows[rowIndex];
            if (row.IsNewRow)
                return;

            DataGridViewCell cell = row.Cells[colIndex];
            if (cell.ReadOnly || dgv.Columns[colIndex].ReadOnly)
                return;

            cell.Value = value;
            SetCellModified(dgv, rowIndex, colIndex);
        }

        #endregion

    }
}
