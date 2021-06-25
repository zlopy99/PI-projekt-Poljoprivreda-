using System;
using System.Collections.Generic;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Biljka
    {
        public Biljka()
        {
            BiljkaUpotrebas = new HashSet<BiljkaUpotreba>();
            BiljkeKorisniks = new HashSet<BiljkeKorisnik>();
            FazaRazvojaBiljkes = new HashSet<FazaRazvojaBiljke>();
            NarodnaImenas = new HashSet<NarodnaImena>();
        }

        public int BiljkaId { get; set; }
        public string Vrsta { get; set; }
        public int? Razred { get; set; }
        public int? Red { get; set; }
        public int? Porodica { get; set; }
        public int? Rod { get; set; }
        public string Slika { get; set; }
        public string Opis { get; set; }
        public string Naziv { get; set; }

        public virtual Porodica PorodicaNavigation { get; set; }
        public virtual Razred RazredNavigation { get; set; }
        public virtual Red RedNavigation { get; set; }
        public virtual Rod RodNavigation { get; set; }
        public virtual ICollection<BiljkaUpotreba> BiljkaUpotrebas { get; set; }
        public virtual ICollection<BiljkeKorisnik> BiljkeKorisniks { get; set; }
        public virtual ICollection<FazaRazvojaBiljke> FazaRazvojaBiljkes { get; set; }
        public virtual ICollection<NarodnaImena> NarodnaImenas { get; set; }
    }
}
