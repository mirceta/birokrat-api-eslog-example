using BiroWoocommerceHub.logic.eslog_gen;
using BiroWoocommerceHub.structs_wc_to_biro;
using System.Collections.Generic;

namespace BironextWordpressIntegrationHub
{

    public class EslogGen
    {
        public static string CreateXML(Order order, List<BirokratPostavka> postavke) {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<?xml-stylesheet type='text/xsl' href='http://vizualiziraj.si/eInvoiceVizualization_20110530.xslt'?>
<IzdaniRacunEnostavni xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"" xmlns:xds=""http://uri.etsi.org/01903/v1.1.1#"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""http://www.gzs.si/e-poslovanje/sheme/eSlog_1-6_EnostavniRacun.xsd"">
    <Racun Id=""data"">
        {Preamble.Get(order.DateCreated,
            order.Number, 
            order.Id,
            order.Billing.City)}
        {PartnerDetails.Preamble()}
        {PartnerDetails.Billing(order.Billing)}
        {PartnerDetails.Shipping(order.Shipping)}
        {InvoiceSpecification.Get(postavke)}
        {Postamble.Get()}
    </Racun>
</IzdaniRacunEnostavni>
            ";
        }
    }

    public class Order {
        public string DateCreated;
        public string Number;
        public int Id;
        public Billing Billing;
        public Shipping Shipping;
    }
}
