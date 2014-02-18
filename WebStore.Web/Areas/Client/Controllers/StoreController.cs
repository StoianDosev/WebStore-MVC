using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Web.Areas.Client.Models;
using WebStore.Web.Areas.Client.ViewModels;
using WebStore.Web.ViewModels;
using PagedList;

namespace WebStore.Web.Areas.Client.Controllers
{
    public class StoreController : Controller
    {
        private CategoryTreeListModel treeModel;

        private ProductIndexViewModel productModel;

        private ShoppingCartIndexViewModel ShoppingCart;

        public StoreController(IUowData data)
        {
            this.treeModel = new CategoryTreeListModel(data);
            this.productModel = new ProductIndexViewModel(data);
            this.ShoppingCart = new ShoppingCartIndexViewModel(data);
        }

        public ActionResult Index(int? id, int languageId = 1, int page = 1)
        {
            this.productModel.Load();

            if (id == null)
            {
                id = 1;
            }

            this.productModel.CurrentCategoryId = id.Value;

            this.productModel.CurrentPage = page;

            this.productModel.Load();

            IList<ProductViewModel> products = this.productModel.GetAllProductsFromCategoty(id.Value, languageId);

            var pagedProducts = products.ToPagedList(page, 5);

            this.productModel.PagedProductViewModels = pagedProducts;

            if(Request.IsAjaxRequest()==true)
            {
                return PartialView(this.productModel);
            }

            return View(this.productModel);
        }

        public ActionResult ProductList(int? id,int languageId = 1,int page=1)
        {

            this.productModel.Load();

            if (id == null)
            {
                id = 1;
            }

            this.productModel.CurrentCategoryId = id.Value;

            this.productModel.CurrentPage = page;

            

            IList<ProductViewModel> products = this.productModel.GetAllProductsFromCategoty(id.Value, languageId);


            var pagedProducts = products.ToPagedList(page, 5);

            this.productModel.PagedProductViewModels = pagedProducts;


            return PartialView("_ProductList", this.productModel.PagedProductViewModels);
        }

        public ActionResult TopProducts(int? languageId)
        {

            if(languageId==null)
            {
                languageId = 1;
            }

            this.productModel.Load(languageId.Value);

            IList<ProductViewModel> topProducts = this.productModel.GetTopProducts();
            this.productModel.TopProductViewModels = topProducts;

            return View(this.productModel);
        }
       

        public JsonResult GetTreeCategoriesJson(int? id)
        {
            if (id == null)
            {
                id = 1;
            }
            int languageId = id.Value;
            this.treeModel.Load(languageId);
            return Json(this.treeModel.CategoryTreeModels, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int? id, int? languageId)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (languageId == null)
            {
                this.productModel.Load();
            }
            else
            {
                this.productModel.Load(languageId.Value);
            }

            ProductViewModel product = new ProductViewModel();
            product = this.productModel.ProductViewModels.Where(x => x.ProductId == id.Value).FirstOrDefault();

            if (product == null)
            {
                this.productModel.Load();
                product = this.productModel.ProductViewModels.Where(x => x.ProductId == id.Value).FirstOrDefault();
                ViewBag.Message = "No translation to that language yet.";
            }

            product.Load();
            return View(product);
        }

        public ActionResult ShoppingCartDetails()
        {
            if (Session["ShoppingCart"] == null)
            {
                Session["ShoppingCart"] = this.ShoppingCart;
            }

            this.ShoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];
            

            if (this.ShoppingCart.ShoppingcartOrders.Count == 0)
            {
                ViewBag.NoItemsMessage = "No products in the cart!";
            }
            bool isAboveZeroQuantity = this.ShoppingCart.CheckAboveZeroQuantity();
            if (!isAboveZeroQuantity)
            {
                ViewBag.NoItemsMessage = "Zero products in the cart!";
            }
           
            return View(this.ShoppingCart);
        }

        public ActionResult UpdateQuantity(int? newQuantity, int productId)
        {

            if(newQuantity == null || newQuantity.Value==0)
            {
                return RedirectToAction("ShoppingCartDetails");
            }

            this.ShoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];

            bool inStockQuantity = this.ShoppingCart.CheckForEnoughInStockProductsUpdate(productId, newQuantity.Value);

