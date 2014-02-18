using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Core.Interfaces;
using WebStore.Core.Models;
using System.Security.Cryptography;

namespace WebStore.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private byte[] tmpSource;
        private byte[] tmpHash;

        public CustomerRepository(DataContext data):base(data)
        {

        }
        public Customer GetByNameAndPassword(string name, string password)
        {

            string hashedPassword = HashPassword(password);
            Customer custumer = this.DbSet.Where(x => x.Username == name && x.Password == hashedPassword).FirstOrDefault();
            return custumer;
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
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (int i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
