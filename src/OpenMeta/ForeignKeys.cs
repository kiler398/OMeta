using System;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace OMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(false), ClassInterface(ClassInterfaceType.AutoDual)]
#endif 
	public class ForeignKeys : Collection, IForeignKeys, IEnumerable, ICollection
	{
		public ForeignKeys()
		{

		}

		internal DataColumn f_PKTableCatalog	= null;
		internal DataColumn f_PKTableSchema		= null;
		internal DataColumn f_PKTableName		= null;
		internal DataColumn f_FKTableCatalog	= null;
		internal DataColumn f_FKTableSchema		= null;
		internal DataColumn f_FKTableName		= null;
		internal DataColumn f_Ordinal			= null;
		internal DataColumn f_UpdateRule		= null;
		internal DataColumn f_DeleteRule		= null;
		internal DataColumn f_PKName			= null;
		internal DataColumn f_FKName			= null;
		internal DataColumn f_Deferrability 	= null;

		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PK_TABLE_CATALOG"))    f_PKTableCatalog = metaData.Columns["PK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("PK_TABLE_SCHEMA"))	 f_PKTableSchema = metaData.Columns["PK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("PK_TABLE_NAME"))		 f_PKTableName = metaData.Columns["PK_TABLE_NAME"];
				if(metaData.Columns.Contains("FK_TABLE_CATALOG"))	 f_FKTableCatalog = metaData.Columns["FK_TABLE_CATALOG"];
				if(metaData.Columns.Contains("FK_TABLE_SCHEMA"))	 f_FKTableSchema = metaData.Columns["FK_TABLE_SCHEMA"];
				if(metaData.Columns.Contains("FK_TABLE_NAME"))		 f_FKTableName = metaData.Columns["FK_TABLE_NAME"];
				if(metaData.Columns.Contains("ORDINAL"))			 f_Ordinal = metaData.Columns["ORDINAL"];
				if(metaData.Columns.Contains("UPDATE_RULE"))		 f_UpdateRule = metaData.Columns["UPDATE_RULE"];
				if(metaData.Columns.Contains("DELETE_RULE"))		 f_DeleteRule = metaData.Columns["DELETE_RULE"];
				if(metaData.Columns.Contains("PK_NAME"))			 f_PKName = metaData.Columns["PK_NAME"];
				if(metaData.Columns.Contains("FK_NAME"))			 f_FKName= metaData.Columns["FK_NAME"];
				if(metaData.Columns.Contains("DEFERRABILITY"))		 f_Deferrability = metaData.Columns["DEFERRABILITY"];

				_fieldsBound = true;
			}
		}

        private void BindToColumnsCustom(DataTable metaData, NameValueCollection map)
        {
            if (false == _fieldsBound)
            {
                if (metaData.Columns.Contains(map["PK_TABLE_CATALOG"])) f_PKTableCatalog = metaData.Columns[map["PK_TABLE_CATALOG"]];
                if (metaData.Columns.Contains(map["PK_TABLE_SCHEMA"])) f_PKTableSchema = metaData.Columns[map["PK_TABLE_SCHEMA"]];
                if (metaData.Columns.Contains(map["PK_TABLE_NAME"])) f_PKTableName = metaData.Columns[map["PK_TABLE_NAME"]];
                if (metaData.Columns.Contains(map["FK_TABLE_CATALOG"])) f_FKTableCatalog = metaData.Columns[map["FK_TABLE_CATALOG"]];
                if (metaData.Columns.Contains(map["FK_TABLE_SCHEMA"])) f_FKTableSchema = metaData.Columns[map["FK_TABLE_SCHEMA"]];
                if (metaData.Columns.Contains(map["FK_TABLE_NAME"])) f_FKTableName = metaData.Columns[map["FK_TABLE_NAME"]];
                if (metaData.Columns.Contains(map["ORDINAL"])) f_Ordinal = metaData.Columns[map["ORDINAL"]];
                if (metaData.Columns.Contains(map["UPDATE_RULE"])) f_UpdateRule = metaData.Columns[map["UPDATE_RULE"]];
                if (metaData.Columns.Contains(map["DELETE_RULE"])) f_DeleteRule = metaData.Columns[map["DELETE_RULE"]];
                if (metaData.Columns.Contains(map["PK_NAME"])) f_PKName = metaData.Columns[map["PK_NAME"]];
                if (metaData.Columns.Contains(map["FK_NAME"])) f_FKName = metaData.Columns[map["FK_NAME"]];
                if (metaData.Columns.Contains(map["DEFERRABILITY"])) f_Deferrability = metaData.Columns[map["DEFERRABILITY"]];

                _fieldsBound = true;
            }
        }

		virtual internal void LoadAll()
		{

		}

		virtual internal void LoadAllIndirect()
		{

		}

        internal void PopulateArray(DataTable metaData)
        {
            PopulateArray(metaData, null);
        }

        internal void PopulateArray(DataTable metaData, NameValueCollection map)
        {
            if (map != null)
                BindToColumnsCustom(metaData, map);
            else
                BindToColumns(metaData);

			ForeignKey key  = null;
			string keyName = "";

			foreach(DataRowView rowView in metaData.DefaultView)
			{
				try
				{
					DataRow row = rowView.Row;

                    keyName = row[f_FKName] as string;

					key = this.GetByName(keyName);

					if(null == key)
					{
						key = (ForeignKey)this.dbRoot.ClassFactory.CreateForeignKey();
						key.dbRoot = this.dbRoot;
						key.ForeignKeys = this;
						key.Row = row;
						this._array.Add(key);
					}

                    string catalog = (DBNull.Value == row[f_PKTableCatalog]) ? string.Empty : (row[f_PKTableCatalog] as string);
                    string schema = (DBNull.Value == row[f_PKTableSchema]) ? string.Empty : (row[f_PKTableSchema] as string);
                    key.AddForeignColumn(catalog, schema, (string)row[f_PKTableName], (string)row["PK_COLUMN_NAME"], true);

                    catalog = (DBNull.Value == row[f_FKTableCatalog]) ? string.Empty : (row[f_FKTableCatalog] as string);
                    schema = (DBNull.Value == row[f_FKTableSchema]) ? string.Empty : (row[f_FKTableSchema] as string);
                    key.AddForeignColumn(catalog, schema, (string)row[f_FKTableName], (string)row["FK_COLUMN_NAME"], false);
				}
				catch {}
			}
		}

        internal void PopulateArrayNoHookup(DataTable metaData, NameValueCollection map)
        {
            if (map != null)
                BindToColumnsCustom(metaData, map);
            else
                BindToColumns(metaData);

            ForeignKey key = null;
            string keyName = "";

            foreach (DataRowView rowView in metaData.DefaultView)
            {
                DataRow row = rowView.Row;

                keyName = row[f_FKName] as string;

                key = this.GetByName(keyName);

                if (null == key)
                {
                    key = (ForeignKey)this.dbRoot.ClassFactory.CreateForeignKey();
                    key.dbRoot = this.dbRoot;
                    key.ForeignKeys = this;
                    key.Row = row;
                    this._array.Add(key);
                }
            }
        }

		internal void PopulateArrayNoHookup(DataTable metaData)
		{
            PopulateArrayNoHookup(metaData, null);
		}

		internal void AddForeignKey(ForeignKey fk)
		{
			IForeignKey exists = this[fk.Name];

			if(null == exists)
			{
				this._array.Add(fk);
			}
		}

		#region indexers

		virtual public IForeignKey this[object index]
		{
			get
			{
				if(index.GetType() == Type.GetType("System.String"))
				{
					return GetByPhysicalName(index as String);
				}
				else
				{
					int idx = Convert.ToInt32(index);
					return this._array[idx] as ForeignKey;
				}
			}
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif
		public ForeignKey GetByName(string name)
		{
			ForeignKey obj = null;
			ForeignKey tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as ForeignKey;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif		
		internal ForeignKey GetByPhysicalName(string name)
		{
			ForeignKey obj = null;
			ForeignKey tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as ForeignKey;

				if(this.CompareStrings(name,tmp.Name))
				{
					obj = tmp;
					break;
				}
			}

			return obj;
		}

		#endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<IForeignKey> Members

        public IEnumerator<IForeignKey> GetEnumerator() {
            foreach (object item in _array)
                yield return item as IForeignKey;
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
				return Table.UserDataXPath + @"/ForeignKeys";
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
				if(this.Table.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./ForeignKeys";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "ForeignKeys", null);
			parentNode.AppendChild(myNode);
		}

		#endregion

		#region IList Members

		object System.Collections.IList.this[int index]
		{
			get	{ return this[index];}
			set	{ }
		}

		#endregion

		internal Table Table = null;

        
    }
}
