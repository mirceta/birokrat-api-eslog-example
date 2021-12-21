using BirokratNext;
using BironextWordpressIntegrationHub;
using BiroWoocommerceHub.structs_wc_to_biro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EslogExample
{

    /*
     api stran: https://birokratapiv1.docs.apiary.io/
     */

    class Program
    {
        static async Task Main(string[] args) {

            // spremeni apikey in path_to_save_invoice_pdf
            string api_address = "https://next.birokrat.si/api/";
            string path_to_save_invoice_pdf = @"C:\Users\km\desktop\some.pdf";
            string apikey = "SO3onPC7AhmrSgI54J6uNDKEfYmFpJlk+Ze7nskPQQw=";
            string zakljucno_besedilo = "Hvala za nakup!";

            Order order = new Order() { // tole je bilo misljeno za Woocommerce narocilo zato je tak format, ima pa vse relevantne podatke v strukturi notri.

                /*
                 Number in Id bi morala delat tako, da je Stevilka racuna v Birokratu potem $"{Number}-{Id}". Ampak ko sprobam v bistvu ne dela tako - vbistvu je Number
                 nepomemben, Stevilka racuna v birokratu pa postane $"{naslednja stevilka od prejsnjega racuna}-{Id}".
                 */
                Number = "158", 
                Id = 8559, 


                DateCreated = "2021-04-30 10:54:50.000000",
                Billing = new Billing() {
                    // podatki od nekega zapisa v SifrantuPartnerjev - Trenutno je nujno, da je partner ze v sifrantu partnerjev. Zelo verjetno to v prihodnosti ne bo vec pogoj - prosim ne sprasujte kdaj ker ne vem in ni odvisno od mene.
                    // Mislim, da dela tako: izbere prvi zapis v tabeli Partner, kjer je polje partner enako $"{FirstName} {LastName}" - ce jih je vec potem gleda se ostala polja (not sure katera in v katerem vrstnem redu).
                    FirstName = "Kristijan",
                    LastName = "Mirceta",
                    Company = "Birokrat d.o.o.",
                    Email = "kristijan.mirceta@gmail.com",
                    Country = "SI",
                    City = "Ljubljana",
                    Postcode = "1000",
                    Address1 = "dunajska cesta 191",
                },
                Shipping = new Shipping() {
                    // lahko je drug partner kot pri billingu, izpolnjen na isti nacin.
                    FirstName = "Kristijan",
                    LastName = "Mirceta",
                    Company = "kristijan.mirceta@gmail.com",
                    Country = "SI",
                    City = "Ljubljana",
                    Postcode = "1000",
                    Address1 = "dunajska cesta 191",
                }
            };

            List<BirokratPostavka> postavke = new List<BirokratPostavka>();
            /*
              // Tole dela trenutno po tem principu: Subtotal je skupna cena 5-ih artiklov, brez popusta. 
            Sepravi Birokrat bo za vse skupaj dal ceno 20 evrov, potem pa odstel 20% popusta in bo na racunu na koncu 16 evrov. 
            Potem bo sam izracunal koliko bi mogla biti cena posameznega primerka artikla.
             */
            postavke.Add(new BirokratPostavka() {
                BirokratSifra = "4900PPTR", // Trenutno je nujno, da je artikel ze v sifrantu artiklov. Zelo verjetno to v prihodnosti ne bo vec pogoj - prosim ne sprasujte kdaj ker ne vem in ni odvisno od mene.
                DiscountPercent = 20,
                Quantity = 5,
                Subtotal = "50"
            });

            string eslog = EslogGen.CreateXML(order, postavke);

            ApiClientV2 client = new ApiClientV2(api_address, apikey); // tole je api key od testnega racuna - zamenjaj s svojim

            string id = await CreateInvoice(client, order, eslog, zakljucno_besedilo);
            await SavePdf(client, id, path_to_save_invoice_pdf);

            Console.Read();
        }

        private static async Task<string> CreateInvoice(ApiClientV2 client, Order order, string xml, string finaltext1) {
            string racun_path = "poslovanje/racuni/izstavitevinpregled";
            string result = await client.document.CreateEslog(racun_path, xml);

            

            // update
            var dic = new Dictionary<string, object>();
            dic["cmbPredloga"] = "Privzeto"; // imas vec predlog po katerih lahko sprintas racune - poklici Birokrat podporo.

            // drzava
            if (order.Billing.Country != "SI") { // Tako lahko nastavis da racun pride ven v anglescini
                dic["cmbJezik"] = "003 Angleščina";
            } else {
                dic["cmbJezik"] = "002 Slovenščina";
            }

            dic["DodatnaStevilka"] = "" + order.Id; // katerikoli hoces dodatni identifikator

            // klavzula
            dic["Klavzula"] = finaltext1; // tole je v stilu "Hvala ker ste nakupovali pri nas, pridite se kdaj!"


            // v klicu UpdateParameters vidis katera vsa polja lahko nastavis po principu kot zgoraj v objektu "dic". 
            var res = await client.document.UpdateParameters(racun_path, result);
            var postback = res.ToDictionary(x => x.Koda, y => y.PrivzetaVrednost); // ce zelis nastavljati vrednosti v slovarju "dic" potem glej polje x.Koda - 
            dic["DatumValute"] = "2021-09-08"; // recimo "DatumValue" je res[0].Koda - lahko pogledas tudi res[0].PrivzetaVrednost da vidis template za to kaksna mora vrednost biti

            await client.document.Update(racun_path, result, dic);

            return result;
        }

        private static async Task SavePdf(ApiClientV2 client, string id, string pdf_path) {
            string racun_path = "poslovanje/racuni/izstavitevinpregled";

            string pdf = await client.document.GetPdf(racun_path, id);
            var ano = new { content = "" };
            pdf = JsonConvert.DeserializeAnonymousType(pdf, ano).content;

            var bytes = Convert.FromBase64String(pdf);
            var file = new FileStream(pdf_path, FileMode.OpenOrCreate);
            var srm = new MemoryStream(bytes);
            await srm.CopyToAsync(file);
            srm.Close();
            file.Close();
        }
    }
}
