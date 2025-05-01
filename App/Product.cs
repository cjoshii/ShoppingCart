public interface IProduct
{
    long Id { get; }
    string Name { get; }
    double Price { get; }
}

public class Product : IProduct
{
    public Product(long id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public long Id { get; }
    public string Name { get; }
    public double Price { get; }
}