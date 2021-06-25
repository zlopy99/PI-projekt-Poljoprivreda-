using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Narudžba
    {
        public Narudžba()
        {
            DetaljiNarudžbes = new HashSet<DetaljiNarudžbe>();
        }

        public int NaruzdbaId { get; set; }
        public int Kupac { get; set; }
        [Required(ErrorMessage = "Unesite datum narudžbe.")]
        public DateTime? DatumNarudzbe { get; set; }
        [Required(ErrorMessage = "Unesite datum isporuke.")]
        public DateTime? DatumIsporuke { get; set; }
        public int Korisnik { get; set; }

        public virtual Kupac KupacNavigation { get; set; }
        public virtual Korisnik KorisnikNavigation { get; set; }
        public virtual ICollection<DetaljiNarudžbe> DetaljiNarudžbes { get; set; }
    }
}
