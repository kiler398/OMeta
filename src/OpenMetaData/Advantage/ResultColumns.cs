using System;
using System.Data;
using System.Data.OleDb;
using System.Text;

namespace OMeta.Advantage
{
 
	public class AdvantageResultColumns : ResultColumns
	{
		public AdvantageResultColumns()
		{

		}

		override internal void LoadAll()
		{
            try
            {
                string schema = "";

                if (-1 == this.Procedure.Schema.IndexOf("."))
                {
                    schema = this.Procedure.Schema + ".";
                }

                StringBuilder select =
                    new StringBuilder(
                        $"SET FMTONLY ON EXEC [{this.Procedure.Database.Name}].{schema}{this.Procedure.Name} ");

                int paramCount = this.Procedure.Parameters.Count;

                if (paramCount > 0)
                {
                    IParameters parameters = this.Procedure.Parameters;
                    IParameter param = null;

                    int c = parameters.Count;

                    for (int i = 0; i < c; i++)
                    {
                        param = parameters[i];

                        if (param.Direction == ParamDirection.ReturnValue)
                        {
                            paramCount--;
                        }
                    }
                }

                for (int i = 0; i < paramCount; i++)
                {
                    if (i > 0)
                    {
                        select.Append(",");
                    }

                    select.Append("null");
                }

                OleDbDataAdapter adapter = new OleDbDataAdapter(select.ToString(), this.dbRoot.ConnectionString);
                DataTable metaData = new DataTable();

                adapter.Fill(metaData);

                AdvantageResultColumn resultColumn = null;

                int count = metaData.Columns.Count;
                for (int i = 0; i < count; i++)
                {
                    resultColumn = this.dbRoot.ClassFactory.CreateResultColumn() as Advantage.AdvantageResultColumn;
                    resultColumn.dbRoot = this.dbRoot;
                    resultColumn.ResultColumns = this;
                    resultColumn._column = metaData.Columns[i];
                    this._array.Add(resultColumn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
	}
}
