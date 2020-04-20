using System;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using OpenMeta.Meta;


namespace OMeta
{
    public class Database : Single, IDatabase, INameValueItem
	{
        protected Hashtable dataTypeTables = new Hashtable();

		public Database()
		{

		}

        virtual public IResultColumns ResultColumnsFromSQL(string sql)
        {
            IResultColumns columns = null;
			using (OleDbConnection cn = new OleDbConnection(dbRoot.ConnectionString))
            {
                cn.Open();
                columns = ResultColumnsFromSQL(sql, cn);
            }
            return columns;
        }

		virtual public DataSet ExecuteSql(string sql)
		{
            DataSet oRS = new DataSet();
			OleDbConnection cn = null;
			OleDbDataReader reader = null;

			try 
			{
				cn = new OleDbConnection(dbRoot.ConnectionString);
				cn.Open();
                try
                {
                    cn.ChangeDatabase(this.Name);
                }
                catch { } // some databases don't have the concept of catalogs. Catch this and throw it out
                
				OleDbCommand command = new OleDbCommand(sql, cn);
				command.CommandType = CommandType.Text;

				reader = command.ExecuteReader();

				DataTable schema = reader.GetSchemaTable();

                oRS.Tables.Add(schema);

				cn.Close();
            }
			catch (Exception ex) 
			{
				if ((reader != null) && (!reader.IsClosed)) 
				{
					reader.Close();
					reader = null;
				}
				if ((cn != null) && (cn.State == ConnectionState.Open)) 
				{
					cn.Close();
					cn = null;
				}
				throw ex;
			}

			return oRS;
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oledbType"></param>
        /// <param name="providerTypeInt"></param>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <param name="numericPrecision"></param>
        /// <param name="numericScale"></param>
        /// <param name="isLong"></param>
        /// <param name="dbTypeName"></param>
        /// <param name="dbTypeNameComplete"></param>
        /// <returns></returns>
        protected virtual bool GetNativeType(
            OleDbType oledbType, int providerTypeInt, string dataType, 
            int length, int numericPrecision, int numericScale, bool isLong,
            out string dbTypeName, out string dbTypeNameComplete)
        {
            bool rval = false;

            IProviderType provType = null;
            if (providerTypeInt >= 0)
            {
                if (!dataTypeTables.ContainsKey(providerTypeInt))
                {
                    foreach (IProviderType ptypeLoop in dbRoot.ProviderTypes)
                    {
                        if (ptypeLoop.DataType == providerTypeInt)
                        {
                            if ((provType == null) ||
                                (ptypeLoop.BestMatch && !provType.BestMatch) ||
                                (isLong && ptypeLoop.IsLong && !provType.IsLong) ||
                                (!ptypeLoop.IsFixedLength && provType.IsFixedLength))
                            {
                                provType = ptypeLoop;
                            }
                        }
                    }
                    dataTypeTables[providerTypeInt] = provType;
                }
                else
                {
                    provType = dataTypeTables[providerTypeInt] as IProviderType;
                }
            }

            if (provType != null)
            {
                string dtype = provType.Type;
                string[] parms = provType.CreateParams.Split(',');
                if (parms.Length > 0)
                {
                    dtype += "(";
                    int xx = 0;
                    for (int i = 0; i < parms.Length; i++)
                    {
                        switch (parms[i])
                        {
                            case "precision":
                                dtype += (xx > 0 ? ", " : "") + numericPrecision.ToString();
                                xx++;
                                break;
                            case "scale":
                                dtype += (xx > 0 ? ", " : "") + numericScale.ToString();
                                xx++;
                                break;
                            case "length":
                            case "max length":
                                dtype += (xx > 0 ? ", " : "") + length.ToString();
                                xx++;
                                break;
                        }
                    }
                    dtype += ")";
                    if (xx == 0) dtype = dtype.Substring(0, dtype.Length - 2);
                }

                dbTypeName = provType.Type;
                dbTypeNameComplete = dtype;
                rval = true;
            }
            else
            {
                dbTypeName = string.Empty;
                dbTypeNameComplete = string.Empty;
            }
            return rval;
        }

        protected IResultColumns ResultColumnsFromSQL(string sql, IDbConnection cn)
        {
            OMetaPluginContext context = new OMetaPluginContext(null, null);
            DataTable metaData = context.CreateResultColumnsDataTable();
            Plugin.PluginResultColumns resultCols = new Plugin.PluginResultColumns(null);
            resultCols.dbRoot = dbRoot;

            IDbCommand command = cn.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable schema;
                //DataTable data;
                string dataType, fieldname;
                int length;

                // Skip columns contains the index of any columns that we cannot handle, array types and such ...
                Hashtable skipColumns = null;

                reader.Read();
                schema = reader.GetSchemaTable();

                IProviderType provType = null;
                int columnOrdinal = 0, numericPrecision = 0, numericScale = 0, providerTypeInt = -1, colID = 0;
                bool isLong = false;
                string dbTypeName = string.Empty, dbTypeNameComplete = string.Empty;

                foreach (DataRow row in schema.Rows)
                {
                    DataRow metarow = metaData.NewRow();
                    fieldname = row["ColumnName"].ToString();
                    dataType = row["DataType"].ToString();
                    length = 0;

                    provType = null;
                    columnOrdinal = 0; 
                    numericPrecision = 0; 
                    numericScale = 0; 
                    providerTypeInt = -1;
                    isLong = false;

                    if (row["ColumnSize"] != DBNull.Value) length = Convert.ToInt32(row["ColumnSize"]);
                    if (row["ColumnOrdinal"] != DBNull.Value) columnOrdinal = Convert.ToInt32(row["ColumnOrdinal"]);
                    if (row["NumericPrecision"] != DBNull.Value) numericPrecision = Convert.ToInt32(row["NumericPrecision"]);
                    if (row["NumericScale"] != DBNull.Value) numericScale = Convert.ToInt32(row["NumericScale"]);
                    if (row["IsLong"] != DBNull.Value) isLong = Convert.ToBoolean(row["IsLong"]);
                    if (row["ProviderType"] != DBNull.Value) providerTypeInt = Convert.ToInt32(row["ProviderType"]);

                    OleDbType oledbType;
                    try { oledbType = (OleDbType)providerTypeInt; }
                    catch { oledbType = OleDbType.IUnknown; }

                    this.GetNativeType(oledbType, providerTypeInt, dataType, length, numericPrecision, numericScale, isLong, out dbTypeName, out dbTypeNameComplete);

                    metarow["COLUMN_NAME"] = fieldname;
                    metarow["ORDINAL_POSITION"] = columnOrdinal;
                    metarow["DATA_TYPE"] = providerTypeInt;
                    metarow["TYPE_NAME"] = dbTypeName;
                    metarow["TYPE_NAME_COMPLETE"] = dbTypeNameComplete;

                    metaData.Rows.Add(metarow);
                }

                resultCols.Populate(metaData);
            }

            return resultCols;
        }

