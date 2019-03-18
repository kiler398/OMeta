using System;
using System.Runtime.InteropServices;

namespace OMeta
{
    /// <summary>
    /// ITable represents a table in your DBMS.
    /// </summary>
    /// <remarks>
    ///	ITable Collections:
    /// <list type="table">
    ///		<item><term>Columns</term><description>Contains all of the columns for the table</description></item>
    ///		<item><term>ForeignKeys</term><description>Contains all of the foreign keys this table is involved in</description></item>
    ///		<item><term>Indexes</term><description>Contains all of the indexes for the table</description></item>
    ///		<item><term>PrimaryKeys</term><description>Contains all of the columns that are also primary keys, this is an IColumns collection</description></item>
    ///		<item><term>Properties</term><description>A collection that can hold key/value pairs of your choosing</description></item>
    ///		<item><term>GlobalProperties</term><description>A collection that can hold key/value pairs of your choosing for all Tables with the same Database</description></item>
    ///		<item><term>AllProperties</term><description>A collection that combines the Properties and GlobalProperties Collections</description></item>
    ///	</list>
    /// </remarks>
    /// <example>
    /// VBScript
    /// <code>
    /// Dim objTable
    /// For Each objTable in objDatabase.Tables
    ///     output.writeLn objTable.Name
    ///	    output.writeLn objTable.Alias
    /// Next
    /// </code>
    /// JScript
    /// <code>
    /// var objTable;
    /// for (var j = 0; j &lt; objDatabase.Tables.Count; j++) 
    /// {
    ///	    objColumn = objDatabase.Tables.Item(j);
    ///	    
    ///	    output.writeln(objTable.Name);
    ///	    output.writeln(objTable.Alias);
    /// }
    /// </code>
    /// </example>
    [Guid("208AC4C1-CB91-4818-BB73-E57D43D64978"),InterfaceType(ComInterfaceType.InterfaceIsDual)] 
    public interface ITable: IEquatable<ITable>
	{
		// Collections
		/// <summary>
		/// The Columns collection for this table in ordinal order. See <see cref="IColumn"/>
		/// </summary>
		IColumns Columns { get; }

		/// <summary>
		/// The ForeignKeys for this table. See <see cref="ForeignKeys"/>
		/// </summary>
		IForeignKeys ForeignKeys { get; }

		/// <summary>
		/// The Indexes for this table. See <see cref="IIndex"/>
		/// </summary>
		IIndexes Indexes { get; }

		/// <summary>
		/// The PrimaryKeys for this table. This is really just a Column collection containing only the Columns which are primary keys. See <see cref="IColumn"/>
		/// </summary>
		IColumns PrimaryKeys { get; }

		/// <summary>
		/// The Properties for this table. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window. 
		/// To save any data added to this collection call MyMeta.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection Properties { get; }

		/// <summary>
		/// The Properties for all Tables within the same Database. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
		/// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window. 
		/// To save any data added to this collection call MyMeta.SaveUserMetaData(). See <see cref="IProperty"/>
		/// </summary>
		IPropertyCollection GlobalProperties { get; }

		/// <summary>
		/// AllProperties is essentially a read-only collection consisting of a combination of both the <see cref="Properties"/> (local) collection and the <see cref="GlobalProperties"/> (global) collection. The local properties are added first, 
		/// and then the global properties are added however, only global properties for which no local property exists -- are added. This makes local properties overlay global properties. Global properties can
		/// act as a default value which can be overridden by a local property. See <see cref="IProperty"/>.
		/// </summary>
		IPropertyCollection AllProperties { get; }

		// Objects

		/// <summary>
		/// Parent Database of this Table
		/// </summary>
		IDatabase Database { get; }

		// User Meta Data
		string UserDataXPath { get; }

		/// <summary>
		/// You can override the physical name of the Table. If you do not provide an Alias the value of 'Table.Name' is returned.
		/// If your table in your DBMS is 'Q99AAB' you might want to give it an Alias of 'Employees' so that your business object names will make sense.
		/// You can provide an Alias the User Meta Data window. You can also set this during a script and then call MyMeta.SaveUserMetaData().
		/// See <see cref="Name"/>
		/// </summary>
		[DispId(0)]
		string Alias { get; set; }
	
		/// <summary>
		/// This is the physical table name as stored in your DBMS system. See <see cref="Alias"/>
		/// </summary>
		string Name { get; }	

		/// <summary>
		/// This is the schema of the Table.
		/// </summary>
		string Schema { get; }

		// Properties
		/// <summary>
		/// The table type, 'TABLE' if not provided.
		/// </summary>
		/// <remarks>
		///
		/// <list type="bullet">
		///		<item>ALIAS</item>
		///		<item>TABLE</item>
		///		<item>SYNONYM</item>
		///		<item>SYSTEM TABLE</item>
		///		<item>GLOBAL TEMPORARY</item>
		///		<item>LOCAL TEMPORARY</item>		
		/// </list>
		///
		///</remarks>
		string Type { get; }

		/// <summary>
		/// Tab;e GUID. For Providers that do not use GUIDs to identify tables 'Guid.Empty' is returned.
		/// </summary>
		Guid Guid { get; }

		/// <summary>
		/// Human-readable description of the table. Blank if there is no description associated with the table.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Column property ID. For Providers that do not associate PROPIDs with columns 0 is returned.
		/// </summary>
		System.Int32 PropID { get; }

		/// <summary>
		/// Date when the table was created or '1/1/0001' if the provider does not have this information. 
		/// </summary>
		DateTime DateCreated { get; }

		/// <summary>
		/// Date when the table definition was last modified or '1/1/0001' if the provider does not have this information. 
		/// </summary>
        DateTime DateModified { get; }

        /// <summary>
        /// Fetch any database specific meta data through this generic interface by key. The keys will have to be defined by the specific database provider
        /// </summary>
        /// <param name="key">A key identifying the type of meta data desired.</param>
        /// <returns>A meta-data object or collection.</returns>
        object DatabaseSpecificMetaData(string key);
	}
}

