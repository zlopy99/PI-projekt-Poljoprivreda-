using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PI_projekt.Models
{ 
    public partial class Korisnik
    {
        public Korisnik()
        {
            Alats = new HashSet<Alat>();
            BiljkeKorisniks = new HashSet<BiljkeKorisnik>();
            OstvareneNarudzbes = new HashSet<OstvareneNarudzbe>();
            PlaniraniTroškovis = new HashSet<PlaniraniTroškovi>();
            Troškovis = new HashSet<Troškovi>();
            Lokacijas = new HashSet<Lokacija>();
            Narudžbas = new HashSet<Narudžba>();
        }
        
        public int KorisnikId { get; set; }
        [Required(ErrorMessage = "Unesite Ime!")]
        public string ImePrezime { get; set; }
        [Required(ErrorMessage = "Unesite Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Unesite lozinku!")]
        [MinLength(6, ErrorMessage = "Minimalno 6 znakova!")]
        public string Lozinka { get; set; }

        public virtual ICollection<Alat> Alats { get; set; }
        public virtual ICollection<BiljkeKorisnik> BiljkeKorisniks { get; set; }
        public virtual ICollection<OstvareneNarudzbe> OstvareneNarudzbes { get; set; }
        public virtual ICollection<PlaniraniTroškovi> PlaniraniTroškovis { get; set; }
        public virtual ICollection<Troškovi> Troškovis { get; set; }
        public virtual ICollection<Lokacija> Lokacijas { get; set; }
        public virtual ICollection<Narudžba> Narudžbas { get; set; }
    }
}
