using System;
namespace Basket.API.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } 
    public decimal TotalPrice {
        get
        {
            if (Items == null)
            {
                return 0;
            }
            decimal totalPrice = 0;
            foreach (ShoppingCartItem? item in Items)
            {
                totalPrice += item.Quantity * item.Price;
            }
            return totalPrice;


        }
    }

    public ShoppingCart()
    {

    }

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }


}

