using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiroWoocommerceHub.structs_wc_to_biro
{
    public class BirokratPostavka
    {
        public string BirokratSifra { get; set; }
        public int Quantity { get; set; }
        public string Subtotal { get; set; }
        public int DiscountPercent { get; set; }
    }
}
