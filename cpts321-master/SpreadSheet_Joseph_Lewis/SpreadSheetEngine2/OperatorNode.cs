// <copyright file="OperatorNode.cs" company="Joseph Lewis 11567186">
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
    /// This is the Operator Node class.
    /// </summary>
    public class OperatorNode : Node
    {
        private char operatorValue;
        private Node left;
        private Node right;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// </summary>
        /// <param name="symbol">
        /// The operator symbol used to represent the operator value.
        /// </param>
        public OperatorNode(char symbol)
        {
            this.operatorValue = symbol;
            this.left = null;
            this.right = null;
        }

        /// <summary>
        /// Gets the operator value.
        /// </summary>
        public char OperatorValue
        {
            get
            {
                return this.operatorValue;
            }
        }

        /// <summary>
        /// Gets or sets for the left Node.
        /// </summary>
        public Node Left
        {
            get
            {
                return this.left;
            }

            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Gets or sets for the right Node.
        /// </summary>
        public Node Right
        {
            get
            {
                return this.right;
            }

            set
            {
                this.right = value;
            }
        }
    }
}
