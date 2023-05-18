using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _19T1021035.BusinessLayers;
using _19T1021035.DomainModels;
using _19T1021035.Web.Models;
using System.IO;

namespace _19T1021035.Web.Controllers
{
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH = "SearchProductCondition";

        /// <summary>
        /// Tìm kiếm, hiển thị mặt hàng dưới dạng phân trang
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ProductSearchInput condition = Session[PRODUCT_SEARCH] as ProductSearchInput;
           
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                 
                };
            }

            return View(condition);
        }

        public ActionResult Search(ProductSearchInput condition)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(condition.Page, condition.PageSize, condition.SearchValue, out rowCount, condition.CategoryID, condition.SupplierID);

            var result = new ProductSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                CategoryID = condition.CategoryID,
                SupplierID = condition.SupplierID,
                Data = data,
            };

            Session[PRODUCT_SEARCH] = condition;
            return View(result);
        }
        /// <summary>
        /// Tạo mặt hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            try
            {
                var product = new Product()
                {
                    ProductID = 0
                };
                var data = new ProductAPOutput()
                {
                    product = product
                };
                ViewBag.Title = "Bổ Sung Sản phẩm";
                return View(data);
            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra vui lòng thử lại sau");
            }
        }
        /// <summary>
        /// Cập nhật thông tin mặt hàng, 
        /// Hiển thị danh sách ảnh và thuộc tính của mặt hàng, điều hướng đến các chức năng
        /// quản lý ảnh và thuộc tính của mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Edit(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");


                var data = ProductDataService.GetProduct(id);
                var ListPhoto = ProductDataService.ListPhotos(id);
                var ListAttribute = ProductDataService.ListAttributes(id);

                var result = new ProductAPOutput()
                {
                    product = data,
                    listAttribute = ListAttribute,
                    listPhoto = ListPhoto,
                };
                if (data == null)
                    return RedirectToAction("Index");


               
                ViewBag.Title = "Cập nhật Mặt hàng";
                return View(result);
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi (ex)
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");
            ViewBag.Title = "Product | Delete";
            int productID = Convert.ToInt32(id);

            if (Request.HttpMethod == "GET")
            {
                var data = ProductDataService.GetProduct(productID);
                return View(data);
            }
            else
            {
                ProductDataService.DeleteProduct(productID);
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Các chức năng quản lý ảnh của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method?}/{productID?}/{photoID?}")]
        public ActionResult Photo(string method = "add", int productID = 0, long photoID = 0)
        {
            
            switch (method)
            {
                case "add":
                    var photo = new ProductPhoto()
                    {
                        PhotoID = 0
                    };

                    
                    ViewBag.Title = "Bổ sung ảnh";
                    return View(photo);
                case "edit":
                    var data = ProductDataService.GetPhoto(photoID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ViewBag.Title = "Thay đổi ảnh";
                    return View(data);
                case "delete":
                    ProductDataService.DeletePhoto(photoID);
                    return RedirectToAction($"Edit/{productID}"); 
                default:
                    return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Các chức năng quản lý thuộc tính của mặt hàng
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method?}/{productID?}/{attributeID?}")]
        public ActionResult Attribute(string method = "add", int productID = 0, long attributeID = 0)
        {
            switch (method)
            {
                case "add":
                    var attribute = new ProductAttribute()
                    {
                        AttributeID = 0
                    };
                    ViewBag.Title = "Bổ sung thuộc tính";
                    return View(attribute);
                case "edit":
                    var data = ProductDataService.GetAttribute(attributeID);
                    if (data == null)
                        return RedirectToAction($"Edit/{productID}");
                    ViewBag.Title = "Thay đổi thuộc tính";
                    return View(data);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeID);
                    return RedirectToAction($"Edit/{productID}"); //return RedirectToAction("Edit", new { productID = productID });
                default:
                    return RedirectToAction("Index");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProductAPOutput data, HttpPostedFileBase uploadPhoto)
        {
            try
            {
                // Kiểm soát đầu vào (hợp lệ hay không)
                if (string.IsNullOrWhiteSpace(data.product.ProductName))
                    ModelState.AddModelError("ProductName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.product.Unit))
                    ModelState.AddModelError("Unit", "Dơn vị tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.product.Price))
                    ModelState.AddModelError("Price", "Giá không được để trống");
                if (string.IsNullOrWhiteSpace(data.product.Photo))
                    data.product.Photo = "";

                if (!ModelState.IsValid)
                {
                    if (data.product.ProductID != 0)
                    {
                        ViewBag.Title = "Cập nhật mặt hàng";
                        return View("Edit", data);
                    }
                    else
                    {
                        ViewBag.Title = "bổ sung mặt hàng";
                        return View("Create", data);
                    }
                }


                if (uploadPhoto != null)
                {
                    var path = Server.MapPath("~/Photos/products");
                    var fileName = $"{data.product.ProductID}_{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    var filePath = Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    data.product.Photo = fileName;
                }


                if (data.product.ProductID == 0)
                {

                    ProductDataService.AddProduct(data.product);
                }
                else
                {
                    ProductDataService.UpdateProduct(data.product);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi (ex)
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePhoto(ProductPhoto data ,HttpPostedFileBase uploadPhoto)
        {
            try
            {
               
                // Kiểm soát đầu vào (hợp lệ hay không)
                if (string.IsNullOrWhiteSpace(data.Description))
                    ModelState.AddModelError("Description", "Phần mô tả không được để trống");
                if (data.DisplayOrder == null)
                    data.DisplayOrder =1;
                if (data.IsHidden == null)
                    data.IsHidden = true;
               
                if (!ModelState.IsValid)
                {
                    if (data.PhotoID != 0)
                    {
                        ViewBag.Title = "Cập nhật ảnh";
                        return View("Photo", data);
                    }
                    else
                    {
                        ViewBag.Title = "bổ sung ảnh";
                        return View("Photo", data);
                    }
                }
                if (uploadPhoto != null)
                {
                    var path = Server.MapPath("~/Photos/products");
                    var fileName = $"{data.ProductID}_{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    var filePath = Path.Combine(path, fileName);
                    uploadPhoto.SaveAs(filePath);
                    data.Photo = fileName;
                }
                if (data.PhotoID == 0)
                {

                    ProductDataService.AddPhoto(data);
                }
                else
                {
                    ProductDataService.UpdatePhoto(data);
                }

                return RedirectToAction($"Edit/{data.ProductID}");
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi (ex)
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAttribute(ProductAttribute data)
        {
            try
            {

                // Kiểm soát đầu vào (hợp lệ hay không)
                if (string.IsNullOrWhiteSpace(data.AttributeName))
                    ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.AttributeValue))
                    ModelState.AddModelError("AttributeValue", "Giá trị của thuộc tính không được để trống");
                if (data.DisplayOrder==null)
                    ModelState.AddModelError("DisplayOrder", "thứ tự hiển thị không được để trống");

                if (!ModelState.IsValid)
                {
                    if (data.AttributeID != 0)
                    {
                        ViewBag.Title = "Cập nhật thuộc tính";
                        return View("Attribute", data);
                    }
                    else
                    {
                        ViewBag.Title = "bổ sung thuộc tính";
                        return View("Attribute", data);
                    }
                }
               
                if (data.AttributeID == 0)
                {

                    ProductDataService.AddAttribute(data);
                }
                else
                {
                    ProductDataService.UpdateAttribute(data);
                }

                return RedirectToAction($"Edit/{data.ProductID}");
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi (ex)
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }
    }
}