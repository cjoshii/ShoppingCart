public interface IPricingService
{
    double CalculatePrice(long productId, int qty, double unitPrice);
}

public interface IDiscountService
{
    double ApplyDiscount(double totalPrice);
}

public interface ITaxService
{
    double ApplyTax();
}

public class PricingService : IPricingService
{
    public PricingService(ITaxService taxService, IDiscountService discountService)
    {
        this.taxService = taxService;
        this.discountService = discountService;
    }
    private readonly ITaxService taxService;
    private readonly IDiscountService discountService;

    public double CalculatePrice(long productId, int qty, double unitPrice)
    {
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }


        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be greater than zero.");
        }

        double totalPrice = qty * unitPrice;

        totalPrice -= discountService.ApplyDiscount(totalPrice);
        totalPrice += taxService.ApplyTax();
        return totalPrice;
    }
}