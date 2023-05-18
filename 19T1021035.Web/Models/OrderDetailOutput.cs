using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class OrderDetailOutput
    {
        public Order order { get; set; }

        public List<OrderDetail> listOrderDetail { get; set; }

        public decimal total { get; set; }
    }
}