using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;

        Product apple = new Product("Яблуко", 5, 100);
        Console.WriteLine(apple.GetInfo());

        apple.Sells(20);
        Console.WriteLine(apple.GetInfo());

        apple.AddQuantity(50);
        Console.WriteLine(apple.GetInfo());

        apple.Price = 7;
        Console.WriteLine(apple.GetInfo());

        apple.Name = "Зелене яблуко";
        Console.WriteLine(apple.GetInfo());

        try
        {
            apple.Price = -10;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        try
        {
            apple.Name = "";
        }
        catch (Exception er)
        {
            Console.WriteLine(er.Message);
        }

        apple.Sells(200);
    }
}