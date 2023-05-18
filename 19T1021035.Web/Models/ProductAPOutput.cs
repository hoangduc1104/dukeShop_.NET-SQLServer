using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.DomainModels;

namespace _19T1021035.Web.Models
{
    public class ProductAPOutput
    {
        public Product product { get; set; }

        public List<ProductAttribute> listAttribute { get; set; }
        public List<ProductPhoto> listPhoto { get; set; }
        
    }
}