		protected DataSet ExecuteIntoRecordset(string sql, IDbConnection cn)
		{
			DataSet oRS = new DataSet();
			IDataReader reader = null;

			try 
			{
				IDbCommand command = cn.CreateCommand();
				command.CommandText = sql;
				command.CommandType = CommandType.Text;

				reader = command.ExecuteReader();

                DataTable schema = reader.GetSchemaTable();

                oRS.Tables.Add(schema);

			}
			catch (Exception ex) 
			{
				if ((reader != null) && (!reader.IsClosed)) 
				{
					reader.Close();
					reader = null;
				}
				if ((cn != null) && (cn.State == ConnectionState.Open)) 
				{
					cn.Close();
					cn = null;
				}
				throw ex;
			}

			return oRS;
		}



		protected DataTypeEnum GetADOType(string sType)
		{
			switch(sType)
			{
				case null:
					return DataTypeEnum.adEmpty;

				case "System.Byte":
					return DataTypeEnum.adUnsignedTinyInt;

				case "System.SByte":
					return DataTypeEnum.adTinyInt;

				case "System.Boolean":
					return DataTypeEnum.adBoolean;

				case "System.Int16":
					return DataTypeEnum.adSmallInt;

				case "System.Int32":
					return DataTypeEnum.adInteger;

				case "System.Int64":
					return DataTypeEnum.adBigInt;

				case "System.Single":
					return DataTypeEnum.adSingle;

				case "System.Double":
					return DataTypeEnum.adDouble;

				case "System.Decimal":
					return DataTypeEnum.adDecimal;

				case "System.DateTime":
					return DataTypeEnum.adDate;

				case "System.Guid":
					return DataTypeEnum.adGUID;

				case "System.String":
					return DataTypeEnum.adBSTR; //.adChar;

				case "System.Byte[]":
					return DataTypeEnum.adBinary;

				case "System.Array":
					return DataTypeEnum.adArray;

				case "System.Object":
					return DataTypeEnum.adVariant;

				default:
					return 0;
			}
		}


		virtual public ITables Tables
		{
			get
			{
				if(null == _tables)
				{
					_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
					_tables.dbRoot = this._dbRoot;
					_tables.Database = this;
					_tables.LoadAll();
				}

				return _tables;
			}
		}

		virtual public IViews Views
		{
			get
			{
				if(null == _views)
				{
					_views = (Views)this.dbRoot.ClassFactory.CreateViews();
					_views.dbRoot = this._dbRoot;
					_views.Database = this;
					_views.LoadAll();
				}

				return _views;
			}
		}

		virtual public IProcedures Procedures
		{
			get
			{
				if(null == _procedures)
				{
					_procedures = (Procedures)this.dbRoot.ClassFactory.CreateProcedures();
					_procedures.dbRoot = this._dbRoot;
					_procedures.Database = this;
					_procedures.LoadAll();
				}

				return _procedures;
			}
		}

