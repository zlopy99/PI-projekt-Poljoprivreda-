using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Kupac
    {
        public Kupac()
        {
            Narudžbas = new HashSet<Narudžba>();
            ProdajaBezNarudzbes = new HashSet<ProdajaBezNarudzbe>();
            OstvareneNarudzbes = new HashSet<OstvareneNarudzbe>();
        }

        public int KupacId { get; set; }
        [Required(ErrorMessage = "unesite ime i prezime kupca!")]
        public string ImePrezime { get; set; }
        [Required(ErrorMessage = "unesite kontakt kupca!")]
        public string Kontkat { get; set; }
        [Required(ErrorMessage = "unesite adresu kupca!")]
        public string Adresa { get; set; }

        public virtual ICollection<Narudžba> Narudžbas { get; set; }
        public virtual ICollection<OstvareneNarudzbe> OstvareneNarudzbes { get; set; }
        public virtual ICollection<ProdajaBezNarudzbe> ProdajaBezNarudzbes { get; set; }
    }
}
