// <copyright file="Cell.cs" company="Joseph Lewis 11567186">
// Copyright (c) Joseph Lewis 11567186. All rights reserved.
// </copyright>

namespace SpreadSheet_Joseph_Lewis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Instanciable Cell.
    /// </summary>
    public class Cell : SpreadSheetCell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="rowIndex">
        /// The row index.
        /// </param>
        /// <param name="columnIndex">
        /// The column index.
        /// </param>
        public Cell(int rowIndex, int columnIndex)
            : base(rowIndex, columnIndex)
        {
        }
    }
}
