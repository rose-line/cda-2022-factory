using Factory.Business.Models.Commerce;
using System;

namespace Factory.Business.Models.Shipping
{
  public class LuxembourgPostalServiceShippingProvider : ShippingProvider
  {
    private readonly string apiKey;

    public LuxembourgPostalServiceShippingProvider(
        string apiKey,
        ShippingCostCalculator shippingCostCalculator,
        CustomsHandlingOptions customsHandlingOptions,
        InsuranceOptions insuranceOptions)
    {
      this.apiKey = apiKey;

      ShippingCostCalculator = shippingCostCalculator;
      CustomsHandlingOptions = customsHandlingOptions;
      InsuranceOptions = insuranceOptions;
    }

    public override string GenerateShippingLabelFor(Order order)
    {
      var shippingId = GetShippingId();

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
		// Invocation API avec l'API Key...

      return $"LUX-{Guid.NewGuid()}";
    }
  }
}
