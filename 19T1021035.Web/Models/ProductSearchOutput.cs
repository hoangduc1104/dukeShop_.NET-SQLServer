using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class ProductSearchOutput : PaginationSearchOutput
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }

        public List<Product> Data { get; set; }
    }
}