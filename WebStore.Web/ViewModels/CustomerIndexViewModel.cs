using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using WebStore.Infrastructure.Repositories;
using PagedList;
using Microsoft.AspNet.Identity;
using WebStore.Infrastructure;
using System.Text;
using System.Security.Cryptography;

namespace WebStore.Web.ViewModels
{
    public class CustomerIndexViewModel
    {
        private IUowData db;

        public IPagedList<CustomerViewModel> CustomerViewModels { get; set; }

        public IList<CustomerViewModel> CustomerViewModelsList { get; set; }

        private byte[] tmpSource;
        private byte[] tmpHash;


        public CustomerIndexViewModel(IUowData data)
        {
            this.db = data;
        }
        public CustomerIndexViewModel()
            : this(new UowData())
        { }

        public void Load(int page = 1)
        {
            this.CustomerViewModels = db.Customers.All().Select(x =>
                new CustomerViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    City = x.City,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    Phone = x.Phone,
                    Username = x.Username,
                    Password = x.Password,
                }).OrderBy(x => x.FirstName).ToPagedList(page, 5);

            this.CustomerViewModelsList = db.Customers.All().Select(x =>
                new CustomerViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Address = x.Address,
                    City = x.City,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    Phone = x.Phone,
                    Username = x.Username,
                    Password = x.Password,
                }).OrderBy(x => x.FirstName).ToList();
        }

        public CustomerViewModel FindCustomerByNameAndPass(string name, string password)
        {
            using (DataContext data = new DataContext())
            {
                CustomerRepository customerRepo = new CustomerRepository(data);

                Customer entity = customerRepo.GetByNameAndPassword(name, password);

                CustomerViewModel model = new CustomerViewModel();

                if (entity != null && entity.IsEnabled != false)
                {
                    
                       model.Address = entity.Address;
                       model.City = entity.City;
                       model.Email = entity.Email;
                       model.FirstName = entity.FirstName;
                       model.Id = entity.Id;
                       model.IsEnabled = entity.IsEnabled;
                       model.LastName = entity.LastName;
                       model.Password = entity.Password;
                       model.Phone = entity.Phone;
                       model.Username = entity.Username;

                       return model;
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdateCustomer(CustomerViewModel model)
        {
            Customer customer = db.Customers.GetById(model.Id);


            customer.Password = HashPassword(model.Password);
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.Email = model.Email;
            customer.IsEnabled = model.IsEnabled;
            customer.Phone = model.Phone;
            customer.Username = model.Username;
            db.Customers.Update(customer);
            db.SaveChanges();
        }

        public int CreateCustomer(CustomerViewModel model)
        {
            Customer customer = new Customer();



            customer.Password = HashPassword(model.Password);
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.Email = model.Email;
            customer.IsEnabled = true;
            customer.Phone = model.Phone;
            customer.Username = model.Username;

            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.Id;
        }

        private string HashPassword(string password)
        {
            tmpSource = ASCIIEncoding.ASCII.GetBytes(password);
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            string pass = ByteArrayToString(tmpHash);

            return pass;
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}


