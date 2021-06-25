using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class PosloviBiljka
    {
        public int PosloviBiljkaId { get; set; }
        public int Poslovi { get; set; }
        public int? Biljka { get; set; }
        public string Opis { get; set; }

        public virtual BiljkeKorisnik BiljkaNavigation { get; set; }
        public virtual Poslovi PosloviNavigation { get; set; }
    }
}
