using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _19T1021035.BusinessLayers;
using _19T1021035.DomainModels;
using _19T1021035.Web.Models;
using _19T1021035.Web.codes;
using System.IO;

namespace _19T1021035.Web.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 8;
        private const string EMPLOYEE_SEARCH = "SearchEmployeeCondition";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: Employee
        //public ActionResult Index(int page = 1, int pageSize = 8, string searchValue = "")
        //{
        //    int rowCount = 0;
        //    var data = CommonDataService.ListOfEmployees(page, pageSize, searchValue, out rowCount);

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
            var input = Session[EMPLOYEE_SEARCH] as PaginationSearchInput ?? new PaginationSearchInput
            {
                Page = 1,
                PageSize = PAGE_SIZE,
                SearchValue = ""
            };

            return View(input);

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
                var data = CommonDataService.ListOfEmployees(condition.Page,
                                                            condition.PageSize,
                                                            condition.SearchValue,
                                                            out rowCount);
                var result = new EmployeeSearchOutput()
                {
                    Page = condition.Page,
                    PageSize = condition.PageSize,
                    SearchValue = condition.SearchValue,
                    RowCount = rowCount,
                    Data = data
                };
                Session[EMPLOYEE_SEARCH] = condition;
                return View(result);
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
        /// <summary>
        /// Giao diện bổ sung nhân viên mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var employee = new Employee
            {
                EmployeeID = 0,
                BirthDate = new DateTime()
            };

            ViewBag.Title = "Bổ Sung Nhân Viên";
            return View("Edit", employee);

        }
        /// <summary>
        /// Giao diện sửa nhân viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            try
            {

                if (id == 0)
                    return RedirectToAction("Index");


                var data = CommonDataService.GetEmployee(id);
                if (data == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập nhật Nhân Viên";
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
        public ActionResult Save(Employee employee, string birthday, HttpPostedFileBase uploadPhoto)
        {
            var date = codes.Converter.DmyStringToDateTime(birthday);
            if (date == null)
                ModelState.AddModelError("BirthDate", "Birth Date is invalid");
            else
                employee.BirthDate = date.Value;

            try
            {
                if (string.IsNullOrWhiteSpace(employee.FirstName))
                    ModelState.AddModelError("FirstName", "First Name is required");
                if (string.IsNullOrWhiteSpace(employee.LastName))
                    ModelState.AddModelError("LastName", "Last Name is required");
                if (string.IsNullOrWhiteSpace(employee.Email))
                    ModelState.AddModelError("Email", "Email is required");
                if (string.IsNullOrWhiteSpace(employee.Password))
                    ModelState.AddModelError("Password", "Password is required");
                if (string.IsNullOrWhiteSpace(employee.Notes))
                    ModelState.AddModelError("Notes", "Notes is required");
                if (string.IsNullOrWhiteSpace(employee.Photo))
                    employee.Photo = "";

                if (!ModelState.IsValid)
                    return View("Edit", employee);

                if (uploadPhoto != null)
                {
                    var path = Server.MapPath("~/Photos");
                    var fileName = $"{employee.EmployeeID}_{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    var filePath = Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    employee.Photo = fileName;
                }

                if (employee.EmployeeID == 0)
                    CommonDataService.AddEmployee(employee);
                else
                    CommonDataService.UpdateEmployee(employee);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return PartialView("Error", e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            try
            {
                int employeeID = Convert.ToInt32(id);
                if (Request.HttpMethod == "GET")
                {
                    var data = CommonDataService.GetEmployee(employeeID);
                    return View(data);
                }
                else
                {
                    CommonDataService.DeleteEmployee(employeeID);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại");
            }
        }
    }
}