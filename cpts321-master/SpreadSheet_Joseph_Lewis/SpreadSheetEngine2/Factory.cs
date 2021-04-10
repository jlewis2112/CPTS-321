// <copyright file="Factory.cs" company="Joseph Lewis 11567186">
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
    /// This is the factory class. used to create operation nodes.
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Factory"/> class.
        /// </summary>
        public Factory()
        {
        }

        /// <summary>
        /// This function creates a new operator node.
        /// </summary>
        /// <param name="operatorSymbol">
        /// The operator symbol of the new node.
        /// </param>
        /// <returns>
        /// The new operator node.
        /// </returns>
        public OperatorNode CreateOperatorNode(char operatorSymbol)
        {
            List<char> legalSymbols = new List<char>() { '+', '-', '*', '/' };
            if (legalSymbols.Contains(operatorSymbol))
            {
                return new OperatorNode(operatorSymbol);
            }

            return null;
        }
    }
}
