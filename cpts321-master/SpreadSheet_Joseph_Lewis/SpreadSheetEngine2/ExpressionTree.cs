// <copyright file="ExpressionTree.cs" company="Joseph Lewis 11567186">
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
    /// This is the ExpressionTree class.
    /// </summary>
    public class ExpressionTree
    {
        private static readonly Dictionary<string, int> OperatorOrder = new Dictionary<string, int>() { { "*", 3 }, { "/", 3 }, { "+", 2 }, { "-", 2 }, { "(", 0 } };
        private string expression;
        private Node rootNode;
        private Dictionary<string, double> variables = new Dictionary<string, double>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="newExpression">
        /// This is the input of the expression.
        /// </param>
        public ExpressionTree(string newExpression)
        {
            this.variables.Clear();
            this.expression = newExpression;
            this.rootNode = this.Compile(this.expression);
        }

        /// <summary>
        /// Gets for expression.
        /// </summary>
        public string Expression
        {
            get
            {
                return this.expression;
            }
        }

        /// <summary>
        /// This function sets a variable value in the expression.
        /// </summary>
        /// <param name="variableName">
        /// This is the name of the variable.
        /// </param>
        /// <param name="variableValue">
        /// This is the set value of the variable.
        /// </param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (this.variables.ContainsKey(variableName))
            {
                this.variables[variableName] = variableValue;
            }
            else
            {
                this.variables.Add(variableName, variableValue);
            }
        }

        /// <summary>
        /// This functions evalutes the tree.
        /// </summary>
        /// <returns>
        /// The Evaluate node.
        /// </returns>
        public double Evaluate()
        {
            if (this.rootNode == null)
            {
                return 0.0;
            }
            else
            {
                return this.CalculateTree(this.rootNode);
            }
        }

        /// <summary>
        /// Splits the expression into a string List. Used to for the Shuting yard algorithm.
        /// </summary>
        /// <param name="equation">
        /// The string of the expression.
        /// </param>
        /// <returns>
        /// the expression splitt into a string.
        /// </returns>
        private List<string> SplitEquation(string equation)
        {
            List<string> equationSplit = new List<string>();
            string buffer = string.Empty;
            for (int equationIndex = 0; equationIndex < equation.Length; equationIndex++)
            {
                switch (equation[equationIndex])
                {
                    case '+':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    case '-':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    case '*':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    case '/':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    case '(':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    case ')':
                        if (buffer != string.Empty)
                        {
                            equationSplit.Add(buffer);
                        }

                        equationSplit.Add(equation[equationIndex].ToString());
                        buffer = string.Empty;
                        break;
                    default:
                        buffer = buffer + equation[equationIndex];
                        break;
                }
            }

            if (buffer != string.Empty)
            {
                equationSplit.Add(buffer);
            }

            buffer = string.Empty;
            return equationSplit;
        }

        /// <summary>
        /// This the shunting yard algorithm used to parse the string expression.
        /// </summary>
        /// <param name="equation">
        /// The entered expression.
        /// </param>
        /// <returns>
        /// A string list representing the postfix string.
        /// </returns>
        private List<string> ShuntingYard(string equation)
        {
            List<string> result = new List<string>();
            Stack<string> operatorStack = new Stack<string>();
            List<string> equationList = this.SplitEquation(equation);
            string buffer = string.Empty;
            int orderHolder = 0;

            foreach (string equationNode in equationList)
            {
                switch (equationNode)
                {
                    case "(":
                        operatorStack.Push(equationNode);
                        break;
                    case ")":
                        while (buffer != "(")
                        {
                            buffer = operatorStack.Pop();
                            if (buffer != "(")
                            {
                                result.Add(buffer);
                            }
                        }

                        buffer = string.Empty;
                        break;
                    case "+":
                        while (orderHolder == 0)
                        {
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push("+");
                                orderHolder = 1;
                            }
                            else if (OperatorOrder["+"] <= OperatorOrder[operatorStack.Peek()])
                            {
                                result.Add(operatorStack.Pop());
                            }
                            else
                            {
                                operatorStack.Push("+");
                                orderHolder = 1;
                            }
                        }

                        orderHolder = 0;
                        break;
                    case "-":
                        while (orderHolder == 0)
                        {
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push("-");
                                orderHolder = 1;
                            }
                            else if (OperatorOrder["-"] <= OperatorOrder[operatorStack.Peek()])
                            {
                                result.Add(operatorStack.Pop());
                            }
                            else
                            {
                                operatorStack.Push("-");
                                orderHolder = 1;
                            }
                        }

                        orderHolder = 0;
                        break;
                    case "*":
                        while (orderHolder == 0)
                        {
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push("*");
                                orderHolder = 1;
                            }
                            else if (OperatorOrder["*"] <= OperatorOrder[operatorStack.Peek()])
                            {
                                result.Add(operatorStack.Pop());
                            }
                            else
                            {
                                operatorStack.Push("*");
                                orderHolder = 1;
                            }
                        }

                        orderHolder = 0;
                        break;
                    case "/":
                        while (orderHolder == 0)
                        {
                            if (operatorStack.Count == 0)
                            {
                                operatorStack.Push("/");
                                orderHolder = 1;
                            }
                            else if (OperatorOrder["/"] <= OperatorOrder[operatorStack.Peek()])
                            {
                                result.Add(operatorStack.Pop());
                            }
                            else
                            {
                                operatorStack.Push("/");
                                orderHolder = 1;
                            }
                        }

                        orderHolder = 0;
                        break;
                    default:
                        result.Add(equationNode);
                        break;
                }
            }

            while (operatorStack.Count != 0)
            {
                result.Add(operatorStack.Pop());
            }

            return result;
        }

        /// <summary>
        /// This is the comile algorithm that creates the expression tree.
        /// </summary>
        /// <param name="equation">
        /// The entered expression.
        /// </param>
        /// <returns>
        /// The expression Tree.
        /// </returns>
        private Node Compile(string equation)
        {
            List<string> yardInputs = new List<string>();
            Stack<Node> buildingStack = new Stack<Node>();
            Factory nodeCreator = new Factory();
            if (string.IsNullOrEmpty(equation))
            {
                return null;
            }

            yardInputs = this.ShuntingYard(equation);
            foreach (string yardIndex in yardInputs)
            {
                switch (yardIndex)
                {
                    case "+":
                        OperatorNode operatorNodeAdd = nodeCreator.CreateOperatorNode('+');
                        operatorNodeAdd.Right = buildingStack.Pop();
                        operatorNodeAdd.Left = buildingStack.Pop();
                        buildingStack.Push(operatorNodeAdd);
                        break;
                    case "-":
                        OperatorNode operatorNodeSub = nodeCreator.CreateOperatorNode('-');
                        operatorNodeSub.Right = buildingStack.Pop();
                        operatorNodeSub.Left = buildingStack.Pop();
                        buildingStack.Push(operatorNodeSub);
                        break;
                    case "*":
                        OperatorNode operatorNodeMult = nodeCreator.CreateOperatorNode('*');
                        operatorNodeMult.Right = buildingStack.Pop();
                        operatorNodeMult.Left = buildingStack.Pop();
                        buildingStack.Push(operatorNodeMult);
                        break;
                    case "/":
                        OperatorNode operatorNodeDiv = nodeCreator.CreateOperatorNode('/');
                        operatorNodeDiv.Right = buildingStack.Pop();
                        operatorNodeDiv.Left = buildingStack.Pop();
                        buildingStack.Push(operatorNodeDiv);
                        break;
                    default:
                        double nodeValue;
                        if (double.TryParse(yardIndex, out nodeValue))
                        {
                            // We need a ConstantNode
                            buildingStack.Push(new ConstantNode() { Value = nodeValue });
                        }
                        else
                        {
                            this.variables.Add(yardIndex, 0.0);
                            buildingStack.Push(new VariableNode() { VariableName = yardIndex });
                        }

                        break;
                }
            }

            return buildingStack.Pop();
        }

        /// <summary>
        /// This function evaluates an expression returning the result.
        /// </summary>
        /// <returns>
        /// The double representing the evaluated expression.
        /// </returns>
        private double CalculateTree(Node calculatedNode)
        {
            ConstantNode constantNode = calculatedNode as ConstantNode;
            if (constantNode != null)
            {
                return constantNode.Value;
            }

            VariableNode variableNode = calculatedNode as VariableNode;
            if (variableNode != null)
            {
                if (this.variables.ContainsKey(variableNode.VariableName))
                {
                    return this.variables[variableNode.VariableName];
                }
                else
                {
                    return 0.0;
                }
            }

            OperatorNode operatorNode = calculatedNode as OperatorNode;
            if (operatorNode != null)
            {
                switch (operatorNode.OperatorValue)
                {
                    case '+':
                        return this.CalculateTree(operatorNode.Left) + this.CalculateTree(operatorNode.Right);
                    case '-':
                        return this.CalculateTree(operatorNode.Left) - this.CalculateTree(operatorNode.Right);
                    case '*':
                        return this.CalculateTree(operatorNode.Left) * this.CalculateTree(operatorNode.Right);
                    case '/':
                        return this.CalculateTree(operatorNode.Left) / this.CalculateTree(operatorNode.Right);
                    default: // if it is not any of the operators that we support, throw an exception:
                        throw new NotSupportedException(
                            "Operator " + operatorNode.OperatorValue.ToString() + " not supported.");
                }
            }

            throw new NotSupportedException();
        }
    }
}
