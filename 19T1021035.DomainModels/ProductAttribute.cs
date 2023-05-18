using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19T1021035.DomainModels
{
    public class ProductAttribute
    {
        public long AttributeID { get; set; }
        ///<summary>
        ///
        ///</summary>
        public int ProductID { get; set; }
        ///<summary>
        ///
        ///</summary>
        public string AttributeName { get; set; }
        ///<summary>
        ///
        ///</summary>
        public string AttributeValue { get; set; }
        ///<summary>
        ///
        ///</summary>
        public int DisplayOrder { get; set; }
    }
}
