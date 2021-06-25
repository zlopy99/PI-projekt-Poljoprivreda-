using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class DetaljiNarudžbe
    {
        public int DetaljiNarudzbeId { get; set; }
        public int? Narudzba { get; set; }
        [Required(ErrorMessage = "Izaberite biljku!")]
        public int? Biljka { get; set; }
        [Required(ErrorMessage = "Unesite cijenu!")]
        public string UkupnaCijena { get; set; }
        public string Opis { get; set; }
        public string kolicina { get; set; }

        public virtual BiljkeKorisnik BiljkaNavigation { get; set; }
        public virtual Narudžba NarudzbaNavigation { get; set; }
    }
}
