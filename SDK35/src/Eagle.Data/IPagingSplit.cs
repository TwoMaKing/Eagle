using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core.Query;

namespace Eagle.Data
{
    public interface IPagingSplit : IQueryPaging
    {
        DataSet GetPagingData(int pageNumber);

        IDataReader GetPagingDataReadOnly(int pageNumber);

        Database Database { get; }

        string Where { get; }

        string OrderBy { get; }

        object[] ParamValues { get; }
    }
}
