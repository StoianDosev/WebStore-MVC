using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure;
using WebStore.Infrastructure.Repositories;

namespace WebStore.Web.ViewModels
{
    public class AdminIndexViewModel
    {
        public ICollection<AdminViewModel> AdminViewModels { get; set; }

        private IUowData db;

        public AdminIndexViewModel(IUowData data)
        {
            this.db = data;
        }
        public AdminIndexViewModel()
            : this(new UowData())
        { }

        public void Load()
        {
            this.AdminViewModels = db.Admins.All().Select(x => new AdminViewModel()
                {
                    Id = x.Id,
                    HashPasword = x.PasswordHash,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    IsEnabled = x.IsEnabled,
                    Email = x.Email,
                }).ToList();
        }

        public void CreateAdmin(AdminViewModel model)
        {
            
            ApplicationUser admin = new ApplicationUser();
            var hasher = new PasswordHasher();
            string password = hasher.HashPassword(model.HashPasword);
            admin.PasswordHash = password;
            admin.IsEnabled = model.IsEnabled;
            admin.UserName = model.UserName;
            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.Email = model.Email;
            

            db.Admins.Add(admin);
            db.SaveChanges();


        }

        public bool FindAdminIsEnabled(string id, string password)
        {
            using (DataContext data = new DataContext())
            {
                AdminRepository adminRepo = new AdminRepository(data);

                ApplicationUser admin = adminRepo.GetById(id);

                if(admin.IsEnabled==false)
                {
                    return false;
                }
                else
                {
                    return true;
                         
                }
                
            }
        }

        public void UpdateAdmin(AdminViewModel model)
        {
            using(  DataContext data = new DataContext())
            {
                AdminRepository adminRepo = new AdminRepository(data);

                ApplicationUser admin= adminRepo.GetById(model.Id);
                
                admin.IsEnabled = model.IsEnabled;
                admin.UserName = model.UserName;
                admin.FirstName = model.FirstName;
                admin.LastName = model.LastName;
                admin.Email = model.Email;

                adminRepo.Update(admin);
                data.SaveChanges();
            }
            
        }
    }
}