using System.Collections.Concurrent;

public interface IInventoryService
{
    bool Reserve(long productId, int qty);
    bool Release(long productId, int qty);
    int GetStock(long productId);
}

public class InMemoryInventoryService : IInventoryService
{

    private readonly ConcurrentDictionary<long, Lock> productLocks;
    private readonly ConcurrentDictionary<long, int> products;

    private Lock GetProductLock(long productId)
    {
        return productLocks.GetOrAdd(productId, _ => new Lock());
    }

    public InMemoryInventoryService()
    {
        productLocks = new ConcurrentDictionary<long, Lock>();
        products = new ConcurrentDictionary<long, int>();
    }

    public int GetStock(long productId)
    {
        if (products.TryGetValue(productId, out var stock))
        {
            return stock;
        }
        return 0;
    }

    public bool Release(long productId, int qty)
    {
        if (qty <= 0)
        {
            return false;
        }

        var productLock = GetProductLock(productId);
        lock (productLock)
        {
            if (products.TryGetValue(productId, out var stock))
            {
                products[productId] = stock + qty;
                return true;
            }
            else
            {
                products[productId] = qty;
                return true;
            }
        }
    }

    public bool Reserve(long productId, int qty)
    {
        if (qty <= 0)
        {
            return false;
        }

        var productLock = GetProductLock(productId);
        lock (productLock)
        {
            if (products.TryGetValue(productId, out var stock))
            {
                if (stock >= qty)
                {
                    products[productId] = stock - qty;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}