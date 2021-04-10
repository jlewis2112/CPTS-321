// <copyright file="Form1.cs" company="Joseph Lewis 11567186">
// Copyright (c) Joseph Lewis 11567186. All rights reserved.
// </copyright>

namespace SpreadSheet_Joseph_Lewis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// This is the form class for the spreadsheet.
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly SpreadSheet sheet;
        private readonly Button dynamicButton = new Button();
        private Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// Constructor for the form.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.dataGridView1.ColumnCount = 26;
            this.dataGridView1.Columns[0].Name = "A";
            this.dataGridView1.Columns[1].Name = "B";
            this.dataGridView1.Columns[2].Name = "C";
            this.dataGridView1.Columns[3].Name = "D";
            this.dataGridView1.Columns[4].Name = "E";
            this.dataGridView1.Columns[5].Name = "F";
            this.dataGridView1.Columns[6].Name = "G";
            this.dataGridView1.Columns[7].Name = "H";
            this.dataGridView1.Columns[8].Name = "I";
            this.dataGridView1.Columns[9].Name = "J";
            this.dataGridView1.Columns[10].Name = "K";
            this.dataGridView1.Columns[11].Name = "L";
            this.dataGridView1.Columns[12].Name = "M";
            this.dataGridView1.Columns[13].Name = "N";
            this.dataGridView1.Columns[14].Name = "O";
            this.dataGridView1.Columns[15].Name = "P";
            this.dataGridView1.Columns[16].Name = "Q";
            this.dataGridView1.Columns[17].Name = "R";
            this.dataGridView1.Columns[18].Name = "S";
            this.dataGridView1.Columns[19].Name = "T";
            this.dataGridView1.Columns[20].Name = "U";
            this.dataGridView1.Columns[21].Name = "V";
            this.dataGridView1.Columns[22].Name = "W";
            this.dataGridView1.Columns[23].Name = "X";
            this.dataGridView1.Columns[24].Name = "Y";
            this.dataGridView1.Columns[25].Name = "Z";

            for (int a = 1; a < 51; a++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[a - 1].HeaderCell.Value = a.ToString();
            }

            this.sheet = new SpreadSheet(50, 26);
            this.dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.DataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(this.DataGridView1_CellEndEdit);
            this.sheet.PropertyChanged += this.CellPropertyChanging;
            this.dynamicButton.Click += new EventHandler(this.Button1_Click);
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.sheet.GetCell(e.RowIndex + 1, e.ColumnIndex).Text;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<string> undoEntree = new List<string>();
            try
            {
                DataGridView dgv = sender as DataGridView;
                SpreadSheetCell editedCell = this.sheet.GetCell(e.RowIndex + 1, e.ColumnIndex);
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    undoEntree.Add(e.RowIndex.ToString());
                    undoEntree.Add(e.ColumnIndex.ToString());
                    undoEntree.Add("txt");
                    undoEntree.Add(editedCell.Text);
                    editedCell.Text = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    this.sheet.AddUndo(undoEntree);
                }
                else
                {
                    editedCell.Text = string.Empty;
                }

                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editedCell.ValueStr;
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Index Out Of Range Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// The cell begin edit event handler.
        /// </summary>
        /// <param name="sender">
        /// The object of the event.
        /// </param>
        /// <param name="e">
        /// The event's parameters.
        /// </param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.sheet.GetCell(e.RowIndex + 1, e.ColumnIndex).Text;
        }

        /// <summary>
        /// The event handler for cell end edit.
        /// </summary>
        /// <param name="sender">
        /// The event odject.
        /// </param>
        /// <param name="e">
        /// The event arguements.
        /// </param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<string> undoEntree = new List<string>();
            try
            {
                DataGridView dgv = sender as DataGridView;
                SpreadSheetCell editedCell = this.sheet.GetCell(e.RowIndex + 1, e.ColumnIndex);
                if (dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    undoEntree.Add(e.RowIndex.ToString());
                    undoEntree.Add(e.ColumnIndex.ToString());
                    undoEntree.Add("txt");
                    undoEntree.Add(editedCell.Text);
                    editedCell.Text = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    this.sheet.AddUndo(undoEntree);
                    this.sheet.ClearRedo();
                }
                else
                {
                    editedCell.Text = string.Empty;
                }

                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editedCell.ValueStr;
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Index Out Of Range Error", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// This is the event function when the button is clicked.
        /// </summary>
        private void Button1_Click(object sender, EventArgs e)
        {
            for (int addRowB = 1; addRowB < 51; addRowB++)
            {
                this.sheet.GetCell(addRowB, 1).Text = "6";
            }

            for (int addRowA = 1; addRowA < 51; addRowA++)
            {
                this.sheet.GetCell(addRowA, 0).Text = "=B" + addRowA.ToString();
            }
        }

        /// <summary>
        /// Coverts a uint to a color.
        /// </summary>
        /// <param name="color">
        /// The uint of the color.
        /// </param>
        /// <returns>
        /// the color value of the uint.
        /// </returns>
        private Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Coverts color to uint.
        /// </summary>
        /// <param name="color">
        /// The colors value.
        /// </param>
        /// <returns>
        /// The uint value for the color.
        /// </returns>
        private uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

        /// <summary>
        ///  This is the event function for if a cell of a spreadsheet changes.
        /// </summary>
        private void CellPropertyChanging(object sender, PropertyChangedEventArgs e)
        {
            SpreadSheetCell eventCell = sender as SpreadSheetCell;
            this.dataGridView1.Rows[eventCell.RowIndex].Cells[eventCell.ColumnIndex].Value = eventCell.ValueStr;
            this.dataGridView1.Rows[eventCell.RowIndex].Cells[eventCell.ColumnIndex].Style.BackColor = this.UIntToColor(eventCell.Color);
        }

        /// <summary>
        /// The form1 load event handler.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Event handler for the change color menue item.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void ChangeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> undoEntree = new List<string>();
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    SpreadSheetCell editedCell = this.sheet.GetCell(cell.RowIndex + 1, cell.ColumnIndex);
                    undoEntree.Add(cell.RowIndex.ToString());
                    undoEntree.Add(cell.ColumnIndex.ToString());
                    undoEntree.Add("color");
                    undoEntree.Add(editedCell.Color.ToString());
                    editedCell.Color = this.ColorToUInt(dlg.Color);
                }

                this.sheet.AddUndo(undoEntree);
            }
        }

        /// <summary>
        /// The event handler for the cell menue item being clicked.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void CellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.sheet.DetermineUndo() == -1)
            {
                this.undoToolStripMenuItem.Enabled = false;
                this.undoToolStripMenuItem.Text = "undo";
            }
            else if (this.sheet.DetermineUndo() == 0)
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = "undo cell text";
            }
            else
            {
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = "undo cell color";
            }

            if (this.sheet.DetermineRedo() == -1)
            {
                this.redoToolStripMenuItem.Enabled = false;
                this.redoToolStripMenuItem.Text = "redo";
            }
            else if (this.sheet.DetermineRedo() == 0)
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = "redo cell text";
            }
            else
            {
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = "redo cell color";
            }
        }

        /// <summary>
        /// The event handler for the undo menue item.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Undo();
        }

        /// <summary>
        /// The event handler for the redo menue item.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sheet.Redo();
        }

        /// <summary>
        /// The event Handler for the Save menue item.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog { Title = "Save the spreadsheet to a file.", Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*", FilterIndex = 2 };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream sw = new FileStream(fileDialog.FileName, FileMode.Create))
                {
                    try
                    {
                        this.sheet.Save(sw);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: problem with saving file. Original Error: " + ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// The event handler for the Load menue item.
        /// </summary>
        /// <param name="sender">
        /// The sender object.
        /// </param>
        /// <param name="e">
        /// The events parameters.
        /// </param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog { Title = "Select a file to be displayed in the spreadsheet.", Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*", FilterIndex = 2 };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.ClearGrid();
                using (FileStream sr = new FileStream(fileDialog.FileName, FileMode.Open))
                {
                    this.sheet.Load(sr);
                }
            }

            this.sheet.ClearStacks();
        }

        /// <summary>
        /// Clears the spreasheet and sets the colors of each cell to white.
        /// </summary>
        private void ClearGrid()
        {
            for (int rowIndex = 0; rowIndex < 50; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 26; columnIndex++)
                {
                    this.sheet.GetCell(rowIndex + 1, columnIndex).Text = string.Empty;
                    this.sheet.GetCell(rowIndex + 1, columnIndex).Color = 0xFFFFFFFF;
                }
            }
        }
    }
}