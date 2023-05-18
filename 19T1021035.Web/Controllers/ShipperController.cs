using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _19T1021035.BusinessLayers;
using _19T1021035.DomainModels;
using _19T1021035.Web.Models;

namespace _19T1021035.Web.Controllers
{
    [Authorize]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string SHIPPER_SEARCH = "SearchShipperCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Shipper
        //public ActionResult Index(int page = 1, int pageSize = 8, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfShippers(page, pageSize, searchValue, out rowCount);

        //    int pageCount = rowCount / pageSize;
        //    if (rowCount % pageSize > 0)
        //        pageCount += 1;

        //    ViewBag.Page = page;
        //    ViewBag.PageCount = pageCount;
        //    ViewBag.RowCount = rowCount;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.SearchValue = searchValue;

        //    return View(data);
        //    //trả về giao diện mà có truyền thêm dữ liệu cho giao diện (data)
        //}
        public ActionResult Index()
        {
            PaginationSearchInput condition = Session[SHIPPER_SEARCH] as PaginationSearchInput;

            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(condition);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult Search(PaginationSearchInput condition)
        {
            try
            {
                int rowCount = 0;
                var data = CommonDataService.ListOfShippers(condition.Page, condition.PageSize, condition.SearchValue, out rowCount);

                var result = new ShipperSearchOutput()
                {
                    Page = condition.Page,
                    PageSize = condition.PageSize,
                    SearchValue = condition.SearchValue,
                    RowCount = rowCount,
                    Data = data
                };

                Session[SHIPPER_SEARCH] = condition;
                return View(result);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
        /// <summary>
        /// Giao diện bổ sung người giao hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            try
            {
                var data = new Shipper()
                {
                    ShipperID = 0
                };
                ViewBag.Title = "Bổ Sung Người Giao Hàng";
                return View("Edit", data);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            try
            {

                if (id == 0)
                    return RedirectToAction("Index");


                var data = CommonDataService.GetShipper(id);
                if (data == null)
                    return RedirectToAction("Index");
                ViewBag.Title = "Cập Nhật Người Giao Hàng";
                return View(data);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Shipper data)
        {
            try
            {
                // Kiểm soát đầu vào (hợp lệ hay không)
                if (string.IsNullOrWhiteSpace(data.ShipperName))
                    ModelState.AddModelError("ShipperName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Phone))
                    ModelState.AddModelError("Phone", "Tên giao dịch không được để trống");

                //... viết tiếp cho đầy đủ??
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.ShipperID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
                    return View("Edit", data);
                }


                if (data.ShipperID == 0)
                {
                    CommonDataService.AddShipper(data);
                }
                else
                {
                    CommonDataService.UpdateShipper(data);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            ViewBag.Title = "Shipper | Delete";

            if (Request.HttpMethod != "GET")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var shipper = CommonDataService.GetShipper(id);
            if (shipper == null)
                return RedirectToAction("Index");
            return View(shipper);
        }
    }
}