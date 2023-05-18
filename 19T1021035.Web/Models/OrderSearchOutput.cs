using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class OrderSearchOutput : PaginationSearchOutput
    {
        public List<Order> Data { get; set; }
    }
}