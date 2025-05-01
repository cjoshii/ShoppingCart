var inventoryService = new InMemoryInventoryService();
var shoppingCart = new ShoppingCart(inventoryService);

shoppingCart.AddItem(1, 2, 1.99);
shoppingCart.AddItem(2, 3, 19.98);
shoppingCart.RemoveItem(1, 1);
shoppingCart.RemoveItem(2, 1);

double totalPrice = shoppingCart.GetTotalPrice();

Console.WriteLine($"Total Price: {totalPrice}");