using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PI_projekt.Models
{
    public partial class Urod
    {
        public int UrodId { get; set; }
        public int Mikrolokacija { get; set; }
        [Required(ErrorMessage = "Unesite opis!")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Unesite količinu!")]
        public string Kolicina { get; set; }

        public virtual Mikrolokacija MikrolokacijaNavigation { get; set; }
    }
}
