using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021035.DomainModels;


namespace _19T1021035.DataLayers
{

    /// <summary>
    /// Các chức năng sử lý dữ liệu cho quốc gia
    /// </summary>
    public interface ICountryDAL
    {
        /// <summary>
        /// lấy danh sách tất cả các quốc gia
        /// </summary>
        /// <returns></returns>
        IList<Country> List();
    }
}
