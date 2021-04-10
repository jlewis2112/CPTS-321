// <copyright file="SpreadSheet.cs" company="Joseph Lewis 11567186">
// Copyright (c) Joseph Lewis 11567186. All rights reserved.
// </copyright>

namespace SpreadSheet_Joseph_Lewis
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// This is the SpreadSheetClass.
    /// </summary>
    public class SpreadSheet
    {
        private readonly SpreadSheetCell[,] cellObject;
        private int rowCount;
        private int columnCount;
        private ExpressionTree spreadSheetTree = new ExpressionTree(null);
        private List<char> columnSymbols = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private List<char> operations = new List<char>() { '+', '-', '*', '/', ')' };
        private Stack<List<string>> undoStack = new Stack<List<string>>();
        private Stack<List<string>> redoStack = new Stack<List<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheet"/> class.
        /// </summary>
        /// <param name="rowInput">
        /// Input for the number of rows.
        /// </param>
        /// <param name="columnInput">
        /// input for the number of columns.
        /// </param>
        public SpreadSheet(int rowInput, int columnInput)
        {
            this.rowCount = rowInput;
            this.columnCount = columnInput;
            this.cellObject = new SpreadSheetCell[this.rowCount, this.columnCount];
            for (int rowIndex = 0; rowIndex < this.rowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++)
                {
                    this.cellObject[rowIndex, columnIndex] = new Cell(rowIndex, columnIndex);
                    this.cellObject[rowIndex, columnIndex].Text = string.Empty;
                    this.cellObject[rowIndex, columnIndex].ValueStr = string.Empty;
                    this.cellObject[rowIndex, columnIndex].PropertyChanged += new PropertyChangedEventHandler(this.CellPropertyChanged);
                }
            }
        }

        /// <summary>
        /// This is the event handler for the spreadsheetclass.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets for RowCount.
        /// </summary>
        public int RowCount
        {
            get { return this.rowCount; }
            set { this.rowCount = value; }
        }

        /// <summary>
        /// Gets or sets for ColumnCount.
        /// </summary>
        public int ColumnCount
        {
            get { return this.columnCount; }
            set { this.columnCount = value; }
        }

        /// <summary>
        /// This function returns the specified cell.
        /// </summary>
        /// <param name="rowIndex">
        /// The row number of the needed cell.
        /// </param>
        /// <param name="columnIndex">
        /// the columnIndex of the needed cell.
        /// </param>
        /// <returns>
        /// The wanted cell in the row and column parameters.
        /// </returns>
        public SpreadSheetCell GetCell(int rowIndex, int columnIndex)
        {
            return this.cellObject[rowIndex - 1, columnIndex];
        }

        /// <summary>
        /// Adds an undo entree to the stack.
        /// </summary>
        /// <param name="undoInfo">
        /// This is the undo entree.
        /// </param>
        public void AddUndo(List<string> undoInfo)
        {
            this.undoStack.Push(undoInfo);
        }

        /// <summary>
        /// Determines if the current undo is a color or text change.
        /// </summary>
        /// <returns>
        /// An integer determining the result.
        /// </returns>
        public int DetermineUndo()
        {
            if (this.undoStack.Count == 0)
            {
                return -1;
            }
            else if (this.undoStack.Peek()[2] == "txt")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Clears the stacks when a sheet is being uploaded.
        /// </summary>
        public void ClearStacks()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        /// <summary>
        /// Clears the redo stack when declared.
        /// </summary>
        public void ClearRedo()
        {
            this.redoStack.Clear();
        }

        /// <summary>
        /// Save the spreadsheet to the input stream.
        /// </summary>
        /// <param name="outfile">
        /// The name of the new saved file.
        /// </param>
        public void Save(Stream outfile)
        {
            XmlWriter xmlWriter = XmlWriter.Create(outfile);
            xmlWriter.WriteStartElement("spreadsheet");

            for (int rowIndex = 0; rowIndex < 50; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 26; columnIndex++)
                {
                    if (this.DetermineDefault(this.GetCell(rowIndex + 1, columnIndex)) == 0)
                    {
                        this.GetCell(rowIndex + 1, columnIndex).WriteXml(xmlWriter);
                    }
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        /// <summary>
        /// Load the spreadsheet.
        /// </summary>
        /// <param name="infile">
        /// The selected xml file.
        /// </param>
        public void Load(Stream infile)
        {
            // XDocument is abstract, so obtain reference to it and laod infile.
            XDocument xmlReader = XDocument.Load(infile);

            foreach (XElement tag in xmlReader.Root.Elements("SpreadSheetCell"))
            {
                SpreadSheetCell cell = this.GetCell(int.Parse(tag.Element("cellrow").Value) + 1, int.Parse(tag.Element("columnrow").Value));
                cell.Text = tag.Element("celltext").Value;
                cell.Color = uint.Parse(tag.Element("color").Value);
            }
        }

        /// <summary>
        /// Determines if a cell is at default or not.
        /// </summary>
        /// <param name="inputCell">
        /// The examined cell.
        /// </param>
        /// <returns>
        /// A 1 or 0 integer to determine if it is at default.
        /// </returns>
        public int DetermineDefault(SpreadSheetCell inputCell)
        {
            if (inputCell.Text != string.Empty)
            {
                return 0;
            }
            else if (inputCell.Color != 0xFFFFFFFF)
            {
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// Determines if the next redo is a color or text change.
        /// </summary>
        /// <returns>
        /// An integer determining the result.
        /// </returns>
        public int DetermineRedo()
        {
            if (this.redoStack.Count == 0)
            {
                return -1;
            }
            else if (this.redoStack.Peek()[2] == "txt")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// This will be the undo function for txt.
        /// </summary>
        public void Undo()
        {
            List<string> topStack = this.undoStack.Pop();
            string redoInput = string.Empty;
            if (topStack[2] == "txt")
            {
                redoInput = this.GetCell(int.Parse(topStack[0]) + 1, int.Parse(topStack[1])).Text;
                this.GetCell(int.Parse(topStack[0]) + 1, int.Parse(topStack[1])).Text = topStack[3];
                topStack[3] = redoInput;
            }
            else
            {
                for (int topIndex = 0; topIndex < topStack.Count; topIndex += 4)
                {
                    redoInput = this.GetCell(int.Parse(topStack[topIndex]) + 1, int.Parse(topStack[topIndex + 1])).Color.ToString();
                    this.GetCell(int.Parse(topStack[topIndex]) + 1, int.Parse(topStack[topIndex + 1])).Color = uint.Parse(topStack[topIndex + 3]);
                    topStack[topIndex + 3] = redoInput;
                }
            }

            this.redoStack.Push(topStack);
        }

        /// <summary>
        /// This will be the redo function for txt.
        /// </summary>
        public void Redo()
        {
            List<string> topStack = this.redoStack.Pop();
            string undoInput = string.Empty;
            if (topStack[2] == "txt")
            {
                undoInput = this.GetCell(int.Parse(topStack[0]) + 1, int.Parse(topStack[1])).Text;
                this.GetCell(int.Parse(topStack[0]) + 1, int.Parse(topStack[1])).Text = topStack[3];
                topStack[3] = undoInput;
            }
            else
            {
                for (int topIndex = 0; topIndex < topStack.Count; topIndex += 4)
                {
                    undoInput = this.GetCell(int.Parse(topStack[topIndex]) + 1, int.Parse(topStack[topIndex + 1])).Color.ToString();
                    this.GetCell(int.Parse(topStack[topIndex]) + 1, int.Parse(topStack[topIndex + 1])).Color = uint.Parse(topStack[topIndex + 3]);
                    topStack[topIndex + 3] = undoInput;
                }
            }

            this.undoStack.Push(topStack);
        }

        /// <summary>
        /// This is event handler for spreedsheet when a property is changed.
        /// </summary>
        /// <param name="sender">
        /// Represents the cell object being changed.
        /// </param>
        /// <param name="e">
        /// Represents the event arguements.
        /// </param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadSheetCell eventCell = sender as SpreadSheetCell;
            if (eventCell.Text.Length == 0)
            {
                eventCell.ValueStr = string.Empty;
            }
            else if (eventCell.Text.StartsWith("="))
            {
                int ifSelf = 0;
                string text = eventCell.Text.Substring(1);
                this.spreadSheetTree = new ExpressionTree(text);
                string variableStr = string.Empty;
                string variableStrParser = string.Empty;
                double declaredValue = 0.0;
                int tempCol = 0;
                int tempRow = 0;
                string combiner = string.Empty;
                if (this.DetermineBadReference(text) == true)
                {
                    eventCell.ValueStr = "!bad reference";
                }
                else if (this.DetermineSelfReference(text, eventCell) == true)
                {
                    eventCell.ValueStr = "!self reference";
                    ifSelf = 1;
                }
                else
                {
                    if (text[text.Length - 1] == ')')
                    {
                            text = text.Substring(0, text.Length - 1);
                    }

                    for (int textIndex = 0; textIndex < text.Length; textIndex++)
                    {
                        if (this.columnSymbols.Contains(text[textIndex]))
                        {
                            if (textIndex + 2 == text.Length)
                            {
                                variableStr = text.Substring(textIndex);
                                tempCol = variableStr[0] - 65;
                                tempRow = variableStr[1] - 49;
                                variableStr = variableStr.Substring(0, 2);
                                try
                                {
                                    if (this.GetCell(tempRow + 1, tempCol) == eventCell)
                                    {
                                        eventCell.ValueStr = "!self reference";
                                        ifSelf = 1;
                                        break;
                                    }
                                    else
                                    {
                                        declaredValue = Convert.ToDouble(this.GetCell(tempRow + 1, tempCol).ValueStr);
                                    }
                                }
                                catch (Exception)
                                {
                                    declaredValue = 0.0;
                                }

                                this.spreadSheetTree.SetVariable(variableStr, declaredValue);
                                this.GetCell(tempRow + 1, tempCol).DependencyChanged += eventCell.OnDependencyChanged;
                            }
                            else if (textIndex + 3 == text.Length)
                            {
                                variableStr = text.Substring(textIndex);
                                tempCol = variableStr[0] - 65;
                                variableStrParser = variableStr.Substring(1);
                                int.TryParse(variableStrParser, out tempRow);
                                tempRow--;
                                try
                                {
                                    if (this.GetCell(tempRow + 1, tempCol) == eventCell)
                                    {
                                        eventCell.ValueStr = "!self reference";
                                        ifSelf = 1;
                                        break;
                                    }
                                    else
                                    {
                                        declaredValue = Convert.ToDouble(this.GetCell(tempRow + 1, tempCol).ValueStr);
                                    }
                                }
                                catch (Exception)
                                {
                                    declaredValue = 0.0;
                                }

                                this.spreadSheetTree.SetVariable(variableStr, declaredValue);
                                this.GetCell(tempRow + 1, tempCol).DependencyChanged += eventCell.OnDependencyChanged;
                            }
                            else
                            {
                                variableStr = text.Substring(textIndex);
                                if (this.operations.Contains(variableStr[2]))
                                {
                                    tempCol = variableStr[0] - 65;
                                    tempRow = variableStr[1] - 49;
                                    combiner = variableStr[0].ToString() + variableStr[1].ToString();
                                    try
                                    {
                                        if (this.GetCell(tempRow + 1, tempCol) == eventCell)
                                        {
                                            eventCell.ValueStr = "!self reference";
                                            ifSelf = 1;
                                            break;
                                        }
                                        else
                                        {
                                            declaredValue = Convert.ToDouble(this.GetCell(tempRow + 1, tempCol).ValueStr);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        declaredValue = 0.0;
                                    }

                                    this.spreadSheetTree.SetVariable(combiner, declaredValue);
                                    this.GetCell(tempRow + 1, tempCol).DependencyChanged += eventCell.OnDependencyChanged;
                                }
                                else
                                {
                                    tempCol = variableStr[0] - 65;
                                    variableStrParser = variableStr.Substring(1, 2);
                                    int.TryParse(variableStrParser, out tempRow);
                                    tempRow--;
                                    combiner = variableStr[0].ToString() + variableStr[1].ToString() + variableStr[2].ToString();
                                    try
                                    {
                                        if (this.GetCell(tempRow + 1, tempCol) == eventCell)
                                        {
                                            eventCell.ValueStr = "!self reference";
                                            ifSelf = 1;
                                            break;
                                        }
                                        else
                                        {
                                            declaredValue = Convert.ToDouble(this.GetCell(tempRow + 1, tempCol).ValueStr);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        declaredValue = 0.0;
                                    }

                                    this.spreadSheetTree.SetVariable(combiner, declaredValue);
                                    this.GetCell(tempRow + 1, tempCol).DependencyChanged += eventCell.OnDependencyChanged;
                                }
                            }
                        }
                    }

                    if (ifSelf == 0 && this.DetermineCircularReference(text, eventCell) == true)
                    {
                        eventCell.ValueStr = "!circular reference";
                    }
                    else if (ifSelf == 0)
                    {
                        eventCell.ValueStr = this.spreadSheetTree.Evaluate().ToString();
                    }
                }
            }
            else
            {
                eventCell.ValueStr = eventCell.Text;
            }

            this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("ValueStr"));
        }

        /// <summary>
        /// Determines if a self reference exists.
        /// </summary>
        /// <param name="expression">
        /// The cells expression.
        /// </param>
        /// <param name="comparedCell">
        /// The spreadsheet cell used to determine if a self reference occured.
        /// </param>
        /// <returns>
        /// A bool to determine if a self reference has occured.
        /// </returns>
        private bool DetermineSelfReference(string expression, SpreadSheetCell comparedCell)
        {
            bool result = false;
            int tempCol = 0;
            string rowString = string.Empty;

            if (expression == string.Empty)
            {
                return false;
            }

            for (int textIndex = 0; textIndex < expression.Length; textIndex++)
            {
                if (this.columnSymbols.Contains(expression[textIndex]))
                {
                    if (textIndex + 2 < expression.Length)
                    {
                        if (char.IsNumber(expression[textIndex + 1]))
                        {
                            if (char.IsNumber(expression[textIndex + 2]))
                            {
                                tempCol = expression[textIndex] - 65;
                                rowString = expression[textIndex + 1].ToString() + expression[textIndex + 2].ToString();
                                if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                tempCol = expression[textIndex] - 65;
                                rowString = expression[textIndex + 1].ToString();
                                if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        tempCol = expression[textIndex] - 65;
                        rowString = expression[textIndex + 1].ToString();

                        if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines if a circular reference exists.
        /// </summary>
        /// <param name="expression">
        /// The cells expression.
        /// </param>
        /// <param name="comparedCell">
        /// The spreadsheet cell used to determine if a circular reference occured.
        /// </param>
        /// <returns>
        /// A bool to determine if a circular reference has occured.
        /// </returns>
        private bool DetermineCircularReference(string expression, SpreadSheetCell comparedCell)
        {
            bool result = false;
            int tempCol = 0;
            string rowString = string.Empty;

            if (expression == string.Empty)
            {
                return false;
            }

            for (int textIndex = 0; textIndex < expression.Length; textIndex++)
            {
                if (this.columnSymbols.Contains(expression[textIndex]))
                {
                    if (textIndex + 2 < expression.Length)
                    {
                        if (char.IsNumber(expression[textIndex + 1]))
                        {
                            if (char.IsNumber(expression[textIndex + 2]))
                            {
                                tempCol = expression[textIndex] - 65;
                                rowString = expression[textIndex + 1].ToString() + expression[textIndex + 2].ToString();

                                if (this.DetermineSelfReference(this.GetCell(int.Parse(rowString), tempCol).Text, this.GetCell(int.Parse(rowString), tempCol)) == true)
                                {
                                    return false;
                                }

                                if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                                {
                                    return true;
                                }

                                result = this.DetermineCircularReference(this.GetCell(int.Parse(rowString), tempCol).Text, comparedCell);
                                if (result == true)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                tempCol = expression[textIndex] - 65;
                                rowString = expression[textIndex + 1].ToString();

                                if (this.DetermineSelfReference(this.GetCell(int.Parse(rowString), tempCol).Text, this.GetCell(int.Parse(rowString), tempCol)) == true)
                                {
                                    return false;
                                }

                                if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                                {
                                    return true;
                                }

                                result = this.DetermineCircularReference(this.GetCell(int.Parse(rowString), tempCol).Text, comparedCell);
                                if (result == true)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        tempCol = expression[textIndex] - 65;
                        rowString = expression[textIndex + 1].ToString();

                        if (this.DetermineSelfReference(this.GetCell(int.Parse(rowString), tempCol).Text, this.GetCell(int.Parse(rowString), tempCol)) == true)
                        {
                            return false;
                        }

                        if (this.GetCell(int.Parse(rowString), tempCol) == comparedCell)
                        {
                                return true;
                        }

                        result = this.DetermineCircularReference(this.GetCell(int.Parse(rowString), tempCol).Text, comparedCell);
                        if (result == true)
                        {
                            return true;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines if an expression is a bad referrence.
        /// </summary>
        /// <param name="expression">
        /// The string of the expression.
        /// </param>
        /// <returns>
        /// A bool if bad reference or not.
        /// </returns>
        private bool DetermineBadReference(string expression)
        {
            int firstDigit = 0;
            int secondDigit = 0;
            int columnResult = 0;
            if (this.operations.Contains(expression[0]))
            {
                return true;
            }

            for (int textIndex = 0; textIndex < expression.Length; textIndex++)
            {
                if (expression[textIndex] == '(')
                {
                    columnResult++;
                }
                else if (expression[textIndex] == ')')
                {
                    columnResult--;
                }
            }

            if (columnResult != 0)
            {
                return true;
            }

            for (int textIndex = 0; textIndex < expression.Length; textIndex++)
            {
                if (!char.IsNumber(expression[textIndex]))
                {
                    if (this.columnSymbols.Contains(expression[textIndex]))
                    {
                        if (char.IsNumber(expression[textIndex + 1]))
                        {
                            if (textIndex + 2 >= expression.Length)
                            {
                                return false;
                            }

                            if (char.IsNumber(expression[textIndex + 2]))
                            {
                                firstDigit = int.Parse(expression[textIndex + 1].ToString());
                                if (firstDigit == 5)
                                {
                                    secondDigit = int.Parse(expression[textIndex + 2].ToString());
                                    if (secondDigit != 0)
                                    {
                                        return true;
                                    }
                                }
                                else if (firstDigit > 5)
                                {
                                    return true;
                                }

                                if (textIndex + 3 >= expression.Length)
                                {
                                    return false;
                                }

                                if (char.IsNumber(expression[textIndex + 3]))
                                {
                                    return true;
                                }

                                if (!this.operations.Contains(expression[textIndex + 3]))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                if (!this.operations.Contains(expression[textIndex + 2]))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (this.operations.Contains(expression[textIndex]))
                    {
                    }
                    else if (expression[textIndex] == '(')
                    {
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
