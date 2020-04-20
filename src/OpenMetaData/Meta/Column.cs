using System;
using System.Xml;
using System.Runtime.InteropServices;

namespace OMeta
{

	public class Column : Single, IColumn, INameValueItem
	{
		public Column()
		{

		}

		virtual internal Column Clone()
		{
			Column c = (Column)this.dbRoot.ClassFactory.CreateColumn();

			c.dbRoot	= this.dbRoot;
			c.Columns	= this.Columns;
			c._row		= this._row;

			c._foreignKeys	= Column._emptyForeignKeys;

			return c;
		}


		#region Objects

		public ITable Table
		{
			get
			{
				ITable theTable = null;

				if(null != Columns.Table)
				{
					theTable = Columns.Table;
				}
				else if(null != Columns.Index)
				{
					theTable =  Columns.Index.Indexes.Table;
				}
				else if(null != Columns.ForeignKey)
				{
					theTable =  Columns.ForeignKey.ForeignKeys.Table;
				}

				return theTable;
			}
		}

		public IView View
		{
			get
			{
				IView theView = null;

				if(null != Columns.View)
				{
					theView = Columns.View;
				}

				return theView;
			}
		}

		public IDomain Domain
		{
			get
			{
				IDomain theDomain = null;

				if(this.HasDomain)
				{
					theDomain = this.Columns.GetDatabase().Domains[this.DomainName];
				}

				return theDomain;
			}
		}

		#endregion

		#region Properties

