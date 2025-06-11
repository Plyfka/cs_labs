using Laboratorna_2;
using System;
using System.Collections.Generic;

namespace Laboratorna_2
{
    internal class Program
    {
        static void Main()
        {
            bool isExit = false;
            bool isLog = false;

            User administrator = new Admin("Stepan", "styopa121@gmail.com", "admin222");
            User moder = new Moderator("Alex", "alex@gmail.com", "molot23");
            User person1 = new RegularUser("Dog", "dogeat@gmail.com", "12345");
            User person2 = new RegularUser("Egor", "egorist@gmail.com", "1234554");
            User person3 = new RegularUser("Alibaba", "alik@gmail.com", "aloali");

            List<User> users = new List<User>
            {
                administrator,
                person3,
                person2,
                person1,
                moder
            };

            foreach (var user in users)
            {
                user.DisplayInfo();
            }

            while (!isExit)
            {
                Console.Write("Enter Email: ");
                string inputEmail = Console.ReadLine();
                Console.Write("Enter password: ");
                string inputPassword = Console.ReadLine();
                isLog = true;

                bool authenticated = false;
                foreach (var user in users)
                {
                    if (user.Authenticate(inputPassword, inputEmail))
                    {
                        Console.WriteLine("Correct authentication\n");
                        authenticated = true;

                        while (isLog)
                        {
                            Console.WriteLine(
                                "User menu:\n1.myInfo - see your information;\n2.changePassword - change your password;" +
                                "\n3.ban(only admin) - ban someone;\n4.changePost(only for admin and moderator) - change post;" +
                                "\n5.doPost - write something on comments;\n6.logOut - exit from account;\n7.exit - finish the program;");

                            string inputCommand = Console.ReadLine();

                            switch (inputCommand)
                            {
                                case "ban":
                                    if (user is Admin admin)
                                    {
                                        admin.BlockUser(person1);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Command 'ban' accessible only to the administrator.\n");
                                    }
                                    break;

                                case "doPost":
                                    if (user is Admin adminPost)
                                    {
                                        adminPost.PostComment();
                                    }
                                    else if (user is Moderator modPost)
                                    {
                                        modPost.PostComment();
                                    }
                                    else if (user is RegularUser regPost)
                                    {
                                        regPost.PostComment();
                                    }
                                    break;

                                case "changePost":
                                    if (user is Admin adminChange)
                                    {
                                        adminChange.ModerateContent();
                                    }
                                    else if (user is Moderator modChange)
                                    {
                                        modChange.ModerateContent();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Команда 'changePost' accessible only to the administrator and moderator.\n");
                                    }
                                    break;

                                case "myInfo":
                                    user.DisplayInfo();
                                    break;

                                case "changePassword":
                                    Console.Write("Enter new password: ");
                                    string inputNewPass = Console.ReadLine();
                                    user.SetPassword(inputPassword, inputEmail, inputNewPass);
                                    break;

                                case "logOut":
                                    Console.WriteLine("LogOut completed!\n");
                                    isLog = false;
                                    break;

                                case "exit":
                                    Console.WriteLine("Exit completed!\n");
                                    isExit = true;
                                    isLog = false;
                                    break;

                                default:
                                    Console.WriteLine("Unknown command.\n");
                                    break;
                            }

                            if (!isLog || isExit)
                                break;
                        }

                        if (!isLog)
                            break;
                    }
                }

                if (!authenticated)
                {
                    Console.WriteLine("invalid data; authentication failed\n");
                }
            }
        }
    }
}
