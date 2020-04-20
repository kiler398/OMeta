using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
 
	public class Procedure : Single, IProcedure, INameValueItem
	{
		public Procedure()
		{

		}

		#region Collections

		virtual public IResultColumns ResultColumns
		{
			get
			{
				if(null == _resultColumns)
				{
					_resultColumns = (ResultColumns)this.dbRoot.ClassFactory.CreateResultColumns();
					_resultColumns.Procedure = this;
					_resultColumns.dbRoot = this.dbRoot;
					_resultColumns.LoadAll();
				}
				return _resultColumns;
			}
		}

		virtual public IParameters Parameters
		{
			get
			{
				if(null == _parameters)
				{
					_parameters = (Parameters)this.dbRoot.ClassFactory.CreateParameters();
					_parameters.Procedure = this;
					_parameters.dbRoot = this.dbRoot;
					_parameters.LoadAll();
				}
				return _parameters;
			}
		}

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Procedures.Database as Database;
				if(null == db._procedureProperties)
				{
					db._procedureProperties = new PropertyCollection();
					db._procedureProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Procedure", null);
						parentNode.AppendChild(xmlNode);
					}

					db._procedureProperties.LoadAllGlobal(xmlNode);
				}

				return db._procedureProperties;
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

		#region Objects

		public IDatabase Database
		{
			get
			{
				return this.Procedures.Database;
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
				return this.GetString(Procedures.f_Name);
			}
		}

		public string Schema
		{
			get
			{
				return this.GetString(Procedures.f_Schema);
			}
		}

		virtual public System.Int16 Type
		{
			get
			{
				return this.GetInt16(Procedures.f_Type);
			}
		}

		virtual public string ProcedureText
		{
			get
			{
				return this.GetString(Procedures.f_ProcedureDefinition);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Procedures.f_Description);
			}
		}

		virtual public DateTime DateCreated
		{
			get
			{
				return this.GetDateTime(Procedures.f_DateCreated);
			}
		}

		virtual public DateTime DateModified
		{
			get
			{
				return this.GetDateTime(Procedures.f_DateModified);
			}
		}

		#endregion

		#region XML User Data
 
		override public string UserDataXPath
		{ 
			get
			{
				return Procedures.UserDataXPath + @"/Procedure[@p='" + this.Name + "']";
			} 
		}
 
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.Procedures.Database.GlobalUserDataXPath + "/Procedure";
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
				if(this.Procedures.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Procedure[@p='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Procedure", null);
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

		internal Procedures Procedures = null;
		private Parameters _parameters = null;
		private ResultColumns _resultColumns = null;
	}
}
