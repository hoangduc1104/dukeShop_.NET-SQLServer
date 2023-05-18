using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _19T1021035.BusinessLayers;
using System.Web.Mvc;

namespace _19T1021035.Web.codes
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> Countries()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn quốc gia --"
            });
            foreach (var item in CommonDataService.ListOfCountries())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CountryName,
                    Text = item.CountryName
                });
            }

            return list;
        }
        public static List<SelectListItem> Category()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn loại hàng --"
            });
            foreach (var item in CommonDataService.ListOfCategories(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName
                });
            }

            return list;
        }

        public static List<SelectListItem> Supplier()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhà cung cấp --"
            });
            foreach (var item in CommonDataService.ListOfSuppliers(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });
            }

            return list;
        }

        public static List<SelectListItem> Customer()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Chọn khách hàng --"
            });
            foreach (var item in CommonDataService.ListOfCustomers(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.CustomerID.ToString(),
                    Text = item.CustomerName
                });
            }

            return list;
        }

        public static List<SelectListItem> Employee()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn nhân viên --"
            });
            foreach (var item in CommonDataService.ListOfEmployees(""))
            {
                list.Add(new SelectListItem()
                {
                    Value = item.EmployeeID.ToString(),
                    Text = item.FirstName + " " +item.LastName
                });
            }

            return list;
        }

        public static List<SelectListItem> Status()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Trạng Thái --"
            });

            list.Add(new SelectListItem()
            {
                Value = "1",
                Text = "-- Đơn hàng mới (chờ duyệt) --"
            });
            list.Add(new SelectListItem()
            {
                Value = "2",
                Text = "-- Đơn hàng đã duyệt (chờ chuyển hàng) --"
            });
            list.Add(new SelectListItem()
            {
                Value = "3",
                Text = "-- Đơn hàng đang được giao --"
            });
            list.Add(new SelectListItem()
            {
                Value = "4",
                Text = "-- Đơn hàng đã giao thành công --"
            });
            list.Add(new SelectListItem()
            {
                Value = "-1",
                Text = "-- Đơn hàng bị hủy --"
            });
            list.Add(new SelectListItem()
            {
                Value = "-2",
                Text = "-- Đơn hàng bị từ chối --"
            });

            return list;
        }

    }
}