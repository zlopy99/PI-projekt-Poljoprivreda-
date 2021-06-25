using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Lokacija
    {
        public Lokacija()
        {
            Mikrolokacijas = new HashSet<Mikrolokacija>();
        }

        public int LokacijaId { get; set; }
        [Required(ErrorMessage = "Unesite naziv!")]
        public string Naziv { get; set; }
        public int? VrstaTla { get; set; }
        public string UkupnaPovrsinaParcele { get; set; }
        public string ObrađenaPovrsina { get; set; }
        public int? Korisnik { get; set; }

        public virtual VrstaTla VrstaTlaNavigation { get; set; }
        public virtual Korisnik KorisnikNavigation { get; set; }
        public virtual ICollection<Mikrolokacija> Mikrolokacijas { get; set; }
    }
}
