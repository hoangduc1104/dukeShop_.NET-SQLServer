using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _19T1021035.DomainModels
{
    public class Product
    {
        /// <summary>
        /// Mã hàng
        /// </summary>
        public int ProductID { get; set; }
        /// <summary>
        /// Tên hàng
        /// </summary>
        public String ProductName { get; set; }
        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public int SupplierID { get; set; }
        /// <summary>
        /// Mã loại hàng
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Unit { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        public String Price { get; set; }
        /// <summary>
        /// Ảnh
        /// </summary>
        public String Photo { get; set; }
    }
}
