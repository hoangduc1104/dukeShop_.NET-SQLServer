using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class CategorySearchOutput : PaginationSearchOutput
    {
        /// <summary>
        /// Danh sách loại hàng
        /// </summary>
        public List<Category> Data { get; set; }
    }

}