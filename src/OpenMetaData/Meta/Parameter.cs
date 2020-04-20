using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
 
	public class Parameter : Single, IParameter, INameValueItem
	{
		public Parameter()
		{

		}

		#region Collections

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Parameters.Procedure.Procedures.Database as Database;
				if(null == db._parameterProperties)
				{
					db._parameterProperties = new PropertyCollection();
					db._parameterProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Parameter", null);
						parentNode.AppendChild(xmlNode);
					}

					db._parameterProperties.LoadAllGlobal(xmlNode);
				}

				return db._parameterProperties;
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
				return this.GetString(Parameters.f_ParameterName);
			}
		}

		virtual public System.Int32 Ordinal
		{
			get
			{
				return this.GetInt32(Parameters.f_Ordinal);
			}
		}

		virtual public System.Int32 ParameterType
		{
			get
			{
				return this.GetInt32(Parameters.f_Type);
			}
		}

		virtual public System.Boolean HasDefault
		{
			get
			{
				return this.GetBool(Parameters.f_HasDefault);
			}
		}

		virtual public string Default
		{
			get
			{
				if(this.HasDefault && null != Parameters.f_Default)
				{
					object o = _row[Parameters.f_Default];

					if(o == DBNull.Value)
					{
						return "<null>";
					}
				}

				return this.GetString(Parameters.f_Default);
			}
		}

		virtual public System.Boolean IsNullable
		{
			get
			{
				return this.GetBool(Parameters.f_IsNullable);
			}
		}

		virtual public System.Int32 DataType
		{
			get
			{
				return this.GetInt32(Parameters.f_DataType);
			}
		}

		virtual public System.Int32 CharacterMaxLength
		{
			get
			{
				return this.GetInt32(Parameters.f_CharMaxLength);
			}
		}

		virtual public System.Int32 CharacterOctetLength
		{
			get
			{
				return this.GetInt32(Parameters.f_CharOctetLength);
			}
		}

		virtual public System.Int32 NumericPrecision
		{
			get
			{
				return this.GetInt32(Parameters.f_NumericPrecision);
			}
		}

		virtual public System.Int32 NumericScale
		{
			get
			{
				return this.GetInt16(Parameters.f_NumericScale);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Parameters.f_Description);
			}
		}

		virtual public string TypeName
		{
			get
			{
				return this.GetString(Parameters.f_TypeName);
			}
		}

		virtual public string LocalTypeName
		{
			get
			{
				return this.GetString(Parameters.f_LocalTypeName);
			}
		}

		virtual public ParamDirection Direction
		{
			get
			{
				System.Int32 dir = this.ParameterType;

				switch(dir)
				{
					case 1:
						return ParamDirection.Input;
					case 2:
						return ParamDirection.InputOutput;
					case 3:
						return ParamDirection.Output;
					case 4:
						return ParamDirection.ReturnValue;
					default:
						return ParamDirection.Unknown;
				}
			}
		}

		virtual public string LanguageType
		{
			get
			{
				if(dbRoot.LanguageNode != null)
				{
					string xPath = @"./Type[@From='" + this.TypeName + "']";

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
					string xPath = @"./Type[@From='" + this.TypeName + "']";

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
				return this.GetString(Parameters.f_FullTypeName);
			}
		}


		#endregion

		#region XML User Data
 
		override public string UserDataXPath
		{ 
			get
			{
				return Parameters.UserDataXPath + @"/Parameter[@p='" + this.Name + "']";
			} 
		}
 
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.Parameters.Procedure.Procedures.Database.GlobalUserDataXPath + "/Parameter";
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
				if(this.Parameters.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Parameter[@p='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Parameter", null);
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

		internal Parameters Parameters = null;
	}
}
