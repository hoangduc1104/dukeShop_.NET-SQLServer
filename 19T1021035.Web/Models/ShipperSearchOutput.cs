using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class ShipperSearchOutput : PaginationSearchOutput 
    {
        /// <summary>
        /// Danh sách người giao hàng
        /// </summary>
        public List<Shipper> Data { get; set; }
    }
}