using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Web.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public bool IsEnabled { get; set; }

        public string Password { get; set; }
    }
}