		virtual public IDomains Domains
		{
			get
			{
				if(null == _domains)
				{
					_domains = (Domains)this.dbRoot.ClassFactory.CreateDomains();
					_domains.dbRoot = this._dbRoot;
					_domains.Database = this;
					_domains.LoadAll();
				}

				return _domains;
			}
		}

//		virtual public IPropertyCollection GlobalProperties 
//		{ 
//			get
//			{
//				Database db = this as Database;
//				if(null == db._columnProperties)
//				{
//					db._columnProperties = new PropertyCollection();
//					db._columnProperties.Parent = this;
//
//					string xPath    = this.GlobalUserDataXPath;
//					XmlNode xmlNode = this.dbRoot.UserData.SelectSingleNode(xPath, null);
//
//					if(xmlNode == null)
//					{
//						XmlNode parentNode = db.CreateGlobalXmlNode();
//
//						xmlNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Database", null);
//						parentNode.AppendChild(xmlNode);
//					}
//
//					db._columnProperties.LoadAllGlobal(xmlNode);
//				}
//
//				return db._columnProperties;
//			}
//		}

 
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

        public virtual string XmlMetaDataKey
        {
            get 
            {
                if (dbRoot.UserDataDatabaseMappings.ContainsKey(Name))
                {
                    return dbRoot.UserDataDatabaseMappings[Name];
                }
                else
                {
                    return Name;
                }
            }
        }

		override public string Name
		{
			get
			{
				return this.GetString(Databases.f_Catalog);
			}
		}

		virtual public string Description
		{
			get
			{
				return this.GetString(Databases.f_Description);
			}
		}

		virtual public string SchemaName
		{
			get
			{
				return this.GetString(Databases.f_SchemaName);
			}
		}

		virtual public string SchemaOwner
		{
			get
			{
				return this.GetString(Databases.f_SchemaOwner);
			}
		}

		virtual public string DefaultCharSetCatalog
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetCat);
			}
		}

		virtual public string DefaultCharSetSchema
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetSchema);
			}
		}

		virtual public string DefaultCharSetName
		{
			get
			{
				return this.GetString(Databases.f_DefCharSetName);
			}
		}

		virtual public dbRoot Root
		{
			get
			{
				return this.dbRoot;
			}
		}

		#region XML User Data
 	
		override public string UserDataXPath
		{ 
			get
			{
				return Databases.UserDataXPath + @"/Database[@p='" + this.XmlMetaDataKey + "']";
			} 
		}
 
		override public string GlobalUserDataXPath
		{
			get
			{
                return @"//MyMeta/Global/Databases/Database[@p='" + this.XmlMetaDataKey + "']";
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
				if(this.Databases.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
                    string xPath = @"./Database[@p='" + this.XmlMetaDataKey + "']";
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
 	
		internal XmlNode CreateGlobalXmlNode()
		{
			XmlNode node = null;

			XmlNode parentNode = null;
			this.dbRoot.GetXmlNode(out parentNode, true);

			node = parentNode.SelectSingleNode(@"./Global");
			if(node == null)
			{
				node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Global", null);
				parentNode.AppendChild(node);
			}
			parentNode = node;

			node = parentNode.SelectSingleNode(@"./Databases");
			if(node == null)
			{
				node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Databases", null);
				parentNode.AppendChild(node);
			}
			parentNode = node;

			node = parentNode.SelectSingleNode(@"./Database[@p='" + this.XmlMetaDataKey + "']");
			if(node == null)
			{
				node = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Database", null);
				parentNode.AppendChild(node);

				XmlAttribute attr;

				attr = node.OwnerDocument.CreateAttribute("p");
                attr.Value = this.XmlMetaDataKey;
				node.Attributes.Append(attr);
			}

			return node;
		}
 	
		override public void CreateUserMetaData(XmlNode parentNode)
		{
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Database", null);
			parentNode.AppendChild(myNode);

			XmlAttribute attr;

			attr = parentNode.OwnerDocument.CreateAttribute("p");
            attr.Value = this.XmlMetaDataKey;
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

		internal  Databases Databases = null;
		protected Tables _tables = null;
		protected Views _views = null;
		protected Procedures _procedures = null;
		protected Domains _domains = null;

		// Global properties are per Database
		internal PropertyCollection  _columnProperties = null;
		internal PropertyCollection  _databaseProperties = null;
		internal PropertyCollection  _foreignkeyProperties = null;
		internal PropertyCollection  _indexProperties = null;
		internal PropertyCollection  _parameterProperties = null;
		internal PropertyCollection  _procedureProperties = null;
		internal PropertyCollection  _resultColumnProperties = null;
		internal PropertyCollection  _tableProperties = null;
		internal PropertyCollection  _viewProperties = null;
		internal PropertyCollection  _domainProperties = null;
	}
}