            if (inStockQuantity == false)
            {

                Session["ShoppingCart"] = this.ShoppingCart;
                ViewBag.NoItemsMessage = "Not enough products in store!";
                return View("ShoppingCartDetails", this.ShoppingCart);
            }

            if(ModelState.IsValid)
            {
                this.ShoppingCart.UpdateShoppingCart(newQuantity.Value, productId);
            }

            return RedirectToAction("ShoppingCartDetails");
        }

        public ActionResult AddToShoppingCart(int id, int? quantity )
        {
            if (Session["ShoppingCart"] == null)
            {
                Session["ShoppingCart"] = this.ShoppingCart;
            }

            this.ShoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];

           

            this.productModel.Load();
            var product = this.productModel.ProductViewModels.Where(x => x.ProductId == id).FirstOrDefault();
            
            if(product.InStock==false)
            {
                ViewBag.NoItemsMessage = "The product is out of stock!";
                this.ShoppingCart.InStockQuantity = false;
                Session["ShoppingCart"] = this.ShoppingCart;
                return PartialView("_ShoppingCartSummary", this.ShoppingCart);
            }


            if(quantity==null || quantity.Value==0)
            {
                ViewBag.NoItemsMessage = "You must enter number above zero!";
                return PartialView("_ShoppingCartSummary", this.ShoppingCart);
            }

            bool inStockQuantity = this.ShoppingCart.CheckForEnoughInStokProductsAdd(id, quantity.Value);

            if (inStockQuantity == false)
            {
                this.ShoppingCart.InStockQuantity = false;
                Session["ShoppingCart"] = this.ShoppingCart;
                ViewBag.NoItemsMessage = "Not enough products in store!";
                return PartialView("_ShoppingCartSummary", this.ShoppingCart);
            }

            if(quantity!=null && quantity.Value!=0)
            {
                this.ShoppingCart.AddToShoppinCart(id, quantity.Value);

                return PartialView("_ShoppingCartSummary", this.ShoppingCart);
            }

            return PartialView("_ShoppingCartSummary", this.ShoppingCart);
        }

        public ActionResult RemoveItem(int? id)
        {
            
            this.ShoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];
            
            if(this.ShoppingCart.ShoppingcartOrders.Count !=0)
            {
                this.ShoppingCart.RemoveFromShoppingCart(id);
            }

            Session["ShoppingCart"] = this.ShoppingCart;

            return RedirectToAction("ShoppingCartDetails");
        }

        public ActionResult CartSummary()
        {
            if (Session["ShoppingCart"] == null)
            {
                Session["ShoppingCart"] = this.ShoppingCart;
            }
            var shoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];

            return PartialView("_ShoppingCartSummary", shoppingCart);
        }

        public ActionResult CheckOut(string returnUrl)
        {
            this.ShoppingCart = (ShoppingCartIndexViewModel)Session["ShoppingCart"];

            bool isAboveZeroQuantity = this.ShoppingCart.CheckAboveZeroQuantity();



            if (isAboveZeroQuantity==false )
            {
                return RedirectToAction("ShoppingCartDetails");
            }

            if (Session["Customer"] == null)
            {
                return RedirectToAction("Login", "CustomerStore", new { ReturnUrl = returnUrl });
            }

            CustomerViewModel customer = (CustomerViewModel)Session["Customer"];

            ShoppingCartIndexViewModel cart = new ShoppingCartIndexViewModel();

            OrderViewModel orderModel = cart.ConvertCartToOrder(this.ShoppingCart, customer);

            Session["OrderModel"] = orderModel;


            return View(orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel confirmationModel)
        {
            OrderViewModel model = (OrderViewModel)Session["OrderModel"];

            model.Address = confirmationModel.Address;
            model.City = confirmationModel.City;
            model.Email = confirmationModel.Email;
            model.FirstName = confirmationModel.FirstName;
            model.LastName = confirmationModel.LastName;
            model.Phone = confirmationModel.Phone;

            if (ModelState.IsValid)
            {
                ShoppingCartIndexViewModel cart = new ShoppingCartIndexViewModel();

                cart.SubmitOrder(model);

                Session["ShoppingCart"] = null;


                return RedirectToAction("OrderComplete");

            }

            return View(confirmationModel);
        }


        public ActionResult OrderComplete()
        {
            ViewBag.Message = "Order is successful";
            return View();

        }
    }
}
