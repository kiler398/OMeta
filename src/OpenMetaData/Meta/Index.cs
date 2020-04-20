using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
 
	public class Index : Single, IIndex
	{
		public Index()
		{
			
		}

		internal void AddColumn(string physicalColumnName)
		{
			if(null == _columns)
			{
				_columns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
				_columns.dbRoot = this.dbRoot;
				_columns.Index = this;
			}

			Column column  = this.Indexes.Table.Columns[physicalColumnName] as Column;
			_columns.AddColumn(column);
		}

		#region Objects

		public ITable Table
		{
			get
			{
				return this.Indexes.Table;
			}
		}

		#endregion

		#region Collections

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Indexes.Table.Tables.Database as Database;
				if(null == db._indexProperties)
				{
					db._indexProperties = new PropertyCollection();
					db._indexProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Index", null);
						parentNode.AppendChild(xmlNode);
					}

					db._indexProperties.LoadAllGlobal(xmlNode);
				}

				return db._indexProperties;
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
				return this.GetString(Indexes.f_IndexName);
			}
		}

		virtual public string Schema
		{
			get
			{
				return this.GetString(Indexes.f_IndexSchema);
			}
		}

		virtual public System.Boolean Unique
		{
			get
			{
				return this.GetBool(Indexes.f_Unique);
			}
		}

		virtual public System.Boolean Clustered
		{
			get
			{
				return this.GetBool(Indexes.f_Clustered);
			}
		}

		virtual public string Type
		{
			get
			{
				System.Int32 i = this.GetInt32(Indexes.f_Type);

				switch(i)
				{
					case 1:
						return "BTREE";
					case 2:
						return "HASH";
					case 3:
						return "CONTENT";
					case 4:
						return "OTHER";
					default:
						return "OTHER";
				}
			}
		}

		virtual public System.Int32 FillFactor
		{
			get
			{
				return this.GetInt32(Indexes.f_FillFactor);
			}
		}

		virtual public System.Int32 InitialSize
		{
			get
			{
				return this.GetInt32(Indexes.f_InitializeSize);
			}
		}

		virtual public System.Boolean SortBookmarks
		{
			get
			{
				return this.GetBool(Indexes.f_SortBookmarks);
			}
		}

		virtual public System.Boolean AutoUpdate
		{
			get
			{
				return this.GetBool(Indexes.f_AutoUpdate);
			}
		}

		virtual public string NullCollation
		{
			get
			{
				System.Int32 i = this.GetInt32(Indexes.f_NullCollation);

				switch(i)
				{
					case 1:
						return "END";
					case 2:
						return "HIGH";
					case 4:
						return "LOW";
					case 8:
						return "START";
					default:
						return "UNKNOWN";
				}
			}
		}

		virtual public string Collation
		{
			get
			{
				System.Int32 i = this.GetInt16(Indexes.f_Collation);

				switch(i)
				{
					case 1:
						return "ASCENDING";
					case 2:
						return "DECENDING";
					default:
						return "UNKNOWN";
				}
			}
		}

		virtual public Decimal Cardinality
		{
			get
			{
				return this.GetDecimal(Indexes.f_Cardinality);
			}
		}

		virtual public System.Int32 Pages
		{
			get
			{
				return this.GetInt32(Indexes.f_Pages);
			}
		}

		virtual public string FilterCondition
		{
			get
			{
				return this.GetString(Indexes.f_FilterCondition);
			}
		}

		virtual public System.Boolean Integrated
		{
			get
			{
				return this.GetBool(Indexes.f_Integrated);
			}
		}
		#endregion

		#region XML User Data
 	
		override public string UserDataXPath
		{ 
			get
			{
				return Indexes.UserDataXPath + @"/Index[@p='" + this.Name + "']";
			} 
		}
 	
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.Indexes.Table.Tables.Database.GlobalUserDataXPath + "/Index";
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
				if(this.Indexes.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Index[@p='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Index", null);
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

		virtual public IColumns Columns
		{
			get
			{
				return _columns;
			}
		}

		private Columns _columns = null;
		internal Indexes Indexes = null;
	}
}
