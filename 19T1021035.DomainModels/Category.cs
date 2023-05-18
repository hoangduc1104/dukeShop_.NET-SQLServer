using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19T1021035.DomainModels
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public string ParentCategoryId { get; set; }

    }
}
