using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
 
	public class ForeignKey : Single, IForeignKey
	{
		public ForeignKey()
		{

		}

		#region Objects

		virtual public ITable PrimaryTable
		{
			get
			{
				string cat_schema = "";

				try
				{		
					string catalog = (DBNull.Value == this._row["PK_TABLE_CATALOG"]) ? string.Empty : (this._row["PK_TABLE_CATALOG"] as string);
					string schema  = (DBNull.Value == this._row["PK_TABLE_SCHEMA"])  ? string.Empty : (this._row["PK_TABLE_SCHEMA"] as string);

					if( (catalog != null && catalog.Length > 0) || 
						(schema  != null && schema.Length  > 0) )
					{
						cat_schema = catalog != string.Empty ? catalog : schema;
					}
					else
					{
						cat_schema = this.ForeignKeys.Table.Database.Name;
					}
				}
				catch
				{
					cat_schema = this.ForeignKeys.Table.Database.Name;
				}

				return this.dbRoot.Databases[cat_schema].Tables[this.GetString(ForeignKeys.f_PKTableName)];
			}
		}

		virtual public ITable ForeignTable
		{
			get
			{
				string cat_schema = "";

				try
				{

					string catalog = (DBNull.Value == this._row["FK_TABLE_CATALOG"]) ? string.Empty : (this._row["FK_TABLE_CATALOG"] as string);
					string schema  = (DBNull.Value == this._row["FK_TABLE_SCHEMA"])  ? string.Empty : (this._row["FK_TABLE_SCHEMA"] as string);

					if( (catalog != null && catalog.Length > 0) || 
						(schema  != null && schema.Length  > 0) )
					{
						cat_schema = catalog != string.Empty ? catalog : schema;
					}
					else
					{
						cat_schema = this.ForeignKeys.Table.Database.Name;
					}
				}
				catch
				{
					cat_schema = this.ForeignKeys.Table.Database.Name;
				}

				return this.dbRoot.Databases[cat_schema].Tables[this.GetString(ForeignKeys.f_FKTableName)];
			}
		}

		#endregion

		#region Collections

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.ForeignKeys.Table.Tables.Database as Database;
				if(null == db._foreignkeyProperties)
				{
					db._foreignkeyProperties = new PropertyCollection();
					db._foreignkeyProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ForeignKey", null);
						parentNode.AppendChild(xmlNode);
					}

					db._foreignkeyProperties.LoadAllGlobal(xmlNode);
				}

				return db._foreignkeyProperties;
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

		#endregion

		#region Properties

#if ENTERPRISE
		[DispId(0)]
#endif
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
				return this.GetString(ForeignKeys.f_FKName);
			}
		}

		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(ForeignKeys.f_Ordinal);
			}
		}

		virtual public string UpdateRule
		{
			get
			{
				return this.GetString(ForeignKeys.f_UpdateRule);
			}
		}

		virtual public string DeleteRule
		{
			get
			{
				return this.GetString(ForeignKeys.f_DeleteRule);
			}
		}

		virtual public string PrimaryKeyName
		{
			get
			{
				return this.GetString(ForeignKeys.f_PKName);
			}
		}

		virtual public string Deferrability            
		{
			get
			{
				System.Int16 i = this.GetInt16(ForeignKeys.f_Deferrability);

				switch(i)
				{
					case 1:
						return "INITIALLY_DEFERRED";
					case 2:
						return "INITIALLY_IMMEDIATE";
					case 3:
						return "NOT_DEFERRABLE";
					default:
						return "UNKNOWN";
				}
			}
		}

		#endregion

		#region XML User Data
 
		override public string UserDataXPath
		{ 
			get
			{
				return ForeignKeys.UserDataXPath + @"/ForeignKey[@p='" + this.Name + "']";
			} 
		}
 
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.ForeignKeys.Table.Tables.Database.GlobalUserDataXPath + "/ForeignKey";
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
				if(this.ForeignKeys.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ForeignKey[@p='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ForeignKey", null);
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

		virtual public IColumns PrimaryColumns
		{
			get
			{
				return _primaryColumns;
			}
		}

		virtual public IColumns ForeignColumns
		{
			get
			{
				return _foreignColumns;
			}
		}

		internal virtual void AddForeignColumn(string catalog, string schema,
			string physicalTableName, string physicalColumnName, bool primary)
		{
			Tables tables = null;

			if( (catalog != null && catalog.Length > 0) || 
				(schema  != null && schema.Length  > 0) )
			{
				string cat_schema = catalog != string.Empty ? catalog : schema;
				tables  = this.dbRoot.Databases[cat_schema].Tables as Tables;
			}
			else
			{
				// This DBMS is a one horse database
				tables = (Tables)this.ForeignKeys.Table.Database.Tables;
			}

			Column column = tables[physicalTableName].Columns[physicalColumnName] as Column;
			Column c = column.Clone();

			if(primary)
			{
				if(null == _primaryColumns)
				{
					_primaryColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_primaryColumns.ForeignKey = this;
				}

				_primaryColumns.AddColumn(c);
			}
			else
			{
				if(null == _foreignColumns)
				{
					_foreignColumns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_foreignColumns.ForeignKey = this;
				}

				_foreignColumns.AddColumn(c);
			}

			column.AddForeignKey(this);
		}

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

		protected Columns _primaryColumns = null;
		protected Columns _foreignColumns = null;
		internal ForeignKeys ForeignKeys = null;
	}
}
