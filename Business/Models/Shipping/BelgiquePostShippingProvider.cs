using Factory.Business.Models.Commerce;
using System;

namespace Factory.Business.Models.Shipping
{
  public class BelgiquePostShippingProvider : ShippingProvider
  {
    private readonly string clientId;
    private readonly string secret;

    public BelgiquePostShippingProvider(
        string clientId,
        string secret,
        ShippingCostCalculator shippingCostCalculator,
        CustomsHandlingOptions customsHandlingOptions,
        InsuranceOptions insuranceOptions)
    {
      this.clientId = clientId;
      this.secret = secret;

      ShippingCostCalculator = shippingCostCalculator;
      CustomsHandlingOptions = customsHandlingOptions;
      InsuranceOptions = insuranceOptions;
    }

    public override string GenerateShippingLabelFor(Order order)
    {
      var shippingId = GetShippingId();

      if (order.Recipient.Country != order.Sender.Country)
      {
        throw new NotSupportedException("Livraison internationale non implémentée");
      }

      var shippingCost = ShippingCostCalculator.CalculateFor(order.Recipient.Country,
          order.Sender.Country,
          order.TotalWeight);

      return $"Id livraison : {shippingId} {Environment.NewLine}" +
              $"Dest : {order.Recipient.To} {Environment.NewLine}" +
              $"Total : {order.Total} {Environment.NewLine}" +
              $"Taxes : {CustomsHandlingOptions.TaxOptions} {Environment.NewLine}" +
              $"Frais livraison : {shippingCost}";
    }

    private string GetShippingId()
    {
      // Invocation service avec credentials...

      return $"BEL-{Guid.NewGuid()}";
    }
  }
}
