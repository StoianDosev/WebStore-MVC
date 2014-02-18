using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;
using WebStore.Web.Areas.Client.Models;
using WebStore.Web.ViewModels;

namespace WebStore.Web.Areas.Client.ViewModels
{
    public class ShoppingCartIndexViewModel
    {
        private IUowData db;

        public List<ShoppingCart> ShoppingcartOrders { get; set; }

        public IEnumerable<SelectListItem> LanguageSelectItems { get; set; }

        public decimal TotalOrderPrice { get; set; }

        public int TotalProductCount { get; set; }

        public bool InStockQuantity { get; set; }

        public ShoppingCartIndexViewModel(IUowData data)
        {
            this.db = data;
            this.ShoppingcartOrders = new List<ShoppingCart>();
            this.LanguageSelectItems = new List<SelectListItem>();
        }

        public ShoppingCartIndexViewModel()
            : this(new UowData())
        { }

        public void Load()
        {
            var languages = db.Languages.All().ToList();

            this.LanguageSelectItems = languages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });
        }

        public OrderViewModel ConvertCartToOrder(ShoppingCartIndexViewModel cart, CustomerViewModel customer)
        {

            List<OrderDetailsViewModel> orderDetails = new List<OrderDetailsViewModel>();
            foreach (var item in cart.ShoppingcartOrders)
            {
                orderDetails.Add(new OrderDetailsViewModel(db)
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        PruductCode = item.ProductCode,
                    });
            }
            OrderViewModel order = new OrderViewModel(db)
            {
                Address = customer.Address,
                City = customer.City,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                OrderDetailsViewModel = orderDetails,
                TotalPrice = cart.TotalOrderPrice,
            };

            return order;
        }

        public void SubmitOrder(OrderViewModel order)
        {
            OrderIndexViewModel indexModel = new OrderIndexViewModel(db);
            indexModel.CreateOrder(order);
        }

        public void UpdateShoppingCart(int quantity, int productId)
        {
            foreach (var item in this.ShoppingcartOrders)
            {
                if (item.ProductId == productId)
                {
                    item.Quantity = quantity;
                    item.TotalPrice = quantity * item.Price;
                }
            }
            this.TotalOrderPrice = GetTotalOrderPrice();
            this.TotalProductCount = GetTotalProductCount();
        }

        public void AddToShoppinCart(int productId, int quantity, int languageId = 1)
        {
            ProductLanguage product = db.ProductLanguages.All().Where(x => x.ProductId == productId && x.LanguageId == languageId).FirstOrDefault();

            ShoppingCart existingProduct = null;

            if (this.ShoppingcartOrders.Count != 0)
            {
                existingProduct = this.ShoppingcartOrders.Where(x => x.ProductId == productId).FirstOrDefault();
            }

            if (existingProduct == null)
            {
                this.ShoppingcartOrders.Add(new ShoppingCart()
                {
                    ProductId = product.ProductId,
                    LanguageId = languageId,
                    Image = product.Product.Image,
                    Price = product.Product.Price,
                    ProductCode = product.Product.ProductCode,
                    ProductName = product.Name,
                    Quantity = quantity,
                    ProductLanguageId = product.Id,
                    TotalPrice = quantity * product.Product.Price,

                });
            }
            else
            {
                existingProduct.Quantity += quantity;
                existingProduct.TotalPrice = product.Product.Price * existingProduct.Quantity;
            }

            this.TotalOrderPrice = GetTotalOrderPrice();

            this.TotalProductCount = GetTotalProductCount();
        }

        public void RemoveFromShoppingCart(int? productId)
        {
            if (productId != null)
            {
                ShoppingCart cartOrder = this.ShoppingcartOrders.Where(x => x.ProductId == productId).FirstOrDefault();

                this.TotalOrderPrice = this.TotalOrderPrice - cartOrder.Price * cartOrder.Quantity;

                this.TotalProductCount = this.TotalProductCount - cartOrder.Quantity;

                this.ShoppingcartOrders.Remove(cartOrder);


            }

        }


        public bool CheckForEnoughInStockProductsUpdate(int productId, int quantity)
        {
            Product product = db.Products.All().Where(x => x.Id == productId).FirstOrDefault();

            bool inStockQuantity = true;

            if (quantity > product.Quantity)
            {
                inStockQuantity = false;
            }

            return inStockQuantity;
        }

        public bool CheckForEnoughInStokProductsAdd(int productId, int quantity)
        {
            Product product = db.Products.All().Where(x => x.Id == productId).FirstOrDefault();

            bool inStockQuantity = true;


            if (this.ShoppingcartOrders.Count != 0)
            {
                ShoppingCart existingProduct = existingProduct = this.ShoppingcartOrders.Where(x => x.ProductId == productId).FirstOrDefault();
                if (existingProduct != null)
                {
                    if (quantity > (product.Quantity - existingProduct.Quantity))
                    {
                        inStockQuantity = false;
                    }
                    if (product.Quantity < existingProduct.Quantity + quantity)
                    {
                        inStockQuantity = false;
                    }
                }
            }
            else
            {
                if (quantity > product.Quantity)
                {
                    inStockQuantity = false;
                }
            }



            return inStockQuantity;
        }

        public bool CheckAboveZeroQuantity()
        {
            foreach (var item in this.ShoppingcartOrders)
            {
                if (item.Quantity == 0)
                {
                    return false;
                }
            }
            if(this.ShoppingcartOrders.Count==0)
            {
                return false;
            }
            return true;
        }

        private decimal GetTotalOrderPrice()
        {
            decimal result = 0;
            foreach (var item in this.ShoppingcartOrders)
            {
                result += item.TotalPrice;
            }
            return result;
        }

        public int GetTotalProductCount()
        {
            int productCount = 0;

            foreach (var item in this.ShoppingcartOrders)
            {
                productCount += item.Quantity;
            }
            return productCount;
        }

    }
}