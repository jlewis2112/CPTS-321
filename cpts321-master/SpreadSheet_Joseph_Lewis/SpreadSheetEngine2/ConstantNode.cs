// <copyright file="ConstantNode.cs" company="Joseph Lewis 11567186">
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
    /// This is the Constant node class. holds a double for the node value.
    /// </summary>
    internal class ConstantNode : Node
    {
        /// <summary>
        /// Gets or sets for the value of the Constant node class.
        /// </summary>
        public double Value { get; set; }
    }
}
