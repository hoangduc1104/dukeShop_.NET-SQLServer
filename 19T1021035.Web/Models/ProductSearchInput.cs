using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
        public int SupplierID { get; set; } = 0;
        public int CategoryID { get; set; } = 0;

    }
}