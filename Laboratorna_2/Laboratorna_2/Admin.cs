using Laboratorna_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_2
{
    internal class Admin : User
    {

        public Admin(string userName, string email, string password) : base(userName, email, password)
        {

        }

        public void BlockUser(User user)
        {
            Console.WriteLine($"User {user.UserName} blocked\n");
        }
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Role: Admin");
        }
        public void ModerateContent()
        {
            Console.WriteLine("Content updated\n");
        }

    }
}
