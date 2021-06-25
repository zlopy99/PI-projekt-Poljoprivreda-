using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class BiljkeKorisnik
    {
        public BiljkeKorisnik()
        {
            
            DetaljiNarudžbes = new HashSet<DetaljiNarudžbe>();
            Mikrolokacijas = new HashSet<Mikrolokacija>();
            PosloviBiljkas = new HashSet<PosloviBiljka>();
            ProdajaBezNarudzbes = new HashSet<ProdajaBezNarudzbe>();
            OstvareneNarudzbes = new HashSet<OstvareneNarudzbe>();
        }

        public int BiljkaKorisnikId { get; set; }
        [Required]
        public int Biljka { get; set; }
        public int Korisnik { get; set; }
        public string CijenaPoKg { get; set; }
        public string Opis { get; set; }

        public virtual Biljka BiljkaNavigation { get; set; }
        public virtual Korisnik KorisnikNavigation { get; set; }
        public virtual ICollection<DetaljiNarudžbe> DetaljiNarudžbes { get; set; }
        public virtual ICollection<Mikrolokacija> Mikrolokacijas { get; set; }
        public virtual ICollection<PosloviBiljka> PosloviBiljkas { get; set; }
        public virtual ICollection<ProdajaBezNarudzbe> ProdajaBezNarudzbes { get; set; }
        public virtual ICollection<OstvareneNarudzbe> OstvareneNarudzbes { get; set; }
    }
}
