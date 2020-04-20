using System;
using System.Data;
using Npgsql;

namespace OMeta.PostgreSQL
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumns))]
#endif 
	public class PostgreSQLColumns : Columns
	{
		static string _query = 
			"SELECT CAST(current_database() AS character varying) AS table_catalog, " +
			"CAST(nc.nspname AS character varying) AS table_schema, " +
			"CAST(c.relname AS character varying) AS table_name, " +
			"CAST(a.attname AS character varying) AS column_name, " +
			"CAST(a.attnum AS integer) AS ordinal_position, " +
//			"CAST(CASE WHEN u.usename = current_user THEN a.adsrc ELSE null END " +
			"CAST(a.adsrc AS character varying) " +
			"AS column_default, " +

			"CAST(CASE WHEN a.attnotnull OR (t.typtype = 'd' AND t.typnotnull) THEN 'NO' ELSE 'YES' END " +
			"AS character varying) " +
			"AS is_nullable, " +

			"CAST( " +
			"CASE WHEN t.typtype = 'd' THEN " +
			"CASE WHEN bt.typelem <> 0 AND bt.typlen = -1 THEN 'ARRAY' " +
			"WHEN nbt.nspname = 'pg_catalog' THEN format_type(t.typbasetype, null) " +
			"ELSE 'USER-DEFINED' END " +
			"ELSE " +
			"CASE WHEN t.typelem <> 0 AND t.typlen = -1 THEN 'ARRAY' " +
			"WHEN nt.nspname = 'pg_catalog' THEN format_type(a.atttypid, null) " +
			"ELSE 'USER-DEFINED' END " +
			"END " +
			"AS character varying) " +
			"AS TYPE_NAMECOMPLETE, " +

			"CAST( " +
			"CASE WHEN t.typtype = 'd' THEN " +
			"CASE WHEN t.typbasetype IN (1042, 1043) AND t.typtypmod <> -1 " +
			"THEN t.typtypmod - 4  " +
			"WHEN t.typbasetype IN (1560, 1562) AND t.typtypmod <> -1 " +
			"THEN t.typtypmod  " +
			"ELSE null END " +
			"ELSE " +
			"CASE WHEN a.atttypid IN (1042, 1043) AND a.atttypmod <> -1 " +
			"THEN a.atttypmod - 4 " +
			"WHEN a.atttypid IN (1560, 1562) AND a.atttypmod <> -1 " +
			"THEN a.atttypmod " +
			"ELSE null END " +
			"END " +
			"AS integer) " +
			"AS character_maximum_length, " +

			"CAST( " +
			"CASE WHEN t.typtype = 'd' THEN " +
			"CASE WHEN t.typbasetype IN (25, 1042, 1043) THEN 2^30 ELSE null END " +
			"ELSE " +
			"CASE WHEN a.atttypid IN (25, 1042, 1043) THEN 2^30 ELSE null END " +
			"END " +
			"AS integer) " +
			"AS character_octet_length, " +

			"CAST( " +
			"CASE (CASE WHEN t.typtype = 'd' THEN t.typbasetype ELSE a.atttypid END) " +
			"WHEN 21 THEN 16 " +
			"WHEN 23 THEN 32 " +
			"WHEN 20 THEN 64 " +
			"WHEN 1700 THEN ((CASE WHEN t.typtype = 'd' THEN t.typtypmod ELSE a.atttypmod END - 4) >> 16) & 65535 " +
			"WHEN 700 THEN 24  " +
			"WHEN 701 THEN 53 " +
			"ELSE null END " +
			"AS integer) " +
			"AS numeric_precision, " +

//			"CAST( " +
//			"CASE WHEN t.typtype = 'd' THEN " +
//			"CASE WHEN t.typbasetype IN (21, 23, 20, 700, 701) THEN 2 " +
//			"WHEN t.typbasetype IN (1700) THEN 10 " +
//			"ELSE null END " +
//			"ELSE " +
//			"CASE WHEN a.atttypid IN (21, 23, 20, 700, 701) THEN 2 " +
//			"WHEN a.atttypid IN (1700) THEN 10 " +
//			"ELSE null END " +
//			"END " +
//			"AS integer) " +
//			"AS numeric_precision_radix, " +

			"CAST( " +
			"CASE WHEN t.typtype = 'd' THEN " +
			"CASE WHEN t.typbasetype IN (21, 23, 20) THEN 0 " +
			"WHEN t.typbasetype IN (1700) THEN (t.typtypmod - 4) & 65535 " +
			"ELSE null END " +
			"ELSE " +
			"CASE WHEN a.atttypid IN (21, 23, 20) THEN 0 " +
			"WHEN a.atttypid IN (1700) THEN (a.atttypmod - 4) & 65535 " +
			"ELSE null END " +
			"END " +
			"AS integer) " +
			"AS numeric_scale, " +

			"CAST( " +
			"CASE WHEN t.typtype = 'd' THEN " +
			"CASE WHEN t.typbasetype IN (1083, 1114, 1184, 1266) " +
			"THEN (CASE WHEN t.typtypmod <> -1 THEN t.typtypmod ELSE null END) " +
			"WHEN t.typbasetype IN (1186) " +
			"THEN (CASE WHEN t.typtypmod <> -1 THEN t.typtypmod & 65535 ELSE null END) " +
			"ELSE null END " +
			"ELSE " +
			"CASE WHEN a.atttypid IN (1083, 1114, 1184, 1266) " +
			"THEN (CASE WHEN a.atttypmod <> -1 THEN a.atttypmod ELSE null END) " +
			"WHEN a.atttypid IN (1186) " +
			"THEN (CASE WHEN a.atttypmod <> -1 THEN a.atttypmod & 65535 ELSE null END) " +
			"ELSE null END " +
			"END " +
			"AS integer) " +
			"AS datetime_precision, " +

//			"CAST(null AS character varying) AS interval_type,  " +
//			"CAST(null AS character varying) AS interval_precision,  " +
 
			"CAST(null AS character varying) AS character_set_catalog, " +
			"CAST(null AS character varying) AS character_set_schema, " +
			"CAST(null AS character varying) AS character_set_name, " +

//			"CAST(null AS character varying) AS collation_catalog, " +
//			"CAST(null AS character varying) AS collation_schema, " +
//			"CAST(null AS character varying) AS collation_name, " +

//			"CAST(CASE WHEN t.typtype = 'd' THEN current_database() ELSE null END " +
//			"AS character varying) AS domain_catalog, " +
//			"CAST(CASE WHEN t.typtype = 'd' THEN nt.nspname ELSE null END " +
//			"AS character varying) AS domain_schema, " +
//			"CAST(CASE WHEN t.typtype = 'd' THEN t.typname ELSE null END " +
//			"AS character varying) AS domain_name, " +

//			"CAST(current_database() AS character varying) AS udt_catalog, " +
//			"CAST(coalesce(nbt.nspname, nt.nspname) AS character varying) AS udt_schema, " +
			"CAST(coalesce(bt.typname, t.typname) AS character varying) AS TYPE_NAME " +
//			"description AS DESCRIPTION " +

//			"CAST(null AS character varying) AS scope_catalog, " +
//			"CAST(null AS character varying) AS scope_schema, " +
//			"CAST(null AS character varying) AS scope_name, " +

//			"CAST(null AS integer) AS maximum_cardinality, " +
//			"CAST(a.attnum AS character varying) AS dtd_identifier, " +
//			"CAST('NO' AS character varying) AS is_self_referencing " +

			"FROM (pg_attribute LEFT JOIN pg_attrdef ON attrelid = adrelid AND attnum = adnum) AS a, " +
//			"LEFT JOIN pg_description ON objsubid = a.attnum, " +
			"pg_class c, pg_namespace nc, pg_user u, " +
			"(pg_type t JOIN pg_namespace nt ON (t.typnamespace = nt.oid)) " +
			"LEFT JOIN (pg_type bt JOIN pg_namespace nbt ON (bt.typnamespace = nbt.oid)) ON (t.typtype = 'd' AND t.typbasetype = bt.oid) " +
			

			"WHERE a.attrelid = c.oid " +
			"AND a.atttypid = t.oid " +
			"AND u.usesysid = c.relowner " +
			"AND nc.oid = c.relnamespace " +

			"AND a.attnum > 0 AND NOT a.attisdropped AND c.relkind in ('r', 'v') " +

			"AND (u.usename = current_user " +
			"OR has_table_privilege(c.oid, 'SELECT') " +
			"OR has_table_privilege(c.oid, 'INSERT') " +
			"OR has_table_privilege(c.oid, 'UPDATE') " +
			"OR has_table_privilege(c.oid, 'REFERENCES')) ";

		public PostgreSQLColumns()
		{

		}

		internal DataColumn f_TypeName			= null;
		internal DataColumn f_AutoKey			= null;
		internal DataColumn f_TypeNameComplete	= null;

		override internal void LoadForTable()
		{
			try
			{

				//				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Table.Database.Name);
				//
				//				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select * from public.\"MasterTypes\"", cn);
				//
				//				DataTable metaData = new DataTable();
				//
				//				adapter.Fill(metaData);
				//
				//				NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(adapter);
				//
				//				NpgsqlCommand cmd = builder.GetInsertCommand(metaData.Rows[0]);
				//
				//				cn.Close();
				//
				//				
				//
				//				foreach(NpgsqlParameter p in cmd.Parameters)
				//				{
				//					Console.WriteLine("<Type From=\"" + p.SourceColumn + "\" To=\"" + p.DbType.ToString() + "\" />");
				//				}
				//
				//
				//				int i = 9;

				// real code below here

				string query = _query +
					"AND nc.nspname = '" + this.Table.Schema + "' AND c.relname = '" + this.Table.Name + "' " + "ORDER BY attnum";

				/*=================================
				 * 
				string query = 
					"SELECT CAST(current_database() AS character varying) AS table_catalog, CAST(nc.nspname AS character varying) AS table_schema, CAST(c.relname AS character varying) AS table_name, CAST(a.attname AS character varying) AS column_name, CAST(a.attnum AS integer) AS ordinal_position, CAST(CASE WHEN u.usename = current_user THEN a.adsrc ELSE null END AS character varying) AS column_default, CAST(CASE WHEN a.attnotnull OR (t.typtype = 'd' AND t.typnotnull) THEN 'NO' ELSE 'YES' END AS character varying) AS is_nullable, CAST(CASE WHEN t.typtype = 'd' THEN CASE WHEN bt.typelem <> 0 AND bt.typlen = -1 THEN 'ARRAY'WHEN nbt.nspname = 'pg_catalog' THEN format_type(t.typbasetype, null) ELSE 'USER-DEFINED' END ELSE CASE WHEN t.typelem <> 0 AND t.typlen = -1 THEN 'ARRAY' WHEN nt.nspname = 'pg_catalog' THEN format_type(a.atttypid, null) ELSE 'USER-DEFINED' END END AS character varying) AS TYPE_NAMECOMPLETE, CAST( CASE WHEN t.typtype = 'd' THEN CASE WHEN t.typbasetype IN (1042, 1043) AND t.typtypmod <> -1 THEN t.typtypmod - 4 WHEN t.typbasetype IN (1560, 1562) AND t.typtypmod <> -1 THEN t.typtypmod ELSE null END ELSE CASE WHEN a.atttypid IN (1042, 1043) AND a.atttypmod <> -1 THEN a.atttypmod - 4 WHEN a.atttypid IN (1560, 1562) AND a.atttypmod <> -1 THEN a.atttypmod ELSE null END END AS integer) AS character_maximum_length, CAST( CASE WHEN t.typtype = 'd' THEN CASE WHEN t.typbasetype IN (25, 1042, 1043) THEN 2^30 ELSE null END ELSE CASE WHEN a.atttypid IN (25, 1042, 1043) THEN 2^30 ELSE null END END AS integer) AS character_octet_length, CAST( CASE (CASE WHEN t.typtype = 'd' THEN t.typbasetype ELSE a.atttypid END) WHEN 21 THEN 16 WHEN 23 THEN 32 WHEN 20 THEN 64 WHEN 1700 THEN " +
					"((CASE WHEN t.typtype = 'd' THEN t.typtypmod ELSE a.atttypmod END - 4) >> 16) & 65535 WHEN 700 THEN 24 WHEN 701 THEN 53 ELSE null END AS integer) AS numeric_precision, CAST( CASE WHEN t.typtype = 'd' THEN CASE WHEN t.typbasetype IN (21, 23, 20, 700, 701) THEN 2 WHEN t.typbasetype IN (1700) THEN 10 ELSE null END ELSE CASE WHEN a.atttypid IN (21, 23, 20, 700, 701) THEN 2 WHEN a.atttypid IN (1700) THEN 10 ELSE null END END AS integer) AS numeric_precision_radix, CAST( CASE WHEN t.typtype = 'd' THEN CASE WHEN t.typbasetype IN (21, 23, 20) THEN 0 WHEN t.typbasetype IN (1700) THEN (t.typtypmod - 4) & 65535 ELSE null END ELSE CASE WHEN a.atttypid IN (21, 23, 20) THEN 0 WHEN a.atttypid IN (1700) THEN (a.atttypmod - 4) & 65535 ELSE null END END AS integer) AS numeric_scale, CAST( CASE WHEN t.typtype = 'd' THEN CASE WHEN t.typbasetype IN (1083, 1114, 1184, 1266) THEN (CASE WHEN t.typtypmod <> -1 THEN t.typtypmod ELSE null END) WHEN t.typbasetype IN (1186) THEN (CASE WHEN t.typtypmod <> -1 THEN t.typtypmod & 65535 ELSE null END) ELSE null END ELSE CASE WHEN a.atttypid IN (1083, 1114, 1184, 1266) THEN (CASE WHEN a.atttypmod <> -1 THEN a.atttypmod ELSE null END) WHEN a.atttypid IN (1186) THEN (CASE WHEN a.atttypmod <> -1 THEN a.atttypmod & 65535 ELSE null END) ELSE null END END AS integer) AS datetime_precision, CAST(null AS character varying) AS interval_type, CAST(null AS character varying) AS interval_precision, CAST(null AS character varying) AS character_set_catalog, CAST(null AS character varying) AS character_set_schema, CAST(null AS character varying) AS character_set_name, CAST(null AS character varying) AS collation_catalog, CAST(null AS character varying) " +
					"AS collation_schema, CAST(null AS character varying) AS collation_name, CAST(CASE WHEN t.typtype = 'd' THEN current_database() ELSE null END AS character varying) AS domain_catalog, CAST(CASE WHEN t.typtype = 'd' THEN nt.nspname ELSE null END AS character varying) AS domain_schema, CAST(CASE WHEN t.typtype = 'd' THEN t.typname ELSE null END AS character varying) AS domain_name, CAST(current_database() AS character varying) AS udt_catalog, CAST(coalesce(nbt.nspname, nt.nspname) AS character varying) AS udt_schema, CAST(coalesce(bt.typname, t.typname) AS character varying) AS TYPE_NAME, CAST(null AS character varying) AS scope_catalog, CAST(null AS character varying) AS scope_schema, CAST(null AS character varying) AS scope_name, CAST(null AS integer) AS maximum_cardinality, CAST(a.attnum AS character varying) AS dtd_identifier, CAST('NO' AS character varying) AS is_self_referencing FROM (pg_attribute LEFT JOIN pg_attrdef ON attrelid = adrelid AND attnum = adnum) AS a, pg_class c, pg_namespace nc, pg_user u, (pg_type t JOIN pg_namespace nt ON (t.typnamespace = nt.oid)) LEFT JOIN (pg_type bt JOIN pg_namespace nbt ON (bt.typnamespace = nbt.oid)) ON (t.typtype = 'd' AND t.typbasetype = bt.oid) WHERE a.attrelid = c.oid AND a.atttypid = t.oid AND u.usesysid = c.relowner AND nc.oid = c.relnamespace AND a.attnum > 0 AND NOT a.attisdropped AND c.relkind in ('r', 'v') AND " +
					"nc.nspname = '" + this.Table.Schema + "' AND c.relname = '" + this.Table.Name + "' AND	(u.usename = current_user OR has_table_privilege(c.oid, 'SELECT') OR has_table_privilege(c.oid, 'INSERT') OR has_table_privilege(c.oid, 'UPDATE') OR has_table_privilege(c.oid, 'REFERENCES') ) ORDER BY a.attnum;";

				/*===============================*/

				//				string query =
				//					"SELECT  a.attname as COLUMN_NAME, a.attnum as ORDINAL_POSITION, format_type(a.atttypid, a.atttypmod) as TYPE_NAMECOMPLETE, " + 
				//					"attnotnull as IS_NULLABLE, atthasdef as COLUMN_HASDEFAULT, atttypmod as CHARACTER_MAXIMUM_LENGTH, " +
				//					"d.adsrc as COLUMN_DEFAULT,  typname as TYPE_NAME, description AS DESCRIPTION, " +
				//					"FROM pg_catalog.pg_class c INNER JOIN " +
				//					"pg_catalog.pg_namespace n ON (c.relnamespace = n.oid) INNER JOIN " +
				//					"pg_catalog.pg_attribute a ON (a.attrelid = c.oid) LEFT JOIN " +
				//					"pg_catalog.pg_attrdef d ON (d.adrelid = c.oid and d.adnum = a.attnum) INNER JOIN " +
				//					"pg_catalog.pg_type t ON (a.atttypid = t.oid) LEFT OUTER JOIN " +
				//					"pg_description des ON des.objsubid = a.attnum " +
				//					"WHERE n.nspname = '" + this.Table.Schema + "' AND c.relname = '" + this.Table.Name + "' AND a.attisdropped = false AND a.attnum > 0 " +
				//					"ORDER BY attnum";

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.Table.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
				
				if(metaData.Columns.Contains("TYPE_NAME"))         f_TypeName = metaData.Columns["TYPE_NAME"];
				if(metaData.Columns.Contains("TYPE_NAMECOMPLETE")) f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];

			}
			catch {}
		}

		override internal void LoadForView()
		{
			try
			{
//				string query =
//					"SELECT  a.attname as COLUMN_NAME, a.attnum as ORDINAL_POSITION, format_type(a.atttypid, a.atttypmod) as TYPE_NAMECOMPLETE, " + 
//					"attnotnull as IS_NULLABLE, atthasdef as COLUMN_HASDEFAULT, atttypmod as CHARACTER_MAXIMUM_LENGTH, " +
//					"d.adsrc as COLUMN_DEFAULT,  typname as TYPE_NAME " +
//					"FROM pg_catalog.pg_class c INNER JOIN " +
//					"pg_catalog.pg_namespace n ON (c.relnamespace = n.oid) INNER JOIN " +
//					"pg_catalog.pg_attribute a ON (a.attrelid = c.oid) LEFT JOIN " +
//					"pg_catalog.pg_attrdef d ON (d.adrelid = c.oid and d.adnum = a.attnum) INNER JOIN " +
//					"pg_catalog.pg_type t ON (a.atttypid = t.oid) " +
//					"WHERE n.nspname = '" + this.View.Schema + "' AND c.relname = '" + this.View.Name + "' AND a.attisdropped = false AND a.attnum > 0 " +
//					"ORDER BY attnum";

				string query = _query +
					"AND nc.nspname = '" + this.View.Schema + "' AND c.relname = '" + this.View.Name + "' " + "ORDER BY attnum";

				NpgsqlConnection cn = ConnectionHelper.CreateConnection(this.dbRoot, this.View.Database.Name);

				DataTable metaData = new DataTable();
				NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, cn);

				adapter.Fill(metaData);
				cn.Close();
		
				PopulateArray(metaData);
				
				if(metaData.Columns.Contains("TYPE_NAME"))         f_TypeName = metaData.Columns["TYPE_NAME"];
				if(metaData.Columns.Contains("TYPE_NAMECOMPLETE")) f_TypeNameComplete = metaData.Columns["TYPE_NAMECOMPLETE"];
			}
			catch {}
		}
	}
}
