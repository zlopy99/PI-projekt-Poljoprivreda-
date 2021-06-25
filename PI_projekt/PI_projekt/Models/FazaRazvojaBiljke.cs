using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class FazaRazvojaBiljke
    {
        public int FazaRazvojaBiljkeId { get; set; }
        public int Biljka { get; set; }
        public int FazaRazvoja { get; set; }
        public string TrajanjeFaze { get; set; }
        public string GodisnjeDobaOdvijanjaFaze { get; set; }
        public string Opis { get; set; }

        public virtual Biljka BiljkaNavigation { get; set; }
        public virtual FazaRazvoja FazaRazvojaNavigation { get; set; }
    }
}
