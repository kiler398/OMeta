using System;
using System.Data;


namespace OMeta.Firebird
{
 
	public class FirebirdView : View
	{
		public FirebirdView()
		{

		}

		override public IViews SubViews 
		{ 
			get
			{
				if(!_subViewInfoLoaded)
				{
					LoadSubViewInfo();
				}
				return _views;				
			}
		}

		override public ITables SubTables
		{ 
			get
			{
				if(!_subViewInfoLoaded)
				{
					LoadSubViewInfo();
				}
				return _tables;
			}
		}

		private void LoadSubViewInfo()
		{
			_views  = (Views)this.dbRoot.ClassFactory.CreateViews();
			_tables = (Tables)this.dbRoot.ClassFactory.CreateTables();
        }
	}
}
