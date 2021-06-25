using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class OstvareneNarudzbe
    {
        public int OstvareneNarudzbeId { get; set; }
        [Required(ErrorMessage = "Izaberite biljku!")]
        public int? Biljka { get; set; }
        [Required(ErrorMessage = "Unesite datum!")]
        public DateTime? Datum { get; set; }
        [Required(ErrorMessage = "Izaberite kupca!")]
        public int Kupac { get; set; }
        [Required(ErrorMessage = "Unesite zaradu!")]
        public string Zarada { get; set; }
        public int? Korisnik { get; set; }
        public string Opis { get; set; }
        public string Kolicina { get; set; }

        public virtual Korisnik KorisnikNavigation { get; set; }
        public virtual BiljkeKorisnik BiljkeKorisnikNavigation { get; set; }
        public virtual Kupac KupacNavigation { get; set; }
    }
}
