public class ShoppingCart
{
    private readonly Dictionary<long, (int, double)> cartItems = new();
    private readonly IInventoryService inventoryService;

    public ShoppingCart(IInventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
    }

    public void AddItem(long productId, int qty, double unitPrice)
    {
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        if (inventoryService.Reserve(productId, qty))
        {
            if (cartItems.ContainsKey(productId))
            {
                cartItems[productId] = (cartItems[productId].Item1 + qty, cartItems[productId].Item2);
            }
            else
            {
                cartItems[productId] = (qty, unitPrice);
            }
        }
        else
        {
            throw new InvalidOperationException("Not enough stock available.");
        }
    }

    public void RemoveItem(long productId, int qty)
    {
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }

        if (cartItems.ContainsKey(productId))
        {
            if (cartItems[productId].Item1 < qty)
            {
                throw new InvalidOperationException("Not enough items in the cart to remove.");
            }

            cartItems[productId] = (cartItems[productId].Item1 - qty, cartItems[productId].Item2);

            if (cartItems[productId].Item1 == 0)
            {
                cartItems.Remove(productId);
            }

            inventoryService.Release(productId, qty);
        }
        else
        {
            throw new KeyNotFoundException("Product not found in the cart.");
        }
    }

    public double GetTotalPrice()
    {
        double total = 0.0;
        foreach (var item in cartItems)
        {
            total += item.Value.Item2 * item.Value.Item1;
        }
        return total;
    }
}