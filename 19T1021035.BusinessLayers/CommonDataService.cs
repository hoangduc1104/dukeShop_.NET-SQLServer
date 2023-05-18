using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _19T1021035.DomainModels;
using _19T1021035.DataLayers;
using System.Configuration;


namespace _19T1021035.BusinessLayers
{
    public static class CommonDataService
    {
        private static ICountryDAL countryDB;
        private static ICommonDAL<Supplier> supplierDB;
        private static ICommonDAL<Shipper> shipperDB;
        private static ICommonDAL<Customer> customerDB;
        private static ICommonDAL<Employee> employeeDB;
        private static ICommonDAL<Category> categoryDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            countryDB = new DataLayers.SQLServer.CountryDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);
        }
        /// <summary>
        /// Lấy danh sách các quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListOfCountries()
        {
            return countryDB.List().ToList();
        }
        #region Các nghiệp vụ liên quan đến nhà cung cấp

        /// <summary>
        /// Tìm kiếm lấy danh sách các nhà cung cấp dưới dạng phân trang
        /// </summary>
        /// <param name="page"> trang cần xem </param>
        /// <param name="pagesSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm </param>
        /// <param name="rowCount">Output: tổng số dòng tìm đc</param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// tìm kiếm và lấy danh sách nhà cung cấp (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(string searchValue)
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns>mã của nhà cung cấp được bổ sung</returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// xóa nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int supplierID)
        {
            return supplierDB.Delete(supplierID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return supplierDB.Get(supplierID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool InUsedSupplier(int supplierID)
        {
            return supplierDB.InUsed(supplierID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến loại hàng

        /// <summary>
        /// Tìm kiếm lấy danh sách các loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"> trang cần xem </param>
        /// <param name="pagesSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm </param>
        /// <param name="rowCount">Output: tổng số dòng tìm đc</param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// tìm kiếm và lấy danh sách loại hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(string searchValue)
        {
            return categoryDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>mã của loại hàng được bổ sung</returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// <summary>
        /// xóa loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int categoryID)
        {
            return categoryDB.Delete(categoryID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return categoryDB.Get(categoryID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool InUsedCategoryr(int categoryID)
        {
            return categoryDB.InUsed(categoryID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến khách hàng

        /// <summary>
        /// Tìm kiếm lấy danh sách các loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"> trang cần xem </param>
        /// <param name="pagesSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm </param>
        /// <param name="rowCount">Output: tổng số dòng tìm đc</param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// tìm kiếm và lấy danh sách loại hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(string searchValue)
        {
            return customerDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>mã của khách hàng được bổ sung</returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// xóa khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int customerID)
        {
            return customerDB.Delete(customerID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(int customerID)
        {
            return customerDB.Get(customerID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool InUsedCustomerr(int customerID)
        {
            return customerDB.InUsed(customerID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến nhân viên

        /// <summary>
        /// Tìm kiếm lấy danh sách các loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"> trang cần xem </param>
        /// <param name="pagesSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm </param>
        /// <param name="rowCount">Output: tổng số dòng tìm đc</param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// tìm kiếm và lấy danh sách loại hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(string searchValue)
        {
            return employeeDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>mã của khách hàng được bổ sung</returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// xóa khách hàng
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int EmployeeID)
        {
            return employeeDB.Delete(EmployeeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int EmployeeID)
        {
            return employeeDB.Get(EmployeeID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <returns></returns>
        public static bool InUsedEmployeer(int EmployeeID)
        {
            return employeeDB.InUsed(EmployeeID);
        }
        #endregion

        #region Các nghiệp vụ liên quan đến người giao hàng

        /// <summary>
        /// Tìm kiếm lấy danh sách các loại hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"> trang cần xem </param>
        /// <param name="pagesSize">số dòng trên mỗi trang</param>
        /// <param name="searchValue">giá trị tìm kiếm </param>
        /// <param name="rowCount">Output: tổng số dòng tìm đc</param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page, int pageSize, string searchValue, out int rowCount)
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// tìm kiếm và lấy danh sách loại hàng (không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(string searchValue)
        {
            return shipperDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns>mã của khách hàng được bổ sung</returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// xóa khách hàng
        /// </summary>
        /// <param name="ShipperID"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int ShipperID)
        {
            return shipperDB.Delete(ShipperID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int ShipperID)
        {
            return shipperDB.Get(ShipperID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ShipperID"></param>
        /// <returns></returns>
        public static bool InUsedShipperr(int ShipperID)
        {
            return shipperDB.InUsed(ShipperID);
        }
        #endregion
    }
}
