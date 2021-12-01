using Factory.Business;
using Factory.Business.Models.Commerce;
using System;

namespace Factory
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Write("Pays destination : ");
      var recipientCountry = Console.ReadLine().Trim();

      Console.Write("Pays expéditeur : ");
      var senderCountry = Console.ReadLine().Trim();

      Console.Write("Poids total : ");
      var totalWeight = Convert.ToInt32(Console.ReadLine().Trim());

      var order = new Order
      {
        Recipient = new Address
        {
          To = "Roger Cageot",
          Country = recipientCountry
        },

        Sender = new Address
        {
          To = "Dave Murray",
          Country = senderCountry
        },

        TotalWeight = totalWeight
      };

      order.LineItems.Add(new Item("01234", "C# dans ta face", 100m), 1);
      order.LineItems.Add(new Item("56780", "Consulting Cybersécurité", 100m), 1);

      var cart = new ShoppingCart(order);

      var shippingLabel = cart.Finalize();

      Console.WriteLine(shippingLabel);
    }
  }
}
