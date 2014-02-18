using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Models;

namespace WebStore.Infrastructure
{
    public class InitializeDatabase : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            

            //creating role admin
            string role = "admin";

            roleManager.Create(new IdentityRole(role));

            string userName = "Admin";
            string password = "123456";

            //creating user
            ApplicationUser user = new ApplicationUser()
            {
                UserName = userName
            };

            var resultUser = userManager.Create(user, password);

            if (resultUser.Succeeded)
            {
                userManager.AddToRole(user.Id, role);
            }

            //add customer
            ApplicationUser customer = new ApplicationUser()
            {
                UserName = "Stoian",
            };
            userManager.Create(customer, password);
            context.SaveChanges();

            //Add Categories, Languages and Categorylanguages
            context.CategoryLanguages.Add(new CategoryLanguage()
                {
                    Category = new Category()
                    {
                        CreatedOn = DateTime.Now,
                        
                    },
                    Language = new Language()
                    {
                        Name = "EN",
                    },
                    Title = "Man shoes",
                   
                });
            context.CategoryLanguages.Add(new CategoryLanguage()
            {
                Category = new Category()
                {
                    CreatedOn = DateTime.Now
                },
                Language = new Language()
                {
                    Name = "BG",
                },
                Title = "Женски обувки"
            });

            context.SaveChanges();

            //Add sub categories
            context.CategoryLanguages.Add(new CategoryLanguage()
                {
                    Category = new Category()
                    {
                        CreatedOn = DateTime.Now,
                        ParentId = 1,
                    },
                    LanguageID = 1,
                    Title = "Sports shoes"
                });

            context.CategoryLanguages.Add(new CategoryLanguage()
            {
                Category = new Category()
                {
                    CreatedOn = DateTime.Now,
                    ParentId = 2,
                },
                LanguageID = 2,
                Title = "Елегантни обувки"
            });
            context.SaveChanges();


            

            //Add ShoesLanguage
            context.ProductLanguages.Add(new ProductLanguage()
                {
                    Name = "Elegant shoes",
                    Manufacturer = "D&G",
                    LanguageId = 1,
                    Product = new Product()
                    {
                        InStock=true,
                        Price = 50,
                        Size = 38,
                        ProductCode = "2AAA",
                        Image = "http://obuvalka.com/image/cache/data/march/777-36-black-228x171.jpg",
                        Quantity = 20,
                    },
                });

            context.ProductLanguages.Add(new ProductLanguage()
                {
                    Name = "Elegant shoes",
                    Manufacturer = "D&G",
                    LanguageId = 1,
                    Product = new Product()
                    {
                        InStock=true,
                        Price = 70,
                        Size = 39,
                        ProductCode = "2A5A",
                        Image = "http://obuvalka.com/image/cache/data/march/777-36-black-228x171.jpg",
                        Quantity = 200,
                    },
                    
                });

            context.SaveChanges();


            //Add Orders
            context.OrderDetails.Add(new OrderDetail()
                {
                    Order = new Order()
                        {
                            Address = "Milano #2",
                            Email = "myemail@abv.bg",
                            IsProcessed = false,
                            OrderDate = DateTime.Now,
                            Phone = "984454598",
                            TotalPrice = 51,
                            City="Sofia",
                            FirstName="Pesho",
                            LastName="Petrov",
                        },
                    Quantity = 2,
                    ProductId =2,
                });

            context.OrderDetails.Add(new OrderDetail()
            {
                Order = new Order()
                {
                    Address = "Roma #1",
                    Email = "myemail@abv.bg",
                    IsProcessed = false,
                    OrderDate = DateTime.Now,
                    Phone = "12344545",
                    TotalPrice = 60,
                    City = "Sofia",
                    FirstName = "Gesho",
                    LastName = "Getrov",
                },
                Quantity = 1,
                ProductId = 1,
            });
        }

    }
}
