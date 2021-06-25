using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Troškovi
    {
        public int TroskoviId { get; set; }
        [Required(ErrorMessage = "Unesite namjenu!")]
        public string Namjena { get; set; }
        [Required(ErrorMessage = "Unesite iznos!")]
        public string Iznos { get; set; }
        [Required(ErrorMessage = "Unesite datum!")]
        public DateTime? Datum { get; set; }
        public int? Korisnik { get; set; }
        public string Opis { get; set; }

        public virtual Korisnik KorisnikNavigation { get; set; }
    }
}
