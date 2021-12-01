using Factory.Business.Models.Commerce;
using Factory.Business.Models.Shipping;
using System;

namespace Factory.Business
{
  public class ShoppingCart
  {
    private readonly Order order;

    public ShoppingCart(Order order)
    {
      this.order = order;
    }

    public string Finalize()
    {
      ShippingProvider shippingProvider;

      if (order.Sender.Country == "Belgique")
      {
        var shippingCostCalculator = new ShippingCostCalculator(
            internationalShippingFee: 250,
            extraWeightFee: 500
        )
        {
          ShippingType = ShippingType.Standard
        };

        var customsHandlingOptions = new CustomsHandlingOptions
        {
          TaxOptions = TaxOptions.PrePaid
        };

        var insuranceOptions = new InsuranceOptions
        {
          ProviderHasInsurance = false,
          ProviderHasExtendedInsurance = false,
          ProviderRequiresReturnOnDamange = false
        };

        shippingProvider = new BelgiquePostShippingProvider("CLIENT_ID",
            "SECRET",
            shippingCostCalculator,
            customsHandlingOptions,
            insuranceOptions);
      }
      else if (order.Sender.Country == "Luxembourg")
      {
        var shippingCostCalculator = new ShippingCostCalculator(
            internationalShippingFee: 50,
            extraWeightFee: 100
        )
        {
          ShippingType = ShippingType.Express
        };

        var customsHandlingOptions = new CustomsHandlingOptions
        {
          TaxOptions = TaxOptions.PayOnArrival
        };

        var insuranceOptions = new InsuranceOptions
        {
          ProviderHasInsurance = true,
          ProviderHasExtendedInsurance = false,
          ProviderRequiresReturnOnDamange = false
        };

        shippingProvider = new LuxembourgPostalServiceShippingProvider("API_KEY",
            shippingCostCalculator,
            customsHandlingOptions,
            insuranceOptions);
      }
      else
      {
        throw new NotSupportedException("Pas de prestataire de livraison pour le pays d'expédition");
      }

      order.ShippingStatus = ShippingStatus.ReadyForShippment;

      return shippingProvider.GenerateShippingLabelFor(order);
    }
  }
}
