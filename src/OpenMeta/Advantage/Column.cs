using System;
using System.Data;
using System.Data.OleDb;

namespace MyMeta.Advantage
{
#if ENTERPRISE
	using System.Runtime.InteropServices;
	[ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), ComDefaultInterface(typeof(IColumn))]
#endif 
	public class AdvantageColumn : Column
	{
		public AdvantageColumn()
		{

		}

		override internal Column Clone()
		{
			Column c = base.Clone();

			return c;
		}

		override public System.Boolean IsAutoKey
		{
			get
			{
				return (this.DataTypeName == "AutoInc") ? true : false;
			}
		}

		override public System.Boolean IsComputed
		{
			get
			{
				if(this.DataTypeName == "timestamp") return true;

				return this.GetBool(Columns.f_IsComputed);
			}
		}


		override public string DataTypeName
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeName;
						}
					}
				}

				AdvantageColumns cols = Columns as AdvantageColumns;
				return this.GetString(cols.f_TypeName);
			}
		}

		override public string DataTypeNameComplete
		{
			get
			{
				if(this.dbRoot.DomainOverride)
				{
					if(this.HasDomain)
					{
						if(this.Domain != null)
						{
							return this.Domain.DataTypeNameComplete;
						}
					}
				}

				return this.DataTypeName;
			}
		}
	}
}
