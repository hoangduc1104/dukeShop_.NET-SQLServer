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
    [RoutePrefix("Order")]
    public class OrderController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const int PAGE_SIZE = 8;
        private const string ORDER_SEARCH = "SearchOrderCondition";
        private const string PRODUCT_SEARCH = "SearchOrderProductCondition";

        /// <summary>
        /// Tìm kiếm, phân trang
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            OrderSearchInput condition = Session[ORDER_SEARCH] as OrderSearchInput;
           
            if (condition == null)
            {
                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                };
            }


            return View(condition);
        }

        public ActionResult Search(OrderSearchInput condition)
        {
            int rowCount = 0;
            var data = OrderService.ListOrder(condition.Page, condition.PageSize, condition.Status, condition.SearchValue, out rowCount);

            var result = new OrderSearchOutput()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue,
                RowCount = rowCount,
                Data = data,
            };

            Session[ORDER_SEARCH] = condition;
            return View(result);
        }

        /// <summary>
        /// Xem thông tin và chi tiết của đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");


                var data = OrderService.GetOrder(id);
                var ListOrderDetail = OrderService.ListOrderDetails(id);
                decimal total = 0;
                foreach(var item in ListOrderDetail)
                {
                    total += item.TotalPrice;
               
                }

                var result = new OrderDetailOutput()
                {
                    order = data,
                    listOrderDetail = ListOrderDetail,
                    total = total,
                };
                if (data == null)
                    return RedirectToAction("Index");



                ViewBag.Title = "Chi tiết đơn hàng";
                return View(result);
            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

            return View();
        }
        /// <summary>
        /// Giao diện Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("EditDetail/{orderID}/{productID}")]
        public ActionResult EditDetail(int orderID = 0, int productID = 0)
        {
            try
            {
                if (orderID == 0)
                    return RedirectToAction("Index");
                if (productID == 0)
                    return RedirectToAction($"Details/{orderID}");

                var data = OrderService.GetOrderDetail(orderID,productID);
               
                if (data == null)
                    return RedirectToAction($"Details/{orderID}");

                ViewBag.Title = "Cập nhật Mặt hàng";
                return View(data);
            }
            catch (Exception ex)
            {
                // Ghi lại log lỗi (ex)
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

        }
        /// <summary>
        /// Thay đổi thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDetail(OrderDetail data)
        {
            try
            {
                // Kiểm soát đầu vào (hợp lệ hay không)
                
                if (string.IsNullOrWhiteSpace(data.Quantity.ToString()))
                    ModelState.AddModelError("Quantity", "Dơn vị tính không được để trống");
                if (string.IsNullOrWhiteSpace(data.SalePrice.ToString()))
                    ModelState.AddModelError("Price", "Giá không được để trống");
                
                if (!ModelState.IsValid)
                {
                    if (data.OrderDetailID != 0)
                    {
                        ViewBag.Title = "Cập nhật mặt hàng";
                        return View("EditDetail", data);
                    }

                }


                OrderService.SaveOrderDetail(data.OrderID, data.ProductID, data.Quantity, data.SalePrice);


                return RedirectToAction($"Details/{data.OrderID}");
            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
            
        }
        /// <summary>
        /// Xóa 1 chi tiết trong đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("DeleteDetail/{orderID}/{productID}")]
        public ActionResult DeleteDetail(int orderID = 0, int productID = 0)
        {

            try
            {
                if (orderID == 0)
                    return RedirectToAction("Index");
                if (productID == 0)
                    return RedirectToAction($"Details/{orderID}");

                var data = OrderService.DeleteOrderDetail(orderID, productID);

               
                return RedirectToAction($"Details/{orderID}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);

                if (data.Status == 1 || data.Status == 2)
                    OrderService.DeleteOrder(id);


                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

        }
        /// <summary>
        /// Chấp nhận đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Accept(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);

                if (data.Status == 1)                    
                    OrderService.AcceptOrder(id);

                return RedirectToAction($"Details/{id}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
           
        }
        /// <summary>
        /// Xác nhận chuyển đơn hàng cho người giao hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Shipping(int id = 0, int shipperID = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);
                
                if (data.Status == 2)
                {
                    if (Request.HttpMethod == "GET")
                        return View(data);
                    OrderService.ShipOrder(id, shipperID);
                }

                return RedirectToAction($"Details/{id}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

           
        }
        /// <summary>
        /// Ghi nhận hoàn tất thành công đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Finish(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);

                if (data.Status == 3)
                    OrderService.FinishOrder(id);

                return RedirectToAction($"Details/{id}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Cancel(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);

                if (data.Status == 1 || data.Status == 2)
                    OrderService.CancelOrder(id);

                return RedirectToAction($"Details/{id}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }

        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Reject(int id = 0)
        {
            try
            {
                if (id == 0)
                    return RedirectToAction("Index");

                var data = OrderService.GetOrder(id);

                if (data.Status == 1)
                    OrderService.RejectOrder(id);

                return RedirectToAction($"Details/{id}");

            }
            catch (Exception ex)
            {
                return Content("Có lỗi xảy ra, vui lòng thử lại sau");
            }
        }

        /// <summary>
        /// Sử dụng 1 biến session để lưu tạm giỏ hàng (danh sách các chi tiết của đơn hàng) trong quá trình xử lý.
        /// Hàm này lấy giỏ hàng hiện đang có trong session (nếu chưa có thì tạo mới giỏ hàng rỗng)
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {
            List<OrderDetail> shoppingCart = Session[SHOPPING_CART] as List<OrderDetail>;
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                Session[SHOPPING_CART] = shoppingCart;
            }
            return shoppingCart;
        }
        /// <summary>
        /// Giao diện lập đơn hàng mới
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {

            ViewBag.ErrorMessage = TempData[ERROR_MESSAGE] ?? "";
            return View(GetShoppingCart());
        }

        /// <summary>
        /// Tìm kiếm mặt hàng để bổ sung vào giỏ hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public ActionResult SearchProducts(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(page, PAGE_SIZE, searchValue, 0, 0, out rowCount);
            ViewBag.Page = page;
            return View(data);
        }
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(OrderDetail data)
        {
            if (data == null)
            {
                TempData[ERROR_MESSAGE] = "Dữ liệu không hợp lệ";
                return RedirectToAction("Create");
            }
            if (data.SalePrice <= 0 || data.Quantity <= 0)
            {
                TempData[ERROR_MESSAGE] = "Giá bán và số lượng không hợp lệ";
                return RedirectToAction("Create");
            }

            List<OrderDetail> shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == data.ProductID);

            if (existsProduct == null) //Nếu mặt hàng cần được bổ sung chưa có trong giỏ hàng thì bổ sung vào giỏ
            {

                shoppingCart.Add(data);
            }
            else //Trường hợp mặt hàng cần bổ sung đã có thì tăng số lượng và thay đổi đơn giá
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice = data.SalePrice;
            }
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id">Mã mặt hàng</param>
        /// <returns></returns>
        public ActionResult RemoveFromCart(int id = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            Session[SHOPPING_CART] = shoppingCart;
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Khởi tạo đơn hàng (với phần thông tin chi tiết của đơn hàng là giỏ hàng đang có trong session)
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init(int customerID = 0, int employeeID = 0)
        {
            List<OrderDetail> shoppingCart = GetShoppingCart();
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                TempData[ERROR_MESSAGE] = "Không thể tạo đơn hàng với giỏ hàng trống";
                return RedirectToAction("Create");
            }

            if (customerID == 0 || employeeID == 0)
            {
                TempData[ERROR_MESSAGE] = "Vui lòng chọn khách hàng và nhân viên phụ trách";
                return RedirectToAction("Create");
            }

            int orderID = OrderService.InitOrder(customerID, employeeID, DateTime.Now, shoppingCart);

            Session.Remove(SHOPPING_CART); //Xóa giỏ hàng 

            return RedirectToAction($"Details/{orderID}");
        }
    }
}