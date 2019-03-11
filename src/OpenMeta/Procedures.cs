using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace MyMeta
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(false), ClassInterface(ClassInterfaceType.AutoDual)]
#endif 
	public class Procedures : Collection, IProcedures, IEnumerable, ICollection
	{
		public Procedures()
		{

		}

		#region XML User Data

#if ENTERPRISE
		[ComVisible(false)]
#endif
		override public string UserDataXPath
		{ 
			get
			{
				return Database.UserDataXPath + @"/Procedures";
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
				if(this.Database.GetXmlNode(out parentNode, forceCreate))
				{
					// See if our user data already exists
					string xPath = @"./Procedures";
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
			XmlNode myNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, "Procedures", null);
			parentNode.AppendChild(myNode);
		}

		#endregion

		internal DataColumn f_Catalog				= null;
		internal DataColumn f_Schema				= null;
		internal DataColumn f_Name					= null;
		internal DataColumn f_Type					= null;
		internal DataColumn f_ProcedureDefinition	= null;
		internal DataColumn f_Description			= null;
		internal DataColumn f_DateCreated			= null;
		internal DataColumn f_DateModified			= null;


		private void BindToColumns(DataTable metaData)
		{
			if(false == _fieldsBound)
			{
				if(metaData.Columns.Contains("PROCEDURE_CATALOG"))		f_Catalog				= metaData.Columns["PROCEDURE_CATALOG"];
				if(metaData.Columns.Contains("PROCEDURE_SCHEMA"))		f_Schema				= metaData.Columns["PROCEDURE_SCHEMA"];
				if(metaData.Columns.Contains("PROCEDURE_NAME"))			f_Name					= metaData.Columns["PROCEDURE_NAME"];
				if(metaData.Columns.Contains("PROCEDURE_TYPE"))			f_Type					= metaData.Columns["PROCEDURE_TYPE"];
				if(metaData.Columns.Contains("PROCEDURE_DEFINITION"))	f_ProcedureDefinition	= metaData.Columns["PROCEDURE_DEFINITION"];
				if(metaData.Columns.Contains("DESCRIPTION"))			f_Description			= metaData.Columns["DESCRIPTION"];
				if(metaData.Columns.Contains("DATE_CREATED"))			f_DateCreated			= metaData.Columns["DATE_CREATED"];
				if(metaData.Columns.Contains("DATE_MODIFIED"))			f_DateModified			= metaData.Columns["DATE_MODIFIED"];

			}
		}

		internal virtual void LoadAll()
		{

		}

		internal void PopulateArray(DataTable metaData)
		{
			BindToColumns(metaData);

			Procedure procedure = null;

			int count = metaData.Rows.Count;
			for(int i = 0; i < count; i++)
			{
				procedure = (Procedure)this.dbRoot.ClassFactory.CreateProcedure();
				procedure.dbRoot = this.dbRoot;
				procedure.Procedures = this;
				procedure.Row = metaData.Rows[i];
				this._array.Add(procedure);
			}
		}

		internal void AddProcedure(Procedure procedure)
		{
			this._array.Add(procedure);
		}

		#region indexers

		virtual public IProcedure this[object index]
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
					return this._array[idx] as Procedure;
				}
			}
		}

#if ENTERPRISE
		[ComVisible(false)]
#endif
		public Procedure GetByName(string name)
		{
			Procedure obj = null;
			Procedure tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Procedure;

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
		public Procedure GetByPhysicalName(string name)
		{
			Procedure obj = null;
			Procedure tmp = null;

			int count = this._array.Count;
			for(int i = 0; i < count; i++)
			{
				tmp = this._array[i] as Procedure;

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

        #region IEnumerable<IProcedure> Members

        public IEnumerator<IProcedure> GetEnumerator() {
            foreach (object item in _array)
                yield return item as IProcedure;
        }

        #endregion

		#region IList Members

		object System.Collections.IList.this[int index]
		{
			get	{ return this[index];}
			set	{ }
		}

		#endregion

		internal Database Database = null;


    }
}
