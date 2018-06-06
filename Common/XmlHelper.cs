using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;

namespace Xiucai.Common
{
    /// <summary>
    /// Summary description for XmlHelper.
    /// </summary>
    public class XmlHelper
    {
        private XmlHelper()
        {
        }




        #region 建立XML文档
        public static XmlDocument CreateXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.InsertBefore(decl, doc.DocumentElement);
            return doc;
        }

        public static XmlDocument CreateXmlDocument(string rootName)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.InsertBefore(decl, doc.DocumentElement);
            XmlNode newNode = doc.CreateElement(rootName);
            doc.AppendChild(newNode);
            return doc;
        }
        #endregion

        #region GetItemValue(node, name, value)
        public static bool GetItemValue(XmlNode node, string itemName, ref string value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                value = node[itemName].InnerText;
                success = true;
            }
            return success;
        }


        // Get Int32 value from a node.
        public static bool GetItemValue(XmlNode node, string itemName, ref Int32 value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                value = Int32.Parse(node[itemName].InnerText);
                success = true;
            }
            return success;
        }


        // Get UInt32 value from a node.
        public static bool GetItemValue(XmlNode node, string itemName, ref UInt32 value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                value = UInt32.Parse(node[itemName].InnerText);
                success = true;
            }
            return success;
        }

        #endregion

        #region SetItemValue(node, name, value)
        // Set string value in a node
        public static bool SetItemValue(XmlNode node, string itemName, string value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                node[itemName].InnerText = value;
                success = true;
            }
            return success;
        }


        // Set int32 value in a node
        public static bool SetItemValue(XmlNode node, string itemName, Int32 value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                node[itemName].InnerText = value.ToString();
                success = true;
            }
            return success;
        }


        // Set uint32 value in a node
        public static bool SetItemValue(XmlNode node, string itemName, UInt32 value)
        {
            bool success = false;
            if (node != null && node[itemName] != null)
            {
                node[itemName].InnerText = value.ToString();
                success = true;
            }
            return success;
        }

        #endregion

        #region GetAttributeValue(node, name, value)
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the value from an attribute at the specified node.
        /// </summary>
        /// <param name="node">The XmlNode from which this method will get the value of an attribute.</param>
        /// <param name="attributeName">Name of the attribute that will be read.</param>
        /// <param name="value">Attribute value read by this method</param>
        /// <returns>True if node is found and value is retrieved successfully.</returns>
        /// -----------------------------------------------------------------------------
        public static bool GetAttributeValue(XmlNode node, string attributeName, ref string value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    value = attribute.Value;
                    success = true;
                }
            }
            return success;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the value from an attribute at the specified node.
        /// </summary>
        /// <param name="node">The XmlNode from which this method will get the value of an attribute.</param>
        /// <param name="attributeName">Name of the attribute that will be read.</param>
        /// <param name="value">Attribute value read by this method</param>
        /// <returns>True if success.</returns>
        /// -----------------------------------------------------------------------------
        public static bool GetAttributeValue(XmlNode node, string attributeName, ref int value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    string strValue = attribute.Value;
                    value = int.Parse(strValue);
                    success = true;
                }
            }
            return success;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the value from an attribute at the specified node.
        /// </summary>
        /// <param name="node">The XmlNode from which this method will get the value of an attribute.</param>
        /// <param name="attributeName">Name of the attribute that will be read.</param>
        /// <param name="value">Attribute value read by this method</param>
        /// <returns>True if success.</returns>
        /// -----------------------------------------------------------------------------
        public static bool GetAttributeValue(XmlNode node, string attributeName, ref UInt32 value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    string strValue = attribute.Value;
                    value = UInt32.Parse(strValue);
                    success = true;
                }
            }
            return success;
        }


        public static string GetAttributeValue(XmlNode node, string attributeName)
        {
            string value = null;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }
            return value;
        }


        #endregion

        #region SetAttributeValue(node, name, value)
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Sets the value of an attribute for a given XmlNode.
        /// </summary>
        /// <param name="node">XmlNode whose attribute will be set.</param>
        /// <param name="attributeName">Name of the attribute to set.</param>
        /// <param name="value">Value to be set</param>
        /// <returns>True if success.</returns>
        /// -----------------------------------------------------------------------------
        public static bool SetAttributeValue(XmlNode node, string attributeName, string value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    attribute.Value = value;
                    success = true;
                }
            }
            return success;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Sets the value of an attribute for a given XmlNode.
        /// </summary>
        /// <param name="node">XmlNode whose attribute will be set.</param>
        /// <param name="attributeName">Name of the attribute to set.</param>
        /// <param name="value">Value to be set</param>
        /// <returns>True if success.</returns>
        /// -----------------------------------------------------------------------------
        public static bool SetAttributeValue(XmlNode node, string attributeName, int value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    attribute.Value = value.ToString();
                    success = true;
                }
            }
            return success;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Sets the value of an attribute for a given XmlNode.
        /// </summary>
        /// <param name="node">XmlNode whose attribute will be set.</param>
        /// <param name="attributeName">Name of the attribute to set.</param>
        /// <param name="value">Value to be set</param>
        /// <returns>True if success.</returns>
        /// -----------------------------------------------------------------------------
        public static bool SetAttributeValue(XmlNode node, string attributeName, UInt32 value)
        {
            bool success = false;
            if (node != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                if (attribute != null)
                {
                    attribute.Value = value.ToString();
                    success = true;
                }
            }
            return success;
        }

        #endregion

        #region More Attribute Methods
        public static bool CopyAttribute(XmlNode fromNode, XmlNode toNode, string attributeName)
        {
            bool success = false;
            XmlDocument doc = toNode.OwnerDocument;
            string val = "";
            if (GetAttributeValue(fromNode, attributeName, ref val))
            {
                if (toNode.Attributes[attributeName] == null)
                {
                    CreateAttribute(toNode, attributeName, val);
                }
                success = SetAttributeValue(toNode, attributeName, val);
            }
            return success;
        }


        public static XmlAttribute CreateAttribute(XmlNode node, string attributeName, string value)
        {
            XmlDocument doc = node.OwnerDocument;
            XmlAttribute attr = null;
            // create new attribute
            attr = doc.CreateAttribute(attributeName);
            attr.Value = value;
            // link attribute to node
            node.Attributes.SetNamedItem(attr);
            return attr;
        }


        public static XmlNode CreateXmlNode(XmlNode rootNode, string nodeName, string innerText)
        {
            XmlDocument doc = rootNode.OwnerDocument;
            XmlNode node = doc.CreateElement(nodeName);
            node.InnerText = innerText;
            rootNode.AppendChild(node);
            return node;
        }

        #endregion

        #region Datatable Manipulation
        /// -----------------------------------------------------------------------------
        /// <summary>s 
        /// Converts a list of Xml nodes to a DataTable.
        /// </summary>
        /// <param name="nodelist">List of Xml nodes</param>
        /// <returns>DataTable</returns>
        /// <remarks>
        /// This method convert
        /// </remarks>
        /// -----------------------------------------------------------------------------
        public static DataTable GetDataTable(XmlNodeList nodelist)
        {
            DataTable table = null;
            XmlNode node = null;
            if (nodelist == null)
                return null;

            // get parameter names
            node = nodelist.Item(0);
            if (node == null)
                return null;

            XmlAttributeCollection attrCollection = node.Attributes;
            if (attrCollection == null)
                return null;
            if (attrCollection.Count == 0)
                return null;

            // create data table
            table = new DataTable();
            foreach (XmlAttribute attr in attrCollection)
            {
                table.Columns.Add(attr.Name);
            }

            // add rows
            DataRow row = null;
            foreach (XmlNode n in nodelist)
            {
                row = table.NewRow();
                foreach (XmlAttribute a in n.Attributes)
                {
                    row[a.Name] = a.Value;
                }
                table.Rows.Add(row);
            }

            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// Converts a list of Xml nodes to a DataTable and sets one of the columns as a primary key.
        /// </summary>
        /// <param name="nodelist"></param>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(XmlNodeList nodelist, string primaryKeyColumn, bool autoIncrement)
        {
            DataTable table = null;
            XmlNode node = null;
            if (nodelist == null)
                return null;

            // get parameter names
            node = nodelist.Item(0);
            if (node == null)
                return null;

            XmlAttributeCollection attrCollection = node.Attributes;
            if (attrCollection == null)
                return null;
            if (attrCollection.Count == 0)
                return null;

            // create data table
            table = new DataTable();
            bool primaryKeyFieldFound = false;
            foreach (XmlAttribute attr in attrCollection)
            {
                if (attr.Name == primaryKeyColumn) primaryKeyFieldFound = true;
                table.Columns.Add(attr.Name);
            }
            if (!primaryKeyFieldFound) throw new Exception("Unable to set primary key in datatable because field '" + primaryKeyColumn + "'was not found.");
            table.PrimaryKey = new DataColumn[] { table.Columns[primaryKeyColumn] };
            if (autoIncrement)
            {
                table.Columns[primaryKeyColumn].AutoIncrement = true;
                table.Columns[primaryKeyColumn].AutoIncrementStep = 1;
            }

            // add rows
            DataRow row = null;
            foreach (XmlNode n in nodelist)
            {
                row = table.NewRow();
                foreach (XmlAttribute a in n.Attributes)
                {
                    row[a.Name] = a.Value;
                }
                table.Rows.Add(row);
            }

            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// Updates the child nodes of "parentNode" by using the fields from a datatable.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="table"></param>
        /// <param name="keyField">The column name of the datatable that acts as a primary key.</param>
        /// <remarks>
        /// The child nodes that will be updated must have attribute fields that correspond to
        /// the DataTable.  The "keyField" will be used to identify the attribute that serves as 
        /// an identifier of the rows.  The datatable can have less fields than the nodes so
        /// you have the chance to update smaller subsets.
        /// Make sure that you did not call "AcceptChanges" before passing the datatable or this
        /// function will not find any change.
        /// </remarks>
        public static void UpdateChildNodesWithDataTable(XmlNode parentNode, DataTable table, string keyField)
        {
            if (parentNode == null)
            {
                throw new ArgumentNullException("Unable to update child nodes because parentNode is null");
            }
            if (parentNode.HasChildNodes)
            {
                XmlNode firstNode = parentNode.ChildNodes[0];
                //
                // Verify that the fields of first child node match the fields in the data table
                // note that it's ok if the datatable has fewer fields than the nodes.
                string missingFields = "";
                StringCollection columnNames = new StringCollection();
                foreach (DataColumn col in table.Columns)
                {
                    if (firstNode.Attributes[col.ColumnName] == null)
                    {
                        if (missingFields.Length == 0)
                            missingFields = col.ColumnName;
                        else
                            missingFields += ", " + col.ColumnName;
                    }
                    else
                        columnNames.Add(col.ColumnName);
                }

                if (missingFields.Length > 0)
                {
                    throw new Exception("Unable to update nodes with datatable because the nodes are missing the fields: " + missingFields);
                }

                ///
                /// Remove nodes that got deleted from datatable
                ///
                DataTable currTable = table.GetChanges(DataRowState.Deleted);
                if (currTable != null)
                {
                    //since there is no way to tell which rows got deleted then just remove all nodes 
                    //that are not present in the datatable.

                    XmlNode nodeToDelete;
                    Trace.WriteLine("Rows Deleted:");
                    foreach (DataRow row in table.Rows)
                    {
                        string keyValue = row[keyField].ToString();
                        nodeToDelete = SelectNode(parentNode, keyField, keyValue);
                        Trace.WriteLine(keyValue);
                        if (nodeToDelete != null)
                        {
                            parentNode.RemoveChild(nodeToDelete);
                        }
                    }
                }

                ///
                /// Update nodes with changes made on the datatable
                ///
                currTable = table.GetChanges(DataRowState.Modified);
                if (currTable != null)
                {
                    XmlNode nodeToUpdate;
                    Trace.WriteLine("Rows Changed:");
                    foreach (DataRow row in currTable.Rows)
                    {
                        string keyValue = row[keyField].ToString();
                        Trace.WriteLine(keyValue);
                        nodeToUpdate = SelectNode(parentNode, keyField, keyValue);
                        if (nodeToUpdate == null) throw new Exception("Unable to update node with '" + keyField + "=" + keyValue + "' because it doesn't exist");

                        string valueToSet;
                        foreach (string colName in columnNames)
                        {
                            if (colName == keyField) continue;
                            valueToSet = row[colName].ToString();
                            SetAttributeValue(nodeToUpdate, colName, valueToSet);
                        }
                    }
                }

                ///
                /// Add new nodes to match new rows added to datatable
                /// 
                currTable = table.GetChanges(DataRowState.Added);
                if (currTable != null)
                {
                    XmlNode newNode;
                    string keyValue;
                    XmlDocument doc = parentNode.OwnerDocument;
                    Trace.WriteLine("Rows Added:");
                    foreach (DataRow row in currTable.Rows)
                    {
                        keyValue = row[keyField].ToString();
                        Trace.WriteLine(keyValue);
                        if (SelectNode(parentNode, keyField, keyValue) == null)
                        {
                            newNode = doc.CreateElement(firstNode.Name);
                            CopyAttributes(row, newNode);
                            parentNode.AppendChild(newNode);
                        }
                    //    else
                    //    {
                    //        System.Windows.Forms.MessageBox.Show("Can not add duplicate nodes. Row with '" + keyField + "=" + keyValue + " was not added.", "Error Updating Nodes from Table");
                    //    }
                    }
                }
                table.AcceptChanges();
            }

        }

        /// <summary>
        /// Update child nodes with data from datatable.
        /// Note that the datatable requires a primary key column defined.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="table"></param>
        public static void UpdateChildNodesWithDataTable(XmlNode parentNode, DataTable table)
        {
            DataColumn[] primaryKeyColumns = table.PrimaryKey;
            if (primaryKeyColumns == null)
                throw new Exception("Can not update Child nodes with Table because the table doesn't have a primary key column");
            else
            {
                UpdateChildNodesWithDataTable(parentNode, table, primaryKeyColumns[0].ColumnName);
            }
        }

        public static void CopyAttributes(DataRow fromRow, XmlNode toNode)
        {
            foreach (DataColumn col in fromRow.Table.Columns)
            {
                CreateAttribute(toNode, col.ColumnName, fromRow[col.ColumnName].ToString());
            }
        }

        #endregion

        #region Misc
        public static XmlNode SelectNode(XmlNode parentNode, string attributeName, string attributeValue)
        {
            XmlNode node = null;
            if (parentNode.HasChildNodes)
            {
                string nodeName = parentNode.ChildNodes[0].Name;
                string path = nodeName + "[@" + attributeName + "='" + attributeValue + "']";
                node = parentNode.SelectSingleNode(path);
            }
            return node;
        }

     

        #endregion

        #region Conversion to Array

        /// <summary>
        /// This method same as getting a column from a table. 
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string[] GetAttributeArray(XmlNodeList nodeList, string attributeName)
        {
            string[] arrayOfValues = null;
            if (nodeList.Count > 0)
            {
                arrayOfValues = new string[nodeList.Count];
                int index = 0;
                XmlNode node;
                foreach (XmlNode n in nodeList)
                {
                    node = n;
                    arrayOfValues[index] = GetAttributeValue(node, attributeName);
                    index += 1;
                }
            }
            return arrayOfValues;
        }


        // Gets only the values of the nodes passed in nodelist
        public static string[] GetArray(XmlNodeList nodeList)
        {
            string[] arrayOfValues = null;
            if (nodeList.Count > 0)
            {
                arrayOfValues = new string[nodeList.Count];
                int index = 0;
                foreach (System.Xml.XmlNode node in nodeList)
                {
                    arrayOfValues[index] = node.InnerText;
                    index += 1;
                }
            }
            return arrayOfValues;
        }


        // This method gets the name value pair based on the first two attributes of every node
        public static NameValueCollection GetNameValuePair(XmlNodeList nodeList)
        {
            NameValueCollection nameVal = new NameValueCollection();
            if (nodeList == null)
                return null;

            // get parameter names
            XmlNode node = nodeList.Item(0);
            if (node == null)
                return null;

            XmlAttributeCollection attrCollection = node.Attributes;
            if (attrCollection == null)
                return null;
            if (attrCollection.Count < 2)
                return null;

            string attrName1 = null, attrName2 = null;
            // read all nodes in nodelist and extract first two attributes
            foreach (XmlNode n in nodeList)
            {
                attrName1 = n.Attributes[0].Value;
                attrName2 = n.Attributes[1].Value;
                nameVal.Add(attrName1, attrName2);
            }
            return nameVal;
        }
        #endregion

        #region Conversions to String
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Returns contents of an XmlNode in a string.
        /// </summary>
        /// <param name="node">The XmlNode whose contents will be read into a string.</param>
        /// <returns>Xml formatted string with contents of "node" parameter.</returns>
        /// -----------------------------------------------------------------------------
        public static string NodeToString(XmlNode node)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            if (node == null)
            {
                writer.WriteStartElement("Node_Empty");
            }
            else
            {
                writer.WriteStartElement(node.Name);

                //  Write any attributes 
                foreach (XmlAttribute attr in node.Attributes)
                {
                    writer.WriteAttributeString(attr.Name, attr.Value);
                }

                //  Write child nodes
                XmlNodeList nodes = node.SelectNodes("child::*");
                NodeNavigator nav = new NodeNavigator();
                if (nodes != null)
                {
                    foreach (XmlNode n in nodes)
                    {
                        nav.LoopThroughChildren(writer, n);
                    }
                }
            }

            writer.WriteEndElement();
            writer.Close();

            return sw.ToString();
        }

        /// <summary>
        /// Method to convert a XmlNodeList to string.
        /// </summary>
        /// <param name="nodeList"></param>
        /// <returns></returns>
        public static string NodeListToString(XmlNodeList nodeList)
        {
            if (nodeList != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (XmlNode node in nodeList)
                {
                    if (sb.Length == 0)
                        sb.Append(NodeToString(node));
                    else
                        sb.Append("\r\n" + NodeToString(node));
                }
                return sb.ToString();
            }
            return "nodeList is null";
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Method to convert a XmlDocument to string.
        /// </summary>
        /// <param name="xmlDoc">XmlDocument that will be converted to string.</param>
        /// <returns>A xml formatted string.</returns>
        /// -----------------------------------------------------------------------------
        public static string DocumentToString(XmlDocument xmlDoc)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            xmlDoc.Save(sw);
            return sw.ToString();
        }
        #endregion

        #region Creation of Multiple child nodes
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Converts a string array to XmlNodes and appends all those nodes to a root node.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        /// -----------------------------------------------------------------------------
        public static void CreateChildNodes(XmlNode rootNode, string[] names)
        {
            XmlDocument doc = rootNode.OwnerDocument;
            XmlNode newNode = null;
            foreach (string name in names)
            {
                newNode = doc.CreateElement(name);
                rootNode.AppendChild(newNode);
            }
        }

        /// <summary>
        /// Creates child nodes that would resemble a table whose values are passed in parameter attributeValues.
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="nodeName"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValues"></param>
        public static void CreateChildNodes(XmlNode rootNode, string nodeName, string attributeName, string[] attributeValues)
        {
            XmlDocument doc = rootNode.OwnerDocument;
            XmlNode newNode = null;
            foreach (string value in attributeValues)
            {
                newNode = doc.CreateElement(nodeName);
                CreateAttribute(newNode, attributeName, value);
                rootNode.AppendChild(newNode);
            }
        }
        #endregion

        #region ?Copyright 2005, Marc Clifton, All Rights Reserved - XmlDatase methods
        /*
		(c) 2005, Marc Clifton
		All Rights Reserved
 
		Redistribution and use in source and binary forms, with or without modification,
		are permitted provided that the following conditions are met:

		Redistributions of source code must retain the above copyright notice, this list
		of conditions and the following disclaimer. 

		Redistributions in binary form must reproduce the above copyright notice, this
		list of conditions and the following disclaimer in the documentation and/or other
		materials provided with the distribution. 

		Neither the name of the Marc Clifton nor the names of its contributors may be
		used to endorse or promote products derived from this software without specific
		prior written permission. 

		THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
		ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
		WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
		IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
		INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
		BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
		DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
		LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
		OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
		ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
		*/

        #region Disclosure
        /*
		 * With Marc Clifton's permission, the methods in this section were copied and 
		 * modified to be used in XmlHelper.  The original source code is located at:
		 * http://www.codeproject.com/dotnet/XmlDb.asp
		*/
        #endregion


        #region Insert
        /// <summary>
        /// Inserts an empty record at the bottom of the hierarchy, creating the
        /// tree as required.
        /// </summary>
        /// <param name="doc">The XmlDocument to which the node will be inserted.</param>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <returns>The XmlNode inserted into the hierarchy.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static XmlNode Insert(XmlDocument doc, string xpath)
        {
            VerifyParameters(doc, xpath);

            string path2 = xpath.Trim('/');   // get rid of slashes in front and back
            string[] segments = path2.Split('/');

            XmlNode firstNode = doc.LastChild;
            int nodeIndex = 0;

            if (segments.Length > 1)
            {
                ///
                /// Check if we can access the last node.  This comes in handy in cases when the path
                /// contains attributes.  For example: "University[@Name='UT']/Student[@Id=12222]/Address"
                /// In example above we would want to get access to last node directly
                ///
                int pos = path2.LastIndexOf('/');
                string path3 = path2.Substring(0, pos);
                XmlNode parentNode = doc.LastChild.SelectSingleNode(path3);
                if (parentNode != null)
                {
                    firstNode = parentNode;
                    nodeIndex = segments.Length - 1;
                }
            }

            XmlNode lastNode = InsertNode(firstNode, segments, nodeIndex);
            return lastNode;
        }

        /// <summary>
        /// Inserts an record with a multiple fields at the bottom of the hierarchy.
        /// </summary>
        /// <param name="doc">The XmlDocument to which the node will be inserted.</param>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="fields">The attribute names that will be created for the node inserted.</param>
        /// <param name="values">The corresponding value of each field.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static void Insert(XmlDocument doc, string xpath, string[] fields, string[] values)
        {
            VerifyParameters(doc, xpath);
            if (fields == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }
            if (values == null)
            {
                throw (new ArgumentNullException("val cannot be null."));
            }

            XmlNode node = Insert(doc, xpath);

            for (int i = 0; i < fields.Length; i++)
            {
                CreateAttribute(node, fields[i], values[i]);
            }
        }

        public static void InsertNode(XmlDocument doc, string xPath, string[] fields, string[] innerTexts)
        {
            VerifyParameters(doc, xPath);
            if (fields == null)
                throw new ArgumentNullException("xml 节点数组为 null!");
            if(innerTexts == null)
                throw new ArgumentNullException("xml 节点值数组为 null!");

            XmlNode rootnode = Insert(doc, xPath);

            for (int i = 0; i < fields.Length; i++)
            {
                CreateXmlNode(rootnode, fields[i], innerTexts[i]);
            }

        }

        public static void InsertNode(XmlDocument doc, string xPath, NameValueCollection nvc)
        {
            VerifyParameters(doc, xPath);
            if (nvc == null)
                throw new ArgumentNullException("xml 节点不能为 null!");
            if (nvc.Count ==0)
                throw new ArgumentNullException("xml 节点不能为 0 !");

            XmlNode rootnode = Insert(doc, xPath);

            IEnumerator iterator = nvc.GetEnumerator();
            string field, innerText;
            while(iterator.MoveNext())
            {
                field = iterator.Current.ToString();
                innerText = nvc[field];
                CreateXmlNode(rootnode,field,innerText);
            }

        }

        /// <summary>
        /// Inserts a record with a single field at the bottom of the hierarchy.
        /// </summary>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="field">The field to add to the record.</param>
        /// <param name="val">The value assigned to the field.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static void Insert(XmlDocument doc, string xpath, string field, string val)
        {
            VerifyParameters(doc, xpath);
            if (field == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }
            if (val == null)
            {
                throw (new ArgumentNullException("val cannot be null."));
            }

            XmlNode node = Insert(doc, xpath);
            CreateAttribute(node, field, val);
        }

        /// <summary>
        /// Insert a record with multiple fields at the bottom of the hierarchy.
        /// </summary>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="fields">The array of fields as field/value pairs.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static void Insert(XmlDocument doc, string xpath, NameValueCollection nameValuePairs)
        {
            VerifyParameters(doc, xpath);
            if (nameValuePairs == null)
            {
                throw (new ArgumentNullException("fields cannot be null."));
            }

            XmlNode node = Insert(doc, xpath);

            System.Collections.IEnumerator iterator = nameValuePairs.GetEnumerator();
            string field, val;
            while (iterator.MoveNext())
            {
                field = iterator.Current.ToString();
                val = nameValuePairs[field];
                CreateAttribute(node, field, val);
            }
        }

        /// <summary>
        /// Inserts a record with multiple fields at bottom of the hierarchy.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="rowValues">The DataRow values that will be added as attributes.</param>
        /// <remarks>
        /// The columns names of the DataRow will become the attribute names and 
        /// the row values of the DataRow will be the attribute values.
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static void Insert(XmlDocument doc, string xpath, DataRow rowValues)
        {
            VerifyParameters(doc, xpath);
            if (rowValues == null)
            {
                throw (new ArgumentNullException("rowValues cannot be null."));
            }

            XmlNode node = Insert(doc, xpath);
            foreach (DataColumn col in rowValues.Table.Columns)
            {
                CreateAttribute(node, col.ColumnName, rowValues[col.ColumnName].ToString());
            }
        }

        /// <summary>
        /// Inserts a record with multiple fields from a DataTable at bottom of the hierarchy.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="rowValues">The DataRow values that will be added as attributes.</param>
        public static void Insert(XmlDocument doc, string xpath, DataTable table)
        {
            VerifyParameters(doc, xpath);
            if (table == null)
            {
                throw (new ArgumentNullException("table cannot be null."));
            }

            foreach (DataRow row in table.Rows)
            {
                Insert(doc, xpath, row);
            }
        }

        /// <summary>
        /// Inserts a record with multiple values at bottom of hierarchy. This is analogous to inserting 
        /// a column of data.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <param name="field">Name of the attribute to be created at node inserted.</param>
        /// <param name="values">Values that will be inserted that correspond to the field name.</param>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to insert an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static void Insert(XmlDocument doc, string xpath, string field, string[] values)
        {
            VerifyParameters(doc, xpath);

            XmlNode node;
            foreach (string val in values)
            {
                node = Insert(doc, xpath);
                CreateAttribute(node, field, val);
            }
        }


        #endregion

        #region Update
        /// <summary>
        /// Update a single field in all records in the specified path.
        /// </summary>
        /// <param name="doc">The XmlDocument whose node will be udpated.</param>
        /// <param name="xpath">The xml path.</param>
        /// <param name="field">The field name to update.</param>
        /// <param name="val">The new value.</param>
        /// <returns>The number of records affected.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>
        /// The "doc" variable must have a root node.  The path should not contain the root node.
        /// The path can contain only the node names or it can contain attributes in XPath query form.
        /// For example to update an "Address" node at the bottom, the following is a valid xpath query
        ///     xpath = "University[@Name='UT']/Student[@Id=12222]/Address"
        /// </remarks>
        public static int Update(XmlDocument doc, string xpath, string field, string val)
        {
            VerifyParameters(doc, xpath);
            if (field == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }
            if (val == null)
            {
                throw (new ArgumentNullException("val cannot be null."));
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlNodeList nodeList = doc.LastChild.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                if (!SetAttributeValue(node, field, val))
                    sb.Append(field + " is not an attribute of: " + NodeToString(node) + "\n");
            }
            if (sb.Length > 0) throw new Exception("Failed to add nodes because:\n" + sb.ToString());
            return nodeList.Count;
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes all records of the specified path.
        /// </summary>
        /// <param name="xpath">The xml XPath query to get to the bottom node.</param>
        /// <returns>The number of records deleted.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>Additional exceptions may be thrown by the XmlDocument class.</remarks>
        public static int Delete(XmlDocument doc, string xpath)
        {
            VerifyParameters(doc, xpath);

            XmlNodeList nodeList = doc.LastChild.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                node.ParentNode.RemoveChild(node);
            }
            return nodeList.Count;
        }

        /// <summary>
        /// Deletes a field from all records on the specified path.
        /// </summary>
        /// <param name="path">The xml path.</param>
        /// <param name="field">The field to delete.</param>
        /// <returns>The number of records affected.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>Additional exceptions may be thrown by the XmlDocument class.</remarks>
        public static int Delete(XmlDocument doc, string xpath, string field)
        {
            VerifyParameters(doc, xpath);
            if (field == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }

            XmlNodeList nodeList = doc.SelectNodes(xpath);
            foreach (XmlNode node in nodeList)
            {
                XmlAttribute attrib = node.Attributes[field];
                node.Attributes.Remove(attrib);
            }
            return nodeList.Count;
        }

        #endregion

        #region Query
        /// <summary>
        /// Return a single string representing the value of the specified field
        /// for the first record encountered.
        /// </summary>
        /// <param name="xpath">The xml path.</param>
        /// <param name="field">The desired field.</param>
        /// <returns>A string with the field's value or null.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>Additional exceptions may be thrown by the XmlDocument class.</remarks>
        public static string QueryScalar(XmlDocument doc, string xpath, string field)
        {
            VerifyParameters(doc, xpath);
            if (field == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }

            string ret = null;
            XmlNode node = doc.LastChild.SelectSingleNode(xpath);
            if (node != null)
            {
                ret = GetAttributeValue(node, field);
            }
            return ret;
        }


        /// <summary>
        /// Returns a DataTable for all rows on the path.
        /// </summary>
        /// <param name="xpath">The xml path.</param>
        /// <returns>The DataTable with the returned rows.
        /// The row count will be 0 if no rows returned.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>Additional exceptions may be thrown by the XmlDocument class.</remarks>
        public static DataTable Query(XmlDocument doc, string xpath)
        {
            VerifyParameters(doc, xpath);

            DataTable dt = new DataTable();
            XmlNodeList nodeList = doc.LastChild.SelectNodes(xpath);
            if (nodeList.Count != 0)
            {
                CreateColumns(dt, nodeList[0]);
            }
            foreach (XmlNode node in nodeList)
            {
                DataRow dr = dt.NewRow();
                foreach (XmlAttribute attr in node.Attributes)
                {
                    dr[attr.Name] = attr.Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Returns an array of values for the specified field for all rows on
        /// the path.
        /// </summary>
        /// <param name="xpath">The xml path.</param>
        /// <param name="field">The desired field.</param>
        /// <returns>The array of string values for each row qualified by the path.
        /// A null is returned if the query results in 0 rows.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when an argument is null.</exception>
        /// <remarks>Additional exceptions may be thrown by the XmlDocument class.</remarks>
        public static string[] QueryField(XmlDocument doc, string xpath, string field)
        {
            VerifyParameters(doc, xpath);

            if (field == null)
            {
                throw (new ArgumentNullException("field cannot be null."));
            }

            XmlNodeList nodeList = doc.LastChild.SelectNodes(xpath);
            string[] s = null;
            if (nodeList.Count != 0)
            {
                s = new string[nodeList.Count];
                int i = 0;
                foreach (XmlNode node in nodeList)
                {
                    s[i++] = node.Attributes[field].Value;
                }
            }
            return s;
        }

        #endregion

        #endregion

        #region NodeNavigator Class
        /// <summary>
        /// Class required to navigate through children nodes
        /// </summary>
        private class NodeNavigator
        {
            //  Recursively loop over a node subtree
            internal void LoopThroughChildren(XmlTextWriter writer, XmlNode rootNode)
            {
                //  Write the start tag
                if (rootNode.NodeType == XmlNodeType.Element)
                {
                    writer.WriteStartElement(rootNode.Name);

                    //  Write any attributes 
                    foreach (XmlAttribute attr in rootNode.Attributes)
                    {
                        writer.WriteAttributeString(attr.Name, attr.Value);
                    }

                    //  Write any child nodes
                    foreach (XmlNode node in rootNode.ChildNodes)
                    {
                        LoopThroughChildren(writer, node);
                    }

                    //  Write the end tag
                    writer.WriteEndElement();
                }
                else
                {
                    //  Write any text
                    if (rootNode.NodeType == XmlNodeType.Text)
                    {
                        writer.WriteString(rootNode.Value);
                    }
                }
            }

        }
        #endregion

        #region Helpers
        /// <summary>
        /// Inserts a node at the specified segment if it doesn't exist, otherwise
        /// traverses the node.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <param name="segments">The path segment list.</param>
        /// <param name="idx">The current segment.</param>
        /// <returns></returns>
        protected static XmlNode InsertNode(XmlNode node, string[] segments, int idx)
        {
            XmlNode newNode = null;

            if (idx == segments.Length)
            {
                // All done.
                return node;
            }

            // Traverse the existing hierarchy but ensure that we create a 
            // new record at the last leaf.
            if (idx + 1 < segments.Length)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == segments[idx])
                    {
                        newNode = InsertNode(child, segments, idx + 1);
                        return newNode;
                    }
                }
            }
            XmlDocument doc = node.OwnerDocument;
            newNode = doc.CreateElement(segments[idx]);
            node.AppendChild(newNode);
            XmlNode nextNode = InsertNode(newNode, segments, idx + 1);
            return nextNode;
        }

        /// <summary>
        /// Creates columns given an XmlNode.
        /// </summary>
        /// <param name="dt">The target DataTable.</param>
        /// <param name="node">The source XmlNode.</param>
        protected static void CreateColumns(DataTable dt, XmlNode node)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                dt.Columns.Add(new DataColumn(attr.Name));
            }
        }


        protected static void VerifyParameters(XmlDocument doc, string xpath)
        {
            if (doc == null)
            {
                throw new Exception("doc cannot be null.");
            }
            if (doc.LastChild.GetType() == typeof(System.Xml.XmlDeclaration))
            {
                throw new Exception("XmlDocument requires at least the a root node");
            }
            if (xpath == null)
            {
                throw (new ArgumentNullException("path cannot be null."));
            }
        }
        #endregion

    }
}
