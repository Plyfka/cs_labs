using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_2
{
    internal class User
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        private string _password { get; set; }

        public User(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            _password = password;
        }
        public void SetPassword(string currentPassword, string currentEmail, string newPassword)
        {
            if (Authenticate(currentPassword, currentEmail))
            {
                _password = newPassword;
                Console.WriteLine("Password changed successfully.\n");
            }
            else
            {
                Console.WriteLine("Password change failed. Authentication required.\n");
            }
        }
        public bool Authenticate(string inputPassword, string inputEmail)
        {
            if (inputPassword == _password && inputEmail == Email)
            {
                return true;
            }
            return false;
        }
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Name: {UserName}, Email: {Email}");
        }
        public void PostComment()
        {
            Console.WriteLine("Comment uploaded\n");
        }
    }
}
