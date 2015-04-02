﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Query
{
    public class PagingResult<T> : IPagingResult<T>
    {
        private int? totalRecords;
        private int? totalPages;
        private int? pageNumber;
        private int? pageSzie;
        private List<T> data;

        public PagingResult(int? totalRecords, int? totalPages, int? pageNumber, int? pageSzie, List<T> data)
        {
            this.totalPages = totalRecords;
            this.totalPages = totalPages;
            this.pageNumber = pageNumber;
            this.pageSzie = pageSzie;
            this.data = data;
        }

        public int? TotalRecords
        {
            get 
            { 
                return this.totalRecords;
            }
        }

        public int? TotalPages
        {
            get 
            { 
                return this.totalPages; 
            }
        }

        public int? PageNumber
        {
            get 
            { 
                return this.pageNumber; 
            }
        }

        public int? PageSize
        {
            get 
            {
                return this.pageSzie;
            }
        }

        public IEnumerable<T> Data
        {
            get 
            {
                return this.data;
            }
        }
    }
}
