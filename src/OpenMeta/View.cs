using System;
using System.Xml;
using System.Data;
using System.Data.OleDb;

namespace MyMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	/// <summary>
	/// 数据库视图元数据
	/// </summary>
	[ComVisible(false), ClassInterface(ClassInterfaceType.AutoDual)]
#endif 
	public class View : Single, IView, INameValueItem, ITabularEntity
	{
		public View()
		{

		}

		#region Collections

		/// <summary>
		/// 数据库字段元数据
		/// </summary>
		public IColumns Columns
		{
			get
			{
				if(null == _columns)
				{
					_columns = (Columns)this.dbRoot.ClassFactory.CreateColumns();
					_columns.View = this;
					_columns.dbRoot = this.dbRoot;
					_columns.LoadForView();
				}
				return _columns;
			}
		}

		virtual public IViews SubViews 
		{ 
			get
			{
				if(null == _views)
				{
					_views = (Views)this.dbRoot.ClassFactory.CreateViews();
					_views.dbRoot = this._dbRoot;
					_views.Database = this.Views.Database;
				}
				return _views;				
			}
		}

		virtual public ITables SubTables
		{ 
			get
			{
				if(null == _tables)
				{
					_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
					_tables.dbRoot = this._dbRoot;
					_tables.Database = this.Views.Database;
				}
				return _tables;
			}
		}

		virtual public IPropertyCollection GlobalProperties 
		{ 
			get
			{
				Database db = this.Views.Database as Database;
				if(null == db._viewProperties)
				{
					db._viewProperties = new PropertyCollection();
					db._viewProperties.Parent = this;

					string xPath    = this.GlobalUserDataXPath;
					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);

					if(xmlNode == null)
					{
						XmlNode parentNode = db.CreateGlobalXmlNode();

						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "View", null);
						parentNode.AppendChild(xmlNode);
					}

					db._viewProperties.LoadAllGlobal(xmlNode);
				}

				return db._viewProperties;
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
				return this.Views.Database;
			}
		}

		#endregion

		#region Properties

#if ENTERPRISE
		/// <summary>
		/// 别名
		/// </summary>
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

		/// <summary>
		/// 名称
		/// </summary>
		override public string Name
		{
			get
			{
				return this.GetString(Views.f_Name);
			}
		}

		/// <summary>
		/// 架构名
		/// </summary>
		public string Schema
		{
			get
			{
				return this.GetString(Views.f_Schema);
			}
		}

		/// <summary>
		/// 视图代码
		/// </summary>
		public virtual string ViewText
		{
			get
			{
				return this.GetString(Views.f_ViewDefinition);
			}
		}

		public System.Boolean CheckOption
		{
			get
			{
				return this.GetBool(Views.f_CheckOption);
			}
		}

		public System.Boolean IsUpdateable
		{
			get
			{
				return this.GetBool(Views.f_IsUpdateable);
			}
		}

		public string Type
		{
			get
			{
				return this.GetString(Views.f_Type);
			}
		}

		public Guid Guid
		{
			get
			{
				return this.GetGuid(Views.f_Guid);
			}
		}

		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			get
			{
				return this.GetString(Views.f_Description);
			}
		}

		public System.Int32 PropID
		{
			get
			{
				return this.GetInt32(Views.f_PropID);
			}
		}

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateTime DateCreated
		{
			get
			{
				return this.GetDateTime(Views.f_DateCreated);
			}
		}

		/// <summary>
		/// 修改日期
		/// </summary>
		public DateTime DateModified
		{
			get
			{
				return this.GetDateTime(Views.f_DateModified);
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
				return Views.UserDataXPath + @"/View[@p='" + this.Name + "']";
			} 
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override public string GlobalUserDataXPath
		{
			get
			{
				return this.Views.Database.GlobalUserDataXPath + "/View";
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
				if(this.Views.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./View[@p='" + this.Name + "']";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "View", null);
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

		internal Views Views = null;
		private Columns _columns = null;
		protected bool _subViewInfoLoaded = false;
		protected Views _views = null;
		protected Tables _tables = null;
	}
}
