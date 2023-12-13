using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, Product> products = new Dictionary<string, Product>();
    static Dictionary<string, int> cart = new Dictionary<string, int>();
    static Dictionary<int, string> productCategories = new Dictionary<int, string>
    {
        { 1, "Produce" },
        { 2, "Meat" },
        { 3, "Frozen Foods "},
        { 4, "Bakery" },
        { 5, "Snacks" },
        { 6, "Beverages" },
        { 7, "Alchohol" },
        { 8, "Health & Beauty" },
        { 9, "Pet Supplies" },
    };
    static double cartTotal = 0.0;

    public delegate void NotificationHandler(string message);
    public static event NotificationHandler Notify;

    static void Main(string[] args)
    {
        Notify += DisplayNotification;

        InitializeProducts();
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to the Store\n");
            Console.ResetColor();

            Console.WriteLine("\nSelect a category number to view products, or type 'X' to exit:\n");
            foreach (var category in productCategories)
            {
                Console.WriteLine($"Category {category.Key}: {category.Value}");
            }
            Console.WriteLine("\n----------------------------------------\n");

            string categoryInput = Console.ReadLine().ToUpper();
            Console.Clear();
            if (categoryInput == "x") break;

            if (int.TryParse(categoryInput, out int categoryNumber) && productCategories.ContainsKey(categoryNumber))
            {
                DisplayProductsInCategory(categoryNumber);

                Console.WriteLine("\nEnter product code to add to cart, or 'BACK' to go back to categories:\n");
                while (true)
                {
                    string productInput = Console.ReadLine().ToUpper();
                    if (productInput == "BACK") break;
                    if (productInput == "X") { exit = true; break; }

                    if (products.ContainsKey(productInput) && products[productInput].Category == categoryNumber)
                    {
                        AddToCart(productInput);
                        DisplayCart();
                    }
                    else
                    {
                        Console.WriteLine("Invalid product code. Please enter a valid code, or 'BACK' to go back to the categories");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid category number.");
            }
        }
        PrintRecipt();
    }

    static void DisplayProductsInCategory(int categoryNumber)
    {
        var productsInCategory = products
            .Where(p => p.Value.Category == categoryNumber)
            .OrderBy(p => p.Key) // Optionally order by product code
            .Select(p => $"{p.Key}: {p.Value.Name} - {p.Value.Price:C}");

        Console.WriteLine($"\nProducts in {productCategories[categoryNumber]} Category:");
        foreach (var product in productsInCategory)
        {
            Console.WriteLine(product);
        }
        Console.WriteLine("----------------------------------------");
    }
    static void InitializeProducts()
    {
        //1. Produce
        products.Add("1A", new Product("Apple", 0.50, 1, false, 1, false, 0, 0, 0));
        products.Add("1B", new Product("Pack of Apples", 2.50, 1, true, 6, false, 0, 0, 0));
        products.Add("1C", new Product("Banana", 0.30, 1, false, 1, false, 0, 0, 0));
        products.Add("1D", new Product("Pineapple", 3.00, 1, false, 1, false, 0, 0, 0));
        products.Add("1E", new Product("Grapes", 2.50, 1, false, 1, false, 0, 0, 0));

        //2. Meat
        products.Add("2A", new Product("Chicken Breast - 500g", 4.50, 2, false, 1, false, 0, 0, 0));
        products.Add("2B", new Product("Salmon", 6.60, 2, false, 2, false, 0, 0, 0));
        products.Add("2C", new Product("Ground Beef - 400g", 4.30, 2, false, 1, false, 0, 0, 0));
        products.Add("2D", new Product("Brisket", 8.20, 2, false, 1, false, 0, 0, 0));
        products.Add("2E", new Product("Clams", 14.50, 2, false, 1, false, 0, 0, 0));

        //3. Frozen Foods
        products.Add("3A", new Product("Ramen", 2.05, 3, false, 1, false, 0, 0, 0));
        products.Add("3B", new Product("Chicken Nuggets", 2.30, 3, false, 1, false, 0, 0, 0));
        products.Add("3C", new Product("Garlic Bread", 1.30, 3, false, 1, true, 4, 3, 0));
        products.Add("3D", new Product("Vanilla Ice Cream", 3.75, 3, false, 1, false, 0, 0, 0));
        products.Add("3E", new Product("Kebab Pizza", 4.60, 3, false, 1, false, 0, 0, 0));

        //4. Bakery
        products.Add("4A", new Product("Baguette", 1.00, 4, false, 1, false, 0, 0, 0));
        products.Add("4B", new Product("Baguette Bag - 3", 2.90, 4, true, 3, false, 0, 0, 0));
        products.Add("4C", new Product("Garlic Bread", 1.30, 4, false, 1, true, 4, 3, 0));
        products.Add("4D", new Product("Vanilla Ice Cream", 3.75, 4, false, 1, false, 0, 0, 0));
        products.Add("4E", new Product("Kebab Pizza", 4.60, 4, false, 1, false, 0, 0, 0));

        //5. Snacks
        products.Add("5A", new Product("Doritos", 2.50, 5, false, 1, true, 2, 1, 0));
        products.Add("5B", new Product("Pringles", 3.00, 5, false, 1, false, 0, 0, 0));
        products.Add("5C", new Product("Haribo - Click Mix", 2.30, 5, false, 1, false, 0, 0, 0));
        products.Add("5D", new Product("Snickers", 1.75, 5, false, 1, false, 0, 0, 0));
        products.Add("5E", new Product("Diz Nuts", 4.20, 5, false, 1, false, 0, 0, 0));

        //6. Beverages
        products.Add("6A", new Product("Coca Cola - 330ml", 1.00, 6, false, 1, false, 0, 0, 0.5));
        products.Add("6B", new Product("Pepsi Max - 330ml", 1.00, 6, false, 1, false, 0, 0, 0.5));
        products.Add("6C", new Product("Mountain Dew - 1.5L", 3.20, 6, false, 1, false, 0, 0, 0.8));
        products.Add("6D", new Product("Fanta - 1.5L", 3.10, 6, false, 1, false, 0, 0, 0.8));
        products.Add("6E", new Product("Blå Bærbrus - 5L", 5.00, 6, false, 1, false, 0, 0, 0.8));

        //7. Alchohol
        products.Add("7A", new Product("Captain Morgan", 10.00, 7, false, 1, true, 3, 2, 0));
        products.Add("7B", new Product("Vodka", 11.00, 7, false, 1, false, 0, 0, 0));
        products.Add("7C", new Product("Tuborg Classic - 330ml", 1.30, 7, false, 1, false, 0, 0, 0.5));
        products.Add("7D", new Product("Tuborg Classic - 6 Pack", 1.30, 7, true, 6, false, 0, 0, 0.5));
        products.Add("7E", new Product("Gordons Gin", 6.90, 7, false, 1, false, 0, 0, 0));

        //8. Health & Beauty
        products.Add("8A", new Product("Colgate", 2.90, 8, false, 1, false, 0, 0, 0));
        products.Add("8B", new Product("Face Lotion", 6.00, 8, false, 1, false, 0, 0, 0));
        products.Add("8C", new Product("Toilet Paper", 2.30, 8, false, 1, false, 0, 0, 0));
        products.Add("8D", new Product("Tooth Brush", 1.45, 8, false, 1, false, 0, 0, 0));
        products.Add("8E", new Product("Hair Spray", 2.00, 8, false, 1, false, 0, 0, 0));

        //9. Pet Supplies
        products.Add("9A", new Product("Dog", 15.20, 9, false, 1, false, 0, 0, 0));
        products.Add("9B", new Product("Cat Food", 13.00, 9, false, 1, false, 0, 0, 0));
        products.Add("9C", new Product("Sqirrel Food", 5.30, 9, false, 1, false, 0, 0, 0));
        products.Add("9D", new Product("Bear Food", 69.75, 9, false, 1, false, 0, 0, 0));
        products.Add("9E", new Product("Fish Food", 2.20, 9, false, 1, false, 0, 0, 0));

    }

    static void AddToCart(string productCode)
    {
        if (!cart.ContainsKey(productCode))
        {
            cart[productCode] = 0;
        }
        cart[productCode]++;

        var product = products[productCode];
        if (product.IsCampaignItem)
        {
            Notify?.Invoke($"Special offer applies to {product.Name}!");
        }
        else if (product.RecyclingFee > 0)
        {
            Notify?.Invoke($"{product.Name} Includes a recycling fee.");
        }
    }

    static void DisplayNotification(string message)
    {
        Console.WriteLine($"Notification: {message}");
    }

    static double CalculateTotal()
    {
        double total = 0.0;
        foreach (var item in cart)
        {
            var product = products[item.Key];
            total += CalculateItemTotal(product, item.Value);
        }
        return total;
    }

    static double CalculateItemTotal(Product product, int quantity)
    {
        double total = 0.0;
        if (product.IsCampaignItem && quantity >= product.CampaignThreshold)
        {
            int discountQuantity = quantity / product.CampaignThreshold * (product.CampaignThreshold - product.CampaignPrice);
            total = (quantity - discountQuantity) * (product.Price + product.RecyclingFee);
        }
        else
        {
            total = quantity * (product.Price + product.RecyclingFee);
        }
        return total;
    }

    static void DisplayCart()
    {
        Console.WriteLine("\nCurrent Cart Contents:");
        Console.WriteLine("----------------------------------------");

        foreach (var item in cart)
        {
            var product = products[item.Key];
            double itemTotal = CalculateItemTotal(product, item.Value);

            string itemDetails = $"{item.Value}x {product.Name} @ {product.Price:C} each";
            if (product.RecyclingFee > 0)
            {
                itemDetails += $" + {product.RecyclingFee:C} recycling fee";

            }
            if (product.IsCampaignItem && item.Value >= product.CampaignThreshold)
            {
                itemDetails += $" (Campaign Applied)";
            }

            Console.WriteLine($"{itemDetails} - Total: {itemTotal:C}");
        }

        double newTotal = CalculateTotal();
        Console.WriteLine($"Current Cart Total: {newTotal:C}");
        Console.WriteLine("----------------------------------------");

    }

    static void PrintRecipt()
    {
        Console.WriteLine("\nFinal Receipt");
        Console.WriteLine("----------------------------------------");

        var groupedItems = cart
            .GroupBy(item => products[item.Key].Category)
            .OrderBy(group => group.Key);

        foreach (var group in groupedItems)
        {
            string categoryName = productCategories[group.Key];
            Console.WriteLine($"\nCategory: {categoryName}");
            Console.WriteLine("----------------------------------------");

            foreach (var item in group)
            {
                var product = products[item.Key];
                double itemTotal = CalculateItemTotal(product, item.Value);
                Console.WriteLine($"{product.Name} - {item.Value} x {product.Price:C} = {itemTotal:C}");
            }
        }

        double totalCost = CalculateTotal();
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Total Cost: {totalCost:C}");
    }

}

class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Category { get; set; }
    public bool IsMultiItemPack { get; set; }
    public int Quantity { get; set; }
    public bool IsCampaignItem { get; set; }
    public int CampaignThreshold { get; set; }
    public int CampaignPrice { get; set; }
    public double RecyclingFee { get; set; }

    public Product(string name, double price, int category, bool isMultiItemPack, int quantity, bool isCampaignItem, int campaignThreshold, int campaignPrice, double recyclingFee)
    {
        Name = name;
        Price = price;
        Category = category;
        IsMultiItemPack = isMultiItemPack;
        Quantity = quantity;
        IsCampaignItem = isCampaignItem;
        CampaignThreshold = campaignThreshold;
        CampaignPrice = campaignPrice;
        RecyclingFee = recyclingFee;
    }
}
