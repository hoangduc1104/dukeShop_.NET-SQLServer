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
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string CATEGORY_SEARCH = "SearchCategoryCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Category
        //public ActionResult Index(int page = 1, int pageSize = 8, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfCategories(page, pageSize, searchValue, out rowCount);

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
            PaginationSearchInput condition = Session[CATEGORY_SEARCH] as PaginationSearchInput;

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

        public ActionResult Search(PaginationSearchInput condition)
        {
            try
            {
                int rowCount = 0;
                var data = CommonDataService.ListOfCategories(condition.Page, condition.PageSize, condition.SearchValue, out rowCount);

                var result = new CategorySearchOutput()
                {
                    Page = condition.Page,
                    PageSize = condition.PageSize,
                    SearchValue = condition.SearchValue,
                    RowCount = rowCount,
                    Data = data
                };

                Session[CATEGORY_SEARCH] = condition;
                return View(result);
            }

            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại saua");
            }
        }
        /// <summary>
        /// Giao diện bổ sung loại hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            try
            {
                var data = new Category()
                {
                    CategoryID = 0
                };
                ViewBag.Title = "Bổ Sung Loại Hàng";
                return View("Edit", data);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
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


                var data = CommonDataService.GetCategory(id);
                if (data == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập Nhật Loại Hàng";
                return View(data);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Category data)
        {
            try
            {
                // Kiểm soát đầu vào (hợp lệ hay không)
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError("CategoryName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Tên giao dịch không được để trống");
                /*if (string.IsNullOrWhiteSpace(Convert.ToString(data.ParentCategoryId)))
                    ModelState.AddModelError("ParentCategoryId", "Tên giao dịch không được để trống");*/

                //... viết tiếp cho đầy đủ??
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = data.CategoryID == 0 ? "Bổ Sung Loại Hàng" : "Cập Nhật Loại Hàng";
                    return View("Edit", data);
                }

                if (data.CategoryID == 0)
                {
                    CommonDataService.AddCategory(data);
                }
                else
                {
                    CommonDataService.UpdateCategory(data);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
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

            ViewBag.Title = "Category | Delete";

            if (Request.HttpMethod == "GET")
            {
                var category = CommonDataService.GetCategory(id);
                if (category == null)
                    return RedirectToAction("Index");
                return View(category);
            }

            CommonDataService.DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}