		[DispId(0)]
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
				return this.GetString(Columns.f_Name);
			}
		}

		virtual public Guid Guid
		{
			get
			{
				return this.GetGuid(Columns.f_Guid);
			}
		}

		virtual public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Columns.f_PropID);
			}
		}

		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(Columns.f_Ordinal);
			}
		}

		virtual public System.Boolean HasDefault
		{
			get
			{
				return this.GetBool(Columns.f_HasDefault);
			}
		}

		virtual public string Default
		{
			get
			{
				return this.GetString(Columns.f_Default);
			}
		}

		virtual public System.Int32 Flags
		{
			get
			{
				return this.GetInt32(Columns.f_Flags);
			}
		}

		virtual public System.Boolean IsNullable
		{
			get
			{
				return this.GetBool(Columns.f_IsNullable);
			}
		}

		virtual public System.Int32 DataType
		{
			get
			{
				return this.GetInt32(Columns.f_DataType);
			}
		}

		virtual public Guid TypeGuid
		{
			get
			{
				return this.GetGuid(Columns.f_TypeGuid);
			}
		}

		virtual public System.Int32 CharacterMaxLength
		{
			get
			{
				return this.GetInt32(Columns.f_MaxLength);
			}
		}

		virtual public System.Int32 CharacterOctetLength
		{
			get
			{
				return this.GetInt32(Columns.f_OctetLength);
			}
		}

		virtual public System.Int32 NumericPrecision
		{
			get
			{
				return this.GetInt32(Columns.f_NumericPrecision);
			}
		}

		virtual public System.Int32 NumericScale
		{
			get
			{
				return this.GetInt32(Columns.f_NumericScale);
			}
		}

		virtual public System.Int32 DateTimePrecision
		{
			get
			{
				return this.GetInt32(Columns.f_DatetimePrecision);
			}
		}

		virtual public string CharacterSetCatalog
		{
			get
			{
				return this.GetString(Columns.f_CharSetCatalog);
			}
		}

		virtual public string CharacterSetSchema
		{
			get
			{
				return this.GetString(Columns.f_CharSetSchema);
			}
		}

		virtual public string CharacterSetName
		{
			get
			{
				return this.GetString(Columns.f_CharSetName);
			}
		}

		virtual public string DomainCatalog
		{
			get
			{
				return this.GetString(Columns.f_DomainCatalog);
			}
		}

		virtual public string DomainSchema
		{
			get
			{
				return this.GetString(Columns.f_DomainSchema);
			}
		}

		virtual public string DomainName
		{
			get
			{
				return this.GetString(Columns.f_DomainName);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Columns.f_Description);
			}
		}

		virtual public System.Int32 LCID
		{
			get
			{
				return this.GetInt32(Columns.f_LCID);
			}
		}

		virtual public System.Int32 CompFlags
		{
			get
			{
				return this.GetInt32(Columns.f_CompFlags);
			}
		}

		virtual public System.Int32 SortID
		{
			get
			{
				return this.GetInt32(Columns.f_SortID);
			}
		}

		virtual public System.Byte[] TDSCollation
		{
			get
			{
				return this.GetByteArray(Columns.f_TDSCollation);
			}
		}

		virtual public System.Boolean IsComputed
		{
			get
			{
				return this.GetBool(Columns.f_IsComputed);
			}
		}

		virtual public System.Boolean IsInPrimaryKey
		{
			get
			{
				bool isPrimaryKey = false;

				if(null != Columns.Table)
				{
                    IColumns keys = Columns.Table.PrimaryKeys;
                    foreach (IColumn key in keys)
                    {
                        if (key != null)
                        {
                            if (key.Name == this.Name)
                            {
                                isPrimaryKey = true;
                                break;
                            }
                        }
                    }
				}

				return isPrimaryKey;
			}
		}

		virtual public System.Boolean IsAutoKey
		{
			get
			{
				return this.GetBool(Columns.f_IsAutoKey);
			}
		}

		virtual public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				return this.GetString(null);
			}
		}

		virtual public string LanguageType
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.LanguageType;
						}
					}
				}

				if(dbRoot.LanguageNode != null)
				{
					string xPath = @"./Type[@From='" + this.DataTypeName + "']";

					XmlNode node = dbRoot.LanguageNode.SelectSingleNode(xPath, null);

					if(node != null)
					{
						string languageType = "";
						if(this.GetUserData(node, "To", out languageType))
						{
							return languageType;
						}
					}
				}

				return "Unknown";
			}
		}

		virtual public string DbTargetType
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DbTargetType;
						}
					}
				}

				if(dbRoot.DbTargetNode != null)
				{
					string xPath = @"./Type[@From='" + this.DataTypeName + "']";

					XmlNode node = dbRoot.DbTargetNode.SelectSingleNode(xPath, null);

					if(node != null)
					{
						string driverType = "";
						if(this.GetUserData(node, "To", out driverType))
						{
							return driverType;
						}
					}
				}

				return "Unknown";
			}
		}

		virtual public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeNameComplete;
						}
					}
				}

				return "Unknown";
			}
		}

		virtual public System.Boolean IsInForeignKey
		{
			get
			{
				if(this.ForeignKeys == Column._emptyForeignKeys)
					return true;
				else
					return this.ForeignKeys.Count > 0 ? true : false;
			}
		}

		virtual public System.Int32 AutoKeySeed
		{
			get
			{
				return this.GetInt32(Columns.f_AutoKeySeed);
			}
		}

		virtual public System.Int32 AutoKeyIncrement
		{
			get
			{
				return this.GetInt32(Columns.f_AutoKeyIncrement);
			}
		}

		virtual public System.Boolean HasDomain
		{
			get
			{
				if(this._row.Table.Columns.Contains("DOMAIN_NAME"))
				{
					object o = this._row["DOMAIN_NAME"];

					if(o != null && o != DBNull.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		#endregion

		#region Collections

		public IForeignKeys ForeignKeys
		{
			get
			{
				if(null == _foreignKeys)
				{
					_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
					_foreignKeys.dbRoot = this.dbRoot;

					if(this.Columns.Table != null)
					{
						IForeignKeys fk = this.Columns.Table.ForeignKeys;
					}
				}
				return _foreignKeys;
			}
		}

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Columns.GetDatabase() as Database;
				if(null == db._columnProperties)
				{
					db._columnProperties = new PropertyCollection();
					db._columnProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Column", null);
						parentNode.AppendChild(xmlNode);
					}

					db._columnProperties.LoadAllGlobal(xmlNode);
				}

				return db._columnProperties;
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

		protected internal virtual void AddForeignKey(ForeignKey fk)
		{
			if(null == _foreignKeys)
			{
				_foreignKeys = (ForeignKeys)this.dbRoot.ClassFactory.CreateForeignKeys();
				_foreignKeys.dbRoot = this.dbRoot;
			}

			this._foreignKeys.AddForeignKey(fk);
		}

		internal PropertyCollectionAll _allProperties = null;

		#endregion

		#region XML User Data

		[ComVisible(false)]
		override public string UserDataXPath
		{ 
			get
			{
				return Columns.UserDataXPath + @"/Column[@p='" + this.Name + "']";
			} 
		}

		[ComVisible(false)]
		override public string GlobalUserDataXPath
		{
			get
			{
				return ((Database)this.Columns.GetDatabase()).GlobalUserDataXPath + "/Column";
			}
		}

		[ComVisible(false)]
		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.Columns.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Column[@p='" + this.Name + "']";
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

		[ComVisible(false)]
		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Column", null);
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

		internal Columns Columns = null;
		protected ForeignKeys _foreignKeys = null;
		static private ForeignKeys _emptyForeignKeys = new ForeignKeys();

        #region IEquatable<IColumn> Members

        public bool Equals(IColumn other) {
            if (other == null)
                return false;
            var o = other as Column;
            if (o == null)
                throw new NotImplementedException();

            return this.dbRoot == o.dbRoot
                && this.Columns == o.Columns
                && this._row == o._row;
        }

        #endregion
    }
}
