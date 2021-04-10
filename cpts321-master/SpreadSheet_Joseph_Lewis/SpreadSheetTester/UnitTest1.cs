// <copyright file="UnitTest1.cs" company="Joseph Lewis 11567186">
// Copyright (c) Joseph Lewis 11567186. All rights reserved.
// </copyright>

namespace SpreadSheet_Joseph_Lewis
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    /// <summary>
    /// This is the test class.
    /// </summary>
    public class UnitTest1
    {
        /// <summary>
        /// Test the constructor of spreadsheet.
        /// </summary>
        [Test]
        public void TestingCounts()
        {
            SpreadSheet test = new SpreadSheet(50, 26);
            Assert.AreEqual(test.RowCount, 50);
            Assert.AreEqual(test.ColumnCount, 26);
        }

        /// <summary>
        /// Testing getcell for Rows.
        /// </summary>
        [Test]
        public void TestingGetCellRow()
        {
            SpreadSheet test = new SpreadSheet(50, 26);
            Assert.AreEqual(test.GetCell(50, 25).RowIndex, 49);
        }

        /// <summary>
        /// Testing getcell for Columns.
        /// </summary>
        [Test]
        public void TestingGetCellColumn()
        {
            SpreadSheet test = new SpreadSheet(50, 26);
            Assert.AreEqual(test.GetCell(49, 25).ColumnIndex, 25);
        }

        /// <summary>
        /// Testing expression for the expression tree.
        /// </summary>
        [Test]
        public void TestingTreeExpression()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("5+5");
            Assert.AreEqual(expressionTreeTest.Expression, "5+5");
        }

        /// <summary>
        /// Testing evaluate add for the expression tree.
        /// </summary>
        [Test]
        public void TestingTreeExpressionAdd()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("6+5+7");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 18.0);
        }

        /// <summary>
        /// Testing evaluate multiply for the expression tree.
        /// </summary>
        [Test]
        public void TestingTreeExpressionMult()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("6*5");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 30.0);
        }

        /// <summary>
        /// Testing evaluate subtract for the expression tree.
        /// </summary>
        [Test]
        public void TestingTreeExpressionSubract()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("6-5");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 1.0);
        }

        /// <summary>
        /// Testing evaluate divide for the expression tree.
        /// </summary>
        [Test]
        public void TestingTreeExpressionDivide()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("10/5");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 2.0);
        }

        /// <summary>
        /// Testing set Variable for the expression tree.
        /// </summary>
        [Test]
        public void TestingSetVariable()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("5+tester");
            expressionTreeTest.SetVariable("tester", 5.0);
            Assert.AreEqual(expressionTreeTest.Evaluate(), 10.0);
        }

        /// <summary>
        /// Testing the intailization of variables to make sure the dictionary sets it to 0.
        /// </summary>
        [Test]
        public void TestingInitailVariable()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("A1");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 0.0);
        }

        /// <summary>
        /// Testing the Evaluations with Parentheses.
        /// </summary>
        [Test]
        public void TestingEvaluateParentheses()
        {
            ExpressionTree expressionTreeTest = new ExpressionTree("(5*(2+2))");
            Assert.AreEqual(expressionTreeTest.Evaluate(), 20.0);
        }

        /// <summary>
        /// Testing Value for Spreadsheetcell.
        /// </summary>
        [Test]
        public void TestingSpreadsheetVale()
        {
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(11, 0).Text = "12";
            test.GetCell(1, 1).Text = "=(A11+13)";
            Assert.AreEqual("25", test.GetCell(1, 1).ValueStr);
        }

        /// <summary>
        /// Testing Text for Spreadsheetcell.
        /// </summary>
        [Test]
        public void TestingSpreadsheetText()
        {
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "12";
            test.GetCell(1, 1).Text = "=(A1+13)";
            Assert.AreEqual(test.GetCell(1, 1).Text, "=(A1+13)");
        }

        /// <summary>
        /// Testing Undo and Redo for Value of Spreadsheetcell.
        /// </summary>
        [Test]
        public void TestingUndoRedo()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "50";
            undoInput.Add("0");
            undoInput.Add("0");
            undoInput.Add("txt");
            undoInput.Add(string.Empty);
            test.AddUndo(undoInput);
            test.Undo();
            Assert.AreEqual(string.Empty, test.GetCell(1, 0).ValueStr);
            test.Redo();
            Assert.AreEqual(test.GetCell(1, 0).ValueStr, "50");
        }

        /// <summary>
        /// Testing Undo and Redo for Color of Spreadsheetcell.
        /// </summary>
        [Test]
        public void TestingColorUndoRedo()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Color = 4286644096;
            undoInput.Add("0");
            undoInput.Add("0");
            undoInput.Add("color");
            undoInput.Add("4294967295");
            test.AddUndo(undoInput);
            test.Undo();
            Assert.AreEqual(test.GetCell(1, 0).Color.ToString(), "4294967295");
            test.Redo();
            Assert.AreEqual(test.GetCell(1, 0).Color.ToString(), "4286644096");
        }

        /// <summary>
        /// Testing for clearing the stack during load and save.
        /// </summary>
        [Test]
        public void TestingClearStack()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            undoInput.Add("0");
            undoInput.Add("0");
            undoInput.Add("color");
            undoInput.Add("4294967295");
            test.AddUndo(undoInput);
            test.ClearStacks();
            Assert.AreEqual(test.DetermineUndo(), -1);
        }

        /// <summary>
        /// Testing for clearing the stack during load and save.
        /// </summary>
        [Test]
        public void TestingIfDefault()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "test";
            Assert.AreEqual(test.DetermineDefault(test.GetCell(1, 0)), 0);
        }

        /// <summary>
        /// Testing for bad reference for an expression.
        /// </summary>
        [Test]
        public void TestingBadReference()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "=Cell678+50";
            Assert.AreEqual(test.GetCell(1, 0).ValueStr, "!bad reference");
        }

        /// <summary>
        /// Testing for self reference for an expression.
        /// </summary>
        [Test]
        public void TestingSelfReference()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "=A1+5";
            Assert.AreEqual(test.GetCell(1, 0).ValueStr, "!self reference");
        }

        /// <summary>
        /// Testing for circular reference for an expression.
        /// </summary>
        [Test]
        public void TestingCircularReference()
        {
            List<string> undoInput = new List<string>();
            SpreadSheet test = new SpreadSheet(50, 26);
            test.GetCell(1, 0).Text = "=A2";
            test.GetCell(2, 0).Text = "=A3";
            test.GetCell(3, 0).Text = "=A1";
            Assert.AreEqual(test.GetCell(3, 0).ValueStr, "!circular reference");
        }
    }
}