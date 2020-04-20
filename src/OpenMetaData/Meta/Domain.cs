using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace OMeta
{
 
	public class Domain : Single, IDomain, INameValueItem
	{
		public Domain()
		{

		}

		#region Objects

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
				return this.GetString(Domains.f_DomainName);
			}
		}

		virtual public System.Boolean HasDefault
		{
			get
			{
				if(this.Default.Length > 0) 
					return true;
				else
					return false;
			}
		}

		virtual public string Default
		{
			get
			{
				return this.GetString(Domains.f_Default);
			}
		}

		virtual public System.Boolean IsNullable
		{
			get
			{
				return this.GetBool(Domains.f_IsNullable);
			}
		}

		virtual public System.Int32 CharacterMaxLength
		{
			get
			{
				return this.GetInt32(Domains.f_MaxLength);
			}
		}

		virtual public System.Int32 CharacterOctetLength
		{
			get
			{
				return this.GetInt32(Domains.f_OctetLength);
			}
		}

		virtual public System.Int32 NumericPrecision
		{
			get
			{
				return this.GetInt32(Domains.f_NumericPrecision);
			}
		}

		virtual public System.Int32 NumericScale
		{
			get
			{
				return this.GetInt32(Domains.f_NumericScale);
			}
		}

		virtual public System.Int32 DateTimePrecision
		{
			get
			{
				return this.GetInt32(Domains.f_DatetimePrecision);
			}
		}

		virtual public string CharacterSetCatalog
		{
			get
			{
				return this.GetString(Domains.f_CharSetCatalog);
			}
		}

		virtual public string CharacterSetSchema
		{
			get
			{
				return this.GetString(Domains.f_CharSetSchema);
			}
		}

		virtual public string CharacterSetName
		{
			get
			{
				return this.GetString(Domains.f_CharSetName);
			}
		}

		virtual public string DomainCatalog
		{
			get
			{
				return this.GetString(Domains.f_DomainCatalog);
			}
		}

		virtual public string DomainSchema
		{
			get
			{
				return this.GetString(Domains.f_DomainSchema);
			}
		}

		virtual public string DomainName
		{
			get
			{
				return this.GetString(Domains.f_DomainName);
			}
		}

		virtual public string DataTypeName
		{
			get
			{
				return this.GetString(Domains.f_DataType);
			}
		}

		virtual public string LanguageType
		{
			get
			{
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
				return "Unknown";
			}
		}

		#endregion

		#region Collections

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Domains.Database as Database;
				if(null == db._domainProperties)
				{
					db._domainProperties = new PropertyCollection();
					db._domainProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Domain", null);
						parentNode.AppendChild(xmlNode);
					}

					db._domainProperties.LoadAllGlobal(xmlNode);
				}

				return db._domainProperties;
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

		#region XML User Data
 
		override public string UserDataXPath
		{ 
			get
			{
				return Domains.UserDataXPath + @"/Domain[@p='" + this.Name + "']";
			} 
		}
 
		override public string GlobalUserDataXPath
		{
			get
			{
				return Domains.UserDataXPath + @"/Domain[@p='" + this.Name + "']";
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
				if(this.Domains.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Domain[@p='" + this.Name + "']";
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

		internal Domains Domains = null;
	}
}
