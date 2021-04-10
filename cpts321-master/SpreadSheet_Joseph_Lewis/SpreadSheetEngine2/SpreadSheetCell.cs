// <copyright file="SpreadSheetCell.cs" company="Joseph Lewis 11567186">
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
    /// This is the Spread Sheet Cell Class.
    /// </summary>
    public abstract class SpreadSheetCell : INotifyPropertyChanged
    {
        /// <summary>
        /// This is the text variable of the cell.
        /// </summary>
        protected string text;

        /// <summary>
        /// This is the value of the cell.
        /// </summary>
        protected string valueStr;
        private readonly int rowIndex;
        private readonly int columnIndex;
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheetCell"/> class.
        /// </summary>
        /// <param name="row">
        /// What row the cell is on.
        /// </param>
        /// <param name="column">
        /// what column the cell is on.
        /// </param>
        public SpreadSheetCell(int row, int column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
            this.color = 0xFFFFFFFF;
        }

        /// <summary>
        /// This is the dependancy changes delegate.
        /// </summary>
        /// <param name="sender">
        /// The object of the cell in needed to be updated.
        /// </param>
        public delegate void DependencyChangedEventHandler(object sender);

        /// <summary>
        /// This is the Cells event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This is the dependency changed event handler.
        /// </summary>
        public event DependencyChangedEventHandler DependencyChanged;

        /// <summary>
        /// Gets row number.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets Column number.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or sets and sets for color.
        /// </summary>
        public uint Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.OnPropertyChanged("color");
            }
        }

        /// <summary>
        /// Gets for Value.
        /// </summary>
        public string ValueStr
        {
            get
            {
                return this.valueStr;
            }

            internal set
            {
                if (this.valueStr != value)
                {
                    this.valueStr = value;
                    this.OnPropertyChanged("ValuerStr");
                }
            }
        }

        /// <summary>
        /// Gets or sets and sets for text.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// Xml-ify a cell.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("SpreadSheetCell");
            writer.WriteElementString("cellrow", this.RowIndex.ToString());
            writer.WriteElementString("columnrow", this.ColumnIndex.ToString());
            writer.WriteElementString("celltext", this.Text);
            writer.WriteElementString("color", this.Color.ToString());
            writer.WriteEndElement();
        }

        /// <summary>
        /// This is a function that is used cell updating subscriptions.
        /// </summary>
        /// <param name="sender">
        /// This is the object needed to get updated.
        /// </param>
        public void OnDependencyChanged(object sender)
        {
            this.OnPropertyChangedDep("Text");
        }

        /// <summary>
        /// Event handler function for SpreadsheetCell.
        /// </summary>
        /// <param name="name">
        /// The newest text changed.
        /// </param>
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            this.DependencyChanged?.Invoke(this);
        }

        /// <summary>
        /// Event handler function for SpreadsheetCell Dependencies.
        /// </summary>
        /// <param name="name">
        /// The newest dependent text changed.
        /// </param>
        protected void OnPropertyChangedDep(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
