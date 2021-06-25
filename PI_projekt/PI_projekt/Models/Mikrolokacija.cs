using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Mikrolokacija
    {
        public Mikrolokacija()
        {
            Urods = new HashSet<Urod>();
        }

        public int MikrolokacijaId { get; set; }
        public string Površina { get; set; }
        public string OčekivaniUrod { get; set; }
        public int Biljka { get; set; }
        public int TipUzgoja { get; set; }
        public int? Lokacija { get; set; }
        public string Opis { get; set; }

        public virtual BiljkeKorisnik BiljkaNavigation { get; set; }
        public virtual Lokacija LokacijaNavigation { get; set; }
        public virtual TipUzgoja TipUzgojaNavigation { get; set; }
        public virtual ICollection<Urod> Urods { get; set; }
    }
}
