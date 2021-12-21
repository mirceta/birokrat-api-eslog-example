using BiroWoocommerceHub.structs_wc_to_biro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiroWoocommerceHub.logic.eslog_gen
{
    public class InvoiceSpecification
    {
        public static string Get(IList<BirokratPostavka> postavke) {
            var x = postavke.Select((x, i) => postavkeRacunaTemplate(x, i));
            return string.Join('\n', x.ToArray());
        }

        private static string postavkeRacunaTemplate(BirokratPostavka x, int stevilkaVrstice) {

            
            string celotnaVrednost = (double.Parse(x.Subtotal) * (1 - 0.01 * x.DiscountPercent)).ToString();
            string odstotek = x.DiscountPercent.ToString();

            return $@"
                <PostavkeRacuna>
                    <Postavka>
                        <StevilkaVrstice>{stevilkaVrstice}</StevilkaVrstice>
                    </Postavka>
                    <DodatnaIdentifikacijaArtikla>
                        <VrstaPodatkaArtikla>5</VrstaPodatkaArtikla>
                        <StevilkaArtiklaDodatna>{x.BirokratSifra}</StevilkaArtiklaDodatna>
                        <VrstaKodeArtiklaDodatna>SA</VrstaKodeArtiklaDodatna>
                    </DodatnaIdentifikacijaArtikla>
                    <OpisiArtiklov>
                        <KodaOpisaArtikla>F</KodaOpisaArtikla>
                        <OpisArtikla>
                            <VrstaArtikla>CU</VrstaArtikla>
                            <OpisArtikla1></OpisArtikla1>
                            <OpisArtikla2></OpisArtikla2>
                        </OpisArtikla>
                    </OpisiArtiklov>
                    <KolicinaArtikla>
                        <VrstaKolicine>47</VrstaKolicine>
                        <Kolicina>{x.Quantity}</Kolicina>
                        <EnotaMere>PCE</EnotaMere>
                    </KolicinaArtikla>
                    <ZneskiPostavke>
                        <VrstaZneskaPostavke>38</VrstaZneskaPostavke>
                        <ZnesekPostavke>{celotnaVrednost}</ZnesekPostavke>
                    </ZneskiPostavke>
                    <ZneskiPostavke>
                        <VrstaZneskaPostavke>203</VrstaZneskaPostavke>
                        <ZnesekPostavke></ZnesekPostavke>
                    </ZneskiPostavke>
                    <CenaPostavke>
                        <VrstaCene>AAA</VrstaCene>
                        <Cena></Cena>
                    </CenaPostavke>
                    <CenaPostavke>
                        <VrstaCene>AAB</VrstaCene>
                        <Cena></Cena>
                    </CenaPostavke>
                    <DavkiPostavke>
                        <DavkiNaPostavki>
                            <VrstaDavkaPostavke>VAT</VrstaDavkaPostavke>
                            <OdstotekDavkaPostavke>22.00</OdstotekDavkaPostavke>
                        </DavkiNaPostavki>
                        <ZneskiDavkovPostavke>
                            <VrstaZneskaDavkaPostavke>125</VrstaZneskaDavkaPostavke>
                            <Znesek></Znesek>
                        </ZneskiDavkovPostavke>
                        <ZneskiDavkovPostavke>
                            <VrstaZneskaDavkaPostavke>124</VrstaZneskaDavkaPostavke>
                            <Znesek></Znesek>
                        </ZneskiDavkovPostavke>
                    </DavkiPostavke>
                    <OdstotkiPostavk>
                        <Identifikator>A</Identifikator>
                        <VrstaOdstotkaPostavke>1</VrstaOdstotkaPostavke>
                        <OdstotekPostavke>{odstotek}</OdstotekPostavke>
                        <VrstaZneskaOdstotka>204</VrstaZneskaOdstotka>
                        <ZnesekOdstotka></ZnesekOdstotka>
                    </OdstotkiPostavk>
                </PostavkeRacuna>
            ";
        }
    }
}
