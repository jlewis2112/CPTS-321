// <copyright file="VariableNode.cs" company="Joseph Lewis 11567186">
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
    /// This is the Variable Node class.
    /// </summary>
    public class VariableNode : Node
    {
        private string variableName;

        /// <summary>
        /// Gets or sets for Variable name.
        /// </summary>
        public string VariableName
        {
            get
            {
                return this.variableName;
            }

            set
            {
                this.variableName = value;
            }
        }
    }
}
