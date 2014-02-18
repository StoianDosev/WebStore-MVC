using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class ProductIndexViewModel
    {
        private IUowData db;

        private IEnumerable<SelectListItem> languageSelectItems;

        public IEnumerable<SelectListItem> LanguageSelectItems
        {
            get { return this.languageSelectItems; }
            set { this.languageSelectItems = value; }
        }

        public IPagedList<ProductViewModel> PagedProductViewModels { get; set; }

        public IList<ProductViewModel> ProductViewModels { get; set; }

        public IList<ProductViewModel> TopProductViewModels { get; set; }

        public int CurrentPage { get; set; }

        public int CurrentCategoryId { get; set; }

        public ProductIndexViewModel()
            : this(new UowData())
        { }

        public ProductIndexViewModel(IUowData data)
        {
            this.db = data;
        }


        public IList<ProductViewModel> GetTopProducts()
        {
            IList<ProductViewModel> topProducts = this.ProductViewModels.Where(x => x.IsTopProduct == true).ToList();
            return topProducts;
        }

        public void DecreaseQuantityOfProducts(OrderViewModel model)
        {
            var products = db.Products.All().ToList();

            foreach (var item in model.OrderDetailsViewModel)
            {
                var productEntity = products.Where(x => x.Id == item.ProductId).FirstOrDefault();

                productEntity.Quantity = productEntity.Quantity - item.Quantity;
                productEntity = CheckProductQuantity(productEntity);

                db.SaveChanges();
            }

        }

        private Product CheckProductQuantity(Product product)
        {
            if (product.Quantity <= 0)
            {
                product.Quantity = 0;
                product.InStock = false;
            }
            return product;
        }

        public List<ProductViewModel> GetAllProductsFromCategoty(int categoryId, int languageId = 1)
        {
            List<CategoryViewModel> categoryModels = GetAllSubCategories(categoryId);
            List<ProductViewModel> productModels = new List<ProductViewModel>();
            List<ProductViewModel> currentProductModels = new List<ProductViewModel>();

            if (categoryModels.Count != 0)
            {
                foreach (var item in categoryModels)
                {
                    currentProductModels = GetProductsFromCategory(item.CategoryId, languageId);//!!!languageId!!!!

                    productModels = ReturnNotRepeatedProducts(productModels, currentProductModels);
                }
            }
            else
            {

            }

            return productModels;
        }

        private List<ProductViewModel> ReturnNotRepeatedProducts(List<ProductViewModel> productModels,
            List<ProductViewModel> currentProductModels)
        {
            List<ProductViewModel> currentResult = new List<ProductViewModel>();

            for (int i = 0; i < currentProductModels.Count; i++)
            {
                bool isFound = false;


                foreach (var item in productModels)
                {
                    if (currentProductModels[i].ProductId == item.ProductId)
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    currentResult.Add(currentProductModels[i]);
                }
            }


            foreach (var item in currentResult)
            {
                productModels.Add(item);
            }
            return productModels;
        }

        private List<CategoryViewModel> GetAllSubCategories(int id)
        {
            int categoryId = id;
            int languageId = 1;

            List<CategoryViewModel> categoryModels = new List<CategoryViewModel>();

            CategoryIndexViewModel categoryIndex = new CategoryIndexViewModel();
            categoryIndex.Load();

            var parentCat = categoryIndex.CategoryViewModels.Where(x => x.CategoryId == categoryId &&
                x.LanguageId == languageId).FirstOrDefault();

            categoryModels.Add(parentCat);

            var childCategories = categoryIndex.CategoryViewModels.Where(x => x.ParentId == categoryId &&
                x.LanguageId == languageId).ToList();


            foreach (var cat in childCategories)
            {
                categoryModels.Add(cat);
                GetChildCategories(categoryModels, cat, categoryIndex);
            }

            return categoryModels;
        }

        private List<ProductViewModel> GetProductsFromCategory(int categoryId, int languageId)
        {
            List<CategoryProduct> categoryProducts = db.CategoryProducts.All()
                .Where(x => x.CategoryId == categoryId).ToList();

            List<ProductViewModel> productModels = new List<ProductViewModel>();
            List<ProductLanguage> productsEntity = db.ProductLanguages.All().Where(x => x.LanguageId == languageId).ToList();

            foreach (var item in categoryProducts)
            {
                foreach (var p in productsEntity)
                {
                    if (item.ProductId == p.ProductId)
                    {
                        ProductViewModel model = new ProductViewModel();

                        model.Description = p.Description;
                        model.Image = p.Product.Image;
                        model.InStock = p.Product.InStock;
                        model.IsTopProduct = p.Product.IsTopProduct;
                        model.LanguageId = p.LanguageId;
                        model.Manufacturer = p.Manufacturer;
                        model.Name = p.Name;
                        model.Price = p.Product.Price;
                        model.ProductCode = p.Product.ProductCode;
                        model.ProductId = p.ProductId;
                        model.Quantity = p.Product.Quantity;
                        model.Size = p.Product.Size;

                        productModels.Add(model);
                    }
                }
            }
            return productModels;
        }

        private void GetChildCategories(List<CategoryViewModel> categoryModels,
            CategoryViewModel category, CategoryIndexViewModel categoryIndex)
        {

            var childCategories = categoryIndex.CategoryViewModels.
                Where(x => x.ParentId == category.CategoryId && x.LanguageId == category.LanguageId).ToList();

            foreach (var cat in childCategories)
            {
                categoryModels.Add(cat);
                GetChildCategories(categoryModels, cat, categoryIndex);
            }
        }

        public void Load(int languageId = 1, int page = 1)
        {
            //loading languages
            var languages = db.Languages.All().ToList();

            this.languageSelectItems = languages.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

            //loading paged products
            this.PagedProductViewModels = db.ProductLanguages.All().Where(x => x.LanguageId == languageId).Select(x => new ProductViewModel()
                {
                    ProductLanguageId = x.Id,
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Manufacturer = x.Manufacturer,
                    LanguageId = x.LanguageId,
                    Description = x.Description,
                    Image = x.Product.Image,
                    InStock = x.Product.InStock,
                    IsTopProduct = x.Product.IsTopProduct,
                    Price = x.Product.Price,
                    ProductCode = x.Product.ProductCode,
                    Quantity = x.Product.Quantity,
                    Size = x.Product.Size,
                    CurrentPage = page,
                }).OrderBy(x => x.ProductId).ToPagedList(page, 5);
            //loading list products
            this.ProductViewModels = db.ProductLanguages.All().Where(x => x.LanguageId == languageId).Select(x => new ProductViewModel()
            {
                ProductLanguageId = x.Id,
                ProductId = x.ProductId,
                Name = x.Name,
                Manufacturer = x.Manufacturer,
                LanguageId = x.LanguageId,
                Description = x.Description,
                Image = x.Product.Image,
                InStock = x.Product.InStock,
                IsTopProduct = x.Product.IsTopProduct,
                Price = x.Product.Price,
                ProductCode = x.Product.ProductCode,
                Quantity = x.Product.Quantity,
                Size = x.Product.Size,
                CurrentPage = page,
            }).OrderBy(x => x.ProductId).ToList();
        }

        private void AddCheckedProductToCategories(ProductViewModel model)
        {
            foreach (var stringId in model.CheckListCategory)
            {
                int checkedCategoryId = int.Parse(stringId);

                Category category = db.Categories.GetById(checkedCategoryId);

                var categoryProducts = db.CategoryProducts.All().Where(x => x.ProductId == model.ProductId).ToList();

                var catProduct = categoryProducts.Where(x => x.CategoryId == category.Id).ToList();

                if (catProduct.Count == 0)
                {
                    db.CategoryProducts.Add(new CategoryProduct()
                    {
                        CategoryId = category.Id,
                        ProductId = model.ProductId,
                    });
                }
            }
            db.SaveChanges();
        }

        private void UpdateCategotyProducts(ProductViewModel model)
        {
            //setting if new checked products is posted to categories
            if (model.CheckListCategory != null)
            {
                AddCheckedProductToCategories(model);
            }

            //finding not checked id
            List<int> uncheckedId = new List<int>();
            uncheckedId = FindNotCheckedCategoryId(model);

            foreach (var id in uncheckedId)
            {
                Category category = db.Categories.GetById(id);
                var categoryProducts = db.CategoryProducts.All().Where(x => x.ProductId == model.ProductId).ToList();

                var catProduct = categoryProducts.Where(x => x.CategoryId == category.Id).FirstOrDefault();

                if (catProduct != null)
                {
                    db.CategoryProducts.Delete(catProduct);
                }
            }
            db.SaveChanges();
        }

        private List<int> FindNotCheckedCategoryId(ProductViewModel model)
        {
            var categories = db.Categories.All().ToList();

            List<int> notCheckedId = new List<int>();

            for (int i = 0; i < categories.Count; i++)
            {
                bool isFound = false;

                if (model.CheckListCategory != null)
                {
                    foreach (var item in model.CheckListCategory)
                    {
                        int checkedCategoryId = int.Parse(item);

                        if (checkedCategoryId == categories[i].Id)
                        {
                            isFound = true;
                            break;
                        }
                    }
                }

                if (!isFound)
                {
                    notCheckedId.Add(categories[i].Id);
                }
            }
            return notCheckedId;
        }

        public void UpdateProduct(ProductViewModel model)
        {

            UpdateCategotyProducts(model);

            Product product = db.Products.GetById(model.ProductId);

            //updating product
            product.Image = model.Image;
            product.InStock = model.InStock;
            product.IsTopProduct = model.IsTopProduct;
            product.Price = model.Price;
            product.ProductCode = model.ProductCode;
            product.Quantity = model.Quantity;
            product.Size = model.Size;


            ProductLanguage productLanguage = product.ProductLanguages.Where(x => x.LanguageId == model.LanguageId).FirstOrDefault();

            //adding new translation
            if (productLanguage == null)
            {
                ProductLanguage entity = new ProductLanguage()
                    {
                        ProductId = model.ProductId,
                        LanguageId = model.LanguageId,
                        Name = model.Name,
                        Description = model.Description,
                        Manufacturer = model.Manufacturer,
                    };

                db.ProductLanguages.Add(entity);
            }
            else//updating translation
            {
                productLanguage.Name = model.Name;
                productLanguage.Manufacturer = model.Manufacturer;
                productLanguage.Description = model.Description;
            }
            db.SaveChanges();
        }

        public int CreateProduct(ProductViewModel model)
        {
            Product product = new Product()
            {
                Image = model.Image,
                InStock = model.InStock,
                IsTopProduct = model.IsTopProduct,
                Price = model.Price,
                ProductCode = model.ProductCode,
                Quantity = model.Quantity,
                Size = model.Size,
            };

            db.Products.Add(product);
            db.SaveChanges();

            ProductLanguage productLanguage = new ProductLanguage()
            {
                ProductId = product.Id,
                LanguageId = model.LanguageId,
                Name = model.Name,
                Description = model.Description,
                Manufacturer = model.Manufacturer,
            };
            db.ProductLanguages.Add(productLanguage);

            //add product to categories
            if (model.CheckListCategory != null)
            {
                foreach (var stringId in model.CheckListCategory)
                {
                    int checkedCategoryId = int.Parse(stringId);

                    Category category = db.Categories.GetById(checkedCategoryId);

                    db.CategoryProducts.Add(new CategoryProduct()
                    {
                        CategoryId = category.Id,
                        ProductId = product.Id,
                    });
                }
            }

            db.SaveChanges();

            return product.Id;
        }
    }
}