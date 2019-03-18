using System;
using System.Xml;

namespace OMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(false), ClassInterface(ClassInterfaceType.AutoDual)]
#endif 
	public class ResultColumn : Single, IResultColumn, INameValueItem
	{
		public ResultColumn()
		{

		}

		#region Collections

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.ResultColumns.Procedure.Procedures.Database as Database;
				if(null == db._resultColumnProperties)
				{
					db._resultColumnProperties = new PropertyCollection();
					db._resultColumnProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ResultColumn", null);
						parentNode.AppendChild(xmlNode);
					}

					db._resultColumnProperties.LoadAllGlobal(xmlNode);
				}

				return db._resultColumnProperties;
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
				return this.GetString(null);
			}
		}

		virtual	public System.Int32 DataType 
		{ 
			get
			{
				return 0;
			}
		}

		virtual public string DataTypeName
		{
			get
			{
				return this.GetString(null);
			}
		}

		virtual public string DataTypeNameComplete
		{
			get
			{
				return this.GetString(null);
			}
		}

		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(null);
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

		#endregion

		#region XML User Data

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override public string UserDataXPath
		{ 
			get
			{
				return ResultColumns.UserDataXPath + @"/ResultColumn[@p='" + this.Name + "']";
			} 
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.ResultColumns.Procedure.Procedures.Database.GlobalUserDataXPath + "/ResultColumn";
			}
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override internal bool GetXmlNode(out XmlNode node, bool forceCreate)
		{
			node = null;
			bool success = false;

			if(null == _xmlNode)
			{
				// Get the parent node
				XmlNode parentNode = null;
				if(this.ResultColumns.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ResultColumn[@p='" + this.Name + "']";
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

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ResultColumn", null);
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
				return this.Ordinal.ToString();
			}
		}

		#endregion

		internal ResultColumns ResultColumns = null;
	}
}
