using System;
class Product
{
    private string name;
    private decimal price;
    private int quantityOfProduct;

    public Product(string name, decimal price, int quantityOfProduct) //конструктор
    {
        Name = name;
        Price = price;
        this.quantityOfProduct = quantityOfProduct;
    }

    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва товару не може бути порожньою!");
            name = value;
        }
    }

    public decimal Price
    {
        get => price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Ціна не може бути від'ємною!");
            price = value;
        }
    }

    public int QuantityOfProduct => quantityOfProduct;

    public decimal TotalValue => price * quantityOfProduct;

    public void AddQuantity(int amount) //метод
    {
        if (amount <= 0)
            throw new ArgumentException("Кількість для поповнення має бути більшою за 0!");
        quantityOfProduct += amount;
    }

    public void Sells(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Кількість для продажу має бути більшою за 0!");
        if (amount > quantityOfProduct)
        {
            Console.WriteLine("Недостатньо товару на складі!");
            return;
        }
        quantityOfProduct -= amount;
    }

    public string GetInfo()
    {
        return $"Товар: {Name}, Ціна: {Price} грн, Кількість: {quantityOfProduct}, Загальна вартість: {TotalValue} грн";
    }
}


