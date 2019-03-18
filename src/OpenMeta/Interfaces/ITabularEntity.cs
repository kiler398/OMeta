using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;

namespace OMeta
{
    /// <summary>
    /// This interface allows all the collections here to be bound to 
    /// Name/Value collection type objects. with ease
    /// </summary>
    [Guid("20236F44-585E-4CB8-984D-33960E88ECDD"),InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ITabularEntity
    {
        /// <summary>
        /// You can override the physical name of the View. If you do not provide an Alias the value of 'entity.Name' is returned.
        /// If your view in your DBMS is 'Q99AAB' you might want to give it an Alias of 'Employees' so that your business object names will make sense.
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
        /// This is the schema of the entity.
        /// </summary>
        string Schema { get; }

        // Objects
        /// <summary>
        /// The parent database of the entity.
        /// </summary>
        IDatabase Database { get; }

        // Collections
        /// <summary>
        /// The Columns collection for this entity in ordinal order. See <see cref="IColumn"/>
        /// </summary>
        IColumns Columns { get; }

        /// <summary>
        /// The Properties for this view. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
        /// Properties consist of key/value pairs.  You can populate this collection during your script or via the Dockable window.
        /// To save any data added to this collection call MyMeta.SaveUserMetaData(). See <see cref="IProperty"/>
        /// </summary>
        IPropertyCollection Properties { get; }

        /// <summary>
        /// The Properties for all Views within the same Database. These are user defined and are typically stored in 'UserMetaData.xml' unless changed in the Default Settings dialog.
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

        // User Meta Data
        string UserDataXPath { get; }

        /// <summary>
        /// The table type, 'TABLE' if not provided.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        ///		<item>VIEW</item>
        ///		<item>SYSTEM VIEW</item>
        /// </list>
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
        /// Date when the view was created or '1/1/0001' if the provider does not have this information. 
        /// </summary>
        DateTime DateCreated { get; }

        /// <summary>
        /// Date when the view definition was last modified or '1/1/0001' if the provider does not have this information. 
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