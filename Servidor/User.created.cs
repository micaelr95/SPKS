using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    partial class User
    {
        public User()
        {
        }

        public User(string username, string password)
        {
            Username = username;
            Salt = Common.GenerateSalt();
            Password = Common.HashPassword(password, Salt);
        }
    }
}
