using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
 
    public class Table : Single, ITable, INameValueItem, ITabularEntity
	{
		public Table()
		{

		}

		#region Collections

		public IColumns Columns
		{
			get
			{
				if(null == _columns)
				{
					_columns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_columns.Table = this;
					_columns.dbRoot = this.dbRoot;
					_columns.LoadForTable();
				}
				return _columns;
			}
		}

		public IForeignKeys ForeignKeys
		{
			get
			{
				if(null == _foreignKeys)
				{
					_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_foreignKeys.Table = this;
					_foreignKeys.dbRoot = this.dbRoot;
					_foreignKeys.LoadAll();
				}
				return _foreignKeys;
			}
		}

		public IForeignKeys IndirectForeignKeys
		{
			get
			{
				if(null == _indirectForeignKeys)
				{
					_indirectForeignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_indirectForeignKeys.Table = this;
					_indirectForeignKeys.dbRoot = this.dbRoot;
					_indirectForeignKeys.LoadAllIndirect();
				}
				return _indirectForeignKeys;
			}
		}

		public IIndexes Indexes
		{
			get
			{
				if(null == _indexes)
				{
					_indexes = (Indexes)this.dbRoot.ClassFactory.CreateIndexes();
					_indexes.Table = this;
					_indexes.dbRoot = this.dbRoot;
					_indexes.LoadAll();
				}
				return _indexes;
			}
		}

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Tables.Database as Database;
				if(null == db._tableProperties)
				{
					db._tableProperties = new PropertyCollection();
					db._tableProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Table", null);
						parentNode.AppendChild(xmlNode);
					}

					db._tableProperties.LoadAllGlobal(xmlNode);
				}

				return db._tableProperties;
			}
		}

		virtual public IPropertyCollection AllProperties 
		{ 
			get
			{
				if(null == _allProperties)
				{
					_allProperties = new PropertyCollectionAll();
					_allProperties.Load(this.Properties, this.GlobalProperties);
				}

				return _allProperties;
			}
		}
		internal PropertyCollectionAll _allProperties = null;

		public virtual IColumns PrimaryKeys
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Objects

		public IDatabase Database
		{
			get
			{
				return this.Tables.Database;
			}
		}

		#endregion

		#region Properties

 	
		override public string Alias
		{
			get
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, false))
				{
					string niceName = null;

					if(this.GetUserData(node, "n", out niceName))
					{
						if(string.Empty != niceName)
							return niceName;
					}
				}

				// There was no nice name
				return this.Name;
			}

			set
			{
				XmlNode node = null;
				if(this.GetXmlNode(out node, true))
				{
					this.SetUserData(node, "n", value);
				}
			}
		}

		override public string Name
		{
			get
			{
				return this.GetString(Tables.f_Name);
			}
		}

		public string Schema
		{
			get
			{
				return this.GetString(Tables.f_Schema);
			}
		}

		public string Type
		{
			get
			{
				return this.GetString(Tables.f_Type);
			}
		}

		public Guid Guid
		{
			get
			{
				return this.GetGuid(Tables.f_Guid);
			}
		}

		public string Description
		{
			get
			{
				return this.GetString(Tables.f_Description);
			}
		}

		public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Tables.f_PropID);
			}
		}

		public DateTime DateCreated
		{
			get
			{
				return this.GetDateTime(Tables.f_DateCreated);
			}
		}

		public DateTime DateModified
		{
			get
			{
				return this.GetDateTime(Tables.f_DateModified);
			}
		}

		#endregion

		#region XML User Data

 
		override public string UserDataXPath
		{ 
			get
			{
				return Tables.UserDataXPath + @"/Table[@p='" + this.Name + "']";
			} 
		}

 
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.Tables.Database.GlobalUserDataXPath + "/Table";
			}
		}

 
		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.Tables.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Table[@p='" + this.Name + "']";
					if(!GetUserData(xPath, parentNode, out _xmlNode) && forceCreate)
					{
						// Create it, and try again
						this.CreateUserMetaData(parentNode);
						GetUserData(xPath, parentNode, out _xmlNode);
					}
				}
			}

			if(null != _xmlNode)
			{
				node = _xmlNode;
				success = true;
			}

			return success;
		}

 
		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Table", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("p");
			attr.Value = this.Name;
			myNode.Attributes.Append(attr);

			attr = parentNode.OwnerDocument.CreateAttribute("n");
			attr.Value = "";
			myNode.Attributes.Append(attr);
		}

		#endregion

		internal Tables Tables = null;
		protected Columns _columns = null;
		protected Columns _primaryKeys = null;
		protected ForeignKeys _foreignKeys = null;
		protected ForeignKeys _indirectForeignKeys = null;
		protected Indexes _indexes = null;
		
		#region INameValueCollection Members

		public string ItemName
		{
			get
			{
				return this.Name;
			}
		}

		public string ItemValue
		{
			get
			{
				return this.Name;
			}
		}

		#endregion

        #region IEquatable<Table> Members

        public bool Equals(ITable other) {
            if (other == null)
                return false;
            var o = other as Table;
            if (o == null)
                throw new NotImplementedException();

            return this.dbRoot == o.dbRoot
                && this._row == o._row;
        }

        #endregion

    }
}
