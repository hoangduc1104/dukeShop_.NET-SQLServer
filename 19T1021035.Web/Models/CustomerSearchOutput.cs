using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class CustomerSearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách khách hàng
        /// </summary>
        public List<Customer> Data { get; set; }
    }
}