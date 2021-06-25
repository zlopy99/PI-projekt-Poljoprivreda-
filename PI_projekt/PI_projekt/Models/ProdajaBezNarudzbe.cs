using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class ProdajaBezNarudzbe
    {
        public int ProdajaBezNarudzbeId { get; set; }
        [Required(ErrorMessage = "Izaberite biljku!")]
        public int? Biljka { get; set; }
        [Required(ErrorMessage = "Izaberite kupca!")]
        public int? Kupac { get; set; }
        [Required(ErrorMessage = "Unesite cijenu!")]
        public string UkupnaCijena { get; set; }
        public string Opis { get; set; }
        public string Kolicina { get; set; }

        public virtual BiljkeKorisnik BiljkaNavigation { get; set; }
        public virtual Kupac KupacNavigation { get; set; }
    }
}
